using Autofac;
using Python.Runtime;
using PythonNetEngine.Interfaces;
using PythonNetEngine.Properties;
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
            SetPythonPaths();
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
