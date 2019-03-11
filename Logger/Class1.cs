using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    class Logger
    {
        /// <summary>
        /// A default constructor that will create a log file using the assembly name
        /// and folder location for the log file name and location. Each log filename will
        /// be prefixed using the assembly filename (without the extension) followed by
        /// a string representing the date and time when the application started.
        /// </summary>
        public Logger()
        {
            Logfile logfile = new Logfile();
            // The following stream allows sharing of the log file with an external process
            Stream myFile = new FileStream(logfile.Path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            /* Create a new text writer using the output stream, and add it to
             * the trace listeners. */
            TextWriterTraceListener myTextListener = new
               TextWriterTraceListener(myFile);
            Trace.Listeners.Add(myTextListener);
            Trace.AutoFlush = true;
        }

        /// <summary>
        /// Writes and error message entry
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="module"></param>
        public void Error(string message, [CallerMemberName]string module = "")
        {
            WriteEntry(message, "[ERROR]", module);
        }
        /// <summary>
        /// Writes an Error log entry for an exception
        /// </summary>
        /// <param name="ex">The exception object</param>
        /// <param name="module"></param>
        public void Error(Exception ex, [CallerMemberName]string module = "")
        {
            WriteEntry(ex.Message, "[ERROR]", module);
            WriteEntry(ex.ToString(), "[ERROR]", module);
        }
        /// <summary>
        /// Writes a warning message entry
        /// </summary>
        /// <param name="message"></param>
        /// <param name="module"></param>
        public void Warning(string message, [CallerMemberName]string module = "")
        {
            WriteEntry(message, "[WARNING]", module);
        }
        /// <summary>
        /// Writes an info message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="module"></param>
        public void Info(string message, [CallerMemberName]string module = "")
        {
            WriteEntry(message, "[INFO]", module);
        }
        /// <summary>
        /// A helper method to write a log entry.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <param name="module"></param>
        private void WriteEntry(string message, string type, string module)
        {
            // Stream myFile = File.OpenWrite(logfile);

            Trace.WriteLine(
                    string.Format("{0},{1},{2},{3}",
                                  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                  type,
                                  module,
                                  message));
            Trace.Flush();
        }
}
    public class Logfile
    {
        public string Path { get; private set; }
        public string Filename { get; private set; }

        public Logfile()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            Filename = fvi.FileDescription;
            string version = fvi.FileVersion;
            string fileDir = System.IO.Path.GetTempPath();
            string Path = String.Format("{0}_{1}.log", System.IO.Path.Combine(fileDir, Filename), DateTime.Now.ToString("yyyyMMddHHmmss"));
        }
    }
}
