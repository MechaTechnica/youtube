using HospitalDemo.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalDemo.Application.Services
{
    public class PatientService : IPatientService
    {
        private List<PatientDto> m_patients = new List<PatientDto>
        {
            new PatientDto{Age = 10, Id = Guid.NewGuid(), PatientName= "Solid", Insured = true },
            new PatientDto{Age = 25, Id = Guid.NewGuid(), PatientName= "π-thon", Insured = true },
            new PatientDto{Age = 8, Id = Guid.NewGuid(), PatientName= "Squeezy", Insured = true },
            new PatientDto{Age = 2, Id = Guid.NewGuid(), PatientName= "Nagini", Insured = false },
            new PatientDto{Age = 13, Id = Guid.NewGuid(), PatientName= "Hissy", Insured = true },
            new PatientDto{Age = 5, Id = Guid.NewGuid(), PatientName= "Balboa", Insured = true },
            new PatientDto{Age = 9, Id = Guid.NewGuid(), PatientName= "Severus", Insured = true }
        };

        public List<PatientDto> Patients()
        {
            return m_patients;
        }
    }
}
