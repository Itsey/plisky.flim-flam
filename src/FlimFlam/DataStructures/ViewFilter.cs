namespace Plisky.FlimFlam {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Plisky.Diagnostics;

    /// <summary>
    /// Summary description for ViewFilter.
    /// </summary>
    [Serializable]
    public class ViewFilter {

        #region Data used to store the actual index settings

        /// <summary>
        /// The textual description of the filter to help describe it to the user
        /// </summary>

        private bool m_caseSensitive;
        private uint m_currentFilterIndex;
        private List<string> m_excludedLocationsClass;
        private List<string> m_excludedLocationsFull;
        private List<string> m_excludedModules;
        private List<KeyDisplayRepresentation> m_excludedThreads;
        private uint m_excludeEventsAboveThisIndex;
        private List<string> m_exclusionStrings;
        private string m_filterDescription;

        private bool m_filterMoreComplexThanType;

        private uint m_flagsForInclude;

        private List<string> m_inclusionStrings;

        private bool m_useIndexBasedFilters;

        /// <summary>
        /// Determines whether the inclusion and exclusion matching strings work in a case sensitive or insensitve way
        /// </summary>
        public bool CaseSensitive {
            get { return m_caseSensitive; }
            set { m_caseSensitive = value; }
        }

        public uint CurrentFilterIndex {
            get { return m_currentFilterIndex; }
            set { m_currentFilterIndex = value; }
        }

        public List<string> ExcludedLocationsClass {
            get { return m_excludedLocationsClass; }
        }

        public List<string> ExcludedLocationsFull {
            get { return m_excludedLocationsFull; }
        }

        public List<string> ExcludedModules {
            get { return m_excludedModules; }
        }

        public List<KeyDisplayRepresentation> ExcludedThreads {
            get { return m_excludedThreads; }
        }

        /// <summary>
        /// The value for the exclusion for above this. Any filtered elements with an index greater than this will be filtered out.
        /// </summary>
        public uint ExcludeEventsAboveThisIndex {
            get { return m_excludeEventsAboveThisIndex; }
            set { m_excludeEventsAboveThisIndex = value; }
        }

        /// <summary>
        /// The value for the exclusion for below this. Any filtered elements with an index less than this will be filtered out.
        /// </summary>
        public uint ExcludeEventsBelowThisIndex {
            get;
            set;
        }

        public List<string> ExclusionStrings {
            get { return m_exclusionStrings; }
        }

        public string FilterDescription {
            get { return m_filterDescription; }
            set { m_filterDescription = value; }
        }

        /// <summary>
        /// The index of the filter, this is used to cache filters so that you know what the result of this filter is.  essentially each time that
        /// a filter changes the index changes, therefore if the index is the same as the last time this filter visited you you can be sure that
        /// nothing has changed and you can cache the result.  The view takes advantage of this, storing the results of the last visted filter.
        /// </summary>

        // Incremental index used to detect changes to the filter
        /// <summary>
        /// List of partial strings that have to be present in the event entry for it to be displayed by this filter.
        /// </summary>

        // List of strings that include debug messages

        public uint FlagsForInclude {
            get { return m_flagsForInclude; }
            set { m_flagsForInclude = value; }
        }

        public List<string> InclusionStrings {
            get { return m_inclusionStrings; }
        }

        /// <summary>
        /// List of partial strings that if found in the event entry will not be displayed by this filter.
        /// </summary>

        // List of strings that exclude debug messages
        // I checked this out, hashtables are faster than even List<strings> for the sorts of data that we are using here
        // which is a bit wierd, however the List is simpler to understand and the difference isnt too great. Also the list
        // is more aligned with the data and simpler to turn into a string array.

        /// <summary>
        /// List of thread identifiers that should not be displayed.  This can be ids or names or anything.
        /// </summary>

        // List of thread ids or names to exclud threads
        /// <summary>
        /// List of module names that should not be displayed.
        /// </summary>

        // List of modules to exclude
        /// <summary>
        /// List of locations which should not be displayed, this is the full location information.
        /// </summary>

        // List of locations to exclude
        /// <summary>
        /// List of class names that are not to be displayed, this is a specaial starts with type format where if a location contains :: the part
        /// before the colons can be filtered out using this class filter.
        /// </summary>

        // Default location is class::method, this will store up to the ::
        /// <summary>
        /// This set of flags determines which events are included
        /// </summary>

        /// <summary>
        /// Determines whether the higher elements are excluded based on the filter index
        /// </summary>
        public bool UseAboveThisFilter {
            get;
            set;
        }

        /// <summary>
        /// Determines whetehr the lower elements are excluded based on the filter index
        /// </summary>
        public bool UseBelowThisFilter {
            get;
            set;
        }

        // A flag used to determine which message types are shown.
        /// <summary>
        /// Determines whether filtering on index is used
        /// </summary>
        public bool UseIndexBasedFilters {
            get { return m_useIndexBasedFilters; }
            set { m_useIndexBasedFilters = value; }
        }

        #endregion Data used to store the actual index settings

        #region Getters, used to populate the filter options from an existing filter.

        internal List<string> GetAdditionalLocationClassExclusions() {
            return m_excludedLocationsClass == null ? new List<string>() : m_excludedLocationsClass;
        }

        internal List<string> GetAdditonalLocationExclusions() {
            return m_excludedLocationsFull == null ? new List<string>() : m_excludedLocationsFull;
        }

        internal string GetExclusionStrings() {
            var sb = new StringBuilder();

            if ((m_exclusionStrings != null) && (m_exclusionStrings.Count > 0)) {
                foreach (string s in m_exclusionStrings) {
                    _ = sb.Append(s + ";");
                }
            }

            return sb.ToString();
        }

        // These methods are used to populate the filter screen from the view filter data.
        internal string GetInclusionStrings() {
            var sb = new StringBuilder();

            if ((m_inclusionStrings != null) && (m_inclusionStrings.Count > 0)) {
                foreach (string s in m_inclusionStrings) {
                    _ = sb.Append(s + ";");
                }
            }

            return sb.ToString();
        }

        internal List<string> GetModuleExclusionNames() {
            return m_excludedModules == null ? new List<string>() : m_excludedModules;
        }

        internal List<KeyDisplayRepresentation> GetThreadExclusionNames() {
            return m_excludedThreads == null ? new List<KeyDisplayRepresentation>() : m_excludedThreads;
        }

        /// <summary>
        /// Takes the name of a secific message type and indicates whether or not this filter will allow this message type to be displayed.
        /// </summary>
        /// <param name="tct">The trace command type to check the filter against</param>
        /// <returns>True if this type is displayed after filter processing, false if the filter removes these types</returns>
        internal bool TraceMessageTypeIncludedByFilter(TraceCommandTypes tct) {
            return (((uint)tct) & m_flagsForInclude) == (uint)tct;
        }

        #endregion Getters, used to populate the filter options from an existing filter.

        /// <summary>
        /// Specialist supporting method that is used to determine whether this message is an internal message designed to control
        /// how the viewer works rather than a specific debug entry message.
        /// </summary>
        /// <param name="ee">the event entry to check the type of</param>
        /// <returns>True if the message is an internal type, false if it is debug information</returns>
        internal static bool EventEntryIsInternalType(EventEntry ee) {
            // TODO : Not sure that this belongs within the filter.

            uint internalTypeMatchMask = 0x0003E308;
            return (ee.cmdType == TraceCommandTypes.ExceptionBlock) && (ee.debugMessage == "EXCEPTIONEND")
|| ((uint)ee.cmdType & internalTypeMatchMask) == (uint)ee.cmdType;
        }

        /// <summary>
        /// At least used by partial purge
        /// </summary>
        /// <param name="incLogs">if set to <c>true</c> [inc logs].</param>
        /// <param name="incVerbose">if set to <c>true</c> [inc verbose].</param>
        /// <param name="incMini">if set to <c>true</c> [inc mini].</param>
        /// <param name="incInternal">if set to <c>true</c> [inc internal].</param>
        /// <param name="incTin">if set to <c>true</c> [inc tin].</param>
        /// <param name="incTout">if set to <c>true</c> [inc tout].</param>
        /// <param name="incTrc">if set to <c>true</c> [inc TRC].</param>
        /// <param name="incAss">if set to <c>true</c> [inc ass].</param>
        /// <param name="incMor">if set to <c>true</c> [inc mor].</param>
        /// <param name="incCmd">if set to <c>true</c> [inc CMD].</param>
        /// <param name="incErr">if set to <c>true</c> [inc err].</param>
        /// <param name="incWarn">if set to <c>true</c> [inc warn].</param>
        /// <param name="incExB">if set to <c>true</c> [inc ex B].</param>
        /// <param name="incExD">if set to <c>true</c> [inc ex D].</param>
        /// <param name="incExE">if set to <c>true</c> [inc ex E].</param>
        /// <param name="incExS">if set to <c>true</c> [inc ex S].</param>
        /// <param name="incSecS">if set to <c>true</c> [inc sec S].</param>
        /// <param name="incSecE">if set to <c>true</c> [inc sec E].</param>
        /// <param name="ResE">if set to <c>true</c> [res E].</param>
        /// <param name="ResP">if set to <c>true</c> [res P].</param>
        /// <returns></returns>
        internal static uint GetFlagTypeByBools(bool incLogs, bool incVerbose, bool incInternal, bool incTin,
          bool incTout, bool incAss, bool incMor, bool incCmd, bool incErr,
          bool incWarn, bool incExB, bool incExD, bool incExE, bool incExS, bool incSecS, bool incSecE, bool ResE, bool ResP) {
            uint result = 0;

            if (incLogs) { result |= (uint)TraceCommandTypes.LogMessage; }
            if (incVerbose) { result |= (uint)TraceCommandTypes.LogMessageVerb; }
            if (incInternal) { result |= (uint)TraceCommandTypes.InternalMsg; }
            if (incTin) { result |= (uint)TraceCommandTypes.TraceMessageIn; }
            if (incTout) { result |= (uint)TraceCommandTypes.TraceMessageOut; }
            //if (incTrc) { result |= (uint)TraceCommandTypes.TraceMessage; }
            if (incAss) { result |= (uint)TraceCommandTypes.AssertionFailed; }
            if (incMor) { result |= (uint)TraceCommandTypes.MoreInfo; }
            if (incCmd) { result |= (uint)TraceCommandTypes.CommandOnly; }
            if (incErr) { result |= (uint)TraceCommandTypes.ErrorMsg; }
            if (incWarn) { result |= (uint)TraceCommandTypes.WarningMsg; }
            if (incExB) { result |= (uint)TraceCommandTypes.ExceptionBlock; }
            if (incExD) { result |= (uint)TraceCommandTypes.ExceptionData; }
            if (incExE) { result |= (uint)TraceCommandTypes.ExcEnd; }
            if (incExS) { result |= (uint)TraceCommandTypes.ExcStart; }
            if (incSecS) { result |= (uint)TraceCommandTypes.SectionStart; }
            if (incSecE) { result |= (uint)TraceCommandTypes.SectionEnd; }
            if (ResE) { result |= (uint)TraceCommandTypes.ResourceEat; }
            if (ResP) { result |= (uint)TraceCommandTypes.ResourcePuke; }

            return result;
        }

        /// <summary>
        /// Retrieves the current index filters that this filter will apply.  Returns a boolean indicating
        /// whether index fitlers are valid.
        /// </summary>
        /// <param name="belowIndex">Indicates the index, below which messages should be filtered out</param>
        /// <param name="aboveThisIndex">Indicates the index, above which messages should be filtered out</param>
        /// <returns>Boolean indicating whether or not index based filtering should be used.</returns>
        internal bool GetIndexFilters(out string belowIndex, out string aboveThisIndex) {
            belowIndex = aboveThisIndex = string.Empty;

            if (!m_useIndexBasedFilters) { return false; }

            belowIndex = UseBelowThisFilter ? ExcludeEventsBelowThisIndex.ToString() : string.Empty;

            aboveThisIndex = UseAboveThisFilter ? ExcludeEventsAboveThisIndex.ToString() : string.Empty;

            return true;
        }

        internal void SetIndexFilters(bool useFilters, string belowThisIndex, string aboveThisIndex) {
            m_useIndexBasedFilters = useFilters;
            if (!useFilters) {
                return;   // if were not using them nothing left to do.
            }

            #region entry code

            //Bilge.Assert(!m_readonly, "You must call BeginFilterUpdate before setting filter values");

            #endregion entry code

            if (m_readonly) {
                //Bilge.Warning("Attempted update of filter when BeginFilterUpdate not been called");
                return;
            }

            // If we are using them we need to parse them to determine which ones are in action

            if ((belowThisIndex != null) && (belowThisIndex.Length > 0)) {
                try {
                    ExcludeEventsBelowThisIndex = uint.Parse(belowThisIndex);
                    UseBelowThisFilter = true;
                } catch (FormatException) {
                    UseBelowThisFilter = false;
                }
            } else {
                UseBelowThisFilter = false;
            }

            if ((aboveThisIndex != null) && (aboveThisIndex.Length > 0)) {
                try {
                    ExcludeEventsAboveThisIndex = uint.Parse(aboveThisIndex);
                    UseAboveThisFilter = true;
                } catch (FormatException) {
                    UseAboveThisFilter = false;
                }
            } else {
                UseAboveThisFilter = false;
            }
        }

        // End EventEntryIsInternalType Method

        #region constructiors / static load filter / save filter

        /// <summary>
        /// Constructor that creeates a new default view filter, this is populated with a restriction on the event type so that
        /// only the basic logigng information hits the view.  Otherwise the filter contains no restrictions.
        /// </summary>
        internal ViewFilter() {
            // Creates a default filter

            m_currentFilterIndex = GetNewFilterIndex();
            m_readonly = true;
            m_flagsForInclude = 0x00001C85;
        }

        /// <summary>
        /// Duplicates the filter provided into this filter, copying all of the filter restrictions, except the index representing
        /// the filter.
        /// </summary>
        /// <param name="vf"></param>
        internal ViewFilter(ViewFilter vf) {
            BeginFilterUpdate();
            m_caseSensitive = vf.m_caseSensitive;

            if (vf.m_excludedLocationsClass != null) {
                m_excludedLocationsClass.AddRange(vf.m_excludedLocationsClass.ToArray());
            }

            if (vf.m_excludedLocationsFull != null) {
                m_excludedLocationsFull.AddRange(vf.m_excludedLocationsFull.ToArray());
            }

            if (vf.m_excludedModules != null) {
                m_excludedModules.AddRange(vf.m_excludedModules.ToArray());
            }

            if (vf.m_excludedThreads != null) {
                m_excludedThreads.AddRange(vf.m_excludedThreads.ToArray());
            }

            m_excludeEventsAboveThisIndex = vf.m_excludeEventsAboveThisIndex;
            ExcludeEventsBelowThisIndex = vf.ExcludeEventsBelowThisIndex;

            if (vf.m_exclusionStrings != null) {
                m_exclusionStrings.AddRange(vf.m_exclusionStrings.ToArray());
            }

            m_filterDescription = vf.m_filterDescription;
            m_flagsForInclude = vf.m_flagsForInclude;
            m_inclusionStrings = vf.m_inclusionStrings;
            UseAboveThisFilter = vf.UseAboveThisFilter;
            UseBelowThisFilter = vf.UseBelowThisFilter;

            EndFilterUpdate();
        }

        internal ViewFilter(uint flags, string[] include, string[] exclude) {
            m_currentFilterIndex = GetNewFilterIndex();
            m_readonly = true;
            m_flagsForInclude = flags;

            m_filterMoreComplexThanType = false;

            if ((include != null) && (include.Length > 0)) {
                m_filterMoreComplexThanType = true;
                m_inclusionStrings = new List<string>();
                m_inclusionStrings.AddRange(include);
            } else {
                m_inclusionStrings = null;
            }

            if ((exclude != null) && (exclude.Length > 0)) {
                m_filterMoreComplexThanType = true;
                m_exclusionStrings = new List<string>();
                m_exclusionStrings.AddRange(exclude);
            }
        }

        internal static ViewFilter LoadFilterFromFile(string filename) {

            #region entry code

            //Bilge.Assert(File.Exists(filename), "ViewFilter..LoadFromFile --> the file name " + filename + " does not exist, load cannot continue", "LoadFromFile expects the caller to handle passing a valid filename into the method");

            #endregion entry code

            var xmls = new XmlSerializer(typeof(ViewFilter));

            ViewFilter result;

            using (var fs = new FileStream(filename, FileMode.Open)) {
                result = (ViewFilter)xmls.Deserialize(fs);
                fs.Close();
            }

            result.BeginFilterUpdate();
            if ((result.m_excludedLocationsClass != null) && (result.m_excludedLocationsClass.Count == 0)) {
                result.m_excludedLocationsClass = null;
            }
            if ((result.m_excludedLocationsFull != null) && (result.m_excludedLocationsFull.Count == 0)) {
                result.m_excludedLocationsFull = null;
            }
            if ((result.m_excludedModules != null) && (result.m_excludedModules.Count == 0)) {
                result.m_excludedModules = null;
            }
            if ((result.m_excludedThreads != null) && (result.m_excludedThreads.Count == 0)) {
                result.m_excludedThreads = null;
            }
            // End update indicates that this is a new filter index.
            result.EndFilterUpdate();

            return result;
        }

        internal static void SaveFilterToFile(string filename, ViewFilter thisFilter, bool includeThreads, bool includeModules, bool includeLocations, bool includeClassLocations) {
            //Bilge.E();
            try {
                if (includeThreads & includeModules & includeLocations & includeClassLocations == false) {
                    //Bilge.VerboseLog("Not all filter data is being persisted, removing filter elements that are not to be stored");

                    thisFilter = new ViewFilter(thisFilter);
                    if (!includeThreads) {
                        thisFilter.m_excludedThreads = null;
                    }
                    if (!includeModules) {
                        thisFilter.m_excludedModules = null;
                    }
                    if (!includeLocations) {
                        thisFilter.m_excludedLocationsFull = null;
                    }
                    if (!includeClassLocations) {
                        thisFilter.m_excludedLocationsClass = null;
                    }
                }

                var xmls = new XmlSerializer(typeof(ViewFilter));

                //Bilge.VerboseLog("About to write the persisted filter data out to the disk, using filename " + filename);
                using var fs = new FileStream(filename, FileMode.Create);
                xmls.Serialize(fs, thisFilter);
                fs.Close();
            } finally {
                //Bilge.X();
            }
        }

        #endregion constructiors / static load filter / save filter

        #region Set Filter Methods - this sets a part of the filter replacing the values

        internal void SetAllMessageTypesToNotIncluded() {
            m_flagsForInclude = 0;
        }

        /// <summary>
        /// Sets the include exclude and exclude strings for the current filter.  This will replace the current filter contents with
        /// ones that are new from the passed in parameters.  If the parameters are null then the corresponding include and exclude
        /// strings will be removed from the filter.
        /// </summary>
        /// <param name="includeStrings">The include strings to be set / reset.</param>
        /// <param name="excludeStrings">The exclude strings to be set / reset.</param>
        /// <param name="caseSensitve">if set to <c>true</c> the inclusions and exclusions are done in a case sensitive fasion.</param>
        internal void SetIncludeExcludeStrings(string[] includeStrings, string[] excludeStrings, bool caseSensitve) {

            #region entry code

            //Bilge.Assert(!m_readonly, "You must call BeginFilterUpdate before setting filter values");

            #endregion entry code

            if (m_readonly) {
                //Bilge.Warning("Attempted update of filter when BeginFilterUpdate not been called");
                return;
            }

            m_caseSensitive = caseSensitve;

            if ((includeStrings == null) || (includeStrings.Length == 0)) {
                m_inclusionStrings = null;
            } else {
                m_inclusionStrings = new List<string>();
                if (!caseSensitve) {
                    for (int i = 0; i < includeStrings.Length; i++) {
                        m_inclusionStrings.Add(includeStrings[i].ToLower());
                    }
                } else {
                    m_inclusionStrings.AddRange(includeStrings);
                }
            }

            if ((excludeStrings == null) || (excludeStrings.Length == 0)) {
                m_exclusionStrings = null;
            } else {
                m_exclusionStrings = new List<string>();
                if (!caseSensitve) {
                    for (int i = 0; i < excludeStrings.Length; i++) {
                        m_exclusionStrings.Add(excludeStrings[i].ToLower());
                    }
                } else {
                    m_exclusionStrings.AddRange(excludeStrings);
                }
            }
        }

        /// <summary>
        /// Sets the filter type based on the parameters passed.  One boolean for each known type is passed to set the filter.
        /// </summary>
        internal void SetMessageTypeInclude(bool incLogs, bool incVerbose, bool incInternal, bool incTin,
          bool incTout, bool incAss, bool incMor, bool incCmd, bool incErr,
          bool incWarn, bool incExB, bool incExD, bool incExE, bool incExS, bool incSecS, bool incSecE, bool ResE, bool ResP) {

            #region entry code

            //Bilge.Assert(!m_readonly, "You must call BeginFilterUpdate before setting filter values");

            #endregion entry code

            if (m_readonly) {
                //Bilge.Warning("Attempted update of filter when BeginFilterUpdate not been called");
                return;
            }

            m_flagsForInclude = GetFlagTypeByBools(incLogs, incVerbose, incInternal, incTin, incTout, incAss, incMor, incCmd, incErr, incWarn, incExB, incExD, incExE, incExS, incSecS, incSecE, ResE, ResP);
        } // End SetMessageTypeInclude

        internal void SetMessageTypeIncludeByType(TraceCommandTypes tct, bool newValue) {

            #region entry code

            //Bilge.Assert(!m_readonly, "You must call BeginFilterUpdate before setting filter values");

            #endregion entry code

            if (newValue) {
                m_flagsForInclude |= (uint)tct;  // turn the bit on
            } else {
                m_flagsForInclude &= ~(uint)tct;  // turn the bit off
            }
        }

        /// <summary>
        /// Sets the threads modules locations using the parameters passed in, nb this is a reference copy not a duplication.
        /// </summary>
        /// <param name="excludeThreadNames">The exclude thread names to replace the current exclude thread names.</param>
        /// <param name="excludeModuleNames">The exclude module names to replace the current exclude module names.</param>
        /// <param name="excludeLocations">The exclude locations to replace the current exclude locations.</param>
        /// <param name="excludeLocClasses">The excluded class names to filter based on class name</param>
        internal void SetThreadsModulesLocations(List<KeyDisplayRepresentation> excludeThreadNames, List<string> excludeModuleNames, List<string> excludeLocations, List<string> excludeLocClasses) {

            #region entry code

            //Bilge.Assert(!m_readonly, "You must call BeginFilterUpdate before setting filter values");

            #endregion entry code

            m_excludedModules = (excludeModuleNames == null) || (excludeModuleNames.Count == 0) ? null : excludeModuleNames;

            m_excludedThreads = (excludeThreadNames == null) || (excludeThreadNames.Count == 0) ? null : excludeThreadNames;

            m_excludedLocationsFull = (excludeLocations == null) || (excludeLocations.Count == 0) ? null : excludeLocations;

            m_excludedLocationsClass = (excludeLocClasses == null) || (excludeLocClasses.Count == 0) ? null : excludeLocClasses;
        } // End SetThreadsModulesLocations

        #endregion Set Filter Methods - this sets a part of the filter replacing the values

        #region append filter methods, modify an existing filter

        internal void AppendExcludeString(string newExcludeString) {

            #region entry code

            //Bilge.Assert(!m_readonly, "You must call BeginFilterUpdate before setting filter values");

            #endregion entry code

            if ((newExcludeString == null) || (newExcludeString.Length == 0)) { return; }
            if (!m_caseSensitive) { newExcludeString = newExcludeString.ToLower(); }

            m_exclusionStrings ??= new List<string>();

            m_exclusionStrings.Add(newExcludeString);
        }

        internal void AppendIncludeString(string newIncludeString) {

            #region entry code

            //Bilge.Assert(!m_readonly, "You must call BeginFilterUpdate before setting filter values");

            #endregion entry code

            if ((newIncludeString == null) || (newIncludeString.Length == 0)) { return; }
            if (!m_caseSensitive) { newIncludeString = newIncludeString.ToLower(); }

            m_inclusionStrings ??= new List<string>();
            m_inclusionStrings.Add(newIncludeString);
        }

        internal void AppendLocationExclusion(string theLocation) {
            m_excludedLocationsFull ??= new List<string>();
            if (!m_excludedLocationsFull.Contains(theLocation)) {
                m_excludedLocationsFull.Add(theLocation);
            }
        }

        internal void AppendLocClassExclusion(string theLocation) {
            m_excludedLocationsClass ??= new List<string>();
            if (!m_excludedLocationsClass.Contains(theLocation)) {
                m_excludedLocationsClass.Add(theLocation);
            }
        }

        internal void AppendModuleExclusion(string theModule) {
            m_excludedModules ??= new List<string>();
            if (!m_excludedModules.Contains(theModule)) {
                m_excludedModules.Add(theModule);
            }
        }

        internal void AppendThreadExclusion(KeyDisplayRepresentation threadId) {
            m_excludedThreads ??= new List<KeyDisplayRepresentation>();
            if (!m_excludedThreads.Contains(threadId)) {
                m_excludedThreads.Add(threadId);
            }
        }

        #endregion append filter methods, modify an existing filter

        #region Include this entry methods, use the filter against an entry

        /// <summary>
        /// Primary filter method for known event entries.  The event entry passed is compared to the filter settings and a boolean returned indicating whether
        /// or not it should be displayed based on the current filter settings.  The event entry will also be updated to reflect the current filter settings
        /// so that the next time it is studied unless the filter has changed the answer is the same
        /// </summary>
        /// <param name="ee">the event entry to determine whether it should be displayed or not</param>
        /// <returns>Bool indicating whether or not to display the entry.</returns>
        internal bool IncludeThisEventEntry(EventEntry ee) {
            // Enable the caching on the objects themselves.  This costs ram but should be way faster for most types of filter.
            // This routine only applies the last used filter however IF the filter is more complex than the cmd type test which is already optimised.

            if (ee.LastVisitedFilter == m_currentFilterIndex) { return ee.LastVisitedFilterResult; }

            // Cache this filter index so until the filter changes this event entry can be processed rapidly.
            ee.LastVisitedFilter = m_currentFilterIndex;

            bool result = BaseIncludeEventEntry(ee);

            ee.LastVisitedFilterResult = result;
            return result;
        }

        internal bool IncludeThisEventEntryTimingsView(EventEntry ee) {
            uint baseFlag = m_flagsForInclude;

            // Alter the type filter to be only those which we are interested in for timed views.
            m_flagsForInclude = TraceMessageFormat.SECTIONCOMMANDS;

            bool result = BaseIncludeEventEntry(ee);
            m_flagsForInclude = baseFlag;

            return result;
        }

        internal bool IncludeThisNonEventEntry(NonTracedApplicationEntry nta) {
            bool includeIt = true;

            // TODO : Implement filter caching and measure the perf increase.
            // if (ee.LastVisitedFilter == m_currentFilterIndex) { return ee.LastVisitedFilterResult; }

            if (!m_filterMoreComplexThanType) { return true; }

            // For a non traced application event entry all we really need to worry about is whether or not any exclusion or inclusion
            // strings are being used.  We need to check both the PID and the message itself so that we can filter out any unwanted
            // messages or processes that are spamming us.

            string debugCheckString = m_caseSensitive ? nta.DebugEntry : nta.DebugEntry.ToLower();
            debugCheckString += " " + nta.Pid.ToString();  // Add the pid to the search string.

            // Here we check for inclusion strings.  If we are using inclusion strings then the first thing that we need to do is
            // exclude the match so that it only gets inluded if an inclusion string matches.

            if ((m_inclusionStrings != null) && (m_inclusionStrings.Count > 0)) {
                includeIt = false;

                // Do inclusion strings now, if no match is made bomb out.
                for (int i = 0; i < m_inclusionStrings.Count; i++) {
                    if (debugCheckString.IndexOf(m_inclusionStrings[i]) > 0) {
                        includeIt = true;
                        break; // Go on to the inclusions
                    }
                } // End for each of the inclusion strings
            } // end if there are inclusionStrings to worry about

            // now we have gone through all of the inclusions, if this is still false then we dont want this string anyhow.
            // TODO : Add filter caching here
            if (!includeIt) { return false; }

            // Now we check for exclusion strings, even if it was included only by an inclusion string it can still be excluded by
            // an exclusion string.  This
            if ((m_exclusionStrings != null) && (m_exclusionStrings.Count > 0)) {
                // check for exclusion strings

                for (int i = 0; i < m_exclusionStrings.Count; i++) {
                    if (debugCheckString.IndexOf(m_exclusionStrings[i]) > 0) {
                        //ee.LastVisitedFilter = m_currentFilterIndex;
                        //ee.LastVisitedFilterResult = false;
                        // TODO : Add filter caching here.
                        return false;  //  found a rejected one
                    }
                }
            }// end if there are exclusion strings to worry about

            //Bilge.Assert(includeIt == true, "The only way we can reach here is if it was in an inclusion string and not in an exclusions string");
            return true;
        }

        private bool BaseIncludeEventEntry(EventEntry ee) {
            /* filter created in order of most likely reasons to fail therefore for performance tried to shift the types to the top then the
              inclusion / exclusion fitlers as these will be used the most.  Ive optimised this to be a single comparison so it should be very
              quick. */
            if ((m_flagsForInclude & (uint)ee.cmdType) != (uint)ee.cmdType) {
                return false;
            }

            // Performance cache to stop checking all of the complex filters in the default scenario.  Basically the default scenario only
            // filters on type therefore all of the good stuff coming up is pointless.
            if (!m_filterMoreComplexThanType) {
                return true;
            }

            // Now we check for the more complicated filtering options such as index filtering

            if (m_useIndexBasedFilters) {
                if (UseBelowThisFilter) {
                    if (ee.GlobalIndex < ExcludeEventsBelowThisIndex) {
                        return false;
                    }
                }
                if (UseAboveThisFilter) {
                    if (ee.GlobalIndex > ExcludeEventsAboveThisIndex) { return false; }
                }
            }

            if ((m_inclusionStrings != null) && (m_inclusionStrings.Count > 0)) {
                // check for inclusion strings
                string debugCheckString = m_caseSensitive ? ee.debugMessage : ee.debugMessage.ToLower();
                for (int i = 0; i < m_inclusionStrings.Count; i++) {
                    if (debugCheckString.IndexOf(m_inclusionStrings[i]) >= 0) {
                        ee.LastVisitedFilter = m_currentFilterIndex;
                        return true;  // matched one of the inclusion strings
                    }
                } // End for each of the inclusion strings

                return false; // none of the inclusion strings matched
            } // end if there are inclusionStrings to worry about

            if ((m_exclusionStrings != null) && (m_exclusionStrings.Count > 0)) {
                // check for exclusion strings

                string exclusionCheckString = m_caseSensitive ? ee.debugMessage : ee.debugMessage.ToLower();
                for (int i = 0; i < m_exclusionStrings.Count; i++) {
                    if (exclusionCheckString.IndexOf(m_exclusionStrings[i]) >= 0) {
                        ee.LastVisitedFilter = m_currentFilterIndex;
                        ee.LastVisitedFilterResult = false;
                        return false;  //  found a rejected one
                    }
                }
            }// end if there are exclusion strings to worry about

            //Bilge.Assert(ee.Module != null, "The module value should not be null when checked by the filter, it should be empty if not known");
            if ((m_excludedModules != null) && (ee.module.Length > 0)) {
                //Bilge.Assert(m_excludedModules.Count > 0, "There should not be a module list of 0 entries");

                if (m_excludedModules.Contains(ee.module)) {
                    return false;
                }
            }

            //Bilge.Assert(ee.CurrentThreadKey != null, "The thread id for an event entry should not be null when checked by the filter, if its not known it should be empty");
            if ((m_excludedThreads != null) && (ee.CurrentThreadKey.Length > 0)) {
                //Bilge.Assert(m_excludedThreads.Count > 0, "The threads to be excluded should not be of length 0");

                // check for threads we specifically dont include.  This allows new threads to arrive and be included
                if (KeyDisplayRepresentation.ContainsKey(m_excludedThreads, ee.CurrentThreadKey)) {  // return true if its NOT in the exclusions
                    return false;
                }
            }

            //Bilge.Assert(ee.MoreLocationData != null, "The additional location data should not be null when checked by the filter, if its not known it should be empty");
            if ((m_excludedLocationsClass != null) && (ee.moreLocationData.Length > 0)) {
                //Bilge.Assert(m_excludedLocationsClass.Count > 0, "This should not be callable when the number of excluded locations by class is 0");

                int offsetOfColons = ee.moreLocationData.IndexOf("::");
                if (offsetOfColons > 0) {
                    // There is a class look alike
                    string locForEntry = ee.moreLocationData[..offsetOfColons];

                    if (m_excludedLocationsClass.Contains(locForEntry)) {
                        return false;
                    }
                }
            }
            if ((m_excludedLocationsFull != null) && (ee.moreLocationData.Length > 0)) {
                //Bilge.Assert(m_excludedLocationsFull.Count > 0, "This should not be callable when the number of excluded locations is 0");

                ee.LastVisitedFilter = m_currentFilterIndex;
                if (m_excludedLocationsFull.Contains(ee.moreLocationData)) { // return true if its NOT in the exclusions
                    return false;
                }
            }

            return true;
        }

        #endregion Include this entry methods, use the filter against an entry

        #region Filter index support - used for caching filters

        private static uint ViewFilterIndexCount;

        private bool m_readonly = true;

        internal void BeginFilterUpdate() {
            m_readonly = false;
        }

        internal void EndFilterUpdate() {
            m_readonly = true;

            // This is an optimisation that skips a tonne of checking in the default case for most pople most of the time.
            m_filterMoreComplexThanType = (m_excludedLocationsFull != null) || (m_excludedModules != null)
                || (m_excludedThreads != null) || (m_excludedLocationsClass != null)
                || m_useIndexBasedFilters || (m_exclusionStrings != null) || (m_inclusionStrings != null);
            m_currentFilterIndex = GetNewFilterIndex();
        }

        private static uint GetNewFilterIndex() {
            // This will allocate a new filter  - not particularaly sophisticated but not used so
            try {
                ViewFilterIndexCount++;
            } catch (OverflowException) {
                ViewFilterIndexCount = 1;
            }
            return ViewFilterIndexCount;
        }

        #endregion Filter index support - used for caching filters
    }
}