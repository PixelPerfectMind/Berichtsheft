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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Berichtsheft.Pages {

    public partial class AllNotes : Page {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft\Notes";
        public AllNotes() {
            InitializeComponent();
            foreach(var notes in Directory.GetFiles(path)) {
                string note = notes.Replace(path + @"\", "");
                note = note.Replace(".xml", "");
                lb_allNotes.Items.Add(note);
            }
        }

        private void lb_allNotes_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            NoteEditor noteEditor = new NoteEditor(lb_allNotes.SelectedItem.ToString());
            noteEditor.Show();
        }
    }
}
