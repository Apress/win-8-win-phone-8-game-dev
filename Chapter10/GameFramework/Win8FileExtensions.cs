using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace GameFramework
{
    public static class FileExtensions
    {
        /// <summary>
        /// Determine whether a file with the provided name exists in the specified folder
        /// </summary>
        /// <param name="folder">Folder to check</param>
        /// <param name="filename">Name of file to locate (case-insensitive)</param>
        /// <returns></returns>
        async public static Task<bool> FileExistsAsync(this StorageFolder folder, string filename)
        {
            // Get all files in the specified folder
            var files = await folder.GetFilesAsync();
            // Convert the filename to lower-case to help with case-insensitive searching
            filename = filename.ToLower();
            // Loop through looking for our filename
            foreach (var file in files)
            {
                if (file.Name.ToLower() == filename)
                {
                    // Found it
                    return true;
                }
            }
            // Not found
            return false;
        }
    }
}
