using Autofac;
using System;
using System.Collections.Generic;

namespace PythonNetEngine.Interfaces
{
    public interface IPythonEngine : IDisposable
    {
        // executes a Python code
        string ExecuteCommand(string code);

        // executes a file
        string ExecuteFile(string code, string filename);

        // sets an object in Python's scope
        void SetVariable(string name, object value);

        // Python's search path
        IList<string> SearchPaths();

        // sets search parths for specified module
        void SetSearchPath(IList<string> paths, string module);

        // Sets search paths
        void SetSearchPath(IList<string> paths);

        // initializes this engine with the app container
        void Initialize(IContainer appContainer);
    }
}
