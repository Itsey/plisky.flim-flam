using System;
using System.Text;

namespace Plisky.FlimFlam { 

    /// <summary>
    /// Class used to hold all of the exception data so it can be stashed in the treeview for the eception dialog
    /// </summary>
    internal class LooksLikeAnException {
        internal string ExceptionMessage;
        internal string HelpURL;
        internal string IndentFromNesting;
        internal LooksLikeAnException InnerException;
        internal string MoreStuffAboutIt;
        internal string Source;
        internal string StackTrace;
        internal string TargetSite;
        internal string TypeName;

        /// <summary>
        /// Returns the nexting plus the typename in this pseudo exception
        /// </summary>
        /// <returns>string showing the type name of the exception and an indent level</returns>
        public override string ToString() {
            return IndentFromNesting + TypeName;
        }

        internal string GetDescriptionFully() {
            ExceptionMessage = ExceptionMessage ?? "null";
            MoreStuffAboutIt = MoreStuffAboutIt ?? "null";

            StringBuilder sb = new StringBuilder(ExceptionMessage.Length + MoreStuffAboutIt.Length + 500);

            sb.Append("Exception of type: " + TypeName + " was thrown." + Environment.NewLine);
            sb.Append("Exception message: ");
            sb.Append(ExceptionMessage);
            sb.Append(Environment.NewLine + Environment.NewLine);
            sb.Append("Help Url         : "); sb.Append(HelpURL); sb.Append(Environment.NewLine);
            sb.Append("Target Source    : "); sb.Append(TargetSite); sb.Append(Environment.NewLine);
            sb.Append("Source           : "); sb.Append(Source); sb.Append(Environment.NewLine);
            sb.Append("Summary Location : "); sb.Append(Environment.NewLine); sb.Append(Environment.NewLine);
            sb.Append("Stack Trace      : "); sb.Append(Environment.NewLine); sb.Append(StackTrace); sb.Append(Environment.NewLine); sb.Append(Environment.NewLine);
            sb.Append("More Information :"); sb.Append(Environment.NewLine); sb.Append(MoreStuffAboutIt); sb.Append(Environment.NewLine);

            return sb.ToString();
        }
    }
}