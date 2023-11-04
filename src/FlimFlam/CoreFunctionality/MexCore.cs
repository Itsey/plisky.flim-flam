namespace Plisky.FlimFlam {
    using System;
    using System.Diagnostics;
    using System.Threading;
    using Plisky.Diagnostics.FlimFlam;
    using Plisky.FilmFlam;

    /// <summary>
    /// Summary description for Core.
    /// </summary>
    internal class MexCore {
        internal MexOptions Options;
        internal PrimaryWorkManager WorkManager;
        internal DataStructureManager DataManager;
        internal IncomingMessageManager MessageManager;
        internal ViewSupportManager ViewManager;
        internal MexDiagnosticManager Diagnostics;
        internal CacheSupportManager CacheManager;

        private readonly Thread m_coreExecutionThread;
        private volatile bool m_continueRunning = true;

        private void CoreThreadLoop() {
            int idleCount = 0;

            //Bilge.Log("MexCore::CoreThreadLoop is initialised");
            while (m_continueRunning) {
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
                                if (WorkManager.WorkOutstanding || (!m_continueRunning) || MexCore.TheCore.MessageManager.IncomingMessagesQueued) {
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

        internal bool CoreShutdownRequested {
            get { return !m_continueRunning; }
        }

        /// <summary>
        /// Called when a request to shut the application down is made - this should nicely shut down all of our applications
        /// </summary>
        internal void ShutDownCore() {
            m_continueRunning = false;
            WorkManager.ShutDown();
            DataManager.ShutDown();
            MessageManager.ShutDown();
            ViewManager.ShutDown();
            Diagnostics.ShutDown();
            CacheManager.Shutdown();
        }

        /// <summary>
        /// InitialiseCore is called after the core is constructed and adds some initial jobs to the core work manager que.  this is the first
        /// set of tasks that are performed after the mex viewer is created and ready to start work.
        /// </summary>
        private void InitialiseCore() {
            //Bilge.Log("Core online, performing initialisation job requests");
            // Place initialisation job requests in the queue.
            // TODO : DO I need this ? WorkManager.AddJob(new Job_ApplyOptionsForUser());

            m_coreExecutionThread.Start();
        }

        internal void PokeCoreThread() {
            //Bilge.Log("MexCore::Core::PokeCorethread called");
            if (m_coreExecutionThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin) {
                //Bilge.Log("MexCore::Core::PokeCoreThread -> Core thread caught sleeping on the job and woken up....");
                m_coreExecutionThread.Interrupt();
            }
        }

        #region constructor

        private MexCore() {
            //Bilge.Log("MexCore::MexCore - Core being initialised");

            var esf = new EventEntryStoreFactory();
            var dm = new DataManager(esf);
            var store = new OriginIdentityStore();
            // TODO : RawEntryParserChain rapc = RawEntryParserChain.CreateChain(RawEntryParserChain.ALL_LINK, store);
            // TODO : DataParser dp = new DataParser(store, rapc, dm);
            var im = new ImportManager(null);

            //Bilge.Log("MexCore::MexCore - Diagnostics being initialised");
            Diagnostics = new MexDiagnosticManager();

            //Bilge.Log("MexCore::MexCore - about to start Options");
            Options = new MexOptions();

            //Bilge.Log("MexCore::MexCore - about to start WorkManager");
            WorkManager = PrimaryWorkManager.GetPrimaryWorkManager();

            //Bilge.Log("MexCore::MexCore - about to start DataStructures");
            DataManager = new DataStructureManager();

            //Bilge.Log("MexCore::MexCore - about to start IncommingMessageManager");
            MessageManager = IncomingMessageManager.Current;
            //MessageManager.AddFlimFlamCompatibility(im);

            //Bilge.Log("MexCore::MexCore - about to start ViewManager");
            ViewManager = new ViewSupportManager();

            //Bilge.Log("MexCore::MexCore - about to start CacheManager");
            CacheManager = new CacheSupportManager();

            //Bilge.Log("MexCore::MexCore - Starting CoreWorkerThread");
            m_coreExecutionThread = new Thread(new ThreadStart(CoreThreadLoop)) {
                Name = "MexCoreThread"
            };

            //Bilge.Log("MexCore::MexCore - Construction complete");
        }

        #endregion constructor

        #region static singleton support methods

        internal static MexCore TheCore = null;

        static MexCore() {
            //Bilge.Log("MexCore, Static Constructor :: Core Called for the first time  - bringing MexCore online");
            TheCore = new MexCore();
            TheCore.InitialiseCore();
        }

        #endregion static singleton support methods

        internal string DiagnosticsText() {
            return "Core Execution Thread : Alive(" + m_coreExecutionThread.IsAlive.ToString() + ")" + m_coreExecutionThread.ThreadState.ToString();
        }
    }
}