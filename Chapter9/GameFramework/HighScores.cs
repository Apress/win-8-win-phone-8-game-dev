using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Windows.Storage;
using System.Threading.Tasks;

#if WINDOWS_PHONE
using System.IO.IsolatedStorage;
#endif

namespace GameFramework
{
    public class HighScores
    {
        // A reference to the game host object
        private GameHost _game;

        // A Dictionary of all the known highscore tables.
        private Dictionary<string, HighScoreTable> _highscoreTables;

        //-------------------------------------------------------------------------------------
        // Class constructor

        /// <summary>
        /// Class constructor. Scope is internal so external code cannot create instances.
        /// </summary>
        internal HighScores(GameHost game)
        {
            // Store the game reference
            _game = game;
            
            // Set default property values
            FileName = "Scores.dat";

            // Initialize the highscore tables
            Clear();
        }


        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// The filename to and from which the highscore data will be written.
        /// This can be either a fully specified path and filename, or just
        /// a filename alone (in which case the file will be written to the
        /// game engine assembly directory).
        /// </summary>
        public string FileName { get; set; }

        //-------------------------------------------------------------------------------------
        // Class functions

        /// <summary>
        /// Initialize a named high score table
        /// </summary>
        /// <param name="tableName">The name for the table to initialize</param>
        /// <param name="tableSize">The number of entries to store in this table</param>
        public void InitializeTable(string tableName, int tableSize)
        {
            // Delegate to the other version of this function
            InitializeTable(tableName, tableSize, "");
        }
        /// <summary>
        /// Initialize a named high score table
        /// </summary>
        /// <param name="tableName">The name for the table to initialize</param>
        /// <param name="tableSize">The number of entries to store in this table</param>
        /// <param name="tableDescription">A description of this table to show to the player</param>
        public void InitializeTable(string tableName, int tableSize, string tableDescription)
        {
            if (!_highscoreTables.ContainsKey(tableName))
            {
                _highscoreTables.Add(tableName, new HighScoreTable(tableSize, tableDescription));
            }
        }

        /// <summary>
        /// Retrieve the high score table with the specified name
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public HighScoreTable GetTable(string tableName)
        {
            if (_highscoreTables.ContainsKey(tableName))
            {
                return _highscoreTables[tableName];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove all high score tables from the object
        /// </summary>
        /// <remarks>To clear the scores for an individual table, retrieve the
        /// table object using GetTable and call the Clear method on that instead.</remarks>
        public void Clear()
        {
            // Create the table dictionary if it doesn't already exist
            if (_highscoreTables == null)
            {
                _highscoreTables = new Dictionary<string, HighScoreTable>();
            }

            // Tell any known tables to clear their content
            foreach (HighScoreTable table in _highscoreTables.Values)
            {
                table.Clear();
            }
        }

        /// <summary>
        /// Load the high scores from the storage file
        /// </summary>
        /// <remarks>Ensure that the tables have been created using InitializeTable
        /// prior to loading the scores.</remarks>
#if WINDOWS_PHONE
        public void LoadScores()
#else
        public async Task LoadScoresAsync()
#endif
        {
            string fileContent;
            HighScoreTable table;

            // Just in case we have any problems...
            try
            {
                // Clear any existing scores
                Clear();

#if WINDOWS_PHONE
                // Get access to the isolated storage
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!store.FileExists(FileName))
                    {
                        // The score file doesn't exist
                        return;
                    }
                    // Read the contents of the file
                    using (StreamReader sr = new StreamReader(store.OpenFile(FileName, FileMode.Open)))
                    {
                        fileContent = sr.ReadToEnd();
                    }
                }
#else
                // Does the file exist?
                if (!await ApplicationData.Current.LocalFolder.FileExistsAsync(FileName))
                {
                    // It doesn't exist
                    return;
                }
                // Read the contents of the file
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(FileName);
                fileContent = await FileIO.ReadTextAsync(file);
#endif

                // Parse the content XML that was loaded
                XDocument xDoc = XDocument.Parse(fileContent);
                // Create a query to read the score details from the xml
                var result = from c in xDoc.Root.Descendants("entry")
                                select new
                                {
                                    TableName = c.Parent.Parent.Element("name").Value,
                                    Name = c.Element("name").Value,
                                    Score = c.Element("score").Value,
                                    Date = c.Element("date").Value
                                };
                // Loop through the resulting elements
                foreach (var el in result)
                {
                    // Add the entry to the table.
                    table = GetTable(el.TableName);
                    if (table != null) table.AddEntry(el.Name, int.Parse(el.Score), DateTime.Parse(el.Date));
                }
            }
            catch
            {
                // A problem occurred, but don't re-throw the exception or the
                // user won't be able to relaunch the game. Instead just ignore
                // the error and carry on regardless.
                // We will ensure that a partial load hasn't taken place however
                // which could cause unexpected problems, we'll reset back to defaults
                // instead.
                Clear();
            }
        }

        /// <summary>
        /// Save the scores to the storage file
        /// </summary>
#if WINDOWS_PHONE
        public void SaveScores()
#else
        public async Task SaveScoresAsync()
#endif
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(sb);
            HighScoreTable table;

            // Begin the document
            xmlWriter.WriteStartDocument();
            // Write the HighScores root element
            xmlWriter.WriteStartElement("highscores");

