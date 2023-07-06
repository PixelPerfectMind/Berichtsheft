using Berichtsheft.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace Berichtsheft.Pages {

    public partial class AllNotes : Page {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft\Notes";
        public AllNotes() {
            InitializeComponent();
            ReloadNoteList();
        }

        public void ReloadNoteList() {
            try {
                lb_allNotes.Items.Clear();
                foreach(var notes in Directory.GetFiles(path)) {
                    string note = notes.Replace(path + @"\", "");
                    note = note.Replace(".xml", "");
                    lb_allNotes.Items.Add(note);
                }
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        private void lb_allNotes_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            NoteEditor noteEditor = new NoteEditor(lb_allNotes.SelectedItem.ToString());
            noteEditor.Show();
        }

        private void cb_showNoteCreationDialog_Checked(object sender, RoutedEventArgs e) { brdr_newNoteDialog.Visibility = Visibility.Visible; }
        private async void cb_showNoteCreationDialog_Unchecked(object sender, RoutedEventArgs e) {
            await Task.Delay(100);
            brdr_newNoteDialog.Visibility = Visibility.Collapsed;
        }

        private void brdr_newNoteDialog_MouseDown(object sender, MouseButtonEventArgs e) { cb_showNoteCreationDialog.IsChecked = false; }
        private void btn_newNote_Click(object sender, RoutedEventArgs e) { cb_showNoteCreationDialog.IsChecked = true; txt_newNoteName.Text = ""; }

        private void txt_newNoteName_TextChanged(object sender, TextChangedEventArgs e) {
            txt_forbiddenCharacters.Visibility = Visibility.Visible;
            btn_createNote.IsEnabled = false;
            string tmp = txt_newNoteName.Text;
            if(tmp == "") {
                txt_forbiddenCharacters.Text = "Der Notizname darf nicht leer sein";
            } else if(tmp.Contains("\\") || tmp.Contains("/") || tmp.Contains(":") || tmp.Contains("*") || tmp.Contains("?") || tmp.Contains("\"") || tmp.Contains("<") || tmp.Contains(">") || tmp.Contains("|")) {
                txt_forbiddenCharacters.Text = "Der Name darf keine der folgenden Zeichen enthalten: \\ / : * ? \" < > |";
            } else if (Directory.GetFiles(path).Contains(path + @"\" + txt_forbiddenCharacters.Text + @".xml")) {
                txt_forbiddenCharacters.Text = "Es existiert bereits eine Noiz mit diesem Namen";
            } else {
                txt_forbiddenCharacters.Visibility = Visibility.Collapsed;
                btn_createNote.IsEnabled = true;
            }
        }

        private void btn_createNote_Click(object sender, RoutedEventArgs e) {
            try {
                XmlDocument xmlDocument = new XmlDocument();
                XmlNode noteNode = xmlDocument.CreateElement("note");

                XmlAttribute id = xmlDocument.CreateAttribute("id");
                int directoryLenght = Directory.GetFiles(path).Length + 1;
                id.Value = directoryLenght.ToString();

                XmlAttribute color = xmlDocument.CreateAttribute("color");
                color.Value = "#FFFFE0";

                noteNode.Attributes.Append(id);
                noteNode.Attributes.Append(color);

                xmlDocument.AppendChild(noteNode);
                xmlDocument.Save(path + @"\" + txt_newNoteName.Text + ".xml");
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
            cb_showNoteCreationDialog.IsChecked = false;
            ReloadNoteList();
        }
    }
}
