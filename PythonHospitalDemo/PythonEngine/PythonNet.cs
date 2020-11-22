using Autofac;
using Python.Runtime;
using PythonNetEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PythonNetEngine
{
    public class PythonNet : IPythonEngine
    {
        private readonly IPythonLogger m_pythonLogger;
        private readonly Lazy<PyScope> m_scope;

        public PythonNet(IPythonLogger pythonLogger)
        {
            m_pythonLogger = pythonLogger;
            m_scope = new Lazy<PyScope>(Py.CreateScope);
            InitLogger();
        }

        private void InitLogger()
        {
            SetVariable("Logger", m_pythonLogger);
            const string loggerSrc = "import sys\n" +
                                     "from io import StringIO\n" +
                                     "sys.stdout = Logger\n" +
                                     "sys.stdout.flush()\n" +
                                     "sys.stderr = Logger\n" +
                                     "sys.stderr.flush()";
            ExecuteCommand(loggerSrc);
        }

        public void Dispose()
        {
            m_scope.Value.Dispose();
        }

        public string ExecuteCommand(string command)
        {
            return ExecuteFile(command, "");
        }

        public void Initialize(IContainer appContainer)
        {
            SetVariable("DiContainer", new DiContainer(appContainer));
            var startupFile = "./Python/HospitalApi/startup.py";
            var initScript = File.ReadAllText(startupFile);
            ExecuteFile(initScript, startupFile);
        }

        public IList<string> SearchPaths()
        {
            var pythonPaths = new List<string>();
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                foreach(PyObject path in sys.path)
                {
                    pythonPaths.Add(path.ToString());
                }
            }

            return pythonPaths.Select(Path.GetFullPath).ToList();
        }

        public void SetSearchPath(IList<string> paths)
        {
            SetSearchPath(paths, "");
        }

        public void SetSearchPath(IList<string> paths, string module)
        {
            var searchPaths = paths.Where(Directory.Exists).Distinct().ToList();

            using (Py.GIL())
            {
                var src = "import sys\n" +
                          $"sys.path.extend({searchPaths.ToPython()})";
                ExecuteFile(src, module);
            }
        }

        public void SetVariable(string name, object value)
        {
            using (Py.GIL())
            {
                m_scope.Value.Set(name, value.ToPython());
            }
        }

        public string ExecuteFile(string code, string filename)
        {
            string result;
            try
            {
                using (Py.GIL())
                {
                    var pyCompile = PythonEngine.Compile(code, filename);
                    m_scope.Value.Execute(pyCompile);
                    result = m_pythonLogger.ReadStream();
                    m_pythonLogger.flush();
                }
            }
            catch (PythonException ex)
            {
                result = $"Trace: \n{ex.StackTrace} " + "\n" +
                             $"Message: \n {ex.Message}" + "\n";
            }

            return result;
        }
    }
}
