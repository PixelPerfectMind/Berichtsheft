using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Berichtsheft.Dialogs {

    public partial class ModalDialog : Window {

        public MessageBoxResult Result { get; private set; }

        public ModalDialog(string message, string title="", MessageBoxButton messageBoxButton=MessageBoxButton.OK, MessageBoxImage messageBoxImage=MessageBoxImage.None) {
            InitializeComponent();
            txt_title.Text = title;
            txt_message.Text = message;

            switch (messageBoxButton) {
                case MessageBoxButton.OK:
                    btn_ok.Visibility = Visibility.Visible;
                    btn_yes.Visibility = Visibility.Collapsed;
                    btn_no.Visibility = Visibility.Collapsed;
                    btn_cancel.Visibility = Visibility.Collapsed;
                    break;
                case MessageBoxButton.YesNo:
                    btn_ok.Visibility = Visibility.Collapsed;
                    btn_yes.Visibility = Visibility.Visible;
                    btn_no.Visibility = Visibility.Visible;
                    btn_cancel.Visibility = Visibility.Collapsed;
                    break;
                case MessageBoxButton.OKCancel:
                    btn_ok.Visibility = Visibility.Visible;
                    btn_yes.Visibility = Visibility.Collapsed;
                    btn_no.Visibility = Visibility.Collapsed;
                    btn_cancel.Visibility = Visibility.Visible;
                    break;
                case MessageBoxButton.YesNoCancel:
                    btn_ok.Visibility = Visibility.Collapsed;
                    btn_yes.Visibility = Visibility.Visible;
                    btn_no.Visibility = Visibility.Visible;
                    btn_cancel.Visibility = Visibility.Visible;
                break;
            }

            switch (messageBoxImage) {
                case MessageBoxImage.None:
                    img_messageBoxImage.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private async void btn_close_Click(object sender, RoutedEventArgs e) {
            Result = MessageBoxResult.Cancel;
            await Task.Delay(70);
            this.Close();
        }

        private void txt_title_MouseDown(object sender, MouseButtonEventArgs e) {
            if(Mouse.LeftButton == MouseButtonState.Pressed) {
                this.DragMove();
            }
        }

        private async void btn_ok_Click(object sender, RoutedEventArgs e) {
            Result = MessageBoxResult.OK;
            await Task.Delay(70);
            Close();
        }
        private async void btn_yes_Click(object sender, RoutedEventArgs e) {
            Result = MessageBoxResult.Yes;
            await Task.Delay(70);
            Close();
        }
        private async void btn_no_Click(object sender, RoutedEventArgs e) {
            Result = MessageBoxResult.No;
            await Task.Delay(70);
            Close();
        }
        private async void btn_cancel_Click(object sender, RoutedEventArgs e) {
            Result = MessageBoxResult.Cancel;
            await Task.Delay(70);
            Close();
        }
    }
}
