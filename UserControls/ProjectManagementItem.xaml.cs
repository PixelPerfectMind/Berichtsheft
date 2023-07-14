using Berichtsheft.Dialogs;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace Berichtsheft.UserControls {

    public partial class ProjectManagementItem : UserControl {

        public string file = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft\Project tracking\Projects.xml";
        // position = 0 = first item; position = 1 = middle item; position = 2 = last item
        public ProjectManagementItem(string title, string description, int position=1) {
            InitializeComponent();
            txtb_projectName.Text = title;

            // If description is empty, hide textblock
            if(description == "") {
                txtb_projectDescription.Visibility = Visibility.Collapsed;
            } else {

                // If description is longer than 50 characters, cut it and add "..." at the end
                if (description.Length > 50) {
                    description = description.Substring(0, 50) + "...";
                }

                // Assign description to textblock
                txtb_projectDescription.Text = description;
            }
            if(position == 0) {
                brdr_main.CornerRadius = new CornerRadius(10, 10, 0, 0);
            } else if (position == 2) {
                brdr_main.CornerRadius = new CornerRadius(0, 0, 10, 10);
            }
        }

        private void btn_deleteProject_Click(object sender, RoutedEventArgs e) {
            ModalDialog modalDialog = new ModalDialog("Möchten Sie das Projekt \"" + txtb_projectName.Text + "\" wirklich löschen?", "Projekt löschen", MessageBoxButton.YesNo);
            modalDialog.ShowDialog();
            if (modalDialog.Result == MessageBoxResult.Yes) {
                try {
                    // Load the XML document
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(file);

                    // Find the project node with name="Test 2"
                    XmlNode projectNode = xmlDoc.SelectSingleNode("//project[@name='" + txtb_projectName.Text + "']");

                    // Remove the project node if found
                    if (projectNode != null)
                    {
                        XmlNode parentNode = projectNode.ParentNode;
                        parentNode.RemoveChild(projectNode);
                    }

                    // Save the modified XML to a new string
                    xmlDoc.Save(file);
                    chkb_projectActive.IsChecked = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Das Projekt konnte nicht gelöscht werden.\n" + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
