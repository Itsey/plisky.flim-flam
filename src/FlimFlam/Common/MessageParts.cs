namespace FlimFlamUI.Common;

/// <summary>
/// Holds all of the parts that are used to create messages.
/// </summary>
public class MessageParts {
    public string AdditionalLocationData;
    public string ClassName;
    public string DebugMessage;
    public string lineNumber;
    public string MachineName;
    public string MessageType;
    public string MethodName;
    public string ModuleName;
    public bool mpRequiresReplace;
    public string netThreadId;
    public string osThreadId;
    public string ParameterInfo;

    // TODO : work out why this field causes a warning.
    public bool Prepend;

    public string ProcessId;
    public string SecondaryMessage;
    public bool TriggerRefresh;
    // is prepend enabled   Wierdly this IS assigned to but still a warning

    public MessageParts() {
        MachineName = ModuleName = ClassName = MethodName = lineNumber = osThreadId = netThreadId = MessageType = DebugMessage = SecondaryMessage = AdditionalLocationData = ProcessId = string.Empty;
    }
}