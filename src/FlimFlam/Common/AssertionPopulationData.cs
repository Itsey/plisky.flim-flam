namespace Plisky.Plumbing;

/// <summary>
/// This class holds all of the information that is required to populate the Assertion dialog correctly, and is used before an assertion is
/// displayed to the screen.
/// </summary>
internal class AssertionPopulationData {
    internal string m_errorText;
    internal string m_line;
    internal string m_machineid;
    internal string m_moduleName;
    internal string m_morelocinfo;
    internal string m_netThreadId;
    internal string m_processid;
    internal string m_stacktrace;
    internal string m_threadid;

    /// <summary>
    /// The text associated with the assertion failure.
    /// </summary>
    internal string ErrorText {
        get { return m_errorText; }
        set { m_errorText = value; }
    }

    /// <summary>
    /// The line number
    /// </summary>
    internal string Line {
        get { return m_line; }
        set { m_line = value; }
    }

    /// <summary>
    /// An identifier used to represent the machine
    /// </summary>
    internal string MachineId {
        get { return m_machineid; }
        set { m_machineid = value; }
    }

    /// <summary>
    ///  The name of the module
    /// </summary>
    internal string ModuleName {
        get { return m_moduleName; }
        set { m_moduleName = value; }
    }

    /// <summary>
    /// Further inforamtion describing the location
    /// </summary>
    internal string Morelocinfo {
        get { return m_morelocinfo; }
        set { m_morelocinfo = value; }
    }

    /// <summary>
    /// The numeric identifer of the .net thread
    /// </summary>
    internal string NetThreadId {
        get { return m_netThreadId; }
        set { m_netThreadId = value; }
    }

    /// <summary>
    /// The numeric process ID
    /// </summary>
    internal string ProcessId {
        get { return m_processid; }
        set { m_processid = value; }
    }

    /// <summary>
    /// A friendly name describing the process.
    /// </summary>
    internal string ProcessName { get; set; }

    /// <summary>
    /// A detailed stack trace of the assertion location.
    /// </summary>
    internal string Stacktrace {
        get { return m_stacktrace; }
        set { m_stacktrace = value; }
    }

    /// <summary>
    /// The numeric identifier of the OS thread
    /// </summary>
    internal string Threadid {
        get { return m_threadid; }
        set { m_threadid = value; }
    }
}