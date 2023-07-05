using Berichtsheft.Dialogs;
using Berichtsheft.UserControls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace Berichtsheft.Pages {

    public partial class AllProjects : Page {

        public string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft\Project tracking";
        public string file = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft\Project tracking\Projects.xml";
        public AllProjects() {
            InitializeComponent();
            ReadProjectsFromXML();
            AddToList();
        }

        public class Project {
            public Project(string name, string description) {
                this.name = name;
                this.description = description;
            }
            public string name { get; set; }
            public string description { get; set; }
        }   

        public List<Project> projects = new List<Project>();
        void ReadProjectsFromXML() {
            // Foreach node in the XML file
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(file);

            XmlNodeList projectNodes = xmlDocument.SelectNodes("//project");

            foreach (XmlNode projectNode in projectNodes) {
                string name = projectNode.Attributes["name"].Value;
                string description = projectNode.Attributes["description"].Value;
                projects.Add(new Project(name, description));
            }
            projects.Sort((x, y) => x.name.CompareTo(y.name));
        }

        private async void AddToList() {
            Project[] tempProjects = new Project[projects.Count];
            int lastItem = projects.Count - 1;
            for (int i = 0; i < projects.Count; i++) {
                tempProjects[i] = projects[i];
                if(i == lastItem) {
                    sp_myProjects.Children.Add(new ProjectManagementItem(tempProjects[i].name, tempProjects[i].description, 2));
                } else if(i == 0) {
                    sp_myProjects.Children.Add(new ProjectManagementItem(tempProjects[i].name, tempProjects[i].description, 0));
                } else {
                    sp_myProjects.Children.Add(new ProjectManagementItem(tempProjects[i].name, tempProjects[i].description));
                }
                await Task.Delay(90);
            }
        }

        private void btn_newProject_Click(object sender, RoutedEventArgs e) {
            NewProject newProject = new NewProject();
            newProject.ShowDialog();
            projects.Clear();
            sp_myProjects.Children.Clear();
            ReadProjectsFromXML();
            AddToList();
        }
    }
}
