using Autofac;
using Python.Runtime;
using PythonNetEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PythonNetEngine
{
    public class PythonNet : IPythonEngine
    {
        private Lazy<PyScope> m_scope;
        private PythonLogger m_logger = new PythonLogger();

        public PythonNet()
        {
            m_scope = new Lazy<PyScope>(() => Py.CreateScope());
        }

        public void Dispose()
        {
            m_scope.Value.Dispose();
        }

        public string ExecuteCommand(string command)
        {
            string result;
            try
            {
                using (Py.GIL())
                {
                    var pyCompile = PythonEngine.Compile(command);
                    m_scope.Value.Execute(pyCompile);
                    result = m_logger.ReadStream();
                    m_logger.flush();
                }
            }
            catch(Exception ex)
            {
                result = $"Trace: \n{ex.StackTrace} " + "\n" +
                    $"Message: \n {ex.Message}" + "\n";
            }

            return result;
        }

        public void Initialize(IContainer appContainer)
        {
            SetVariable("DiContainer", new DiContainer(appContainer));
            var initScript = File.ReadAllText("./Python/HospitalApi/startup.py");
            ExecuteCommand(initScript);
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
            var searchPaths = paths.Where(Directory.Exists).Distinct().ToList();

            using (Py.GIL())
            {
                var src = "import sys\n" + 
                           $"sys.path.extend({searchPaths.ToPython()})";
                ExecuteCommand(src);
            }
        }

        public void SetVariable(string name, object value)
        {
            using (Py.GIL())
            {
                m_scope.Value.Set(name, value.ToPython());
            }
        }

        private void SetupLogger()
        {
            SetVariable("Logger", m_logger);
            const string loggerSrc = "import sys\n" +
                                     "from io import StringIO\n" +
                                     "sys.stdout = Logger\n" +
                                     "sys.stdout.flush()\n" +
                                     "sys.stderr = Logger\n" +
                                     "sys.stderr.flush()\n";
            ExecuteCommand(loggerSrc);
        }
    }
}
