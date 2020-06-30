using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonHospitalDemo.Controllers
{
    public interface IPythonEngineController
    {
        void Initialize();

        void RunCommand(string command);
    }
}
