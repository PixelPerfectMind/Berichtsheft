using Berichtsheft.Classes;
using Berichtsheft.Dialogs;
using Berichtsheft.Pages;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace Berichtsheft {

    public partial class MainWindow : Window {
        private readonly FileActions fileActions = new FileActions(); // Create a new instance of the FileActions class

        // Set the path to the documents folder
        public string path;

        public MainWindow() {
            try {
                InitializeComponent(); // Initialize the window
                path = fileActions.path; // Set the path to the root folder
                fr_main.Content = new Dashboard(); // Set the start page to the frame
                fileActions.CreateWorkDirectory(); // Create the work directory
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        private void txt_windowTitle_MouseDown(object sender, MouseButtonEventArgs e) {
            // Check if the left mouse button is pressed
            if(Mouse.LeftButton == MouseButtonState.Pressed) {
                DragMove(); // Move the window
            } else if (Mouse.RightButton == MouseButtonState.Pressed) {
                // When the right mouse button is pressed, show the context menu
                ShowContextMenu();
            }
        }

        private async void btn_closeWindow_Click(object sender, RoutedEventArgs e) {
            await Task.Delay(200); // Wait 200ms for the animation to finish
            Close(); // Close the window
        }

        private void btn_maximizeWindow_Click(object sender, RoutedEventArgs e) {
            try {
                // Check if the window is maximized
                if(this.WindowState == WindowState.Normal) {
                    btn_maximizeWindow.Content = "🗗"; // Change the icon of the button
                    Thickness margin = new Thickness(6) { Bottom = (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom) - 30 }; // Create a new margin
                    brdr_windowBorder.Margin = margin; // Apply the newer margin to the window
                    this.WindowState = WindowState.Maximized; // Maximize the window
                } else {
                    try {
                        this.WindowState = WindowState.Normal; // Set the window state to normal
                        btn_maximizeWindow.Content = "🗖"; // Change the icon of the button
                        brdr_windowBorder.Margin = new Thickness(10); // Set the margin of the window
                    } catch (Exception) {
                        // Unfortunately, on some systems, the window can't be maximized, because of a performance problem
                        if(System.Windows.MessageBox.Show("Aufgrund eines Performance-Problems ist diese Anwendung abgestürzt.\nWollen Sie die Anwendung neu starten?", "Ups!", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes) {
                            // Attempt to restart the application
                            Launch launch = new Launch();
                            launch.Show();
                        } else {
                            Environment.Exit(-1); // Close the application
                        }
                    }
                }
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        private void btn_minimizeWindow_Click(object sender, RoutedEventArgs e) { this.WindowState = WindowState.Minimized; } // Minimize the window

        private void brdr_contextMenuBorder_MouseDown(object sender, MouseButtonEventArgs e) { brdr_contextMenuBorder.Visibility = Visibility.Collapsed; } // Hide the context menu

        private void grd_windowInner_MouseDown(object sender, MouseButtonEventArgs e) {
            if(Mouse.RightButton == MouseButtonState.Pressed) { ShowContextMenu(); } // Show the context menu when the right mouse button is pressed
        }

        void ShowContextMenu() {
            try {
                brdr_contextMenuBorder.Visibility = Visibility.Visible; // Makes the context menu visible
                Point mousePosition = Mouse.GetPosition(brdr_contextMenuBorder); // Get the current mouse position
                bc_contextMenu.Margin = new Thickness(mousePosition.X, mousePosition.Y, 0, 0); // Set the margin of the context menu
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        private void btn_start_Click(object sender, RoutedEventArgs e) {
            // Check if the frame is already set to the dashboard
            if (fr_main.Content.GetType() == typeof(Dashboard)) {
                _ = (Dashboard)fr_main.Content; // Set current Dashboard to the frame
            } else {
                fr_main.Content = new Dashboard(); // Create a new instance of the dashboard
            }
        }

        private void btn_allProjects_Click(object sender, RoutedEventArgs e) {
            if (fr_main.Content.GetType() == typeof(AllProjects)) {
                _ = (AllProjects)fr_main.Content; // Set current AllProjects to the frame
            } else {
                fr_main.Content = new AllProjects(); // Create a new instance of AllProjects
            }
        }

        private void btn_dayHistory_Click(object sender, RoutedEventArgs e) {
            if(fr_main.Content.GetType() == typeof(ProjectTracking)) {
                _ = (ProjectTracking)fr_main.Content; // Set current ProjectTracking to the frame
            } else {
                fr_main.Content = new ProjectTracking(); // Create a new instance of ProjectTracking
            }
        }

        private void btn_allNotes_Click(object sender, RoutedEventArgs e) {
            if (fr_main.Content.GetType() == typeof(AllNotes)) {
                _ = (AllNotes)fr_main.Content; // Set current AllNotes to the frame
            } else {
                fr_main.Content = new AllNotes(); // Create a new instance of AllNotes
            }
        }

        private void btn_allEntries_Click(object sender, RoutedEventArgs e) {
            if (fr_main.Content.GetType() == typeof(AllReports)){
                _ = (AllReports)fr_main.Content; // Set current AllReports to the frame
            } else {
                fr_main.Content = new AllReports(); // Create a new instance of AllReports
            }
        }

        private void window_SizeChanged(object sender, SizeChangedEventArgs e) {
            try {
                // Check if the height is greater than the width
                // If so, set the blur radius to 6000 / the width of the window...
                // If not, set the blur radius to 6000 / the height of the window...
                // This is to save performance, when the window is resized
                if(this.Height >= this.Width) {
                    blr_backgroundImage.Radius = 6000 / this.Width; 
                } else if(this.Width >= this.Height) {
                    blr_backgroundImage.Radius = 6000 / this.Height;
                }
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        /// <summary>
        /// Happens when the window is in focus.<br></br>
        /// Refreshes data in several pages.
        /// </summary>
        private void window_Activated(object sender, EventArgs e) {
            try {
                if (fr_main.Content.GetType() == typeof(Dashboard)) {
                    Dashboard dashboard = fr_main.Content as Dashboard;
                    dashboard?.ReloadNoteList();
                    dashboard?.ShowNewestProject();
                }
            } catch { }
        }

        private void btn_search_Click(object sender, RoutedEventArgs e) {
            SearchWindow searchWindow = new SearchWindow();
            searchWindow.ShowDialog();
        }
    }
}