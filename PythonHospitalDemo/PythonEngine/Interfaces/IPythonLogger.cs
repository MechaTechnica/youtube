using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonNetEngine.Interfaces
{
    public interface IPythonLogger
    {
        void flush();

        void write(string str);

        void writelines(string[] str);

        void close();

        string ReadStream();
    }
}
