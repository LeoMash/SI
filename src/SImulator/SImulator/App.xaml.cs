﻿using System;
using System.Windows;
using Settings = SImulator.ViewModel.Model.AppSettings;
using SImulator.ViewModel;
using SImulator.Implementation;
using System.Threading;
using System.IO.IsolatedStorage;
using System.IO;
using SIUI.ViewModel.Core;
#if !DEBUG && LEGACY
using System.Deployment.Application;
using System.Reflection;
#endif

namespace SImulator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
#pragma warning disable IDE0052
        private readonly DesktopManager _manager = new DesktopManager();
#pragma warning restore IDE0052

        /// <summary>
        /// Имя конфигурационного файла пользовательских настроек
        /// </summary>
        private const string ConfigFileName = "user.config";

        internal Settings Settings { get; } = LoadSettings();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

#if !DEBUG && LEGACY
            ProcessAsync();

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                if (CheckUpdate())
                    return;
            }
#endif
            UI.Initialize();

            var main = new MainViewModel(Settings);

            if (e.Args.Length > 0)
            {
                main.PackageSource = new FilePackageSource(e.Args[0]);
            }

#if DEBUG
            main.PackageSource = new SIStoragePackageSource(
                new Services.SI.PackageInfo
                {
                    Description = SImulator.Properties.Resources.TestPackage
                },
                new Uri("https://vladimirkhil.com/sistorage/Основные/1.siq"));
#endif

            MainWindow = new CommandWindow { DataContext = main };
            MainWindow.Show();
        }

#if !DEBUG && LEGACY
        private static bool CheckUpdate()
        {
            try
            {
                UpdateCheckInfo info = null;
                var ad = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = ad.CheckForDetailedUpdate();
                }
                catch (DeploymentDownloadException dde)
                {
                    MessageBox.Show("Невозможно загрузить новую версию приложения. \n\nОшибка: " + dde.Message);
                    return false;
                }
                catch (InvalidDeploymentException ide)
                {
                    MessageBox.Show("Невозможно проверить наличие новой версии приложения. Ошибка: " + ide.Message);
                    return false;
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show("Невозможно обновить это приложение. Ошибка: " + ioe.Message);
                    return false;
                }

                if (info.UpdateAvailable)
                {
                    bool doUpdate = true;

                    if (!info.IsUpdateRequired)
                    {
                        if (MessageBox.Show("Найдено обновление программы. Обновиться сейчас?", "Найдено обновление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        {
                            doUpdate = false;
                        }
                    }

                    if (doUpdate)
                    {
                        try
                        {
                            ad.Update();
                            System.Windows.Forms.Application.Restart();
                            Application.Current.Shutdown();
                            return true;
                        }
                        catch (DeploymentDownloadException dde)
                        {
                            MessageBox.Show("не удалось установить последнюю версию приложения. \n\n Ошибка: " + dde);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Ошибка поиска обновления: " + exc.Message);
            }

            return false;
        }
#endif

        protected override void OnExit(ExitEventArgs e)
        {            
            SaveSettings(Settings);           

            base.OnExit(e);
        }

        private static void SaveSettings(Settings settings)
        {
            try
            {
                if (Monitor.TryEnter(ConfigFileName, 2000))
                {
                    try
                    {
                        using (var file = IsolatedStorageFile.GetUserStoreForAssembly())
                        {
                            using (var stream = new IsolatedStorageFileStream(ConfigFileName, FileMode.Create, file))
                            {
                                settings.Save(stream, DesktopManager.SettingsSerializer);
                            }
                        }
                    }
                    finally
                    {
                        Monitor.Exit(ConfigFileName);
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"{SImulator.Properties.Resources.SavingSettingsError}: {exc.Message}", MainViewModel.ProductName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Loads user settings.
        /// </summary>
        public static Settings LoadSettings()
        {
            try
            {
                using (var file = IsolatedStorageFile.GetUserStoreForAssembly())
                {
                    if (file.FileExists(ConfigFileName) && Monitor.TryEnter(ConfigFileName, 2000))
                    {
                        try
                        {
                            using (var stream = file.OpenFile(ConfigFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                            {
                                return Settings.Load(stream, DesktopManager.SettingsSerializer);
                            }
                        }
                        catch { }
                        finally
                        {
                            Monitor.Exit(ConfigFileName);
                        }
                    }
                }
            }
            catch { }

            return Settings.Create();
        }

#if !DEBUG && LEGACY
        private void ProcessAsync()
        {
            var isFirstAppRun = Settings.IsFirstRun;
            Settings.IsFirstRun = false;

            if (ApplicationDeployment.IsNetworkDeployed) // Это приложение ClickOnce
            {
                if (!isFirstAppRun && ApplicationDeployment.CurrentDeployment.IsFirstRun)
                {
                    Settings.FalseStart = true;
                }
            }
        }
#endif

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var msg = e.Exception.ToString();

            if (msg.Contains("WmClose")) // Normal closing, it's ok
            {
                return;
            }

            if (e.Exception is OutOfMemoryException)
            {
                MessageBox.Show(
                    SImulator.Properties.Resources.OutOfMemoryError,
                    MainViewModel.ProductName,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else if (e.Exception is IOException ioException && IsDiskFullError(ioException))
            {
                MessageBox.Show(
                    SImulator.Properties.Resources.DiskFullError,
                    MainViewModel.ProductName,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else if (e.Exception is System.Windows.Markup.XamlParseException || e.Exception is NotImplementedException)
            {
                MessageBox.Show(
                    string.Format(SImulator.Properties.Resources.RuntimeBrokenError, e.Exception),
                    MainViewModel.ProductName,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show(
                    string.Format(SImulator.Properties.Resources.CommonAppError, e.Exception.Message),
                    MainViewModel.ProductName,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            e.Handled = true;
            Shutdown();
        }

        private static bool IsDiskFullError(Exception ex)
        {
            const int HR_ERROR_HANDLE_DISK_FULL = unchecked((int)0x80070027);
            const int HR_ERROR_DISK_FULL = unchecked((int)0x80070070);

            return ex.HResult == HR_ERROR_HANDLE_DISK_FULL
                || ex.HResult == HR_ERROR_DISK_FULL;
        }
    }
}
