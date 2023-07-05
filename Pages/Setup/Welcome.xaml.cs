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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Berichtsheft.Pages.Setup {

    public partial class Welcome : Page {
        public bool welcomeDone { get; set;}
        public Welcome() {
            welcomeDone = true;
            InitializeComponent();
            txt_version.Text = Properties.Settings.Default.appVersion;
        }
    }
}
