using PythonNetEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonNetEngine
{
    public class PythonLogger : IPythonLogger
    {
        private List<string> m_buffer;

        public PythonLogger()
        {
            m_buffer = new List<string>();
        }

        public void close()
        {
            m_buffer.Clear();
        }

        public void flush()
        {
            m_buffer.Clear();
        }

        public string ReadStream()
        {
            var str = string.Join("\n", m_buffer);
            return str;
        }

        public void write(string str)
        {
            if (str == "\n")
            {
                return;
            }

            m_buffer.Add(str);
            Trace.WriteLine(str);
        }

        public void writelines(string[] str)
        {
            m_buffer.AddRange(str);
        }
    }
}
