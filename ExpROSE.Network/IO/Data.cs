using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace ExpROSE.IO
{
    /// <summary>
    /// Methods that can handle files and directories.
    /// </summary>
    class Data
    {
        /// <summary>
        /// Returns the directory of the executeable (without backslash at end) as a string.
        /// </summary>
        public static string workingDirectory
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);
            }
        }
        /// <summary>
        /// Returns a bool, which indicates if the specified path leads to a file.
        /// </summary>
        /// <param name="fileLocation">The full location of the file.</param>
        public static bool fileExists(string fileLocation)
        {
            return File.Exists(fileLocation);
        }
    }
}