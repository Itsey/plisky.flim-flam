//using Plisky.Plumbing.Legacy;
using System.Collections.Generic;
using System.Drawing;

namespace Plisky.FlimFlam { 

    internal class HighlightRequestsStore {
        internal Coloring[] DefaultHighlightColoring;
        internal List<AHighlightRequest> HighlightRequests;

        internal HighlightRequestsStore() {
            HighlightRequests = new List<AHighlightRequest>();

            DefaultHighlightColoring = new Coloring[6];
            DefaultHighlightColoring[0] = new Coloring(Color.Black, Color.BlueViolet, string.Empty);
            DefaultHighlightColoring[1] = new Coloring(Color.Black, Color.BlanchedAlmond, string.Empty);
            DefaultHighlightColoring[2] = new Coloring(Color.Black, Color.DeepPink, string.Empty);
            DefaultHighlightColoring[3] = new Coloring(Color.Black, Color.Goldenrod, string.Empty);
            DefaultHighlightColoring[4] = new Coloring(Color.Black, Color.Honeydew, string.Empty);
            DefaultHighlightColoring[5] = new Coloring(Color.Black, Color.PaleGreen, string.Empty);
        }

        internal bool ApplyDefaultHighlighting(EventEntry ee) {
            foreach (Coloring cr in DefaultHighlightColoring) {
                if (cr.matchCase.Length > 0) {
                    if (ee.ThreadID == cr.matchCase) {
                        bool highlightIsTheSame = ee.ViewData.isValid;
                        highlightIsTheSame = highlightIsTheSame && ((ee.ViewData.isForegroundHighlighted && cr.ForeColorSpecified) && (ee.ViewData.foregroundHighlightColor == cr.foregroundColor));
                        highlightIsTheSame = highlightIsTheSame && ((ee.ViewData.isBackgroundHighlighted && cr.BackColorSpecified) && (ee.ViewData.backgroundHighlightColor == cr.backgroundColor));

                        if (highlightIsTheSame) {
                            // There was a match made but it was already applied to this element.  Therefore no changes.
                            return false;
                        } else {
                            if (cr.ForeColorSpecified) {
                                ee.ViewData.foregroundHighlightColor = cr.foregroundColor;
                                ee.ViewData.isForegroundHighlighted = true;
                            }

                            if (cr.BackColorSpecified) {
                                ee.ViewData.backgroundHighlightColor = cr.backgroundColor;
                                ee.ViewData.isBackgroundHighlighted = true;
                            }

                            ee.ViewData.isValid = true;

                            return true;
                        }
                    }
                }
            }
            return false;
        }

        internal bool ModifyEventEntryForHighlight(EventEntry ee) {
            // first check to see if there is any highlight data.
            if ((HighlightRequests == null) || (HighlightRequests.Count == 0)) {
                if ((ee.ViewData.isValid) && (ee.ViewData.isHighlighted)) {
                    ee.ViewData.isHighlighted = false;
                    return true;
                }
                return false;
            } // end if the highlight data is null or empty

            // ok now onto checking for the highlight data itself.
            foreach (AHighlightRequest ahr in HighlightRequests) {
                bool matchMade = false;

                if (ahr.MakeMatchUsingType) {
                    if ((ee.cmdType & ahr.matchTct) == ee.cmdType) {
                        matchMade = true;
                    }
                }

                if ((!matchMade) && ahr.MakeMatchUsingString) {
                    string eventEntryCombinedString = ee.DebugMessage + "#X#" + ee.SecondaryMessage + "#X#" + ee.Module + "#X#" + ee.CurrentThreadKey;

                    if (!ahr.CaseSensitive) {
                        ahr.ComparisonStringToMatchOn = ahr.ComparisonStringToMatchOn.ToLower();
                        eventEntryCombinedString = eventEntryCombinedString.ToLower();
                    }

                    if (ahr.NotMatch) {
                        matchMade = eventEntryCombinedString.IndexOf(ahr.ComparisonStringToMatchOn) < 0;
                    } else {
                        matchMade = eventEntryCombinedString.IndexOf(ahr.ComparisonStringToMatchOn) >= 0;
                    }
                }

                if (matchMade) {
                    //Bilge.Warning("PERF Warning, would be more efficient to use ViewData highlight caching rather than this comparison, or for best perf use both");

                    // Split this out into a few lines as it was getting too complex to read.  Essentially we judge that the highlight is the same only if
                    // the specifictions and the colors are the same between the desired highlight and the existing highlight.
                    bool highlightIsTheSame = ee.ViewData.isValid;
                    highlightIsTheSame = highlightIsTheSame && ((ee.ViewData.isForegroundHighlighted && ahr.ForeColorSpecified) && (ee.ViewData.foregroundHighlightColor == ahr.ForegroundColor));
                    highlightIsTheSame = highlightIsTheSame && ((ee.ViewData.isBackgroundHighlighted && ahr.BackColorSpecified) && (ee.ViewData.backgroundHighlightColor == ahr.BackgroundColor));

                    if (highlightIsTheSame) {
                        // There was a match made but it was already applied to this element.  Therefore no changes.
                        return false;
                    } else {
                        if (ahr.ForeColorSpecified) {
                            ee.ViewData.foregroundHighlightColor = ahr.ForegroundColor;
                            ee.ViewData.isForegroundHighlighted = true;
                        }

                        if (ahr.BackColorSpecified) {
                            ee.ViewData.backgroundHighlightColor = ahr.BackgroundColor;
                            ee.ViewData.isBackgroundHighlighted = true;
                        }

                        ee.ViewData.isValid = true;

                        return true;
                    }
                } // end if there was a match made
            } // end for each of the highlights to apply

            // if we get here then there was no match for the current highlight selection.
            if ((ee.ViewData.isValid) && (ee.ViewData.isHighlighted)) {
                ee.ViewData.isHighlighted = false;
                return true;
            } else {
                return false;
            }
        }

