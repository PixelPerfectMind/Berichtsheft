using System;
using System.Windows;
using System.Windows.Input;

namespace Berichtsheft.Dialogs {

    public partial class ExceptionWindow : Window {

        public ExceptionWindow(Exception exc) {
            InitializeComponent();
            txt_message.Text = exc.Message;
            txt_stackTrace.Text = exc.StackTrace.ToString();
        }

        public bool showStackTrace = false;
        private void txt_showStackTrace_MouseDown(object sender, MouseButtonEventArgs e) {
            if(showStackTrace) {
                txt_stackTrace.Visibility = Visibility.Collapsed;
                txt_showStackTrace.Text = "Stacktrace einblenden";
                showStackTrace = false;
            } else {
                txt_stackTrace.Visibility = Visibility.Visible;
                txt_showStackTrace.Text = "Stacktrace ausblenden";
                showStackTrace = true;
            }
        }

        private async void btn_ok_Click(object sender, RoutedEventArgs e) {
            await System.Threading.Tasks.Task.Delay(100);
            Close();
        }
    }
}
