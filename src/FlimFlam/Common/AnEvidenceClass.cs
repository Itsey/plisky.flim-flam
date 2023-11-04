namespace Plisky.Plumbing;
using System;

/// <summary>
/// When the error dialog is created for the Error method the addittional information slots
/// are populated using instances of AnEvidence class.  Each evidence has a text name and
/// a description.  The name should be kept short to be displayed on the form and the theText should
/// contain all of the required information separated frequently using newlines
/// </summary>
internal class AnEvidence {

    /// <summary>
    /// The name of the piece of evidence.
    /// </summary>
    internal string TheName;

    /// <summary>
    /// The full description of the evidence itself
    /// </summary>
    internal string TheText;

    /// <summary>
    /// Constructor override to supply the name and the evidence in one
    /// </summary>
    /// <param name="name">The name of the evidence that is to be supplied.  Cannot be null</param>
    /// <param name="text">The full text of the evidence that is to be supplied.  If this is null the text null will be displayed</param>
    internal AnEvidence(string name, string text) {

        #region entry code
        text ??= "null";

        #endregion entry code

        TheName = name ?? throw new ArgumentNullException("name", "the name parameter for evidence cannot be null"); TheText = text;
    }

    /// <summary>
    /// To string returns the name of the evidence
    /// </summary>
    /// <returns>The displayable name of the evidence</returns>
    public override string ToString() {
        return TheName;
    }
}