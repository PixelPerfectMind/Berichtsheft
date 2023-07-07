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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Berichtsheft.Dialogs.Notes {

    public partial class EditTextNoteProperty : Window {
        public EditTextNoteProperty() {
            InitializeComponent();
        }

        private void txt_textContent_TextChanged(object sender, TextChangedEventArgs e) { txt_preview.Text = txt_textContent.Text; }

        private void txt_textSize_TextChanged(object sender, TextChangedEventArgs e) {
            txt_sizeAlert.Visibility = Visibility.Collapsed;
            try {
                if(txt_textSize.Text != "") {
                    // Font resizing animation
                    double oldSize = Convert.ToDouble(txt_preview.FontSize); // Work variable
                    double newSize = Convert.ToDouble(txt_textSize.Text); // Work variable

                    DoubleAnimation animation = new DoubleAnimation {
                        From = oldSize, // Starting font size
                        To = newSize,   // Ending font size
                        Duration = new Duration(TimeSpan.FromSeconds(1)) // Animation duration
                    };

                    // Apply the animation to the target control
                    txt_preview.BeginAnimation(TextBlock.FontSizeProperty, animation);
                }
            } catch {
                txt_sizeAlert.Visibility = Visibility.Visible;
            }
        }
    }
}
