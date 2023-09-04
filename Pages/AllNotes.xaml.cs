using Berichtsheft.Classes;
using Berichtsheft.Dialogs;
using Berichtsheft.UserControls;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Xml;

namespace Berichtsheft.Pages {

    public partial class AllNotes : Page {

        readonly string path = @"\Notes";

        /// <summary>
        /// This is the code behind for the AllNotes page.
        /// </summary>
        public AllNotes() {
            try {
                FileActions fileActions = new FileActions();                                                                        // Create Instance of FileActions to get the root path
                path = fileActions.path + path;
                InitializeComponent();
                ReloadNoteList();
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        /// <summary>
        /// This method reloads the list which contains all notes.
        /// </summary>
        public async void ReloadNoteList() {
            int noteIndex = 0;
            try {
                sp_myNotes.Children.Clear();
                foreach(var notes in Directory.GetFiles(path)) {
                    string note = notes.Replace(path + @"\", "");                                                                   // Remove the path from the note file name
                    note = note.Replace(".xml", "");

                    if (noteIndex == 0) {
                        sp_myNotes.Children.Add(new NoteOverviewItem(note, 0));
                    } else if (noteIndex == Directory.GetFiles(path).Length) {
                        sp_myNotes.Children.Add(new NoteOverviewItem(note, 2));
                    } else {
                        sp_myNotes.Children.Add(new NoteOverviewItem(note));
                    }
                    
                    noteIndex++;

                    await Task.Delay(90);                                                                                           // 10ms delay to finish the animation
                }
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        private void cb_showNoteCreationDialog_Checked(object sender, RoutedEventArgs e) { brdr_newNoteDialog.Visibility = Visibility.Visible; }
        private async void cb_showNoteCreationDialog_Unchecked(object sender, RoutedEventArgs e) {
            await Task.Delay(100); // 100ms delay to finish the animation
            brdr_newNoteDialog.Visibility = Visibility.Collapsed;
        }

        private void brdr_newNoteDialog_MouseDown(object sender, MouseButtonEventArgs e) { cb_showNoteCreationDialog.IsChecked = false; }
        private void btn_newNote_Click(object sender, RoutedEventArgs e) { cb_showNoteCreationDialog.IsChecked = true; txt_newNoteName.Text = ""; }

        /// <summary>
        /// Prevents the user from entering forbidden characters in the note name.
        /// </summary>
        private void txt_newNoteName_TextChanged(object sender, TextChangedEventArgs e) {
            try {
                txt_forbiddenCharacters.Visibility = Visibility.Visible;
                btn_createNote.IsEnabled = false;
                string tmp = txt_newNoteName.Text;
                if(tmp == "") {                                                                                                     // Appears when the TextBox is empty
                    txt_forbiddenCharacters.Text = "Der Notizname darf nicht leer sein";
                } else if(tmp.Contains("\\") || tmp.Contains("/") || tmp.Contains(":") || tmp.Contains("*") ||
                    tmp.Contains("?") || tmp.Contains("\"") || tmp.Contains("<") || tmp.Contains(">") || tmp.Contains("|")) {       // Appears when the TextBox contains forbidden characters
                    txt_forbiddenCharacters.Text = "Der Name darf keine der folgenden Zeichen enthalten: \\ / : * ? \" < > |";
                } else if (Directory.GetFiles(path).Contains(path + @"\" + txt_forbiddenCharacters.Text + @".xml")) {               // Appears when the TextBox contains a note name which already exists
                    txt_forbiddenCharacters.Text = "Es existiert bereits eine Noiz mit diesem Namen";
                } else {
                    txt_forbiddenCharacters.Visibility = Visibility.Collapsed;
                    btn_createNote.IsEnabled = true;
                }
            } catch { }
        }

        private void btn_createNote_Click(object sender, RoutedEventArgs e) {
            try {
                XmlDocument xmlDocument = new XmlDocument();                                                                        // Create a new XmlDocument
                XmlNode noteNode = xmlDocument.CreateElement("note");                                                               // Create root node

                XmlAttribute id = xmlDocument.CreateAttribute("id");                                                                // Create note attributes
                int directoryLenght = Directory.GetFiles(path).Length + 1;
                id.Value = directoryLenght.ToString();                                                                              // Set the value of the attribute

                XmlAttribute color = xmlDocument.CreateAttribute("color");
                color.Value = "#FFFFE0";

                noteNode.Attributes.Append(id);                                                                                     // Append the attributes to the root node
                noteNode.Attributes.Append(color);

                xmlDocument.AppendChild(noteNode);                                                                                  // Append the root node to the XmlDocument
                xmlDocument.Save(path + @"\" + txt_newNoteName.Text + ".xml");                                                      // Save the XmlDocument
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
            cb_showNoteCreationDialog.IsChecked = false;
            ReloadNoteList();
        }

        /// <summary>
        /// Shows and hides the scroll indicators
        /// </summary>
        private void scr_ScrollChanged(object sender, ScrollChangedEventArgs e) {

            // For the top border
            if (scr.VerticalOffset == 0) {
                // Height animation border to 0
                DoubleAnimation heightAnimation = new DoubleAnimation {
                    To = 0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.1))
                };
                brdr_topScrollIndicator.BeginAnimation(Border.HeightProperty, heightAnimation);
            } else if (scr.VerticalOffset > 0) {
                // Height animation border to 10
                DoubleAnimation heightAnimation = new DoubleAnimation {
                    To = 20,
                    Duration = new Duration(TimeSpan.FromSeconds(0.1))
                };
                brdr_topScrollIndicator.BeginAnimation(Border.HeightProperty, heightAnimation);
            }

            // For the bottom border
            if (scr.VerticalOffset == scr.ScrollableHeight) {
                // Height animation border to 0
                DoubleAnimation heightAnimation = new DoubleAnimation {
                    To = 0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.1))
                };
                brdr_bottomScrollIndicator.BeginAnimation(Border.HeightProperty, heightAnimation);
            } else if (scr.VerticalOffset < scr.ScrollableHeight) {
                // Height animation border to 10
                DoubleAnimation heightAnimation = new DoubleAnimation
                {
                    To = 20,
                    Duration = new Duration(TimeSpan.FromSeconds(0.1))
                };
                brdr_bottomScrollIndicator.BeginAnimation(Border.HeightProperty, heightAnimation);
            }
        }
    }
}
