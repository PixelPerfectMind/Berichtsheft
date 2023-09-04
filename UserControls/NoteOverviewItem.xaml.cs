using Berichtsheft.Classes;
using Berichtsheft.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Berichtsheft.UserControls {

    public partial class NoteOverviewItem : UserControl {

        public NoteOverviewItem(string noteName, int position = 1) {
            InitializeComponent();
            txt_noteName.Text = noteName;

            if (position == 0) {
                brdr_main.CornerRadius = new CornerRadius(10, 10, 0, 0);
            } else if (position == 2) {
                brdr_main.CornerRadius = new CornerRadius(0, 0, 10, 10);
            }
        }

        private async void btn_deleteNote_Click(object sender, RoutedEventArgs e) {
            ModalDialog modalDialog = new ModalDialog("Willst du die Notiz mit dem Namen \"" + txt_noteName.Text + "\" wirklich löschen?", "Notiz löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            modalDialog.ShowDialog();
            if (modalDialog.Result == MessageBoxResult.Yes) {
                try {
                    FileActions fileActions = new FileActions();
                    System.IO.File.Delete(fileActions.path + @"\Notes\" + txt_noteName.Text + ".xml");

                    chkb_projectActive.IsChecked = false;

                    await Task.Delay(100);

                    ((StackPanel)this.Parent).Children.Remove(this);
                } catch (Exception ex) {
                    ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                    exceptionWindow.ShowDialog();
                }
            }
        }

        private void grd_showNoteEditor_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            NoteEditor noteEditor = new NoteEditor(txt_noteName.Text);
            noteEditor.ShowDialog();
        }
    }
}
