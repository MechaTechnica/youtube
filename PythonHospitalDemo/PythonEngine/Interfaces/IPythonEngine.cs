using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonNetEngine.Interfaces
{
    public interface IPythonEngine : IDisposable
    {
        // executes a Python command
        string ExecuteCommand(string command);

        // sets an object in Python's scope
        void SetVariable(string name, object value);

        // Python's search path
        IList<string> SearchPaths();

        // Sets search paths
        void SetSearchPath(IList<string> paths);

        // initializes this engine with the app container
        void Initialize(IContainer appContainer);
    }
}
