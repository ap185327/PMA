// <copyright file="Program.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using PMA.Application;
using PMA.Domain.EventArguments;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Infrastructure;
using PMA.WinForms.Forms;
using PMA.WinForms.Helpers;
using PMA.WinForms.Models;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

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
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, false);
            var configuration = builder.Build();
            var settings = configuration.GetSection("Settings").Get<AppSettings>();

            if (!File.Exists(settings.DataSource))
            {
                MessageBox.Show($@"Cannot find database file: {settings.DataSource}", $@"Pali Morphological Analyzer (version {Version})", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            using (var startupForm = new StartupForm())
            {
                startupForm.Show();

                var containerBuilder = new ContainerBuilder();

                containerBuilder.AddApplicationServices();
                containerBuilder.AddInfrastructureServices(settings.DataSource);

                Scope = containerBuilder.Build().BeginLifetimeScope();

                // Initial actions
                var loggerFactory = Scope.Resolve<ILoggerFactory>();
                loggerFactory.AddProvider(new NLogLoggerProvider());

                Scope.Resolve<IMainViewModel>();
            }

            _modalDialogService = Scope.Resolve<IServiceLocator>().ModalDialogService;

            _modalDialogService.ModalDialogShown += ModalDialogService_ModalDialogShown;

            System.Windows.Forms.Application.Run(new MainForm());
        }

        private static IModalDialogService _modalDialogService;

        /// <summary>
        /// Event handler for the view model property dialog shown.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private static void ModalDialogService_ModalDialogShown(object sender, ModalDialogEventArgs e)
        {
            var buttons = MessageBoxHelper.GetButtons(e.Buttons);
            var icon = MessageBoxHelper.GetIcon(e.Type);

            var result = MessageBox.Show(e.Message, e.Title, buttons, icon);

            var button = MessageBoxHelper.GetButtonType(result);

            _modalDialogService.PressButton(button);
        }
    }
}
