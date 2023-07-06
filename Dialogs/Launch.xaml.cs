using Berichtsheft.Pages.Setup;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Xml;

namespace Berichtsheft.Dialogs {

    public partial class Launch : Window {
        public Launch() {
            // if Left shift key is pressed, reset the app
            if(Keyboard.IsKeyDown(Key.LeftShift)) {
                if(MessageBox.Show("Willst du die Anwendung komplett zurücksetzen?", "Problembehandlung", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                ResetApplication();
                Properties.Settings.Default.Reset(); ;
                Properties.Settings.Default.Save();
                InitializeComponent();
            }

            // For Alpha releases
            if(DateTime.Now.ToFileTimeUtc() > Properties.Settings.Default.expDate.ToFileTimeUtc()) {
                InitializeComponent();
                fr_setup.Content = new VersionExpiredä();
                btn_next.Visibility = Visibility.Collapsed;
                btn_back.Visibility = Visibility.Collapsed;
            } else {
                if(Properties.Settings.Default.setupDone == false) {
                    InitializeComponent();
                    fr_setup.Content = new Berichtsheft.Pages.Setup.Welcome();
                } else {
                    SubMainWIndow smw = new SubMainWIndow();
                    smw.Show();
                    Close();
                }
            }

            // For regular releases
            //if(Properties.Settings.Default.setupDone == false) {
            // fr_setup.Content = new Berichtsheft.Pages.Setup.Welcome();
            //} else {
            // SubMainWIndow smw = new SubMainWIndow();
            // smw.Show();
            // Close();
            // }
        }

        public string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft";
        private void ResetApplication() {
            try {
                Properties.Settings.Default.Reset();
                Properties.Settings.Default.Save();

                Directory.Delete(path, true);
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
            } catch(Exception ex) {
                Dialogs.ExceptionWindow exceptionWindow = new Dialogs.ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        private void btn_next_Click(object sender, RoutedEventArgs e) {
            if(fr_setup.Content.GetType() == typeof(Welcome)) {
                fr_setup.Content = new Tutorial();
            } else if(fr_setup.Content.GetType() == typeof(Tutorial)) {
                Properties.Settings.Default.setupDone = true;
                Properties.Settings.Default.Save();
                btn_closeWindow.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }

        private void btn_back_Click(object sender, RoutedEventArgs e) {
            if(fr_setup.CanGoBack) {
                fr_setup.GoBack();
            }
        }

        private async void btn_closeWindow_Click(object sender, RoutedEventArgs e) {
            await System.Threading.Tasks.Task.Delay(100);
            Close();
        }

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (System.Windows.Input.Mouse.LeftButton == System.Windows.Input.MouseButtonState.Pressed) { 
                DragMove();
            }
        }
    }
}
