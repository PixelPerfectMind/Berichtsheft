using System;
using System.Windows.Controls;

namespace Berichtsheft.Pages
{

    public partial class Dashboard : Page {
        public Dashboard() {
            InitializeComponent();
            txt_name.Text = "Hallo, " + Environment.UserName + "!";
            txt_date.Text = "Heute ist " + DateTime.Now.ToString("dddd") + ", der " + DateTime.Now.ToString("dd. MMMM yyyy");
        }
    }
}
