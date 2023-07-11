using Berichtsheft.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;

namespace Berichtsheft.UserControls.NoteUserControls {
    public partial class TextElement : UserControl {

        public int id;
        public string noteName;
        public TextElement(int id, string noteName, string text, string[] attr, string[] attrContents) {
            InitializeComponent();
            this.id = id;
            this.noteName = noteName;
            txt.Text = text;
            sp_buttons.Visibility = Visibility.Collapsed;
            brdr_highlight.Visibility = Visibility.Collapsed;
            ApplyFontStyle(attr, attrContents);
        }

        void ApplyFontStyle(string[] attr, string[] attrContents) {
            // Loop through all attributes
            for (int i = 0; i < attr.Length; i++) {

                if (attr[i] == "id") {
                    try {
                        this.id = Convert.ToInt16(attrContents[i]);
                    } catch { }
                }
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
                    else if (attrContents[i] == "overline") { txt.TextDecorations = TextDecorations.OverLine; }
                    else if (attrContents[i] == "baseline") { txt.TextDecorations = TextDecorations.Baseline; }
                    else { Console.WriteLine("Unknown text-underline: " + attrContents[i]); }
                }

                // Check if the note has a font-weight attribute
                if (attr[i] == "font-weight") {
                    if (attrContents[i] == "bold") { txt.FontWeight = FontWeights.Bold; }
                    else if (attrContents[i] == "light") { txt.FontWeight = FontWeights.Light; }
                    else if (attrContents[i] == "heavy") { txt.FontWeight = FontWeights.Heavy; }
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
                if (attr[i] == "foreground-color") {
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
            sp_buttons.Visibility = Visibility.Visible;
            brdr_highlight.Visibility = Visibility.Visible;
        }

        private async void StackPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) {
            await System.Threading.Tasks.Task.Delay(100);
            sp_buttons.Visibility = Visibility.Collapsed;
            brdr_highlight.Visibility = Visibility.Collapsed;
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)  {
            Dialogs.Notes.EditTextNoteProperty editTextNoteProperty = new Dialogs.Notes.EditTextNoteProperty(noteName);
            editTextNoteProperty.txt_textContent.Text = txt.Text;
            editTextNoteProperty.txt_textSize.Text = txt.FontSize.ToString();
            editTextNoteProperty.elementId = id;
            editTextNoteProperty.txt_color.Text = txt.Foreground.ToString();
            editTextNoteProperty.ShowDialog();
        }

        public string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft\Notes";
        private void btn_delete_Click(object sender, RoutedEventArgs e) {
            try {
                XmlDocument doc = new XmlDocument();
                doc.Load(path + @"\" + noteName + ".xml");
                XmlNodeList nodes = doc.DocumentElement.SelectNodes("/note/span");
                foreach (XmlNode node in nodes) {
                    if (node.Attributes["id"].Value == id.ToString()) {
                        node.ParentNode.RemoveChild(node);
                        doc.Save(path + @"\" + noteName + ".xml");
                        break;
                    }
                }
                ((StackPanel)this.Parent).Children.Remove(this);
            } catch { }
        }
    }
}