        internal bool ModifyNonTracedEventEntryForHighlight(NonTracedApplicationEntry nta) {
            // first check to see if there is any highlight data.
            if ((HighlightRequests == null) || (HighlightRequests.Count == 0)) {
                if ((nta.ViewData.isValid) && (nta.ViewData.isHighlighted)) {
                    nta.ViewData.isHighlighted = false;
                    return true;
                }
                return false;
            } // end if the highlight data is null or empty

            // ok now onto checking for the highlight data itself.
            foreach (AHighlightRequest ahr in HighlightRequests) {
                if (!ahr.MakeMatchUsingString) { continue; }  // NonTraced types only support string based highlights

                bool matchMade;

                string matchString = nta.DebugEntry + "#X#" + nta.Pid;
                if (!ahr.CaseSensitive) {
                    matchString = matchString.ToLower();
                }

                if (ahr.NotMatch) {
                    matchMade = matchString.IndexOf(ahr.ComparisonStringToMatchOn) < 0;
                } else {
                    matchMade = matchString.IndexOf(ahr.ComparisonStringToMatchOn) >= 0;
                }

                if (matchMade) {
                    //Bilge.Warning("PERF Warning, would be more efficient to use ViewData highlight caching rather than this comparison, or for best perf use both");

                    // Split this out into a few lines as it was getting too complex to read.  Essentially we judge that the highlight is the same only if
                    // the specifictions and the colors are the same between the desired highlight and the existing highlight.
                    bool highlightIsTheSame = nta.ViewData.isValid;
                    highlightIsTheSame = highlightIsTheSame && ((nta.ViewData.isForegroundHighlighted && ahr.ForeColorSpecified) && (nta.ViewData.foregroundHighlightColor == ahr.ForegroundColor));
                    highlightIsTheSame = highlightIsTheSame && ((nta.ViewData.isBackgroundHighlighted && ahr.BackColorSpecified) && (nta.ViewData.backgroundHighlightColor == ahr.BackgroundColor));

                    if (highlightIsTheSame) {
                        // There was a match made but it was already applied to this element.  Therefore no changes.
                        return false;
                    } else {
                        nta.ViewData.isHighlighted = true;
                        nta.ViewData.isValid = true;
                        if (ahr.BackColorSpecified) {
                            nta.ViewData.isBackgroundHighlighted = true;
                            nta.ViewData.backgroundHighlightColor = ahr.BackgroundColor;
                        }
                        if (ahr.ForeColorSpecified) {
                            nta.ViewData.isForegroundHighlighted = true;
                            nta.ViewData.foregroundHighlightColor = ahr.ForegroundColor;
                        }
                        return true;
                    }
                }
            } // end for each of the highlights to apply

            // if we get here then there was no match for the current highlight selection.
            if ((nta.ViewData.isValid) && (nta.ViewData.isHighlighted)) {
                nta.ViewData.isHighlighted = false;
                return true;
            } else {
                return false;
            }
        }
    }
}