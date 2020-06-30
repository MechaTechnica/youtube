using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalDemo.Application.Interfaces
{
    public interface IPatientService
    {
        List<PatientDto> Patients();
    }

    public class PatientDto
    {
        public Guid Id { get; set; }

        public string PatientName { get; set; }
        
        public uint Age { get; set; }

        public bool Insured { get; set; }
    }
}
