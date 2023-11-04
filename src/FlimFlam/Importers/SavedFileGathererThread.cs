//using Plisky.Plumbing.Legacy;
using System;
using System.IO;
using Plisky.Diagnostics.FlimFlam;
using Plisky.Flimflam;

namespace Plisky.FlimFlam { 

    /// <summary>
    /// Summary description for SavedFileGathererThread.
    /// </summary>
    internal class SavedFileGatherer {

        private SavedFileGatherer() {
            // static only class
        }

        private delegate void LoadFileDelegate(string fileName, FileImportMethod style);

        internal static void LoadFromFileAsynch(string fileName, FileImportMethod style) {
            if (!File.Exists(fileName)) { return; }
            try {
                LoadFileDelegate d = new LoadFileDelegate(ReadMessagesFromFile);
                // TODO : FireAndForget support required so we dont leak like a bottomer.
                d.BeginInvoke(fileName, style, null, null);
                // We just let this happen in the background and dissapear when its done.
            } catch (IOException ex) {
                // OK what do we do about errors ?
                //Bilge.Dump(ex, "Error occured during LoadFromFileAsynch method");
                MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.ErrorDuringfileImport, UserMessageType.ErrorMessage, null);
            }
        }

        private static void ReadMessagesFromFile(string fileName, FileImportMethod style) {
            MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.BackgroundFileImportBegins, UserMessageType.InformationMessage, null);
            string nextLine;
            string usedFname;

            usedFname = fileName;

            if (style == FileImportMethod.ADPlusLog) {
                // Preparse the ADPLUS log replacing the timezone offset with CRLF

                //Bilge.Log("Starting preprocessing of ADPLUS log " + fileName);

                string splitTag;

                StreamReader sr = new StreamReader(fileName);
                try {
                    while (true) {
                        nextLine = sr.ReadLine();

                        if (nextLine == null) {
                            // TODO : Throwing an excpetion here is ugly.
                            return;
                        }

                        if (nextLine.StartsWith(MexCore.TheCore.Options.ADPlusImportIdentifierToSplitTags)) {
                            splitTag = nextLine.Substring(MexCore.TheCore.Options.ADPlusImportIdentifierToSplitTags.Length);
                            splitTag = splitTag.Substring((splitTag.Length - 7), 7);  // Lame
                            splitTag += ": {[";
                            // This should hold the date / time that the log was created. Assumes that this is fixed for the length of the log
                            // TODO : Assumes constant date time for the whole file
                            break;
                        }
                    }

                    // if we get here splitTag  is valid.
                    usedFname = Path.GetTempFileName();
                    StreamWriter sw = new StreamWriter(usedFname);
                    try {
                        while (nextLine != null) {
                            nextLine = sr.ReadLine();
                            if (nextLine == null) { break; }

                            if (nextLine.IndexOf(splitTag) >= 0) {  // Look for splatted Tex Logs
                                nextLine = nextLine.Replace(splitTag, Environment.NewLine + "{[");
                            }
                            sw.WriteLine(nextLine);
                        }
                    } finally {
                        sw.Close();
                    }
                } finally {
                    sr.Close();
                }
            }

            StreamReader srMain = new StreamReader(usedFname);
            try {
                // TODO : Could add a SuspendQueueChecks job here so that the import completes then the refresh is performed
                //Bilge.Log("Starting Asynch import of " + usedFname);

                nextLine = srMain.ReadLine();

                while (nextLine != null) {
                    string theLine = nextLine;
                    var nlc = srMain.ReadLine();
                    if (nlc == null) { break; }
                    nextLine = nlc;

                    switch (style) {
                        case FileImportMethod.TextWriterWithTexSupport: {
                                // TextWriterWithTex assumes CRLFs are accidental and it should be one tex line

                                while ((nextLine != null) && (!nextLine.StartsWith("{["))) {
                                    theLine += nextLine;
                                    nextLine = srMain.ReadLine();
                                }
                            }
                            break; // End case where we assume tex output

                        case FileImportMethod.DebugViewTexLog: {
                                if (nextLine.StartsWith("[\\")) {
                                    // The first line of the log tells you where its from we skip this line
                                    continue;
                                }
                                int offs = nextLine.IndexOf("{[");
                                if (offs > 0) {
                                    nextLine = nextLine.Substring(offs);
                                }
                                theLine = nextLine;
                            }
                            break;

                        case FileImportMethod.ADPlusLog: {
                                // ADPLUS Log assumes CRLFs are other lines not Tex related and it skips them

                                while ((nextLine != null) && (!nextLine.StartsWith("{["))) {
                                    nextLine = srMain.ReadLine();
                                }

                                theLine = nextLine;
                            }
                            break; // end case where were studiying ADPlus
                    } // end switch

                    MexCore.TheCore.MessageManager.AddIncomingMessage(InternalSource.FileImport, theLine, -1);
                } // End while there are more lines in the file
                MexCore.TheCore.MessageManager.AddIncomingMessage(InternalSource.FileImport, nextLine, -1);
                MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.BackgroundfileImportEnds, UserMessageType.InformationMessage, null);
            } finally {
                srMain.Close();
            }
        }
    }
}