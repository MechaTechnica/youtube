using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonNetEngine
{
    class DiContainer
    {
        private readonly IContainer m_container;

        public DiContainer(IContainer container)
        {
            this.m_container = container;
        }
        public object Resolve(string typeFullName)
        {
            var type = GetType(typeFullName);
            return m_container.Resolve(type);
        }

        private Type GetType(string fullName)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).ToList();
            return types.Where(s => string.Equals(s.FullName, fullName, StringComparison.Ordinal)).FirstOrDefault();
        }
    }
}
