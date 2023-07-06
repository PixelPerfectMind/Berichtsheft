using Berichtsheft.Pages.Setup;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Berichtsheft.Dialogs {

    public partial class Launch : Window {
        public Launch() {
            InitializeComponent();

            // For Alpha releases
            if(DateTime.Now.ToFileTimeUtc() > Properties.Settings.Default.expDate.ToFileTimeUtc()) {
                fr_setup.Content = new VersionExpiredä();
                btn_next.Visibility = Visibility.Collapsed;
                btn_back.Visibility = Visibility.Collapsed;
            } else {
                if(Properties.Settings.Default.setupDone == false) {
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

        private void btn_next_Click(object sender, RoutedEventArgs e) {
            if(fr_setup.Content.GetType() == typeof(Welcome)) {
                fr_setup.Content = new Tutorial();
            } else if(fr_setup.Content.GetType() == typeof(Tutorial)) {
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
    }
}
