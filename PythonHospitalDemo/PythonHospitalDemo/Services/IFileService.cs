using Autofac;
using HospitalDemo.Application.Interfaces;
using HospitalDemo.Application.Services;
using PythonHospitalDemo.ViewModels;
using System;
using System.Collections.Generic;

namespace PythonHospitalDemo
{
    public interface IFileService
    {
        string FilePrompt(string filter);

        string ReadFile(string fileName);
    }
}
