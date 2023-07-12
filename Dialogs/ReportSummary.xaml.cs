using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Berichtsheft.Dialogs {

    public partial class ReportSummary : Window {
        public ReportSummary() {
            InitializeComponent();
        }

        private async void btn_closeWindow_Click(object sender, RoutedEventArgs e) {
            await Task.Delay(70);
            this.Close();
        }

        private void btn_maximizeWindow_Click(object sender, RoutedEventArgs e) {
            if (this.WindowState == WindowState.Normal) {
                btn_maximizeWindow.Content = "🗗";
                Thickness margin = new Thickness(6) { Bottom = (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom) - 30 };
                brdr_windowBorder.Margin = margin;
                this.WindowState = WindowState.Maximized;
            } else {
                try {
                    this.WindowState = WindowState.Normal;
                    btn_maximizeWindow.Content = "🗖";
                    brdr_windowBorder.Margin = new Thickness(10);
                } catch (Exception) {
                    if (System.Windows.MessageBox.Show("Aufgrund eines Performance-Problems ist diese Anwendung abgestürzt.\nWollen Sie die Anwendung neu starten?", "Ups!", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes) {
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Close();
                    } else {
                        Environment.Exit(-1);
                    }
                }
            }
        }

        private void btn_maximizeWindow_Copy_Click(object sender, RoutedEventArgs e) { this.WindowState = WindowState.Minimized; }

        private void txt_windowTitle_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if(System.Windows.Input.Mouse.LeftButton == System.Windows.Input.MouseButtonState.Pressed) {
                DragMove();
            }
        }

        private void dp_week_CalendarClosed(object sender, RoutedEventArgs e) {
            if (dp_week.SelectedDate.Value.DayOfWeek != DayOfWeek.Monday) {
                dp_week.SelectedDate = dp_week.SelectedDate.Value.AddDays(-(int)dp_week.SelectedDate.Value.DayOfWeek + (int)DayOfWeek.Monday);
            }
            System.Windows.MessageBox.Show("Bitte wählen Sie einen gültigen Montag aus.", "Das geht so leider nicht!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void btn_selectLastMonday_Click(object sender, RoutedEventArgs e) {
            dp_week.SelectedDate = dp_week.SelectedDate.Value.AddDays(-(int)dp_week.SelectedDate.Value.DayOfWeek + (int)DayOfWeek.Monday);
        }
    }
}
