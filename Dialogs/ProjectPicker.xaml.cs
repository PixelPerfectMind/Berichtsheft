using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace Berichtsheft.Dialogs {

    public partial class ProjectPicker : Window {
        public string selectedProject { get; set;}

        public string file = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft\Project tracking\Projects.xml";
        public ProjectPicker() {
            InitializeComponent();
            ReadProjectsFromXML();
        }


        void ReadProjectsFromXML() {
            try {
                // Foreach node in the XML file
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(file);

                XmlNodeList projectNodes = xmlDocument.SelectNodes("//project");

                foreach (XmlNode projectNode in projectNodes) {
                    string name = projectNode.Attributes["name"].Value;
                    lb_projectList.Items.Add(name); // Add the project to the listbox
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
        }

        private void lb_projectList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if(lb_projectList.SelectedItem != null) {
                btn_continue.IsEnabled = true;
            } else {
                btn_continue.IsEnabled = false;
            }
        }

        private void btn_continue_Click(object sender, RoutedEventArgs e) {
            selectedProject = lb_projectList.SelectedItem.ToString();
            this.DialogResult = true;
        }
    }
}
