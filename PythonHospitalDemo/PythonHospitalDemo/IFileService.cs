using Autofac;
using HospitalDemo.Application.Interfaces;
using HospitalDemo.Application.Services;
using PythonHospitalDemo.ViewModels;
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
                Container = builder.Build();
            }
        }
    }
}
