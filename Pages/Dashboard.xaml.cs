using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Xml;
using Berichtsheft.Classes;
using Berichtsheft.Dialogs;
using Berichtsheft.Dialogs.Notes;

namespace Berichtsheft.Pages {

    public partial class Dashboard : Page {

        public string path = "";

        /// <summary>
        /// The constructor of the dashboard page.<br></br>
        /// The dashboard is intended to be the first page the user sees<br></br>
        /// after opening the MainWindow.
        /// </summary>
        public Dashboard() {
            try {
                FileActions fileActions = new FileActions();
                path = fileActions.path + @"\Project tracking\ProjectTracking-" + DateTime.Now.ToString("dd.MM.yyyy") + @".xml";

                InitializeComponent();
                ShowNewestProject();
                ReloadNoteList();

                txt_name.Text = "Hallo, " + Environment.UserName + "!";
                txt_date.Text = "Heute ist " + DateTime.Now.ToString("dddd") + ", der " + DateTime.Now.ToString("dd. MMMM yyyy");
            } catch (Exception ex) { 
                ExceptionWindow exceptionDialog = new ExceptionWindow(ex);
                exceptionDialog.ShowDialog();
            }
        }

        /// <summary>
        /// Displays the current project name 
        /// </summary>
        public void ShowNewestProject() {
            try {
                txt_currentProject.Text = "Ein Fehler beim Abrufen der Informationen ist aufgetreten!";
                txt_currentlyYouWorkAtThis.Visibility = System.Windows.Visibility.Collapsed;
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(path);
                XmlNodeList xmlNodes = xmlDocument.SelectNodes("//project");
                foreach (XmlNode node in xmlNodes) {
                    XmlAttributeCollection xmlAttributes = node.Attributes;
                    if (xmlAttributes["to"].Value == "" && Convert.ToInt16(xmlAttributes["position"].Value) == getNewestActivity()) {
                        txt_currentProject.Text = xmlAttributes["name"].Value + " - Seit " + xmlAttributes["from"].Value + " Uhr";
                        txt_currentlyYouWorkAtThis.Visibility = System.Windows.Visibility.Visible;
                        break;
                    } else {
                        txt_currentProject.Text = "Aktuell wird anscheinend kein Projekt bearbeitet";
                    }
                }
            } catch (Exception ex) {
                ExceptionWindow exceptionDialog = new ExceptionWindow(ex);
                exceptionDialog.ShowDialog();
            }
        }



        /// <summary>
        /// This method is used to get the index of the newest activity
        /// </summary>
        /// <returns></returns>
        int getNewestActivity() {
            XmlDocument xmlDocument = new XmlDocument();                                                                            // Create an XML document object
            xmlDocument.Load(path);                                                                                                 // Load the XML document from the specified file

            XmlNodeList recordNodes = xmlDocument.SelectNodes("//project");                                                         // Select all project nodes

            int temp = 0;
            foreach (XmlNode recordNode in recordNodes) {
                int pos = Convert.ToInt32(recordNode.Attributes["position"].Value);
                if(pos > temp) {
                    temp = pos;
                }
            }
            return temp;
        }


        /// <summary>
        /// This method reloads the list which contains all notes.
        /// </summary>
        public void ReloadNoteList() {
            try {
                FileActions fileActions = new FileActions();                                                                        // Create Instance of FileActions to get the root path
                sp.Children.Clear();

                List<string> noteList = new List<string>();
                foreach (var notes in Directory.GetFiles(fileActions.path + @"\Notes")) {
                    noteList.Add(notes);
                }
                noteList.Sort((x, y) => DateTime.Compare(File.GetCreationTime(y), File.GetCreationTime(x)));                        // Sort by file creation date
                

                foreach (var notes in noteList) {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(notes);
                    XmlNodeList xmlNodes = xmlDocument.SelectNodes("//note");
                    string colorVal = "";
                    foreach (XmlNode node in xmlNodes) {                                                                            // Get the background color value of the note
                        XmlAttributeCollection xmlAttributes = node.Attributes;
                        if (xmlAttributes["color"].Value != "") {
                            colorVal = xmlAttributes["color"].Value;
                            continue;
                        }
                    }
                    xmlDocument.Save(notes);

                    string note = notes.Replace(fileActions.path + @"\Notes\", "");
                    note = note.Replace(".xml", "");     

                    DashboardNoteItem dashboardNoteItem = new DashboardNoteItem(note, colorVal);
                    sp.Children.Add(dashboardNoteItem);
                }
            } catch { }
        }
    }
}
