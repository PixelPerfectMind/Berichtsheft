using System.Windows.Controls;

namespace Berichtsheft.Pages.Setup {

    public partial class Welcome : Page {
        public bool welcomeDone { get; set;}
        public Welcome() {
            Classes.DefaultValues defaultValues = new Classes.DefaultValues();
            welcomeDone = true;
            InitializeComponent();
            txt_version.Text = defaultValues.ProgramVersion();
        }
    }
}
