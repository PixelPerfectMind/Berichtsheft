using Berichtsheft.Classes;
using Berichtsheft.Pages.Setup;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Berichtsheft.Dialogs {

    public partial class Launch : Window {

        private readonly FileActions fileActions = new FileActions();

        /// <summary>
        /// This is the constructor of the Launch window.<br></br>
        /// Herewith, the app checks if the initial setup is<br></br>
        /// done and if the time bomb has expired (only in<br></br>
        /// beta versions).<br></br><br></br>When the setup is not done, the<br></br>
        /// setup window will be shown. When the time bomb has<br></br>
        /// been expired, the expired window will be shown.<br></br>
        /// Otrherwise, the main window will be shown.
        /// </summary>
        public Launch() {
            try {
                DefaultValues defaultValues = new DefaultValues();
                // If Left shift key is pressed on startup, reset the app
                if (Keyboard.IsKeyDown(Key.LeftShift)) {                                                        // Check if the left shift key is pressed, then show a message box
                    if(MessageBox.Show("Willst du die Anwendung komplett zurücksetzen?", "Problembehandlung", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)  {
                        fileActions.ResetApplication();                                                         // Reset the app
                        InitializeComponent();                                                                  // Initialize the window
                    }
                }

                // Check if the version has a time bomb
                if(defaultValues.IsDevRelease() == true) {
                    // Then, Check if the time bomb has expired
                    if (DateTime.Now.ToFileTimeUtc() > defaultValues.TimeBomb().ToFileTimeUtc()) {              // If the check passes, show the expired page
                        ShowExpired();
                    } else {                                                                                    // Else, Check if the setup is done
                        if (Properties.Settings.Default.setupDone == false) {                                   // When setup is not done, show the welcome page
                            ShowSetup();
                        } else {
                            ShowSubMainWindow();                                                                // Else, show the main window and close this window
                        }
                    }
                } else {
                    if (Properties.Settings.Default.setupDone == false) {                                       // Else, Check if the setup is done
                        ShowSetup();                                                                            // When setup is not done, show the welcome page
                    } else {
                        ShowSubMainWindow();                                                                    // Else, show the main window and close this window
                    }
                }
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        private void ShowExpired() {                                                                            // This is called, when the time bomb has expired
            try {
                InitializeComponent();                                                                          // Initialize the window
                fr_setup.Content = new VersionExpiredä();                                                       // Set the start page to the frame
                btn_next.Visibility = Visibility.Collapsed;                                                     // Hide the next button
                btn_back.Visibility = Visibility.Collapsed;                                                     // Hide the back button
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        private void ShowSetup() {                                                                              // This is called, when the app is started for the first time or after a reset
            try {
                InitializeComponent();                                                                          // Initialize the window
                fr_setup.Content = new Welcome();                                                               // Set the 'welcome' page to the frame
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        private void ShowSubMainWindow() {                                                                      // This is called, when the app is started normally
            try {
                SubMainWIndow smw = new SubMainWIndow();                                                        // Create a new instance of the sub-main window
                smw.Show();                                                                                     // Show the new window
                Close();                                                                                        // Close this window
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        private void btn_next_Click(object sender, RoutedEventArgs e) {
            if(fr_setup.Content.GetType() == typeof(Welcome)) {                                                 // Check if the current page is the welcome page
                fr_setup.Content = new Tutorial();                                                              // Then, set the tutorial page to the frame
            } else if(fr_setup.Content.GetType() == typeof(Tutorial)) {                                         // Otherwise, check if the current page is the tutorial page
                Properties.Settings.Default.setupDone = true;                                                   // Then, set the setupDone setting to true
                Properties.Settings.Default.Save();                                                             // Save the settings
                btn_closeWindow.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));                         // Close the window
            }
        }

        private void btn_back_Click(object sender, RoutedEventArgs e) {
            if(fr_setup.CanGoBack) {                                                                            // Check if the current page is the tutorial page
                fr_setup.GoBack();                                                                              // Then, go back to the welcome page
            }
        }

        private async void btn_closeWindow_Click(object sender, RoutedEventArgs e) {
            await System.Threading.Tasks.Task.Delay(100);                                                       // Wait 100ms for the animation to finish
            Close();                                                                                            // Close the window
        }

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (System.Windows.Input.Mouse.LeftButton == System.Windows.Input.MouseButtonState.Pressed) {       // Check if the left mouse button is pressed
                DragMove();                                                                                     // Then, move the window by dragging it
            }
        }
    }
}