using Berichtsheft.Dialogs;
using Berichtsheft.Pages;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Berichtsheft {

    public partial class MainWindow : Window {

        // Set the path to the documents folder
        public string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft";

        public MainWindow() {
            InitializeComponent();
            fr_main.Content = new Dashboard(); // Set the start page to the frame

            try {
                if(!Directory.Exists(path)) { // Check if the folder exists
                    Directory.CreateDirectory(path); // Create the root folder
                    Directory.CreateDirectory(path + @"\Report books"); // Create the report books folder
                    Directory.CreateDirectory(path + @"\Project tracking"); // Create the projects tracker folder
                    Directory.CreateDirectory(path + @"\Notes"); // Create the notes folder
                    File.Create(path + @"\Project tracking\Projects.xml"); // Create the projects file
                }
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        private void txt_windowTitle_MouseDown(object sender, MouseButtonEventArgs e) {
            if(Mouse.LeftButton == MouseButtonState.Pressed) {
                DragMove();
            } else if (Mouse.RightButton == MouseButtonState.Pressed) {
                  brdr_contextMenuBorder.Visibility = Visibility.Visible;
                // Get the current mouse position
                Point mousePosition = Mouse.GetPosition(brdr_contextMenuBorder);

                // Display the mouse coordinates
                Console.WriteLine("Mouse X: " + mousePosition.X);
                Console.WriteLine("Mouse Y: " + mousePosition.Y);

                bc_contextMenu.Margin = new Thickness(mousePosition.X, mousePosition.Y, 0, 0);
            }
        }

        private async void btn_closeWindow_Click(object sender, RoutedEventArgs e) {
            await Task.Delay(200);
            Close();
        }

        private void btn_maximizeWindow_Click(object sender, RoutedEventArgs e) {
            if(this.WindowState == WindowState.Normal) {
                btn_maximizeWindow.Content = "🗗";
                Thickness margin = new Thickness(6);
                margin.Bottom = (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom) - 30;
                brdr_windowBorder.Margin = margin;
                this.WindowState = WindowState.Maximized;
            } else {
                try {
                    this.WindowState = WindowState.Normal;
                    btn_maximizeWindow.Content = "🗖";
                    brdr_windowBorder.Margin = new Thickness(10);
                } catch (Exception) {
                    if(System.Windows.MessageBox.Show("Aufgrund eines Performance-Problems ist diese Anwendung abgestürzt.\nWollen Sie die Anwendung neu starten?", "Ups!", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes) {
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Close();
                    } else {
                        Environment.Exit(-1);
                    }
                }
            }
        }

        private void btn_minimizeWindow_Click(object sender, RoutedEventArgs e) { this.WindowState = WindowState.Minimized; }

        private void brdr_contextMenuBorder_MouseDown(object sender, MouseButtonEventArgs e) { brdr_contextMenuBorder.Visibility = Visibility.Collapsed; }

        private void grd_windowInner_MouseDown(object sender, MouseButtonEventArgs e) {
            if(Mouse.RightButton == MouseButtonState.Pressed) { ShowContextMenu(); }
        }

        void ShowContextMenu() {
            // Get the current mouse position
            Point mousePosition = Mouse.GetPosition(brdr_contextMenuBorder);

            // Display the mouse coordinates
            Console.WriteLine("Mouse X: " + mousePosition.X);
            Console.WriteLine("Mouse Y: " + mousePosition.Y);

            bc_contextMenu.Margin = new Thickness(mousePosition.X, mousePosition.Y, 0, 0);
            brdr_contextMenuBorder.Visibility = Visibility.Visible;
        }

        private void btn_start_Click(object sender, RoutedEventArgs e) {
            if (fr_main.Content.GetType() == typeof(Dashboard)) {
                Dashboard dashboard = (Dashboard)fr_main.Content;
            } else {
                fr_main.Content = new Dashboard();
            }
        }

        private void btn_allProjects_Click(object sender, RoutedEventArgs e) {
            if (fr_main.Content.GetType() == typeof(AllProjects)) {
                AllProjects allProjects = (AllProjects)fr_main.Content;
            } else {
                fr_main.Content = new AllProjects();
            }
        }

        private void btn_dayHistory_Click(object sender, RoutedEventArgs e) {
            if(fr_main.Content.GetType() == typeof(ProjectTracking)) {
                ProjectTracking projectTracking = (ProjectTracking)fr_main.Content;
            } else {
                fr_main.Content = new ProjectTracking();
            }
        }

        private void btn_allNotes_Click(object sender, RoutedEventArgs e) {
            if (fr_main.Content.GetType() == typeof(AllNotes)) {
                AllNotes allNotes = (AllNotes)fr_main.Content;
            } else {
                fr_main.Content = new AllNotes();
            }
        }
    }
}
