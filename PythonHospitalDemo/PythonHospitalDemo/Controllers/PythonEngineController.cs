using PythonNetEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonHospitalDemo.Controllers
{
    public class PythonEngineController : IPythonEngineController
    {
        private readonly IPythonEngine m_pythonEngine;

        public PythonEngineController(IPythonEngine pythonEngine)
        {
            m_pythonEngine = pythonEngine;
            Initialize();
        }
        public void Initialize()
        {
            m_pythonEngine.SetSearchPath(new List<string> { "./Python/" });
            m_pythonEngine.Initialize(Module.Container);
        }

        public void RunCommand(string command)
        {
            m_pythonEngine.ExecuteCommand(command);
        }
    }
}
