using System;
using System.Collections.Generic;

namespace GameFramework
{
    public class HighScoreEntry : IComparer<HighScoreEntry>
    {
        
        //-------------------------------------------------------------------------------------
        // Class constructor

        /// <summary>
        /// Class constructor. Scope is internal so external code cannot create instances.
        /// </summary>
        internal HighScoreEntry()
        {
            // Set the default values for the new entry
            Name = "";
            Score = 0;
            Date = DateTime.MinValue;
        }

        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// Return the entry Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Return the entry Score
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Return the entry Date
        /// </summary>
        public DateTime Date { get; set; }

        //-------------------------------------------------------------------------------------
        // Class functions

        /// <summary>
        /// Compare two highscore entries. This provides a simple mechanism for sorting the
        /// entries into descending order for display.
        /// </summary>
        /// <param name="x">The first score entry to compare</param>
        /// <param name="y">The second score entry to compare</param>
        /// <returns>1 if x is greater than y, -1 if x is less than y, 0 of x and y are equal</returns>
        public int Compare(HighScoreEntry x, HighScoreEntry y)
        {
            // If the scores differ, return a comparison of the two.
            // Compare x's score against y's score so that they sort into descending order.
            if (x.Score != y.Score) return y.Score.CompareTo(x.Score);

            // The scores match, so we will put the oldest one first
            // This time compare y's score against x's score so that the dates sort into ascending order
            return x.Date.CompareTo(y.Date);
        }

    }
}