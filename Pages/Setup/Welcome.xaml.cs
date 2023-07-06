using System.Windows.Controls;

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
