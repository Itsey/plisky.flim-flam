using System.Runtime.Versioning;
[assembly: SupportedOSPlatform("windows")]

namespace MexInternals;

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Plisky.Diagnostics.FlimFlam;
using Plisky.FlimFlam;
using Plisky.Plumbing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal static class Program {

    public static IHost host;
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main(string[] args) {
        bool setLogOptions = false;

        foreach (var l in args) {
            if (l == "/logall") {
                setLogOptions = true;
                break;
            }
        }

        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddSingleton<OdsProcessGatherer>();
        builder.Services.AddSingleton<IncomingMessageManager2>();

        host = builder.Build();


        var hcfp = new FeatureHardCodedProvider();
        hcfp.AddFeature(new Feature("Bilge-ImportChain", true));
        hcfp.AddFeature(new Feature("Bilge-OdsOOP", true));  // Output Debug string Out Of Process

        Feature.AddProvider(hcfp);

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);


        IncomingMessageManager.Current = host.Services.GetRequiredService<IncomingMessageManager2>();

        if (args.Length > 0) {
            if (args[0] == "/log") {
                // Added this in for logging MexPlus data.
                setLogOptions = true;
            }
        }

        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

        var f = new frmMexMainView();
        f.LoadViewerConfigurationData();

        //Bilge.VerboseLog("Options Loaded ok, now going to refresh the filter list for the main screen");
        f.RefreshQuickFilterList();
        //Bilge.VerboseLog("RefreshQuickFilterList completes ok, moving on to loading defaults");
        frmMexMainView.LoadDefaultFiltersAndHighlights();

        //Bilge.VerboseLog("All options and profiles are loaded, running application");

        if (setLogOptions) {
            //Bilge.VerboseLog("Detailed logging is active");
            MexCore.TheCore.Options.PersistEverything = true;
            string filename = string.Format("MexTrace_Msgs_{0}_{1}_on_{2}_{3}_{4}.csv", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
            MexCore.TheCore.Options.CurrentFilename = Path.Combine(Path.GetTempPath(), filename);
        }

        RetrieveUserOptionalSettings();


        
        try {
            Application.Run(f);
        } catch (Exception ex) {
            //Bilge.Dump(ex, "Exception running MexViewer Application");
            Utility.LogExceptionToTempFile("MexCore - Main Loop - Crash.", ex);
            throw;
        }

    }

    private static void RetrieveUserOptionalSettings() {
        ConfigHub.Current.AddDirectoryFallbackProvider("%PLISKYAPPROOT%\\config", "flimflam.donotcommit");

        MexCore.TheCore.Options.UserDefaults.Add("pubsub-projectid", ConfigHub.Current.GetSetting("pubsub-projectid"));
        MexCore.TheCore.Options.UserDefaults.Add("pubsub-sinkname", ConfigHub.Current.GetSetting("pubsub-sinkname"));
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
        var ex = e.ExceptionObject as Exception;
        Utility.LogExceptionToTempFile("MexCore - Main Loop - Crash.", ex, "cax");
    }

    private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
        Utility.LogExceptionToTempFile("MexCore - Main Loop - Crash.", e?.Exception, "atx");
        _ = MessageBox.Show(e.Exception.Message + " " + e.Exception.StackTrace);
    }
}