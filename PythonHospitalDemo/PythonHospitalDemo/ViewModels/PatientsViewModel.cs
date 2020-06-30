using HospitalDemo.Application.Interfaces;
using PythonHospitalDemo.Controllers;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace PythonHospitalDemo.ViewModels
{
    public class PatientsViewModel : ReactiveObject
    {
        private readonly IPatientService m_patientService;
        private readonly IPythonEngineController m_pythonEngineController;
        private readonly IFileService m_fileService;

        public List<PatientDto> Patients { get; set; }
        
        public ReactiveCommand<Unit, Unit> OpenFileCommand { get; set; }

        public PatientsViewModel(IPatientService patientService, 
            IPythonEngineController pythonEngineController,
            IFileService fileService)
        {
            m_patientService = patientService;
            m_pythonEngineController = pythonEngineController;
            m_fileService = fileService;
            Patients = m_patientService.Patients();
            OpenFileCommand = ReactiveCommand.Create(OpenFileCommandHandler);
        }

        private void OpenFileCommandHandler()
        {
            var file = m_fileService.FilePrompt(@"Python files (*.py)|*.py");
            if (!string.IsNullOrEmpty(file))
            {
                var src = m_fileService.ReadFile(file);
                m_pythonEngineController.RunCommand(src);
            }
        }
    }
}
