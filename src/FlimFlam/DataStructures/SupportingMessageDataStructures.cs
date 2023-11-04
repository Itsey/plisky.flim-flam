//using Plisky.Plumbing.Legacy;
using System;
using Plisky.Diagnostics;
using Plisky.Diagnostics.FlimFlam;

namespace Plisky.FlimFlam { 

    public abstract class SupportingMessageData {

        public static SupportingMessageData ParseAsMessageData(TraceCommandTypes tct, ref string PrimaryMessagData, ref string MoreInfoData) {
            try {
                if ((tct == TraceCommandTypes.SectionStart) || (tct == TraceCommandTypes.SectionEnd)) {
                    // Sections can contain timer data, if so then we abstract the timer data here.
                    TimerInstanceData tid = null;
                    if (PrimaryMessagData.StartsWith(Constants.TIMER_SECTIONIDENTIFIER)) {
                        // This is only a timer based section if it has this specfic tag in its debug message.
                        try {
                            tid = new TimerInstanceData(MoreInfoData);
                        } catch (ArgumentException aex) {
                            //Bilge.Dump(aex, "Timer error, couldnt parse timer message as timer message structure:  (" + MoreInfoData + ")");
                            MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.CorruptStringFound, UserMessageType.ErrorMessage, "A timing message was recieved but it was corrupt.  Try removing any strange characters from the timer names.  Timing data may be innaccurate.");
                        }
                        PrimaryMessagData = PrimaryMessagData.Substring(Constants.TIMER_SECTIONIDENTIFIER.Length); // Strip the intiial identifier
                        MoreInfoData = string.Empty;  // All of the more info should be consumed when creating the timer event.
                    }

                    return tid;
                }

                if ((tct == TraceCommandTypes.ResourcePuke) || (tct == TraceCommandTypes.ResourceEat)) {
                    // Resource alteration identifiers contain values for named resources incrementing or decrementing by amounts.
                    ResourceInstanceData rid = new ResourceInstanceData(MoreInfoData);
                    PrimaryMessagData = "Resource Alteration for " + rid.ResourceName;
                    MoreInfoData = "Value Change " + rid.ResourceValue.ToString();
                    return rid;
                }

                if (tct == TraceCommandTypes.ResourceCount) {
                    ResourceInstanceData rid = new ResourceInstanceData(MoreInfoData);
                    PrimaryMessagData = "Resource Set For " + rid.ResourceName;
                    MoreInfoData = "Value = " + rid.ResourceValue;
                    return rid;
                }
            } catch (ArgumentException aex) {
                //Bilge.Dump(aex, "There was a problem when trying to extract data from a message type.");
                //Bilge.Warning("Failed to interpret the data string sent to Mex as valid data containing more info.  Returning null for this instance", "Trace Command Type  : " + tct.ToString());
                //Bilge.FurtherInfo("Primary:" + PrimaryMessagData);
                //Bilge.FurtherInfo("Secondary:" + MoreInfoData);
                MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.CorruptStringFound, UserMessageType.ErrorMessage, "Unable to extract the expected information from a message.  One of the strings arriving in Mex was corrupt.");
            }

            return null;
        }

        internal abstract string RevertToDebugMessage(string currentDebugMessage);

        internal abstract string RevertToMoreInfo(string currentMoreInfo);
    }

    internal class ResourceInstanceData : SupportingMessageData {
        internal string ContextName;
        internal string ResourceName;
        internal long ResourceValue;

        internal ResourceInstanceData(string moreInfo) {
            try {
                string resValueTemp;
                if (!TraceMessageFormat.SplitResourceContentStringByParts(moreInfo, out ResourceName, out ContextName, out resValueTemp)) {
                    throw new ArgumentException("The string containig the resource data could not be parsed into valid resource isntance information");
                }

                try {
                    ResourceValue = long.Parse(resValueTemp);
                } catch (FormatException fex) {
                    //Bilge.Log("WARNING >> Resources Count lost, string arrived in an unparsable format, the res value recieved in trace stream was unreoverable into a double value");
                    throw new ArgumentException("The resource count could not be converted to a valid count", fex);
                } catch (ArgumentOutOfRangeException aoox) {
                    //Bilge.Log("WARNING >> Resources Count lost, string arrived with a value too large, the res value recieved in trace stream was unreoverable into a double value");
                    throw new ArgumentException("The resource count could not be converted to a valid count", aoox);
                }
            } catch (ArgumentOutOfRangeException iox) {
                //Bilge.Dump(iox, "Argument out of range exception when trying to parse the strings to create the resource name value pair");
                throw new ArgumentException("ResourceInstanceData could not parse the strings.", iox);
            }
        }

        internal override string RevertToDebugMessage(string currentDebugMessage) {
            return currentDebugMessage;
        }

        internal override string RevertToMoreInfo(string currentMoreInfo) {
            return TraceMessageFormat.AssembleResourceContentString(ResourceName, ContextName, ResourceValue.ToString());
        }
    }

    internal class TimerInstanceData : SupportingMessageData {
        internal DateTime TimeInstance;
        internal string TimeSinkInstanceDescription;
        internal string TimeSinkName;

        internal TimerInstanceData(string timerString) {
            try {
                if (!TraceMessageFormat.SplitTimerStringByParts(timerString, out TimeSinkInstanceDescription, out TimeSinkName, out TimeInstance)) {
                    throw new ArgumentException("The string containing timer data could not be parsed as valid timer data");
                }
            } catch (IndexOutOfRangeException iorx) {
                throw new ArgumentException("The string contianing timer data could not be parsed as valid timer data", iorx);
            }
        }

        internal override string RevertToDebugMessage(string currentDebugMessage) {
            return Constants.TIMER_SECTIONIDENTIFIER + currentDebugMessage;
        }

        internal override string RevertToMoreInfo(string currentMoreInfo) {
            return TraceMessageFormat.AssembleTimerContentString(TimeSinkInstanceDescription, TimeSinkName, TimeInstance);
        }
    }
}