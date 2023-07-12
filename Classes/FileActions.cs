using Berichtsheft.Dialogs;
using System;
using System.IO;
using System.Xml;

namespace Berichtsheft.Classes {

    /// <summary>
    /// Contains File actions like creating, deleting and resetting the work directory.
    /// </summary>
    internal class FileActions {

        /// <summary>General variable for the path to the root directory. </summary>
        public string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft";

        /// <summary>
        /// <para>Creates the work directory, its subdirectories and its files<br></br>
        /// Based on <see cref="path">path</see>.</para>
        /// </summary>
        public void CreateWorkDirectory() {
            try {
                // Check if the work directory exists
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);                            // Create the root directory
                    Directory.CreateDirectory(path + @"\Notes");                // Create the note directory
                    Directory.CreateDirectory(path + @"\Project tracking");     // Create the project tracking directory
                    Directory.CreateDirectory(path + @"\Report books");         // Create the report books directory

                    // Write neccessary files
                    XmlDocument xmlDocument = new XmlDocument();                // Create a new xml document
                    XmlNode workDay = xmlDocument.CreateElement("projects");    // Create a new xml node called 'projects'
                    xmlDocument.AppendChild(workDay);                           // Append the node to the document
                    xmlDocument.Save(path + @"\Project tracking\Projects.xml"); // Save the document
                }
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);      // Create a new instance of the exception window
                exceptionWindow.ShowDialog();                                   // Show the exception window
            }
        }

        /// <summary>
        /// <para>Deletes the work directory and all its subdirectories and files<br></br>
        /// based on <see cref="path">path</see>.</para>
        /// </summary>
        public void DeleteWorkDirectory() {
            try {
                Directory.Delete(path, true);                                   // Delete the work directory
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);      // Create a new instance of the exception window
                exceptionWindow.ShowDialog(); // Show the exception window
            }
        }

        /// <summary>
        /// <para>Resets the application by deleting the existing work directory based<br></br>
        /// on  <see cref="path">path</see> and resetting the settings. After this procedure,<br></br>
        /// the root directory and its files will be created again.</para>
        /// </summary>
        public void ResetApplication() {
            try {
                Properties.Settings.Default.Reset();                            // Reset settings
                Properties.Settings.Default.Save();                             // Save the resetted settings

                DeleteWorkDirectory();                                          // Delete the work directory
                CreateWorkDirectory();                                          // Create the work directory
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);      // Create a new instance of the exception window
                exceptionWindow.ShowDialog();                                   // Show the exception window
            }
        }
    }
}
