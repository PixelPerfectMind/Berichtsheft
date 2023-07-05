using System;
using System.Windows;
using System.Xml;

namespace Berichtsheft.Dialogs {

    public partial class NewProject : Window {

        public string file = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft\Project tracking\Projects.xml";
        public NewProject() {
            InitializeComponent();
        }
        public NewProject(double parentWindowTop, double ParentWindowLeft)
        {
            InitializeComponent();
        }

        private void btn_confirm_Click(object sender, RoutedEventArgs e) {
            if(txt_projectName.Text != "" && !projectWithThisNameAlreatyExists(txt_projectName.Text)) {
                try {
                    // Write xml node to xml file
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(file);
                    XmlNode rootNode = xmlDocument.SelectSingleNode("projects");
                    XmlElement projectNode = xmlDocument.CreateElement("project");
                    projectNode.SetAttribute("name", txt_projectName.Text);
                    projectNode.SetAttribute("description", txt_projectDescription.Text);
                    rootNode.AppendChild(projectNode);
                    xmlDocument.Save(file);
                    this.Close();
                } catch (Exception ex) {
                    MessageBox.Show("Das Projekt konnte nicht angelegt werden.\n" + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } else {
                MessageBox.Show("Gib zumindest einen gültigen und einzigartigen Projektnamen ein.", "Das geht so leider nicht!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        bool projectWithThisNameAlreatyExists(string projectName) {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(file);
            XmlNodeList projectNodes = xmlDocument.SelectNodes("//project");
            foreach (XmlNode projectNode in projectNodes) {
                if (projectNode.Attributes["name"].Value == projectName) {
                    return true; // Project with this name already exists
                }
            }
            return false; //The project doesn't exist yet
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
