namespace Plisky.FlimFlam;

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using MexInternals;
using Microsoft.Extensions.DependencyInjection;
using Plisky.Diagnostics.FlimFlam;
using Plisky.FilmFlam;

/// <summary>
/// Summary description for Core.
/// </summary>
public class MexCore {
    private readonly Thread coreExecutionThread;

    private volatile bool continueRunning = true;

    static MexCore() {
        //Bilge.Log("MexCore, Static Constructor :: Core Called for the first time  - bringing MexCore online");
        TheCore = new MexCore();
        TheCore.InitialiseCore();
    }

    private MexCore() {
        //Bilge.Log("MexCore::MexCore - Core being initialised");

        var esf = new EventEntryStoreFactory();
        var dm = new DataManager(esf);
        var store = new OriginIdentityStore();
        var im = new ImportManager(null);

        Diagnostics = new MexDiagnosticManager();
        Options = new MexOptions();
        WorkManager = PrimaryWorkManager.GetPrimaryWorkManager();
        DataManager = new DataStructureManager();
        MessageManager = IncomingMessageManager.Current;
        ViewManager = Program.host.Services.GetRequiredService<ViewSupportManager2>();
        CacheManager = new CacheSupportManager();

        coreExecutionThread = new Thread(new ThreadStart(CoreThreadLoop)) {
            Name = "MexCoreThread"
        };

    }

    public MexOptions Options { get; set; }

    internal static MexCore TheCore { get; set; } = null;
    internal CacheSupportManager CacheManager { get; set; }

    internal bool CoreShutdownRequested {
        get { return !continueRunning; }
    }

    internal DataStructureManager DataManager { get; set; }

    internal MexDiagnosticManager Diagnostics { get; set; }

    internal IncomingMessageManager MessageManager { get; set; }

    internal ViewSupportManager ViewManager { get; set; }

    internal PrimaryWorkManager WorkManager { get; set; }

    public void LogEveryting(string context, string message) {
        if (MexCore.TheCore.Options.PersistEverything) {
            try {
                //string when = string.Format("{0}:{1}:{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                //string[] strings = new string[] { when, context, Environment.NewLine, message };
                if (message.EndsWith(Environment.NewLine)) {
                    message = message.Substring(0, message.Length - Environment.NewLine.Length);
                }
                File.AppendAllLines(MexCore.TheCore.Options.CurrentFilename, new string[] { message });
            } catch {
            }
        }
    }

    internal string DiagnosticsText() {
        return "Core Execution Thread : Alive(" + coreExecutionThread.IsAlive.ToString() + ")" + coreExecutionThread.ThreadState.ToString();
    }

    internal void PokeCoreThread() {
        //Bilge.Log("MexCore::Core::PokeCorethread called");
        if (coreExecutionThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin) {
            //Bilge.Log("MexCore::Core::PokeCoreThread -> Core thread caught sleeping on the job and woken up....");
            coreExecutionThread.Interrupt();
        }
    }

    /// <summary>
    /// Called when a request to shut the application down is made - this should nicely shut down all of our applications
    /// </summary>
    internal void ShutDownCore() {
        continueRunning = false;
        WorkManager.ShutDown();
        DataManager.ShutDown();
        MessageManager.ShutDown();
        ViewManager.ShutDown();
        Diagnostics.ShutDown();
        CacheManager.Shutdown();
    }

    private void CoreThreadLoop() {
        int idleCount = 0;

        //Bilge.Log("MexCore::CoreThreadLoop is initialised");
        while (continueRunning) {
            try {
                // Autorefresh has been changed to be timer based rather than instance based.

                if (WorkManager.WorkOutstanding) {
                    WorkManager.DoWork();
                } else {
                    // there was nothing for this thread to do
                    // todo : look up background housekeeping tasks to do

                    // First house keeping task, check the incomming message queue and see if there stuff waiting to be processed.
                    if (MexCore.TheCore.MessageManager.IncomingMessagesQueued) {
                        // The message manager has messages queued, therefore we need to add a task to process these.
                        MexCore.TheCore.WorkManager.AddJob(new Job_CheckIncommingQueue());
                        continue;
                    }

                    // Ok done all housekeeping tasks, now return to messing around
                    Thread.Sleep(0);
                    idleCount++;
                    if (idleCount == 100) {
                        idleCount = 0;
                        //Bilge.VerboseLog("MexCore::CoreThreadLoop - nothing to do - sleeping 10 secs");
                        MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.MexIsIdle, UserMessageType.InformationMessage, "ZzzzZZzzzZ");
                        // At this point the app hasnt recieved a message in a while and is therefore this thread is going to go to sleep.
                        for (int killTime = 0; killTime < 1000; killTime++) {
                            if (WorkManager.WorkOutstanding || (!continueRunning) || MexCore.TheCore.MessageManager.IncomingMessagesQueued) {
                                //Bilge.Log("MexCore::CoreThreadLoop - Awoken, work has arrived");
                                break;
                            }
                            Thread.Sleep(10);
                        }
                        //Bilge.VerboseLog("MexCore::CoreThreadLoop - Sleep loop ended (Check whether awoken occured in last log entry).");
                    }
                }
            } catch (Exception ex) {
#if DEBUG
                MexCore.TheCore.LogEveryting("MexCrash", "MainLoop");
                Utility.LogExceptionToTempFile("MexCore - Main Loop - Crash.", ex);

                if (Debugger.IsAttached) {
                    Debugger.Break();
                    //Bilge.Log("Omg its all gone terribly wrong");
                }

#endif
                // There was an error during our work processing
                throw;
            }
        }
    }

    /// <summary>
    /// InitialiseCore is called after the core is constructed and adds some initial jobs to the core work manager que.  this is the first
    /// set of tasks that are performed after the mex viewer is created and ready to start work.
    /// </summary>
    private void InitialiseCore() {
        //Bilge.Log("Core online, performing initialisation job requests");
        // Place initialisation job requests in the queue.
        // TODO : DO I need this ? WorkManager.AddJob(new Job_ApplyOptionsForUser());

        coreExecutionThread.Start();
    }
}