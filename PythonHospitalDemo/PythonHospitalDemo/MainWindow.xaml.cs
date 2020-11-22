using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using Autofac;
using PythonHospitalDemo.ViewModels;
using ReactiveUI;

namespace PythonHospitalDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Module.Initialize();
            ViewModel = Module.Container.Resolve<MainViewModel>();

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.PythonCode, v => v.PythonInput.Text);
                this.OneWayBind(ViewModel, vm => vm.PythonConsoleText.Text, v => v.PythonOutput.Text);
                Observable.FromEventPattern<KeyEventHandler, KeyEventArgs>(
                        ev => PythonInput.KeyDown += ev,
                        ev => PythonInput.KeyDown -= ev)
                    .Select(x => x.EventArgs)
                    .Where(x => x.Key == Key.Enter)
                    .Select(x => Unit.Default)
                    .InvokeCommand(ViewModel, vm => vm.PythonCommand)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Patients, v => v.PatientsDataGrid.ItemsSource).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.OpenFileCommand, v => v.OpenFileButton).DisposeWith(disposables);
            });
        }
    }

    public class MainWindowBase : ReactiveWindow<MainViewModel> { }
}
