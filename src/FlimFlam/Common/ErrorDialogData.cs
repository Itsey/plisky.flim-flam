﻿#if !BILGE2

using System.Collections.Generic;
using FlimFlamUI.Common;

namespace Plisky.Plumbing {

    /// <summary>
    /// Contains all of the data that is required to intialise and display the error dialog that is used by the Default listener.
    /// </summary>
    internal class ErrorDialogData {
        private string m_errorMessage;

        /// <summary>
        /// The main error message that is to be displayed in the eror dialog.
        /// </summary>
        internal string ErrorMessage {
            get { return m_errorMessage; }
            set { m_errorMessage = value; }
        }

        internal List<AnEvidence> Evidences = new List<AnEvidence>();

        /// <summary>
        /// Adds an evidence piece of information which is displayed in the error dialog.
        /// </summary>
        /// <param name="name">The name of the piece of evidence</param>
        /// <param name="value">The value of the piece of evidence</param>
        internal void AddEvidence(string name, string value) {
            Evidences.Add(new AnEvidence(name, value));
        }
    }
}

#endif