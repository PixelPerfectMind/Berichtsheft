using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Berichtsheft.UserControls {

    public partial class ProjectTrackingElement : UserControl {

        public ImageSource arrowImage_Up = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/arrowUp.png"));
        public ImageSource arrowImage_Normal = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/arrowLine.png"));
        public ImageSource arrowImage_Start = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/arrowStart.png"));

        public ProjectTrackingElement(string projectTitle, string note, string startTime, string endTime, int listLevel=1, bool isLastElement=false) {
            InitializeComponent();
            if(listLevel == 0) {
                img_arrow.Source = arrowImage_Start;
            } else if(listLevel == 2) {
                img_arrow.Source = arrowImage_Up;
            } else {
                img_arrow.Source = arrowImage_Normal;
            }

            if(isLastElement) {
                ell_projectStatus.Fill = Brushes.LightGreen;
                ell_projectStatus.ToolTip = "Als \"Wird aktuell\nbearbeitet\" markiert";
            }

            txtb_projectName.Text = projectTitle;
            if(note == "-") {
                txtb_projectDescription.Visibility = System.Windows.Visibility.Collapsed;
            } else {
                txtb_projectDescription.Text = note;
            }
            txt_startTime.Text = startTime;
            txt_endTime.Text = endTime;
        }


    }
}
