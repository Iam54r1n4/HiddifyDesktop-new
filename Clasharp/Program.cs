using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Logging;
using Avalonia.Media;
using ReactiveUI;
using Serilog;
using Microsoft.Win32;
using Clasharp.Utils;
using System.Media;
using System.Text;
using Clasharp.Services;
using Microsoft.Extensions.Configuration;
using Splat;
using Autofac;
using Avalonia.Threading;
using Clasharp.Cli;
using Clasharp.Interfaces;
using Clasharp.Models.Settings;
using Clasharp.ViewModels;
using Splat.Autofac;
using System.Threading;

namespace Clasharp
{
    static class Program
    {
        // For preventing creating multiple instance of program
        public static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F-HiddifyDesktop}");

        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {

            if (!URIScheme.CheckUriSchemeExist())
            {
                // Register URI Scheme
                URIScheme.RegisterUriScheme();
            }

            InitLogger();

            TaskScheduler.UnobservedTaskException += ExceptionHandler.TaskScheduleExceptionHandler;
            RxApp.DefaultExceptionHandler = ExceptionHandler.RxHandler;

            try
            {
                var app = BuildAvaloniaApp();
                app.StartWithClassicDesktopLifetime(args);
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Fatal error happened, now exit");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void InitLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.log"),
                    Serilog.Events.LogEventLevel.Debug,
                    flushToDiskInterval: TimeSpan.FromSeconds(1),
                    fileSizeLimitBytes: 1024 * 1024 * 10)
                .CreateLogger();
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace(LogEventLevel.Debug)
                .With(new FontManagerOptions()
                {
                    DefaultFamilyName = "Noto Sans"
                })
                .UseReactiveUI();
        }
    }
}