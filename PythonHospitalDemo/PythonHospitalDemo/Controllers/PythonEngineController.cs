using PythonNetEngine.Interfaces;
using System.Collections.Generic;

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

        public void Initialize(string module)
        {
            m_pythonEngine.SetSearchPath(new List<string> { "./Python/" }, module);
            m_pythonEngine.Initialize(Module.Container);
        }

        public void Initialize()
        {
            Initialize("");
        }

        public string RunCommand(string command)
        {
            return m_pythonEngine.ExecuteCommand(command);
        }

        public string RunFile(string code, string fileName)
        {
            Initialize(fileName);
            return m_pythonEngine.ExecuteFile(code, fileName);
        }
    }
}
