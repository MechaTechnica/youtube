using System.Text;
using ReactiveUI;

namespace PythonHospitalDemo.ViewModels
{
    public class ReactiveStringBuilder : ReactiveObject
    {
        private readonly StringBuilder m_builder = new StringBuilder();
        private string m_text;

        public string Text
        {
            get => m_text;
            private set => this.RaiseAndSetIfChanged(ref m_text, value);
        }

        public void Add(string text)
        {
            if (text == null)
            {
                return;
            }

            m_builder.Append(text);
            m_builder.AppendLine();
            Text = m_builder.ToString();
        }
    }
}
