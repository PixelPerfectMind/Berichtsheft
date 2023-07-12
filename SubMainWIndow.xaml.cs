using Berichtsheft.Dialogs;
using Berichtsheft.Classes;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Collections.Generic;

namespace Berichtsheft {

    public partial class SubMainWIndow : Window {

        private readonly FileActions fileActions = new FileActions();

        public string path = "";
        public string file = "";


        /// <summary>
        /// This is a constructor for the SubMainWindow<br></br>
        /// Herewith will be the small TrayIcon window for<br></br>
        /// the bottom right screen corner  initialized
        /// </summary>
        public SubMainWIndow() {
            try {
                InitializeComponent();

                                                                                                                                    // -- Set the path and file variables --
                path = fileActions.path + @"\Project tracking";                                                                     // Set the path to the project tracking folder
                file = path + @"\ProjectTracking-" + DateTime.Now.ToString("dd.MM.yyyy") + @".xml";                                 // Set the file path to the current date

                                                                                                                                    // -- Check, if the root directory exists --
                if (!System.IO.Directory.Exists(path)) {                                                                            // If working directory doesn't exist...
                    System.IO.Directory.CreateDirectory(path);                                                                      // ...create it!
                }

                                                                                                                                    // -- Set window position to bottom right --
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;                                                  // Get the working area of the screen
                this.Left = desktopWorkingArea.Right - 220;                                                                         // Set the left position to the right of the screen
                this.Top = desktopWorkingArea.Bottom - 260;                                                                         // Set the top position to the bottom of the screen

            
                btn_stopTimeTracking.IsEnabled = false;                                                                             // Set the stop tracking button to disabled

                if (!System.IO.File.Exists(file)) {                                                                                 // Create empty project tracking file if it doesn't exist
                    XmlDocument xmlDocument = new XmlDocument();                                                                    // Create a new XML document
                    XmlNode workDay = xmlDocument.CreateElement("workDay");                                                         // Create the root node
                    xmlDocument.AppendChild(workDay);                                                                               // Append the root node to the XML document
                    xmlDocument.Save(file);                                                                                         // Save the XML document
                    }
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        private void chkbox_minimized_Checked(object sender, RoutedEventArgs e) { brdr_smallMenu.Visibility = Visibility.Visible; } // Show the minimized menu

        private async void chkbox_minimized_Unchecked(object sender, RoutedEventArgs e) {
            await Task.Delay(100);                                                                                                  // Wait 100ms for the hiding animation to finish
            brdr_smallMenu.Visibility = Visibility.Collapsed;                                                                       // Hide the minimized menu
        }

        private void brdr_minimized_MouseDown(object sender, MouseButtonEventArgs e) {
            if(chkbox_minimized.IsChecked == true) {                                                                                // If the small menu is visible,...
                chkbox_minimized.IsChecked = false;                                                                                 // ...hide the small menu
                cb_showMiniWindow.IsChecked = false;                                                                                // Hide the small window too
            } else {
                chkbox_minimized.IsChecked = true;                                                                                  // Else, Show the small menu
            }
        }

        private void btn_hideMiniWindow_Click(object sender, RoutedEventArgs e) {
            cb_showMiniWindow.IsChecked = false;                                                                                    // Hide the small window
        }
        private void btn_showMiniWindow_Click(object sender, RoutedEventArgs e) {
            cb_showMiniWindow.IsChecked = true;                                                                                     // Check the cb_showMiniWindow to show the small window
        }

        private void cb_showMiniWindow_Checked(object sender, RoutedEventArgs e) {
            brdr_smallWindow.Visibility = Visibility.Visible;                                                                       // Show the small window
        }
        private async void cb_showMiniWindow_Unchecked(object sender, RoutedEventArgs e) {
            await Task.Delay(200);                                                                                                  // Wait 200ms for the hiding animation to finish
            brdr_smallWindow.Visibility = Visibility.Collapsed;                                                                     // Hide the small window
        }

        private void btn_closeApp_Click(object sender, RoutedEventArgs e) { Environment.Exit(0); }                                  // Close the application

        private void btn_openFullApp_Click(object sender, RoutedEventArgs e) {
            MainWindow mainWindow = new MainWindow();                                                                               // Create a new instance of the MainWindow
            mainWindow.Show();                                                                                                      // Show the MainWindow
        }

        public DateTime startTime = new DateTime();                                                                                 // Variable for the start time of the project tracking

        /// <summary>
        /// When the user clicks on this button, the project tracking will be started
        /// </summary>
        private void btn_trackTime_Click(object sender, RoutedEventArgs e) {
            ProjectPicker projectPicker = new ProjectPicker();                                                                      // Create a new instance of the ProjectPicker
            projectPicker.ShowDialog();                                                                                             // Show the ProjectPicker
            // If the user selected a project
            if(projectPicker.DialogResult == true && projectPicker.selectedProject != "") {                                         // If the user selected a project
                startTime = DateTime.Now;                                                                                           // Set the start time to the current time
                InsertNode(projectPicker.selectedProject, startTime.ToString("HH:mm"));                                             // Insert a new node with the selected project and the start time
                btn_trackTime.IsEnabled = false;                                                                                    // Disable the track time button
                btn_stopTimeTracking.IsEnabled = true;                                                                              // Enable the stop time tracking button
            }
        }

        public DateTime endTime = new DateTime();                                                                                   // Variable for the end time of the project tracking

        /// <summary>
        /// When the user clicks on this button, the project tracking will be stopped and the user<br></br>
        /// can enter notes for his / her project
        /// </summary>
        private void btn_stopTimeTracking_Click(object sender, RoutedEventArgs e) {
                endTime = DateTime.Now;                                                                                            // Set the end time to the current time
                NotesInput notesInput = new NotesInput();                                                                          // Create a new instance of the NotesInput window
                notesInput.ShowDialog();                                                                                           // Show the NotesInput
                if(records.Count == 0) {
                    ChangeToAttributeAt(1, endTime.ToString("HH:mm"), notesInput.note);                                            // Change the start time to the end time
                } else {
                    ChangeToAttributeAt(Convert.ToInt32(getNewestActivity()), endTime.ToString("HH:mm"), notesInput.note);
                }
                btn_trackTime.IsEnabled = true;
                btn_stopTimeTracking.IsEnabled = false;
                txt_recordingTime.Text = "0";
        }

        public List<Record> records = new List<Record>();

        /// <summary>
        /// The Record class is used to store the index of the XML node
        /// </summary>
        public class Record {
            public int index { get; set; }
            public Record(int index) {
                this.index = index;
            }
        }

        /// <summary>
        /// This method is used to get the index of the newest activity
        /// </summary>
        /// <returns></returns>
        string getNewestActivity() {
            records.Clear();                                                                                                        // Clear the records list
            XmlDocument xmlDocument = new XmlDocument();                                                                            // Create an XML document object
            xmlDocument.Load(file);                                                                                                 // Load the XML document from the specified file

            XmlNodeList recordNodes = xmlDocument.SelectNodes("//project");                                                         // Select all project nodes

            foreach (XmlNode recordNode in recordNodes) {
                int pos = Convert.ToInt32(recordNode.Attributes["position"].Value);
                records.Add(new Record(pos));                                                                                       // Add the position attribute to the records list
            }
            records.Sort((y, x) => y.index.CompareTo(x.index));                                                                     // Sort the records list by index
            int temp = 0;
            foreach (var record in records) {
                if(record.index > temp) {
                    temp = Convert.ToInt32(record.index);
                }
            }
            return temp.ToString();
        }

        /// <summary>
        /// This method is used to edit a project node from the XML file
        /// </summary>
        /// <param name="position">Project Id</param>
        /// <param name="value">Project name as string</param>
        /// <param name="note">Note</param>
        void ChangeToAttributeAt(int position, string value, string note) {
            try {
                XmlDocument xmlDoc = new XmlDocument();                                                                             // Create an XML document object
                xmlDoc.Load(file);                                                                                                  // Load the XML document from the specified file

                XmlNode projectNode = xmlDoc.SelectSingleNode("//project[@position='" + position.ToString() + "']");                // Select the project node by position attribute
                XmlNode notesNode = xmlDoc.SelectSingleNode("//project[@note='']");                                                 // Select its note attribute

                if (projectNode != null) {                                                                                          // If the project node exists
                    XmlElement projectElement = (XmlElement)projectNode;                                                            // Cast the node to an XML element
                    projectElement.SetAttribute("to", value);                                                                       // Set the to attribute to the specified value
                }

                if (notesNode != null) {                                                                                            // If the project node exists
                    XmlElement noteElement = (XmlElement)notesNode;                                                                 // Cast the node to an XML element
                    if (note != null) {                                                                                             // If the note is not null...
                        noteElement.SetAttribute("note", note);                                                                     // ... Set the note attribute to the specified value
                    } else {
                        noteElement.SetAttribute("note", "");                                                                       // Else, Set the note attribute to the specified value
                    }
                }

                xmlDoc.Save(file);                                                                                                  // Save the XML modified document to the specified file
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        /// <summary>
        /// This method is used to insert a new project node into the XML file
        /// </summary>
        /// <param name="projectName"> The project name</param>
        /// <param name="startTime">Start time</param>
        void InsertNode(string projectName, string startTime) {
            try {
                XmlDocument xmlDoc = new XmlDocument();                                                                             // Create an XML document object
                xmlDoc.Load(file);                                                                                                  // Load the XML document from the specified file

                XmlElement newProjectElement = xmlDoc.CreateElement("project");                                                     // Create a new project element

                int newNodeIndex = Convert.ToInt32(getNewestActivity()) + 1;                                                        // Get the index of the newest activity and add 1
                newProjectElement.SetAttribute("position", newNodeIndex.ToString());                                                // Set the new position index
                newProjectElement.SetAttribute("name", projectName);                                                                // Set the name attribute to the specified value
                newProjectElement.SetAttribute("from", startTime);                                                                  // Set the from attribute to the specified value
                newProjectElement.SetAttribute("to", "");                                                                           // Set the to attribute to the specified value
                newProjectElement.SetAttribute("note", "");                                                                         // Set the note attribute to the specified value

                XmlElement rootElement = xmlDoc.DocumentElement;                                                                    // Get the root element

                rootElement.AppendChild(newProjectElement);                                                                         // Append the new project element to the root element

                string modifiedXml = xmlDoc.OuterXml;                                                                               // Get the modified XML
                xmlDoc.Save(file);                                                                                                  // Save the XML modified document to the specified file
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }
    }
}