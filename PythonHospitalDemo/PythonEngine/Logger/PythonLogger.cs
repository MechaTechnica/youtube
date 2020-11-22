using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PythonNetEngine.Interfaces;

namespace PythonNetEngine.Logger
{
    public class PythonLogger : IPythonLogger
    {
        private readonly List<string> m_buffer;

        public PythonLogger()
        {
            m_buffer = new List<string>();
        }

        public void flush()
        {
            m_buffer.Clear();
        }

        public void write(string str)
        {
            m_buffer.Add(str);
            Trace.WriteLine(str);
        }

        public void writelines(string[] str)
        {
            m_buffer.AddRange(str);
        }

        public void close()
        {
            m_buffer.Clear();
        }

        public string ReadStream()
        {
            var str = string.Join("\n", m_buffer);
            return str;
        }
    }
}
