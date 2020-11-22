using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonHospitalDemo.Controllers
{
    public interface IPythonEngineController
    {
        void Initialize(string module);
        void Initialize();

        string RunCommand(string command);
        string RunFile(string code, string source);
    }
}
