using System.Windows;

namespace Berichtsheft.Dialogs {

    public partial class NotesInput : Window {

        public string note { get; set; }
        public NotesInput() {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
            if(tb_noteText.Text.Replace(" ", "") == "") {
                note = "-";
            } else {
                note = tb_noteText.Text;
            }
            Close();
        }
    }
}
