using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;
using Color = System.Drawing.Color;

namespace Berichtsheft.Dialogs.Notes {

    public partial class DashboardNoteItem : UserControl {

        public DashboardNoteItem(string noteName, string color="#FFFFE0") {
            InitializeComponent();
            txt.Text = noteName;
            ell.Fill = (Brush)new BrushConverter().ConvertFromString(color);
                                                                                                                    // Create Darker Variant of the color for the border
            Color darkerVariant = ColorTranslator.FromHtml(color);                                                  // Convert the color from the string to a System.Drawing.Color
            darkerVariant = Color.FromArgb(255, darkerVariant.R - 32, darkerVariant.G - 32, darkerVariant.B - 32);  // Create a darker variant of the color
            ell.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(darkerVariant.A,
                darkerVariant.R, darkerVariant.G, darkerVariant.B));                                                // Set the border color
        }

        private void btn_item_Click(object sender, System.Windows.RoutedEventArgs e) {
            try {
                NoteEditor noteEditor = new NoteEditor(txt.Text);
                noteEditor.Show();
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }
    }
}
