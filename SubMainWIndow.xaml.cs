using Berichtsheft.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using MessageBox = System.Windows.MessageBox;
using System.Collections.Generic;

namespace Berichtsheft {

    public partial class SubMainWIndow : Window {

        public string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft\Project tracking";
        public string file = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft\Project tracking\ProjectTracking-" + DateTime.Now.ToString("dd.MM.yyyy") + @".xml";

        public SubMainWIndow() {
            InitializeComponent();

            // Set window position to bottom right
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - 220;
            this.Top = desktopWorkingArea.Bottom - 260;

            // Set the stop tracking button to disabled
            btn_stopTimeTracking.IsEnabled = false;

            // Create empty project tracking file if it doesn't exist
            try {
                if (!System.IO.File.Exists(file)) {
                    XmlDocument xmlDocument = new XmlDocument();
                    XmlElement root = xmlDocument.CreateElement("workDay");
                    xmlDocument.AppendChild(root);
                    xmlDocument.Save(file);
                }
            } catch { }
        }

        public int projectTimerSeconds = 0;

        private void chkbox_minimized_Checked(object sender, RoutedEventArgs e) { brdr_smallMenu.Visibility = Visibility.Visible; }

        private async void chkbox_minimized_Unchecked(object sender, RoutedEventArgs e) {
            await Task.Delay(100);
            brdr_smallMenu.Visibility = Visibility.Collapsed;
        }

        private void brdr_minimized_MouseDown(object sender, MouseButtonEventArgs e) {
            if(chkbox_minimized.IsChecked == true) {
                chkbox_minimized.IsChecked = false;
                brdr_smallWindow.Visibility = Visibility.Collapsed;
            } else {
                chkbox_minimized.IsChecked = true;
                brdr_smallWindow.Visibility = Visibility.Visible;
            }
        }

        private void btn_hideMiniWindow_Click(object sender, RoutedEventArgs e) {
            cb_showMiniWindow.IsChecked = false;
            //sp_main.Margin = new Thickness(0, 0, 0, 0);
        }

        private void btn_showMiniWindow_Click(object sender, RoutedEventArgs e) {
            //sp_main.Margin = new Thickness(0, 0, 0, -150);
            cb_showMiniWindow.IsChecked = true;
        }

        private void cb_showMiniWindow_Checked(object sender, RoutedEventArgs e) {
            brdr_smallWindow.Visibility = Visibility.Visible;
        }

        private async void cb_showMiniWindow_Unchecked(object sender, RoutedEventArgs e) {
            await Task.Delay(200);
            brdr_smallWindow.Visibility = Visibility.Collapsed;
        }

        private void btn_closeApp_Click(object sender, RoutedEventArgs e) {
            Environment.Exit(0);
        }

        private void btn_openFullApp_Click(object sender, RoutedEventArgs e) {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        public System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public DateTime startTime = new DateTime();
        private void btn_trackTime_Click(object sender, RoutedEventArgs e) {
            ProjectPicker projectPicker = new ProjectPicker();
            projectPicker.ShowDialog();
            if(projectPicker.DialogResult == true && projectPicker.selectedProject != "") {
                startTime = DateTime.Now;
                InsertNode(projectPicker.selectedProject, startTime.ToString("HH:mm"));
                projectTimerSeconds = 0;
                dispatcherTimer.Tick += new EventHandler(TimerTick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();
                btn_trackTime.IsEnabled = false;
                btn_stopTimeTracking.IsEnabled = true;
            } else {
            }
        }

        void TimerTick(object sender, EventArgs e) {
            projectTimerSeconds += 1;
            txt_recordingTime.Text = projectTimerSeconds.ToString();
        }

        public DateTime endTime = new DateTime();
        private void btn_stopTimeTracking_Click(object sender, RoutedEventArgs e) {
            if(dispatcherTimer.IsEnabled == true) {
                endTime = DateTime.Now;
                dispatcherTimer.Stop();
                NotesInput notesInput = new NotesInput();
                notesInput.ShowDialog();
                if(records.Count == 0) {
                    ChangeToAttributeAt(1, endTime.ToString("HH:mm"), notesInput.note);
                } else {
                    ChangeToAttributeAt(Convert.ToInt32(getNewestActivity()), endTime.ToString("HH:mm"), notesInput.note);
                }
                btn_trackTime.IsEnabled = true;
                btn_stopTimeTracking.IsEnabled = false;
                txt_recordingTime.Text = "0";
            }
        }

        public List<Record> records = new List<Record>();
        public class Record {
            public int index { get; set; }
            public Record(int index) {
                this.index = index;
            }
        }

        string getNewestActivity() {
            records.Clear();
            // Foreach node in the XML file
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(file);

            XmlNodeList recordNodes = xmlDocument.SelectNodes("//project");

            foreach (XmlNode recordNode in recordNodes) {
                int pos = Convert.ToInt32(recordNode.Attributes["position"].Value);
                records.Add(new Record(pos));
            }
            records.Sort((y, x) => y.index.CompareTo(x.index));
            int temp = 0;
            foreach (var record in records) {
                if(record.index > temp) {
                    temp = Convert.ToInt32(record.index);
                }
            }
            return temp.ToString();
        }

        void ChangeToAttributeAt(int position, string value, string note) {
            try {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(file);

                // Find the project element with position="3"
                XmlNode projectNode = xmlDoc.SelectSingleNode("//project[@position='" + position.ToString() + "']");
                XmlNode notesNode = xmlDoc.SelectSingleNode("//project[@note='']");

                if (projectNode != null) {
                    XmlElement projectElement = (XmlElement)projectNode;
                    projectElement.SetAttribute("to", value);
                }

                if (notesNode != null) {
                    XmlElement noteElement = (XmlElement)notesNode;
                    if (note != null) {
                        noteElement.SetAttribute("note", note);
                    } else {
                        noteElement.SetAttribute("note", "");
                    }
                }

                // Save the modified XML
                xmlDoc.Save(file);

            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        void InsertNode(string projectName, string startTime) {
            try {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(file);

                // Create a new project element
                XmlElement newProjectElement = xmlDoc.CreateElement("project");

                // Set attributes of the new project element
                int newNodeIndex = Convert.ToInt32(getNewestActivity()) + 1;
                newProjectElement.SetAttribute("position", newNodeIndex.ToString());
                newProjectElement.SetAttribute("name", projectName);
                newProjectElement.SetAttribute("from", startTime);
                newProjectElement.SetAttribute("to", "");
                newProjectElement.SetAttribute("note", "");

                // Get the root element
                XmlElement rootElement = xmlDoc.DocumentElement;

                // Append the new project element to the root element
                rootElement.AppendChild(newProjectElement);

                // Save the modified XML
                string modifiedXml = xmlDoc.OuterXml;
                xmlDoc.Save(file);
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }
    }
}
