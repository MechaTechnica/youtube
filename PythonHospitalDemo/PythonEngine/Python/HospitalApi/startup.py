import builtins

patient_service = DiContainer.Resolve("HospitalDemo.Application.Interfaces.IPatientService")

builtins.patient_service = patient_service
