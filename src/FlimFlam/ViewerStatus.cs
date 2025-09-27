//using Plisky.Plumbing.Legacy;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Plisky.FlimFlam { 
    // Class must be public for serialization into the isolated storage streams even though it is not used outside of this
    // assembly.

    /// <summary>
    /// Holds the current status for the Mex viewer which is serialised and loaded in with the restart of the application.
    /// </summary>
    [Serializable]
    public class MexStatus {
        // If ya change this remember to change the Populate From me entries.

        /// <summary>
        /// Left hand position on screen
        /// </summary>

        private string[] fileMRUList = new string[0];
        private int height;
        private string lastLoadedFile;
        private int width;
        private int xLoc;

        private int yLoc;

        /// <summary>
        /// recent imported log files
        /// </summary>
        public string[] FileMostRecentlyUsedList {
            get { return fileMRUList; }
            set { fileMRUList = value; }
        }

        /// <summary>
        /// height of mex
        /// </summary>
        public int Height {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// last one of these to be used
        /// </summary>
        public string LastLoadedFile {
            get { return lastLoadedFile; }
            set { lastLoadedFile = value; }
        }

        /// <summary>
        ///  Holds the heightn of the lower panel in the extended details screen
        /// </summary>
        public int MoreDetailsBottomPanelHeight {
            get;
            set;
        }

        /// <summary>
        /// Holds the total height of the extended details screen
        /// </summary>
        public int MoreDetailsHeight {
            get;
            set;
        }

        /// <summary>
        /// Holds the height of the upper panel in the extended details screen
        /// </summary>
        public int MoreDetailsTopPanelHeight {
            get;
            set;
        }

        /// <summary>
        /// Holds the total width of the extended details screen
        /// </summary>
        public int MoreDetailsWidth {
            get;
            set;
        }

        /// <summary>
        /// column widths debug message
        /// </summary>
        public int ProcessViewDebugMessageColumnWidth {
            get;
            set;
        }

        /// <summary>
        /// column widths index
        /// </summary>
        public int ProcessViewIndexColumnWidth {
            get;
            set;
        }

        /// <summary>
        /// column widths location ident
        /// </summary>
        public int ProcessViewLocationColumnWidth {
            get;
            set;
        }

        /// <summary>
        /// column widths physical loc
        /// </summary>
        public int ProcessViewPhysicalLocationColumnWidth {
            get;
            set;
        }

        // A value or -1 if not visible
        /// <summary>
        /// column widths thread
        /// </summary>
        public int ProcessViewThreadColumnWidth {
            get;
            set;
        }

        /// <summary>
        /// width of mex
        /// </summary>
        public int Width {
            get { return width; }
            set { width = value; }
        }

        public int XLoc {
            get { return xLoc; }
            set { xLoc = value; }
        }

        /// <summary>
        /// Top position on screen
        /// </summary>
        public int YLoc {
            get { return yLoc; }
            set { yLoc = value; }
        }

        /// <summary>
        /// Adds a most recently used file to the list of most recently used import files.  This checks that there isnt already
        /// one in the list and resizes the array to handle the new file.
        /// </summary>
        /// <param name="newFile">The name of the file to add to the list</param>
        public void AddMostRecentlyUsedFile(string newFile) {
            string[] newList = new string[FileMostRecentlyUsedList.Length + 1];
            Array.Copy(FileMostRecentlyUsedList, newList, FileMostRecentlyUsedList.Length);
            newList[FileMostRecentlyUsedList.Length] = newFile;  // Zero based therefore its back to the lenght of the original array for the new entry
            FileMostRecentlyUsedList = newList;
        }

        /// <summary>
        /// Initialises the status class with sensible values.
        /// </summary>
        public void LoadDefaultStatus() {
            //Bilge.E();
            try {
                //Bilge.Log("Loading default status values for Mex");

                LastLoadedFile = string.Empty;
                FileMostRecentlyUsedList = new string[0];
                XLoc = 50;
                YLoc = 50;
                Width = 800;
                Height = 545;

                MoreDetailsWidth = 682;
                MoreDetailsHeight = 387;

                ProcessViewIndexColumnWidth = 60;
                ProcessViewThreadColumnWidth = 60;
                ProcessViewLocationColumnWidth = 160;
                ProcessViewDebugMessageColumnWidth = 500;
                ProcessViewPhysicalLocationColumnWidth = -1;
            } finally {
                //Bilge.X();
            }
        }

        /// <summary>
        /// Loads the viewer status values from a saved version of the viewer status,  This allows settings to persist
        /// across sessions if the user wishes.
        /// </summary>
        /// <param name="storeStream">The stream from which to read the settings</param>
        public void LoadViewerStatus(Stream storeStream) {
            //Bilge.E();
            try {
                XmlSerializer xmls = new XmlSerializer(typeof(MexStatus));
                MexStatus temp = (MexStatus)xmls.Deserialize(storeStream);
                if ((temp.XLoc < 0) || (temp.YLoc < 0) || (temp.XLoc > System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width) || (temp.YLoc > System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height)) {
                    // TODO : Eliminate the cause for this.
                    //Bilge.Warning("INVALID settings for the size of Mex being loaded, the screen would not be visible. Resetting to defaults.  WHY is this occuring!?");
                    //Bilge.Dump(temp, "Invalid MexStatus detected.");
                    this.LoadDefaultStatus();
                    //Bilge.Warning("Invalid settings caused defaults to be loaded, user settings have been lost");
                } else {
                    this.PopulateFromMe(temp);
                }
            } finally {
                //Bilge.X();
            }
        }

        /// <summary>
        /// Saves the viewer status to a stream so that it can be reloaded in the future.
        /// </summary>
        /// <param name="storeStream">The stream to write the status out into</param>
        public void SaveViewerStatus(Stream storeStream) {
            //Bilge.E();
            try {
                if ((this.XLoc < 0) || (this.YLoc < 0) || (this.XLoc > System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width) || (this.YLoc > System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height)) {
                    //Bilge.Warning("SaveViewerStatus, saving data that will corrupt the display of the process next time it starts.  This is not right, how did we get like this");
                    //Bilge.Warning("Not saving user preferences");
                } else {
                    XmlSerializer xmls = new XmlSerializer(typeof(MexStatus));
                    xmls.Serialize(storeStream, this);
                }
            } finally {
                //Bilge.X();
            }
        }

        private void PopulateFromMe(MexStatus ms) {
            this.XLoc = ms.XLoc;
            this.YLoc = ms.YLoc;
            this.Width = ms.Width;
            this.Height = ms.Height;

            if (ms.FileMostRecentlyUsedList == null) {
                this.FileMostRecentlyUsedList = new string[0];
            } else {
                this.FileMostRecentlyUsedList = new string[ms.FileMostRecentlyUsedList.Length];
                Array.Copy(ms.FileMostRecentlyUsedList, this.FileMostRecentlyUsedList, ms.FileMostRecentlyUsedList.Length);
            }
            if (ms.LastLoadedFile == null) {
                this.LastLoadedFile = string.Empty;
            } else {
                this.LastLoadedFile = ms.LastLoadedFile;
            }

            this.ProcessViewIndexColumnWidth = ms.ProcessViewIndexColumnWidth;
            this.ProcessViewThreadColumnWidth = ms.ProcessViewThreadColumnWidth;
            this.ProcessViewLocationColumnWidth = ms.ProcessViewLocationColumnWidth;
            this.ProcessViewDebugMessageColumnWidth = ms.ProcessViewDebugMessageColumnWidth;
            this.ProcessViewPhysicalLocationColumnWidth = ms.ProcessViewPhysicalLocationColumnWidth;

            this.MoreDetailsTopPanelHeight = ms.MoreDetailsTopPanelHeight;
            this.MoreDetailsBottomPanelHeight = ms.MoreDetailsBottomPanelHeight;

            this.MoreDetailsWidth = ms.MoreDetailsWidth;
            this.MoreDetailsHeight = ms.MoreDetailsHeight;
        }
    }
}