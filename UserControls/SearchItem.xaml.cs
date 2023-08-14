using Berichtsheft.Dialogs;
using Berichtsheft.Pages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Berichtsheft.UserControls {

    public partial class SearchItem : UserControl {

        public string PathToContent { get; set; }
        public SearchItem(string name, string pathToContent) {
            InitializeComponent();
            txt_name.Text = name;
            if(name.Contains("ProjectTracking-")) {
                txt_name.Text = name.Replace("ProjectTracking-", "").Replace(".xml", "");
                img_thumbnail.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/timeline_white.png"));
                txt_type.Text = "Projekttracking-Eintrag";
            } else if(name == "Projects.xml") {
                img_thumbnail.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/category.png"));
                txt_type.Text = "Projektsammlung";
            } else if (pathToContent.Contains("\\Notes\\")) {
                txt_name.Text = name.Replace(".xml", "");
                img_thumbnail.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/sticky_note.png"));
                txt_type.Text = "Haftnotiz";
            } else { 
                img_thumbnail.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/unknown_file.png"));
                txt_type.Text = "Nicht identifierbare Datei";
            }
            PathToContent = pathToContent;
        }


        /// <summary>
        /// Opens the file or the corresponding window
        /// </summary>
        private void grd_resultDisplay_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            try {
                // Open the corresponding window
                if(txt_type.Text == "Projekttracking-Eintrag" || txt_type.Text == "Projektsammlung") {
                    Window window = new Window();
                    Frame frame = new Frame();

                    if(txt_type.Text == "Projekttracking-Eintrag") {

                        // Open the project tracking window
                        ProjectTracking projectTracking = new ProjectTracking();
                        frame.Content = projectTracking;
                        if(DateTime.Now.ToString("dd.MM.yyyy") == txt_name.Text) {
                            projectTracking.cb_dateSelection.SelectedIndex = projectTracking.cb_dateSelection.Items.IndexOf("Heute");
                        } else {
                            projectTracking.cb_dateSelection.SelectedIndex = projectTracking.cb_dateSelection.Items.IndexOf(txt_name.Text);
                        }
                        window.Title = "Projekttracking-Eintrag";
                    } else {

                        // Open the project collection window
                        frame.Source = new Uri("Pages/AllProjects.xaml", UriKind.Relative);
                        window.Title = "Projektsammlung";
                    }
                    window.Content = frame;
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.Width = 800;
                    window.Height = 600;
                    window.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 30, 30, 30));
                    window.Show();
                } else if(txt_type.Text == "Haftnotiz") {

                    // Open the note editor
                    NoteEditor noteEditor = new NoteEditor(txt_name.Text);
                    noteEditor.Show();
                } else {
                    // Run in explorer
                    System.Diagnostics.Process.Start(PathToContent);
                }
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.Show();
            }
        }
    }
}