using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Berichtsheft.Dialogs {

    public partial class SearchWindow : Window {
        public SearchWindow() {
            InitializeComponent();
        }

        private async void btn_close_Click(object sender, RoutedEventArgs e) {
            await Task.Delay(70);
            Close();
        }

        private void txt_title_MouseDown(object sender, MouseButtonEventArgs e) {
            if(Mouse.LeftButton == MouseButtonState.Pressed) {
                DragMove();
            }
        }

        private void txt_search_TextChanged(object sender, TextChangedEventArgs e) {
            pb_search.Visibility = Visibility.Collapsed;
            cb_windowSizeAnimation.IsChecked = false;
        }

        private void btn_search_Click(object sender, RoutedEventArgs e) {
            pb_search.Visibility = Visibility.Visible;
            cb_windowSizeAnimation.IsChecked = true;
        }
    }
}
