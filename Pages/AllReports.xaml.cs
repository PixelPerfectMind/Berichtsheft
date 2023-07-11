using Berichtsheft.Dialogs;
using System.Windows;
using System.Windows.Controls;

namespace Berichtsheft.Pages {

    public partial class AllReports : Page {
        public AllReports() {
            InitializeComponent();
        }

        private void btn_summarizeReport_Click(object sender, RoutedEventArgs e) {
            ReportSummary reportSummary = new ReportSummary();
            reportSummary.ShowDialog();
        }
    }
}
