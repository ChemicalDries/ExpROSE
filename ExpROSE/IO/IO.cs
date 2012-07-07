using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ExpROSE.IO
{
    /// <summary>
    /// Provides interface output related functions, such as loggin activities.
    /// </summary>
    public class Out
    {
        /// <summary>
        /// Enum with flags for log importancies. If 'minimumImportance' flag is higher than the action to be logged, then the action won't be logged.
        /// </summary>
        public enum logFlags { ImportantAction = 3, StandardAction = 2, BelowStandardAction = 1, UnimportantAction = 0 }
        /// <summary>
        /// Flag for minimum importance in logs. Adjust this to don't print less important logs.
        /// </summary>
        public static logFlags minimumImportance;
        /// <summary>
        /// Prints a green line of log, together with timestamp and method name.
        /// </summary>
        /// <param name="logText">The log line to be printed.</param>
        public static void WriteLine(string logText,bool Debugmode = false)
        {
            DateTime _DTN = DateTime.Now;
            StackFrame _SF = new StackTrace().GetFrame(1);

            if (Debugmode == true)
            {
                Console.Write("[" + _DTN.ToLongTimeString() + ":" + _DTN.Millisecond.ToString() + "] <");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(_SF.GetMethod().ReflectedType.FullName + "." + _SF.GetMethod().Name);
            }
            else
            {
                Console.Write("[" + _DTN.ToLongTimeString() + "] <");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(_SF.GetMethod().ReflectedType.Name + "." + _SF.GetMethod().Name);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("> :: ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(logText);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        /// <summary>
        /// Prints a green line of log, together with timestamp and method name.
        /// </summary>
        /// <param name="logText">The log line to be printed.</param>
        /// <param name="logFlag">The importance flag of this log line.</param>
        public static void WriteLine(string logText, logFlags logFlag, bool debugmode)
        {
            if ((int)logFlag < (int)minimumImportance)
                return;

            DateTime _DTN = DateTime.Now;
            StackFrame _SF = new StackTrace().GetFrame(1);

            if (debugmode == true)
            {
                Console.Write("[" + _DTN.ToLongTimeString() + ":" + _DTN.Millisecond.ToString() + "] <");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(_SF.GetMethod().ReflectedType.FullName + "." + _SF.GetMethod().Name);
            }
            else
            {
                Console.Write("[" + _DTN.ToLongTimeString() + "] <");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(_SF.GetMethod().ReflectedType.Name + "." + _SF.GetMethod().Name);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("> :: ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(logText);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        /// <summary>
        /// Prints a customizeable line of log, together with timestamp and method name.
        /// </summary>
        /// <param name="logText">The log line to be printed.</param>
        /// <param name="logFlag">The importance flag of this log line.</param>
        /// <param name="colorOne">The color to use on the left.</param>
        /// <param name="colorTwo">The color to use on the right.</param>
        public static void WriteLine(string logText, logFlags logFlag, bool debugmode , ConsoleColor colorOne, ConsoleColor colorTwo)
        {
            if ((int)logFlag < (int)minimumImportance)
                return;

            DateTime _DTN = DateTime.Now;
            StackFrame _SF = new StackTrace().GetFrame(1);

            if (debugmode == true)
            {
                Console.Write("[" + _DTN.ToLongTimeString() + ":" + _DTN.Millisecond.ToString() + "] <");
                Console.ForegroundColor = colorOne;
                Console.Write(_SF.GetMethod().ReflectedType.FullName + "." + _SF.GetMethod().Name);
            }
            else
            {
                Console.Write("[" + _DTN.ToLongTimeString() + "] <");
                Console.ForegroundColor = colorOne;
                Console.Write(_SF.GetMethod().ReflectedType.Name + "." + _SF.GetMethod().Name);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("> :: ");
            Console.ForegroundColor = colorTwo;
            Console.WriteLine(logText);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        /// <summary>
        /// Prints a customizeable line of log, together with timestamp and title.
        /// </summary>
        /// <param name="logText">The log line to be printed.</param>
        /// <param name="logFlag">The importance flag of this log line.</param>
        /// <param name="colorOne">The color to use on the left.</param>
        /// <param name="colorTwo">The color to use on the right.</param>
        /// <param name="Title">The title of the log.</param>
        public static void WriteLine(string logText, logFlags logFlag, bool debugmode, ConsoleColor colorOne, ConsoleColor colorTwo, string Title)
        {
            if ((int)logFlag < (int)minimumImportance)
                return;

            DateTime _DTN = DateTime.Now;

            if (debugmode == true)
            {
                Console.Write("[" + _DTN.ToLongTimeString() + ":" + _DTN.Millisecond.ToString() + "] <");
                Console.ForegroundColor = colorOne;
                Console.Write(Title);
            }
            else
            {
                Console.Write("[" + _DTN.ToLongTimeString() + "] <");
                Console.ForegroundColor = colorOne;
                Console.Write(Title);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("> :: ");
            Console.ForegroundColor = colorTwo;
            Console.WriteLine(logText);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        /// <summary>
        /// Prints a customizeable line of log, together with timestamp and title.
        /// </summary>
        /// <param name="logText">The log line to be printed.</param>
        /// <param name="logFlag">The importance flag of this log line.</param>
        /// <param name="colorOne">The color to use on the left.</param>
        /// <param name="colorTwo">The color to use on the right.</param>
        /// <param name="Title">The title of the log.</param>
        public static void Write(string logText, logFlags logFlag, bool debugmode, ConsoleColor colorOne, ConsoleColor colorTwo, string Title)
        {
            if ((int)logFlag < (int)minimumImportance)
                return;

            DateTime _DTN = DateTime.Now;

            if (debugmode == true)
            {
                Console.Write("[" + _DTN.ToLongTimeString() + ":" + _DTN.Millisecond.ToString() + "] <");
                Console.ForegroundColor = colorOne;
                Console.Write(Title);
            }
            else
            {
                Console.Write("[" + _DTN.ToLongTimeString() + "] <");
                Console.ForegroundColor = colorOne;
                Console.Write(Title);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("> :: ");
            Console.ForegroundColor = colorTwo;
            Console.Write(logText);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static void WriteTrace(string logText)
        {
            return;
        }
        /// <summary>
        /// Prints a red, error line of log, together with timestamp and method name.
        /// </summary>
        /// <param name="logText">The log line to be printed.</param>
        public static void WriteError(string logText, bool debugmode = false)
        {
            DateTime _DTN = DateTime.Now;
            StackFrame _SF = new StackTrace().GetFrame(1);

            if (debugmode == true)
            {
                Console.Write("[" + _DTN.ToLongTimeString() + ":" + _DTN.Millisecond.ToString() + "] <");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(_SF.GetMethod().ReflectedType.FullName + "." + _SF.GetMethod().Name);
            }
            else
            {
                Console.Write("[" + _DTN.ToLongTimeString() + "] <");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(_SF.GetMethod().ReflectedType.Name + "." + _SF.GetMethod().Name);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("> :: ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(logText);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        /// <summary>
        /// Prints a red, error line of log, together with timestamp and method name.
        /// </summary>
        /// <param name="logText">The log line to be printed.</param>
        /// <param name="logFlag">The importance flag of this error.</param>
        public static void WriteError(string logText, logFlags logFlag, bool debugmode = false)
        {
            if ((int)logFlag < (int)minimumImportance)
                return;

            DateTime _DTN = DateTime.Now;
            StackFrame _SF = new StackTrace().GetFrame(1);

            if (debugmode == true)
            {
                Console.Write("[" + _DTN.ToLongTimeString() + ":" + _DTN.Millisecond.ToString() + "] <");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(_SF.GetMethod().ReflectedType.FullName + "." + _SF.GetMethod().Name);
            }
            else
            {
                Console.Write("[" + _DTN.ToLongTimeString() + "] <");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(_SF.GetMethod().ReflectedType.Name + "." + _SF.GetMethod().Name);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("> :: ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(logText);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        /// <summary>
        /// Writes a plain text line.
        /// </summary>
        /// <param name="logText">The log line to be printed.</param>
        public static void WritePlain(string logText)
        {
            Console.WriteLine(logText);
        }
        /// <summary>
        /// Writes a blank line.
        /// </summary>
        public static void WriteBlank()
        {
            Console.WriteLine();
        }
        /// <summary>
        /// Writes a special line of log, with customizeable colors and header coloring of logText.
        /// </summary>
        /// <param name="logText">The log line to be printed.</param>
        /// <param name="logFlag">The importance flag of this log line.</param>
        /// <param name="colorOne">The color to use on the left.</param>
        /// <param name="colorTwo">The color to use on the right.</param>
        /// <param name="headerHead">The string to use infront of logText.</param>
        /// <param name="headerLength">The length of the header to color.</param>
        /// <param name="headerColor">The color for the header in the logText.</param>
        public static void WriteSpecialLine(string logText, logFlags logFlag, bool debugmode, ConsoleColor colorOne, ConsoleColor colorTwo, string headerHead, int headerLength, ConsoleColor headerColor)
        {
            //if ((int)logFlag < (int)minimumImportance)
            //return;

            DateTime _DTN = DateTime.Now;
            StackFrame _SF = new StackTrace().GetFrame(1);

            if (debugmode == true)
            {
                Console.Write("[" + _DTN.ToLongTimeString() + ":" + _DTN.Millisecond.ToString() + "] <");
            }
            else
            {
                Console.Write("[" + _DTN.ToLongTimeString() + "] <");
            }

            Console.ForegroundColor = colorOne;
            Console.Write(_SF.GetMethod().ReflectedType.Name + "." + _SF.GetMethod().Name);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("> " + headerHead + " ");
            Console.ForegroundColor = headerColor;
            Console.Write(logText.Substring(0, headerLength));
            Console.ForegroundColor = colorTwo;
            Console.WriteLine(logText.Substring(headerLength));
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
