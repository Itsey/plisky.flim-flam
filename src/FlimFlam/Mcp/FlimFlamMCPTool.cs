namespace Plisky.FlimFlam;

using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using ModelContextProtocol.Server;

[McpServerToolType]
[Description("Tools that return the trace that has been sent to the flim flam viewer from Bilge.")]
public class FlimFlamTraceTool {

    public FlimFlamTraceTool() {

    }
    [McpServerTool(Title = "Purge Trace Entries")]
    [Description("""
        This will run a purge activity in the trace viewer application.  If a user requests to purge, clear or partially purge trace data then this is the tool that should be used.  The execute purge method
        will remove trace data. If there is no application name specified as the parameter then the currently selected application is purged. If there is an application name passed then that will be purged
        instead.      
        """)]
    public Task<string> ExecutePurge(
        [Description("The application name passed in if the user wants to purge trace from one application.  It is optional and if left out then the default application will be purged.")]
        string applicationName="selected_app") {
        int oneToDo = MexCore.TheCore.ViewManager.SelectedTracedAppIdx;
        MexCore.TheCore.WorkManager.ProcessJob(new Job_PartialPurgeApp(oneToDo));
        return Task.FromResult("Application Purged");
    }



    [McpServerTool(Title = "Find Trace")]
    [Description("Finds a specific trace entry using the string provided.")]
    public Task<List<string>> FindTrace(string traceToFind) {


        List<string> trace = new List<string>();
        trace.Add("is yer uncle");
        trace.Add("is yer auntie");
        return Task.FromResult(trace);
    }
}

