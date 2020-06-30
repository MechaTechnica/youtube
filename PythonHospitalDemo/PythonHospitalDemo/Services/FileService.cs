using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonHospitalDemo.Services
{
    public class FileService : IFileService
    {
        public string FilePrompt(string filter)
        {
            var fileName = "";
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = filter;
            fileDialog.Title = "Open Python Script";
            var file = fileDialog.ShowDialog();
            if (file.HasValue)
            {
                fileName = fileDialog.FileName;
            }

            return fileName;
        }

        public string ReadFile(string fileName)
        {
            return File.ReadAllText(fileName);
        }
    }
}
