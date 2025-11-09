using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Plisky.Plumbing {

    /// <summary>
    /// The settings class contains the settings that are used to control the way that the trace is written, this can be dynamically changed
    /// or configured externally to the application through one of the initialisation routes.
    /// </summary>
    internal class Settings {

        /// <summary>
        /// The name of the application configuration setting which is used to determine whether static initialisation should be bypassed
        /// to allow the application to control all of the initialisation.  Any value other than "False" will bypass the static configuration.
        /// </summary>
        internal const string APPCONFIGBYPASS = "TEXSTATICBYPASS";

        /// <summary>
        /// The name of the setting value within App Config which contains a settings configuration string.
        /// </summary>
        internal const string APPCONFIGNAME = "TEXSETTINGS";

        /// <summary>
        /// The name of the system or user environment variable that is used to read configuration settings from. This environment variable
        /// </summary>
        internal const string ENVIRONMENTVARIABLENAME = "TEXINIT";

        /// <summary>
        /// The identifier used in the configuration file to determine what level should  be used for internal event log based logging.
        /// </summary>
        internal const string INTERNALLOGLEVELNAME = "INTERNALLOGLEVEL";

        private readonly List<string> listenersToAdd;

        /// <summary>
        /// This will create the settings class and then intialise it with default settings for high performance tracing
        /// that is off in production.  The default settings are trace leve off, high performance on and no stack information
        /// This method will also try to identify the main application and window names.
        /// </summary>
        internal Settings() {
            listenersToAdd = [];
            CurrentTraceLevel = TraceLevel.Off;

            QueueMessages = true;
            AddStackInformation = false;
            ClearListenersOnStartup = true;

            try {
                var thisProc = Process.GetCurrentProcess();

                ProcessName = thisProc.ProcessName;
                MachineName = Environment.MachineName;
                MainModule = thisProc.MainModule.FileName;

                WindowTitle = thisProc.MainWindowHandle != IntPtr.Zero ? thisProc.MainWindowTitle : "No Window";
            } catch (InvalidOperationException /*ex*/) {
                // Access denied when trying to retrieve the process ID - probably IIS Hosted
                ProcessName = "UnknownProcess";
                MachineName = "UnknownMachine";
                MainModule = "NoModuleInfo";
                WindowTitle = "UnknownWindow";
            }
        }

        /// <summary>
        ///  Gets or Sets an indicator to determine whether stack information should be added to each trace call. Stack informtion is slow to execute therefore disable
        ///  this if you are concerned about performance.
        /// </summary>
        internal bool AddStackInformation {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the alternative name for the process which is running, this is passed in and allows an application to tell the viewer
        /// to use an alternative, more descriptive name for the proces.
        /// </summary>
        internal string AlternativeName { get; set; }

        /// <summary>
        /// Gets or Sets the Clear Listeners Property.  If this property is true the entire list of listeners is cleared before any specific listeners are added to the collection,
        /// this is the only way to remove listeners with an initialisation
        /// </summary>
        internal bool ClearListenersOnStartup { get; set; }

        /// <summary>
        /// Gets or Sets the default trace level.  This affects what level of output is sent to the trace streams.
        /// </summary>
        internal TraceLevel CurrentTraceLevel { get; set; }

        /// <summary>
        /// Gets or sets a value to determine whether the enhancements are supported such as allowing custom replacements to occur in the string
        /// or embedding the timing information in strings.
        /// </summary>
        internal bool EnableEnhancements { get; set; }

        internal List<string> ImporterPathsToCheck { get; set; } = [];

        /// <summary>
        /// Maintains a list of the listeners that are to be added, these should ideally be custom listeners
        /// </summary>
        internal string[] ListenersToAdd => listenersToAdd.ToArray();

        /// <summary>
        /// Gets or Sets a machine name that is the hosting machine. This can be overriden and is only really used when combining
        /// traces from multiple machines.
        /// </summary>
        internal string MachineName {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the name of the main module, allowing you to override the settings found during initialisation.
        /// </summary>
        internal string MainModule { get; set; }

        /// <summary>
        /// Gets or Sets the process name.  Specifies the friendly name that descrives this process.  If this is not overriden then it is read from the executable
        /// name of the application itself.
        /// </summary>
        internal string ProcessName { get; set; }

        /// <summary>
        /// Gets or Sets a limit on the number of messages that are stored within the internal queue.  Messages above this limit are discarded and never written
        /// to the trace stream.
        /// </summary>
        /// <remarks>When this is set to -1 (default) no messages are discarded</remarks>
        internal int QueueDepthLimit { get; set; }

        /// <summary>
        /// Gets or Sets a value indicating whether messages are queued internally before being written to the trace.  This will almost entirely
        /// eliminate the overhead of the listeners collection but will require additional memory and cpu resources to process the queue.
        /// </summary>
        /// <remarks>When used without a queue limit and with WriteOnFail set to false the behaviour is the same as V 2.x HighPerformanceMode.</remarks>
        internal bool QueueMessages { get; set; }

        /// <summary>
        /// Gets or Sets the value of the title of the window.  Overriding this replaces the text the window title from the windows API.
        /// </summary>
        internal string WindowTitle { get; set; }

        /// <summary>
        /// Gets or Sets the mode where messages are only written to the trace when an error, or exception or a call to WriteStoredMessages is called.
        /// </summary>
        internal bool WriteOnlyOnFail { get; set; }

        /// <summary>
        /// Returns the Settings as a stirng initialisation value.
        /// </summary>
        /// <returns>A string representing the initialisation settings.</returns>
        public override string ToString() {
            return CreateString();
        }

        /// <summary>
        /// Add a listener to the list of listeners that will be added on startup
        /// </summary>
        /// <param name="s">The string containing listener data</param>
        internal void AddListener(string s) {
            if ((s == null) || (s.Length == 0)) { throw new ArgumentException("The listener content can not be null or empty", "s"); }
            listenersToAdd.Add(s);
        }

        /// <summary>
        /// Remove allqueued listeners from the list of listeners.
        /// </summary>
        internal void ClearAddedListeners() {
            listenersToAdd.Clear();
        }

        /// <summary>
        /// Creates a settings configuration string. All of the configuration is stored and used in this way.
        /// </summary>
        /// <remarks>
        /// The string format is [TLV:level][OPT:(opt1)(opt2)...][LST:(list1)(list2)...]
        /// </remarks>
        /// <returns>The string representing the options in this instance.</returns>
        internal string CreateString() {
            string trcLvl = CurrentTraceLevel.ToString().ToUpper();
            string listeners = string.Empty;
            string options = string.Empty;

            if (ListenersToAdd.Length > 0) {
                foreach (string s in ListenersToAdd) {
                    listeners += "(" + s + ")";
                }
                listeners = string.Format("[LST:{0}]", listeners);
            }

            if (ClearListenersOnStartup) {
                options += "(CLL:Y)";
            } else {
                options += "(CLL:N)";
            }
            if (AddStackInformation) {
                options += "(STK:Y)";
            } else {
                options += "(STK:N)";
            }
            if (QueueMessages) {
                options += "(QMS:Y)";
            } else {
                options += "(QMS:N)";
            }
            if (EnableEnhancements) {
                options += "(EEH:Y)";
            } else {
                options += "(EEH:N)";
            }
            if (WriteOnlyOnFail) {
                options += "(WOF:Y)";
            } else {
                options += "(WOF:N)";
            }

            return string.Format("[TLV:{0}][OPT:{1}]{2}", trcLvl, options, listeners);
        }

        /// <summary>
        /// Populates this InitSettings class with the values in the string which should be a well formed initialisation string
        /// </summary>
        /// <param name="initialisationData">The well formed initialisation string.</param>
        /// <exception cref="System.ArgumentException">Thrown when the string was of an invalid format</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when an internal error occurs such as when the string changes type.</exception>
        internal void PopulateFromString(string initialisationData) {
            bool configErrorOccured = false;

            if ((initialisationData == null) || (initialisationData.Length == 0)) { return; }
            if (!initialisationData.StartsWith("[TLV:")) {
                throw new ArgumentException("PopulateFromString called on the wrong format string");
            }
            var r = new Regex(@"\[[TLVOPS]{3}:[A-Za-z0-9;,.:()]+\]");
            var m = r.Match(initialisationData);
            if (m == null) { throw new ArgumentException("No match possible on EnvironmentVariable, likely corrupt or invalid environment variable"); }
#if DEBUG
            int consumed = 0;
#endif
            while ((m != null) && (m.Value.Length > 0)) {
#if DEBUG
                consumed += m.Value.Length;
#endif
                try {
                    string dataSection = m.Captures[0].Value[1..^1];
                    string startTag = dataSection[..4];
                    string data = dataSection[4..];

                    switch (startTag) {
                        case "TLV:":
                            SetTraceLevelFromData(data);
                            break;

                        case "OPT:":
                            SetOptionsFromData(data);
                            break;

                        case "LST:":
                            SetListenersFromData(data);
                            break;

                        default:
                            InternalUtil.LogInternalError(InternalUtil.InternalErrorCodes.CorruptConfiguration, "Invalid Environment Section Detected: " + startTag, TraceLevel.Warning);
                            break;
                    }
                } catch (IndexOutOfRangeException ioxr) {
                    InternalUtil.LogInternalError(InternalUtil.InternalErrorCodes.CorruptConfiguration, "Invalid Data In One of the sections. " + ioxr.Message, TraceLevel.Warning);
                } catch (ArgumentException ax) {
                    InternalUtil.LogInternalError(InternalUtil.InternalErrorCodes.CorruptConfiguration, "Invalid Content In a Configuration Section. " + ax.Message, TraceLevel.Warning);
                }
                m = m.NextMatch();
            }
#if DEBUG
            if (consumed != initialisationData.Length) {
                throw new InvalidOperationException("BUG >> Not all of the environment configuration was consumed, there is lost data :" + consumed.ToString() + " from " + initialisationData);
            }
#endif
            if (configErrorOccured) {
                InternalUtil.LogInternalError(InternalUtil.InternalErrorCodes.CorruptConfiguration, "There was corrupting in the configuration that was requested", TraceLevel.Error);
            }
        }

        internal void ResetForConfigurationTool() {
            ProcessName = string.Empty;
            MachineName = string.Empty;
        }

        private static bool StringValueToBool(string p) {
            if ((p == null) || (p.Length == 0)) { return false; }

            if (p == "Y") { return true; }

            return bool.TryParse(p, out bool result) && result;
        }

        private void SetListenersFromData(string data) {
            if (data == null) { return; }
            int nextIndex;
            listenersToAdd.Clear();

            while (data.Length > 0) {
                if (!data.StartsWith("(")) {
                    // Corrupt string.
                    nextIndex = data.IndexOf("(");
                    data = data[nextIndex..];
                }

                int optionCloseOffset = data.IndexOf(')');
                string nextListener = data[1..optionCloseOffset];
                data = data[(optionCloseOffset + 1)..];
                listenersToAdd.Add(nextListener);
            }
        }

        private void SetOptionsFromData(string data) {
            // Data contains a series of () based options.
            if ((data == null) || (data.Length == 0)) { throw new ArgumentException("Options Data String was Null or empty.", "data"); }
            int nextIndex;

            while (data.Length > 0) {
                if (!data.StartsWith("(")) {
                    // Corrupt string.
                    nextIndex = data.IndexOf("(");
                    if (nextIndex == -1) {
                        // No data was found, this was not the right type of data.
                        throw new ArgumentException("The options data was corrupt.  There was no opening bracket ( found within the data");
                    }
                    data = data[nextIndex..];
                }

                int optionCloseOffset = data.IndexOf(')');
                if (optionCloseOffset == -1) {
                    throw new ArgumentException("The options data was corrupt.  There was no closing bracket ) found within the data");
                }

                string nextOption = data[1..optionCloseOffset].ToUpper();
                data = data[(optionCloseOffset + 1)..];

                if (nextOption.Contains(":")) {
                    // OK String
                    string[] parts = nextOption.Split(':');
                    bool optionValue = Settings.StringValueToBool(parts[1]);

                    switch (parts[0]) {
                        case "QMS":  // QMS has replaced HPF.
                        case "HPF":
                            QueueMessages = optionValue;
                            break;

                        case "STK":
                            AddStackInformation = optionValue;
                            break;

                        case "EEH":
                            EnableEnhancements = optionValue;
                            break;

                        case "CLL":
                            ClearListenersOnStartup = optionValue;
                            break;

                        case "WOF":
                            WriteOnlyOnFail = optionValue;
                            break;

                        default:
                            InternalUtil.LogInternalError(InternalUtil.InternalErrorCodes.CorruptConfiguration, "There was an unknown option specified: " + parts[0], TraceLevel.Warning);
                            break;
                    }
                } else {
                    InternalUtil.LogInternalError(InternalUtil.InternalErrorCodes.CorruptConfiguration, "Unknown Option Value: " + nextOption, TraceLevel.Warning);
                }
            }
        }

        private void SetTraceLevelFromData(string data) {
            if ((data == null) || (data.Length == 0)) { return; }
            try {
                CurrentTraceLevel = (TraceLevel)Enum.Parse(typeof(TraceLevel), data, true);
            } catch (ArgumentException) {
                // Enum value invalid.
            }
        }
    }
}