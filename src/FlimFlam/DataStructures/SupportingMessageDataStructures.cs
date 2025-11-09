//using Plisky.Plumbing.Legacy;
using System;
using Plisky.Diagnostics;

namespace Plisky.FlimFlam {

    public abstract class SupportingMessageData {

        public static SupportingMessageData ParseAsMessageData(TraceCommandTypes tct, ref string primaryMessagData, ref string moreInfoData) {
            try {
                if (tct is TraceCommandTypes.SectionStart or TraceCommandTypes.SectionEnd) {
                    // Sections can contain timer data, if so then we abstract the timer data here.
                    TimerInstanceData tid = null;
                    if (primaryMessagData.StartsWith(Constants.TIMER_SECTIONIDENTIFIER)) {
                        // This is only a timer based section if it has this specfic tag in its debug message.
                        try {
                            tid = new TimerInstanceData(moreInfoData);
                        } catch (ArgumentException) {
                            //Bilge.Dump(aex, "Timer error, couldnt parse timer message as timer message structure:  (" + MoreInfoData + ")");
                            MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.CorruptStringFound, UserMessageType.ErrorMessage, "A timing message was recieved but it was corrupt.  Try removing any strange characters from the timer names.  Timing data may be innaccurate.");
                        }
                        primaryMessagData = primaryMessagData[Constants.TIMER_SECTIONIDENTIFIER.Length..]; // Strip the intiial identifier
                        moreInfoData = string.Empty;  // All of the more info should be consumed when creating the timer event.
                    }

                    return tid;
                }

                if (tct is TraceCommandTypes.ResourcePuke or TraceCommandTypes.ResourceEat) {
                    // Resource alteration identifiers contain values for named resources incrementing or decrementing by amounts.
                    var rid = new ResourceInstanceData(moreInfoData);
                    primaryMessagData = "Resource Alteration for " + rid.resourceName;
                    moreInfoData = "Value Change " + rid.resourceValue.ToString();
                    return rid;
                }

                if (tct == TraceCommandTypes.ResourceCount) {
                    var rid = new ResourceInstanceData(moreInfoData);
                    primaryMessagData = "Resource Set For " + rid.resourceName;
                    moreInfoData = "Value = " + rid.resourceValue;
                    return rid;
                }
            } catch (ArgumentException) {
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
        internal string contextName;
        internal string resourceName;
        internal long resourceValue;

        internal ResourceInstanceData(string moreInfo) {
            try {
                if (!TraceMessageFormat.SplitResourceContentStringByParts(moreInfo, out resourceName, out contextName, out string resValueTemp)) {
                    throw new ArgumentException("The string containig the resource data could not be parsed into valid resource isntance information");
                }

                try {
                    resourceValue = long.Parse(resValueTemp);
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
            return TraceMessageFormat.AssembleResourceContentString(resourceName, contextName, resourceValue.ToString());
        }
    }

    internal class TimerInstanceData : SupportingMessageData {
        internal DateTime timeInstance;
        internal string timeSinkInstanceDescription;
        internal string timeSinkName;

        internal TimerInstanceData(string timerString) {
            try {
                if (!TraceMessageFormat.SplitTimerStringByParts(timerString, out timeSinkInstanceDescription, out timeSinkName, out timeInstance)) {
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
            return TraceMessageFormat.AssembleTimerContentString(timeSinkInstanceDescription, timeSinkName, timeInstance);
        }
    }
}