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
    /// <summary>
    /// Interaktionslogik für Launch.xaml
    /// </summary>
    public partial class Launch : Window {
        public Launch() {
            InitializeComponent();
            if(DateTime.Now.ToFileTimeUtc() > Properties.Settings.Default.expDate.ToFileTimeUtc()) {
                fr_setup.Content = new Berichtsheft.Pages.Setup.VersionExpiredä();
                btn_continue.Visibility = Visibility.Collapsed;
                btn_next.Visibility = Visibility.Collapsed;
                btn_back.Visibility = Visibility.Collapsed;
            } else {
                fr_setup.Content = new Berichtsheft.Pages.Setup.Welcome();
            }
        }

        private void btn_continue_Click(object sender, RoutedEventArgs e) {
            SubMainWIndow subMainWIndow = new SubMainWIndow();
            subMainWIndow.Show();
            Close();
        }
    }
}
