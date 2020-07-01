using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autofac;
using PythonHospitalDemo.ViewModels;
using ReactiveUI;

namespace PythonHospitalDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MainWindowBase
    {
        public MainWindow()
        {
            InitializeComponent();
            Module.Initialize();
            ViewModel = Module.Container.Resolve<PatientsViewModel>();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Patients, v => v.PatientsDataGrid.ItemsSource).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.OpenFileCommand, v => v.OpenFileButton).DisposeWith(disposables);
            });
        }
    }

    public class MainWindowBase : ReactiveWindow<PatientsViewModel> { }
}
