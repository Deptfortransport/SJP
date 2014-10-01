// *********************************************** 
// NAME             : NotesDisplayAdapter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 26 Apr 2011
// DESCRIPTION  	: Responsible for processing Display Notes into the format required by UI
// ************************************************
// 

using System.Collections.Generic;
using System.Web;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;

namespace SJP.Common.Web
{
    /// <summary>
    /// Responsible for processing Display Notes into the format required by UI
    /// </summary>
    public class NotesDisplayAdapter
    {
        #region Private constants
        /// <summary>
        /// Property Key for Web.MaxNotesDisplayed
        /// </summary>
        private const string MaxNotesDisplayed = "JourneyOptions.NotesDisplayed.MaxNumber";

        private const string NewLine = "\n";
        private const string HTMLLineBreak = "<br />";

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public NotesDisplayAdapter()
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Formats Display Notes ready for UI and imposes max displayable notes restriction 
        /// </summary>
        /// <param name="noteStrings">Notes String Array</param>
        /// <returns>HTML formated Display Notes string array</returns>
        public List<string> GetDisplayableNotes(List<string> noteStrings)
        {
            List<string> result = new List<string>();

            if (noteStrings != null && noteStrings.Count > 0)
            {
                // Set MaxNote in case Properties table value is blank
                int maxNotes = Properties.Current[MaxNotesDisplayed].Parse(20);

                int notesCounter = 0;

                string note = string.Empty;

                // For each note
                for (int i = 0; i < noteStrings.Count && notesCounter < maxNotes; i++)
                {
                    // Split note in seperate array at NewLine breaks
                    string[] splitResult = noteStrings[i].Replace(NewLine, "|").Split('|');

                    // For each sub-note in the note
                    for (int j = 0; j < splitResult.Length && notesCounter < maxNotes; j++)
                    {
                        //Append note and HTML line break
                        note = HttpUtility.HtmlEncode(splitResult[j]);

                        result.Add(HttpUtility.HtmlDecode(note));

                        notesCounter++;
                    }
                }
            }

            return result;
        }

        #endregion
    }
}