using Autofac;
using Python.Runtime;
using PythonNetEngine.Interfaces;
using PythonNetEngine.Properties;
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
            SetPythonPaths();
        }

        private void SetPythonPaths()
        {
            var pyInstallDir = Environment.ExpandEnvironmentVariables(Settings.Default.PythonInstallDir);
            CheckPythonInstalled(pyInstallDir);
            SetEnvironmentVariable("@PATH", pyInstallDir);
            SetEnvironmentVariable(@"PYTHONHOME", pyInstallDir);
            SetEnvironmentVariable(@"PYTHONPATH", Path.Combine(pyInstallDir, @"DLLs/"));
            SetEnvironmentVariable(@"PYTHONPATH", Path.Combine(pyInstallDir, @"Lib/site-packages"));
            SetEnvironmentVariable(@"PYTHONPATH", Path.Combine(pyInstallDir, @"Lib/"));
            SetEnvironmentVariable(@"PYTHONPATH", Path.Combine(pyInstallDir, @"Python/"));
        }

        private void CheckPythonInstalled(string installDir)
        {
            if (!Directory.Exists(installDir))
            {
                throw new ArgumentException($@"Python directory: {installDir} does not exist.");
            }

            var pyDll = Directory.GetFiles(installDir).FirstOrDefault(s => s.Contains(@"python3.dll"));
            if (pyDll == null)
            {
                throw new ArgumentException($@"python3.dll not found in {pyDll}");
            }
        }

        private void SetEnvironmentVariable(string variable, string value)
        {
            var target = EnvironmentVariableTarget.Process;
            var currentValue = Environment.GetEnvironmentVariable(variable, target);
            if (currentValue != null)
            {
                var splitValues = currentValue.Split(Path.PathSeparator);
                if (splitValues.Any(s => !string.IsNullOrEmpty(s) && Path.GetFullPath(s) == Path.GetFullPath(value)))
                {
                    return;
                }

                var combinedVariables = Path.GetFullPath(value) + Path.PathSeparator + currentValue;
                Environment.SetEnvironmentVariable(variable, combinedVariables, target);
            }
            else
            {
                Environment.SetEnvironmentVariable(variable, Path.GetFullPath(value), target);
            }
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
