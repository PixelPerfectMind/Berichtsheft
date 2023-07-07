using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Berichtsheft.UserControls.NoteUserControls {
    public partial class TextElement : UserControl {

        public int id;
        public TextElement(int id, string text, string[] attr, string[] attrContents) {
            InitializeComponent();
            this.id = id;
            txt.Text = text;
            ApplyFontStyle(attr, attrContents);
        }

        void ApplyFontStyle(string[] attr, string[] attrContents) {
            // Loop through all attributes
            for (int i = 0; i < attr.Length; i++) {
                // Check if the note has a preset attribute
                if (attr[i] == "preset") {
                    if (attrContents[i] == "h1") { txt.FontSize = 24; txt.FontWeight = FontWeights.Bold; }
                    else if (attrContents[i] == "h2") { txt.FontSize = 20; txt.FontWeight = FontWeights.Bold; }
                    else if (attrContents[i] == "h3") { txt.FontSize = 18; txt.FontWeight = FontWeights.Bold; }
                    else if (attrContents[i] == "h4") { txt.FontSize = 16; txt.FontWeight = FontWeights.Bold; }
                    else if (attrContents[i] == "h5") { txt.FontSize = 14; txt.FontWeight = FontWeights.Bold; }
                    else if (attrContents[i] == "h6") { txt.FontSize = 12; txt.FontWeight = FontWeights.Bold; }
                    else { Console.WriteLine("Unknown preset: " + attrContents[i]); }
                }

                // Check if the note has a text-underline attribute
                if (attr[i] == "text-underline") {
                    if (attrContents[i] == "underline") { txt.TextDecorations = TextDecorations.Underline; }
                    else if (attrContents[i] == "strikethrough") { txt.TextDecorations = TextDecorations.Strikethrough; }
                    else if (attrContents[i] == "overline") { txt.TextDecorations = TextDecorations.Underline; }
                    else if (attrContents[i] == "baseline") { txt.TextDecorations = TextDecorations.Baseline; }
                    else { Console.WriteLine("Unknown text-underline: " + attrContents[i]); }
                }

                // Check if the note has a font-weight attribute
                if (attr[i] == "font-weight") {
                    if (attrContents[i] == "bold") { txt.FontWeight = FontWeights.Bold; }
                    else if (attrContents[i] == "thin") { txt.FontWeight = FontWeights.Thin; }
                    else if (attrContents[i] == "light") { txt.FontWeight = FontWeights.Light; }
                    else if (attrContents[i] == "extralight") { txt.FontWeight = FontWeights.ExtraLight; }
                    else if (attrContents[i] == "demibold") { txt.FontWeight = FontWeights.DemiBold; }
                    else if (attrContents[i] == "medium") { txt.FontWeight = FontWeights.Medium; }
                    else if (attrContents[i] == "heavy") { txt.FontWeight = FontWeights.Heavy; }
                    else if (attrContents[i] == "black") { txt.FontWeight = FontWeights.Black; }
                    else if (attrContents[i] == "extrablack") { txt.FontWeight = FontWeights.ExtraBlack; }
                    else { Console.WriteLine("Unknown font-weight: " + attrContents[i]); }
                }

                // Check if the note has a font-style attribute
                if (attr[i] == "font-style") {
                    if (attrContents[i] == "italic") { txt.FontStyle = FontStyles.Italic; }
                    else if (attrContents[i] == "normal") { txt.FontStyle = FontStyles.Normal; }
                    else if (attrContents[i] == "oblique") { txt.FontStyle = FontStyles.Oblique; }
                    else { Console.WriteLine("Unknown font-style: " + attrContents[i]); }
                }

                // Check if the note has a font-size attribute
                if (attr[i] == "font-size") {
                    try {
                        // Apply the font-size from string to double
                        txt.FontSize = Convert.ToDouble(attrContents[i]);
                    } catch {
                        txt.FontSize = 16;
                        Console.WriteLine("Incorrect font-size: " + attrContents[i]);
                    }
                }

                // Check if the note has a background-color attribute
                if (attr[i] == "forgeround-color") {
                    try {
                        // Apply the foreground color from string
                        txt.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attrContents[i]));
                    } catch {
                        txt.Foreground = new SolidColorBrush(Colors.Black);
                        Console.WriteLine("Incorrect foreground-color: " + attrContents[i]);
                    }
                }   
            }
        }

        private void StackPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) {
            btn_edit.Visibility = Visibility.Visible;
            brdr_highlight.Visibility = Visibility.Visible;
        }

        private void StackPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) {
            btn_edit.Visibility = Visibility.Collapsed;
            brdr_highlight.Visibility = Visibility.Collapsed;
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)  {
            Dialogs.Notes.EditTextNoteProperty editTextNoteProperty = new Dialogs.Notes.EditTextNoteProperty();
            editTextNoteProperty.txt_textContent.Text = txt.Text;
            editTextNoteProperty.txt_textSize.Text = txt.FontSize.ToString();
            editTextNoteProperty.Show();
        }
    }
}