            // Loop for each table
            foreach (string tableName in _highscoreTables.Keys)
            {
                // Retrieve the table object for this table name
                table = _highscoreTables[tableName];

                // Write the Table element
                xmlWriter.WriteStartElement("table");
                // Write the table Name element
                xmlWriter.WriteStartElement("name");
                xmlWriter.WriteString(tableName);
                xmlWriter.WriteEndElement();    // name

                // Create the Entries element
                xmlWriter.WriteStartElement("entries");

                // Loop for each entry
                foreach (HighScoreEntry entry in table.Entries)
                {
                    // Make sure the entry is not blank
                    if (entry.Date != DateTime.MinValue)
                    {
                        // Write the Entry element
                        xmlWriter.WriteStartElement("entry");
                        // Write the score, name and date
                        xmlWriter.WriteStartElement("score");
                        xmlWriter.WriteString(entry.Score.ToString());
                        xmlWriter.WriteEndElement();    // score
                        xmlWriter.WriteStartElement("name");
                        xmlWriter.WriteString(entry.Name);
                        xmlWriter.WriteEndElement();    // name
                        xmlWriter.WriteStartElement("date");
                        xmlWriter.WriteString(entry.Date.ToString("yyyy-MM-ddTHH:mm:ss"));
                        xmlWriter.WriteEndElement();    // date
                        // End the Entry element
                        xmlWriter.WriteEndElement();    // entry
                    }
                }

                // End the Entries element
                xmlWriter.WriteEndElement();    // entries

                // End the Table element
                xmlWriter.WriteEndElement();    // table
            }

            // End the root element
            xmlWriter.WriteEndElement();    // highscores
            xmlWriter.WriteEndDocument();

            // Flush the xml writer, which will put the finished document into the stringbuilder
            xmlWriter.Flush();

#if WINDOWS_PHONE
            // Get access to the isolated storage
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // Create a file and attach a streamwriter
                using (StreamWriter sw = new StreamWriter(store.CreateFile(FileName)))
                {
                    // Write the XML string to the streamwriter
                    sw.Write(sb.ToString());
                }
            }
#else
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, sb.ToString());
#endif
        }

        /// <summary>
        /// Create a series of TextObjects to represent the scores in the specified table
        /// </summary>
        /// <param name="tableName">The name of the table whose scores are to be displayed</param>
        /// <param name="font">The font to use for the score objects</param>
        /// <param name="scale">A scaling factor for the text</param>
        /// <param name="top">The coordinate for the topmost text item</param>
        /// <param name="height">The height of each score item</param>
        /// <param name="firstColor">The color for the topmost item in the table</param>
        /// <param name="lastColor">The color for the final item in the table</param>
        public void CreateTextObjectsForTable(string tableName, SpriteFont font, float scale, float top, float height, Color firstColor, Color lastColor)
        {
            CreateTextObjectsForTable(tableName, font, scale, top, height, firstColor, lastColor, null, Color.White);
        }

        /// <summary>
        /// Create a series of TextObjects to represent the scores in the specified table
        /// </summary>
        /// <param name="tableName">The name of the table whose scores are to be displayed</param>
        /// <param name="font">The font to use for the score objects</param>
        /// <param name="scale">A scaling factor for the text</param>
        /// <param name="top">The coordinate for the topmost text item</param>
        /// <param name="height">The height of each score item</param>
        /// <param name="firstColor">The color for the topmost item in the table</param>
        /// <param name="lastColor">The color for the final item in the table</param>
        /// <param name="highlightEntry">An entry to highlight in the table (e.g., a newly-added item)</param>
        /// <param name="highlightColor">The color for the highlighted entry</param>
        public void CreateTextObjectsForTable(string TableName, SpriteFont font, float scale, float top, float height, Color firstColor, Color lastColor, HighScoreEntry highlightEntry, Color highlightColor)
        {
            HighScoreTable table;
            int entryCount;
            float yPosition;
            TextObject textObject;
            Color entryColor;
            
            table = GetTable(TableName);

            entryCount = table.Entries.Count;
            for (int i = 0; i < entryCount; i++)
            {
                // Find the vertical position for the entry
                yPosition = top + (height * i);

                // Find the color for the entry
                if (table.Entries[i] == highlightEntry)
                {
                    entryColor = highlightColor;
                }
                else
                {
                    entryColor = new Color(Vector3.Lerp(firstColor.ToVector3(), lastColor.ToVector3(), (float)i / entryCount));
                }

                // Create and add a text item for the position and name
                textObject = new TextObject(_game, font, new Vector2(10, yPosition), (i + 1).ToString() + ". " + table.Entries[i].Name);
                textObject.Scale = new Vector2(scale);
                textObject.SpriteColor = entryColor;
                _game.GameObjects.Add(textObject);
                // Create and add a text item for the score
                textObject = new TextObject(_game, font, new Vector2(_game.GraphicsDevice.Viewport.Width - 10, yPosition), table.Entries[i].Score.ToString(), TextObject.TextAlignment.Far, TextObject.TextAlignment.Near);
                textObject.Scale = new Vector2(scale);
                textObject.SpriteColor = entryColor;
                _game.GameObjects.Add(textObject);
            }

        }

    }
}
