using Autofac;
using HospitalDemo.Application.Interfaces;
using HospitalDemo.Application.Services;
using PythonHospitalDemo.Controllers;
using PythonHospitalDemo.Services;
using PythonHospitalDemo.ViewModels;
using PythonNetEngine;
using PythonNetEngine.Interfaces;
using System;
using System.Collections.Generic;

namespace PythonHospitalDemo
{
    class Module
    {
        public static IContainer Container { get; set; }

        public static void Initialize()
        {
            if (Container == null)
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<PatientsViewModel>().InstancePerLifetimeScope();
                builder.RegisterType<PatientService>().As<IPatientService>().InstancePerLifetimeScope();
                builder.RegisterType<FileService>().As<IFileService>().InstancePerLifetimeScope();
                builder.RegisterType<PythonNet>().As<IPythonEngine>().InstancePerLifetimeScope();
                builder.RegisterType<PythonEngineController>().As<IPythonEngineController>().InstancePerLifetimeScope();
                Container = builder.Build();
            }
        }
    }
}
