// <copyright file="Program.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using PMA.Application;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Infrastructure;
using PMA.WinForms.Forms;
using PMA.WinForms.Models;
using System;
using System.Reflection;

namespace PMA.WinForms
{
    internal static class Program
    {
        /// <summary>
        /// An <see cref="ILifetimeScope"/> tracks the instantiation of component instances. It defines a boundary in which instances are shared and configured. Disposing an <see cref="ILifetimeScope"/> will dispose the components that were resolved through it.
        /// </summary>
        public static ILifetimeScope Scope { get; private set; }

        /// <summary>
        /// PMA version.
        /// </summary>
        public static string Version { get; private set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // ReSharper disable once PossibleNullReferenceException
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString()[..^2];

            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
            var configuration = builder.Build();
            var settings = configuration.GetSection("Settings").Get<AppSettings>();

            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            using (var startupForm = new StartupForm())
            {
                startupForm.Show();

                var containerBuilder = new ContainerBuilder();

                containerBuilder.RegisterApplicationTypes();
                containerBuilder.RegisterInfrastructureTypes(settings.DataSource);

                Scope = containerBuilder.Build().BeginLifetimeScope();

                // Initial actions
                var loggerFactory = Scope.Resolve<ILoggerFactory>();
                loggerFactory.AddProvider(new NLogLoggerProvider());

                Scope.Resolve<IMainViewModel>();
            }

            System.Windows.Forms.Application.Run(new MainForm());
        }
    }
}
