using Berichtsheft.UserControls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace Berichtsheft.Pages {

    public partial class ProjectTracking : Page {

        // This is, how the ProjectTracking-xx.xx.xxxx.xml file looks like:
        // <workDay>
        //     <project position="1" name="Project name" note="Project note" from="Project start time" to="Project end time" />
        //     <project position="2" name="Project name" note="Project note" from="Project start time" to="Project end time" />
        //     ...
        // </workDay>

        public string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft\Project tracking"; // Set work directory

        public string file = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft\Project tracking\ProjectTracking-" + DateTime.Now.ToString("dd.MM.yyyy") + @".xml"; // Set today's file
        public ProjectTracking() {
            try {
                InitializeComponent();

                // Create empty project tracking file if it doesn't exist
                if(!System.IO.File.Exists(file)) {
                    System.IO.File.Create(file);
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(file);
                    XmlElement root = xmlDocument.CreateElement("workDay");
                    xmlDocument.AppendChild(root);
                    xmlDocument.Save(file);
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }

            // Add the available file names to the combobox
            foreach(var fileNames in System.IO.Directory.GetFiles(path)) {
                string fileName = fileNames.Replace(path + @"\", "").Replace("ProjectTracking-", "").Replace(".xml", "");
                // Do not add the "Projects.xml" file to the combobox
                if (fileName.Equals("Projects")) { continue; }

                if (fileName.Equals(DateTime.Now.ToString("dd.MM.yyyy"))) {
                    // Add the current day to the combobox
                    cb_dateSelection.Items.Add("Heute");
                } else {
                    // Add the other days to the combobox
                    cb_dateSelection.Items.Add(fileName);
                }
            }

            // Set the default date selection to today
            cb_dateSelection.SelectedIndex = cb_dateSelection.Items.IndexOf("Heute");
        }

        public class Record {
            public Record(int position, string name, string note, string from, string to="") {
                this.position = position;
                this.name = name;
                this.note = note;
                this.from = from;
                this.to = to;
            }
            public int position { get; set; }
            public string name { get; set; }
            public string note { get; set; }
            public string from { get; set; }
            public string to { get; set; }
        }
        public List<Record> records = new List<Record>();

        void ReadRecords(string pathToFile=null) {
            // Foreach node in the XML file
            XmlDocument xmlDocument = new XmlDocument();
            if (pathToFile != null ) {
                // Load the project tracking file from declared path
                xmlDocument.Load(pathToFile);
            } else {
                // Load the project tracking file from the current day
                xmlDocument.Load(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft\Project tracking\ProjectTracking-" + DateTime.Now.ToString("dd.MM.yyyy") + @".xml");
            }

            XmlNodeList recordNodes = xmlDocument.SelectNodes("//project");

            // Add the time records to the list
            foreach (XmlNode recordNode in recordNodes) {
                int pos = Convert.ToInt16(recordNode.Attributes["position"].Value);
                string name = recordNode.Attributes["name"].Value;
                string note = recordNode.Attributes["note"].Value;
                string from = recordNode.Attributes["from"].Value;
                string to = recordNode.Attributes["to"].Value;
                records.Add(new Record(pos, name, note, from, to));
            }

            // Sort the records by the position attribute
            records.Sort((x, y) => y.position.CompareTo(x.position));
        }

        void AddToList() {
            // Temporary list to work with indexes
            Record[] tempProjects = new Record[records.Count];

            // Add the ProjectTrackingElements to the StackPanel
            for (int i = 0; i < records.Count; i++) {
                tempProjects[i] = records[i];
                if (i == 0) {
                    if (tempProjects[i].to == "") {
                        // If the project is still running, set the last (top) element to "running"
                        sp_listItems.Children.Add(new ProjectTrackingElement(tempProjects[i].name, tempProjects[i].note, tempProjects[i].from, tempProjects[i].to, 2, true));
                    } else {
                        // If the project is finished, add it normally
                        sp_listItems.Children.Add(new ProjectTrackingElement(tempProjects[i].name, tempProjects[i].note, tempProjects[i].from, tempProjects[i].to, 2));
                    }
                    continue;
                } else if (i == records.Count - 1) {
                    // If this is the first element, mark it as the "start" element in the timeline
                    sp_listItems.Children.Add(new ProjectTrackingElement(tempProjects[i].name, tempProjects[i].note, tempProjects[i].from, tempProjects[i].to, 0));
                    continue;
                } else {
                    // Normal element
                    sp_listItems.Children.Add(new ProjectTrackingElement(tempProjects[i].name, tempProjects[i].note, tempProjects[i].from, tempProjects[i].to));
                    continue;
                }
            }
            if(records.Count == 0) {
                // If there are no records, show the "no items" text
                txt_noItems.Visibility = Visibility.Visible;
            } else {
                txt_noItems.Visibility = Visibility.Collapsed;
            }
        }

        private void cb_dateSelection_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            // Clear the list and the records
            sp_listItems.Children.Clear();
            records.Clear();
            if(cb_dateSelection.Items.GetItemAt(cb_dateSelection.SelectedIndex).ToString() == "Heute") {
                // Happens, when the user selects "Heute" in the combobox
                ReadRecords(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft\Project tracking\ProjectTracking-" + DateTime.Now.ToString("dd.MM.yyyy") + @".xml");
            } else {
                // Happens, when the user selects another day in the combobox
                ReadRecords(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft\Project tracking\ProjectTracking-" + cb_dateSelection.Items.GetItemAt(cb_dateSelection.SelectedIndex).ToString() + @".xml");
            }
            // Add the records to the StackPanel
            AddToList();
        }
    }
}