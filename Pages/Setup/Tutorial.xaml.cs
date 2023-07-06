using Berichtsheft.Dialogs;
using System;
using System.IO;
using System.Windows.Controls;
using System.Xml;

namespace Berichtsheft.Pages.Setup {

    public partial class Tutorial : Page {

        public string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft";
        public Tutorial() {
            InitializeComponent();
            // Create all needed directories
            try {
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                    Directory.CreateDirectory(path + @"\Notes");
                    Directory.CreateDirectory(path + @"\Notes\Images");
                    Directory.CreateDirectory(path + @"\Project tracking");
                    Directory.CreateDirectory(path + @"\Report books");


                    // Write neccessary files
                    XmlDocument xmlDocument = new XmlDocument();
                    XmlNode workDay = xmlDocument.CreateElement("projects");
                    xmlDocument.AppendChild(workDay);
                    xmlDocument.Save(path + @"\Project tracking\Projects.xml");
                }
            } catch (Exception ex) {
                Dialogs.ExceptionWindow exceptionWindow = new Dialogs.ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }

            // Zugriffslogik für das Element ausführen
            ((Launch)System.Windows.Application.Current.MainWindow).btn_next.Content = "Sehr gut!";
            ((Launch)System.Windows.Application.Current.MainWindow).btn_back.IsEnabled = false;
            SubMainWIndow smw = new SubMainWIndow();
            smw.Show();
        }
    }
}
