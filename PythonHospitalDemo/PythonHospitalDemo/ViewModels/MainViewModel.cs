using HospitalDemo.Application.Interfaces;
using PythonHospitalDemo.Controllers;
using ReactiveUI;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using PythonHospitalDemo.Services;

namespace PythonHospitalDemo.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private readonly IPythonEngineController m_pythonEngineController;
        private readonly IFileService m_fileService;
        private string m_pythonCode;

        public List<PatientDto> Patients { get; set; }

        public ReactiveStringBuilder PythonConsoleText { get; set; }

        public ReactiveCommand<Unit, Unit> PythonCommand { get; set; }

        public string PythonCode
        {
            get => m_pythonCode;
            set => this.RaiseAndSetIfChanged(ref m_pythonCode, value);
        }

        public ReactiveCommand<Unit, Unit> OpenFileCommand { get; set; }

        public MainViewModel(IPatientService patientService, 
            IPythonEngineController pythonEngineController,
            IFileService fileService)
        {
            PythonConsoleText = new ReactiveStringBuilder();
            PythonCommand = ReactiveCommand.Create(RunPythonCommandHandler,
                this.WhenAnyValue(s => s.PythonCode).Any(s => !string.IsNullOrEmpty(s)));
            m_pythonEngineController = pythonEngineController;
            m_fileService = fileService;
            Patients = patientService.Patients();
            OpenFileCommand = ReactiveCommand.Create(OpenFileCommandHandler);
        }

        private void RunPythonCommandHandler()
        {
            PythonConsoleText.Add(PythonCode);

            var pythonOut = m_pythonEngineController.RunCommand(PythonCode);
            PythonConsoleText.Add(pythonOut);
            PythonCode = string.Empty;
        }

        private void OpenFileCommandHandler()
        {
            var file = m_fileService.FilePrompt(@"Python files (*.py)|*.py");
            if (!string.IsNullOrEmpty(file))
            {
                var src = m_fileService.ReadFile(file);
                m_pythonEngineController.RunFile(src, file);
            }
        }
    }
}
