using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public class HighScoreTable
    {

        // A list inside which all of the entries in the table will be stored
        private List<HighScoreEntry> _scoreEntries;
        // The number of entries to store in the table
        private int _tableSize;

        //-------------------------------------------------------------------------------------
        // Class constructor

        /// <summary>
        /// Class constructor. Initialize the table with the specified number of entries.
        /// Scope is internal so external code cannot create instances.
        /// </summary>
        internal HighScoreTable(int tableSize, string tableDescription)
        {
            // Store the parameter values
            Description = tableDescription;
            _tableSize = tableSize;

            // Reset the table
            Clear();
        }

        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// Returns the list of high score entries in the table
        /// </summary>
        public List<HighScoreEntry> Entries
        {
            get
            {
                return _scoreEntries;
            }
        }

        /// <summary>
        /// Return the table description
        /// </summary>
        public string Description { get; set; }

        //-------------------------------------------------------------------------------------
        // Class functions

        /// <summary>
        /// Create/reset the initial empty table
        /// </summary>
        public void Clear()
        {
            // Create the score list
            _scoreEntries = new List<HighScoreEntry>();

            // Add an entry for each position specified in the table
            for (int i = 0; i < _tableSize; i++)
            {
                _scoreEntries.Add(new HighScoreEntry());
            }
        }

        /// <summary>
        /// Add a new entry to the highscore table.
        /// </summary>
        /// <param name="Name">The Name for the new entry</param>
        /// <param name="score">The Score for the new entry</param>
        /// <returns>Returns the HighScoreEntry object added, or if the score
        /// was not high enough to feature in the list, returns null.</returns>
        public HighScoreEntry AddEntry(string name, int score)
        {
            return AddEntry(name, score, DateTime.Now);
        }
        /// <summary>
        /// An internal overload of AddEntry which also allows the entry date
        /// to be specified. This is used when loading the scores from the
        /// storage file.
        /// </summary>
        internal HighScoreEntry AddEntry(string name, int score, DateTime date)
        {
            // Create and initialize a new highscore entry
            HighScoreEntry entry = new HighScoreEntry();
            entry.Name = name;
            entry.Score = score;
            entry.Date = date;

            // Add to the table
            _scoreEntries.Add(entry);

            // Sort into descending order
            _scoreEntries.Sort(new HighScoreEntry());

            // Limit the number of entries to the requested table size
            if (_scoreEntries.Count > _tableSize)
            {
                _scoreEntries.RemoveAt(_tableSize);
            }

            // Is our entry still in the list
            if (_scoreEntries.Contains(entry))
            {
                // Yes, so return the entry object
                return entry;
            }
            // No, so return null
            return null;
        }

        /// <summary>
        /// Determines whether this score is high enough to be added to the table
        /// </summary>
        /// <param name="score">The score to test</param>
        /// <returns>Return true if the score is high enough, false if it is not</returns>
        public bool ScoreQualifies(int score)
        {
            return (score > _scoreEntries[_tableSize - 1].Score);
        }

    }
}