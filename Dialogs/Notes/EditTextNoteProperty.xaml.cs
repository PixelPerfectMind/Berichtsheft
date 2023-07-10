using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml;

namespace Berichtsheft.Dialogs.Notes {

    public partial class EditTextNoteProperty : Window {
        public int elementId { get; set; }
        public string noteName;

        public EditTextNoteProperty(string noteName) {
            InitializeComponent();
            txt_color.Text = "#006B88";
            this.noteName = noteName;
        }

        private void txt_textContent_TextChanged(object sender, TextChangedEventArgs e) { txt_preview.Text = txt_textContent.Text; }

        private async void txt_textSize_TextChanged(object sender, TextChangedEventArgs e) {
            txt_sizeAlert.Visibility = Visibility.Collapsed;
            try {
                if(txt_textSize.Text != "") {
                    // Font resizing animation
                    double oldSize = Convert.ToDouble(txt_preview.FontSize); // Work variable
                    double newSize = Convert.ToDouble(txt_textSize.Text); // Work variable

                    DoubleAnimation animation = new DoubleAnimation {
                        From = oldSize, // Starting font size
                        To = newSize,   // Ending font size
                        Duration = new Duration(TimeSpan.FromSeconds(.25)) // Animation duration
                    };

                    // Apply the animation to the target control
                    txt_preview.BeginAnimation(TextBlock.FontSizeProperty, animation);
                    await Task.Delay(250);
                    txt_preview.FontSize = newSize;
                }
            } catch {
                txt_sizeAlert.Visibility = Visibility.Visible;
            }
        }

        //Text decorations radio buttons
        private void rb_underline1_Checked(object sender, RoutedEventArgs e) { txt_preview.TextDecorations = null; }
        private void rb_underline2_Checked(object sender, RoutedEventArgs e) { txt_preview.TextDecorations = TextDecorations.Underline; }
        private void rb_underline4_Checked(object sender, RoutedEventArgs e) { txt_preview.TextDecorations = TextDecorations.OverLine; }
        private void rb_underline3_Checked(object sender, RoutedEventArgs e) { txt_preview.TextDecorations = TextDecorations.Strikethrough; }
        private void rb_underline5_Checked(object sender, RoutedEventArgs e) { txt_preview.TextDecorations = TextDecorations.Baseline; }

        //Font weight radio buttons
        private void rb_testStretch3_Checked(object sender, RoutedEventArgs e) { txt_preview.FontWeight = FontWeights.Light; }
        private void rb_testStretch1_Checked(object sender, RoutedEventArgs e) { txt_preview.FontWeight = FontWeights.Regular; }
        private void rb_testStretch2_Checked(object sender, RoutedEventArgs e) { txt_preview.FontWeight = FontWeights.Bold; }
        private void rb_testStretch4_Checked(object sender, RoutedEventArgs e) { txt_preview.FontWeight = FontWeights.Heavy; }
        private void rb_testStretch5_Checked(object sender, RoutedEventArgs e) { txt_preview.FontWeight = FontWeights.ExtraBlack; }

        // Font style radio buttons
        private void rb_fontStyle1_Checked(object sender, RoutedEventArgs e) { txt_preview.FontStyle = FontStyles.Normal; }
        private void rb_fontStyle2_Checked(object sender, RoutedEventArgs e) { txt_preview.FontStyle = FontStyles.Italic; }
        private void rb_fontStyle3_Checked(object sender, RoutedEventArgs e) { txt_preview.FontStyle = FontStyles.Oblique; }

        private void txt_color_TextChanged(object sender, TextChangedEventArgs e) {
            try {
                // Apply the foreground color from string
                txt_preview.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(txt_color.Text));
                border_color.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(txt_color.Text));
            } catch {
                txt_color.Foreground = new SolidColorBrush(Colors.White);
                border_color.Background = new SolidColorBrush(Colors.White);
            }
        }

        public string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft\Notes";
        private void btn_apply_Click(object sender, RoutedEventArgs e) {
            try {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(path + @"\" + noteName + @".xml");
                XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//span");
                foreach (XmlNode xmlNode in xmlNodeList) {
                    if (xmlNode.Attributes["id"].Value == elementId.ToString()) {
                        // Change note properties
                        // If foreground-color attribute exists, change it, else create it
                        xmlNode.InnerText = txt_textContent.Text;
                        if (xmlNode.Attributes["foreground-color"] != null) {
                            xmlNode.Attributes["foreground-color"].Value = txt_color.Text;
                        } else {
                            XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("foreground-color");
                            xmlAttribute.Value = txt_color.Text;
                            xmlNode.Attributes.Append(xmlAttribute);
                        }

                        // If font-size attribute exists, change it, else create it
                        if (xmlNode.Attributes["font-size"] != null) {
                            xmlNode.Attributes["font-size"].Value = txt_textSize.Text;
                        } else {
                            XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("font-size");
                            xmlAttribute.Value = txt_textSize.Text;
                            xmlNode.Attributes.Append(xmlAttribute);
                        }

                        string underline = "";
                        if (rb_underline1.IsChecked == true) { underline = "none"; }
                        else if (rb_underline2.IsChecked == true) { underline = "underline"; }
                        else if (rb_underline3.IsChecked == true) { underline = "strikethrough"; }
                        else if (rb_underline4.IsChecked == true) { underline = "overline"; }
                        else if (rb_underline5.IsChecked == true) { underline = "baseline"; }
                        if (xmlNode.Attributes["text-underline"] != null) {
                            xmlNode.Attributes["text-underline"].Value = underline;
                        } else {
                            XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("text-underline");
                            xmlAttribute.Value = underline;
                            xmlNode.Attributes.Append(xmlAttribute);
                        }

                        string fontWeight = "";
                        if (rb_testStretch1.IsChecked == true) { fontWeight = "normal"; }
                        else if (rb_testStretch2.IsChecked == true) { fontWeight = "bold"; }
                        else if (rb_testStretch3.IsChecked == true) { fontWeight = "light"; }
                        else if (rb_testStretch4.IsChecked == true) { fontWeight = "heavy"; }
                        else if (rb_testStretch5.IsChecked == true) { fontWeight = "extrablack"; }
                        if (xmlNode.Attributes["font-weight"] != null) {
                            xmlNode.Attributes["font-weight"].Value = fontWeight;
                        } else {
                            XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("font-weight");
                            xmlAttribute.Value = fontWeight;
                            xmlNode.Attributes.Append(xmlAttribute);
                        }


                        string fontStyle = "";
                        if (rb_fontStyle1.IsChecked == true) { fontStyle = "normal"; }
                        else if (rb_fontStyle2.IsChecked == true) { fontStyle = "italic"; }
                        else if (rb_fontStyle3.IsChecked == true) { fontStyle = "oblique"; }
                        if (xmlNode.Attributes["font-style"] != null) {
                            xmlNode.Attributes["font-style"].Value = fontStyle;
                        } else {
                            XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("font-style");
                            xmlAttribute.Value = fontStyle;
                            xmlNode.Attributes.Append(xmlAttribute);
                        }

                        xmlDocument.Save(path + @"\" + noteName + @".xml");
                        break;
                    }
                }   
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
            QuitSafely();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e) { QuitSafely(); }

        public void QuitSafely() {
            this.Close();
        }
    }
}
