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
using System.Xml;
using TextElement = Berichtsheft.UserControls.NoteUserControls.TextElement;

namespace Berichtsheft.Dialogs {

    public partial class NoteEditor : Window {

        public string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Berichtsheft\Notes";


        public int id;
        public string title;
        public string file;
        public NoteEditor(string title) {
            InitializeComponent();
            this.title = title;
            txt_title.Text = title;
            this.file = path + @"\" + title + @".xml";

            brdr_editor.Visibility = Visibility.Collapsed;

            try {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(file);
                ExtractSpanNodesFromXML(xmlDocument);

                XmlNodeList noteNote = xmlDocument.SelectNodes("/note");
                foreach (XmlNode noteNode in noteNote) {
                    id = Convert.ToInt32(noteNode.Attributes["id"].Value);
                    brdr_postit.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(noteNode.Attributes["color"].Value));
                }
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        public void ExtractSpanNodesFromXML(XmlDocument xmlDocument) {
            contents.Children.Clear();
            try {
                // Declare temporary lists
                List<string> attributes = new List<string>();
                List<string> values = new List<string>();

                // Extract all span nodes from the XML file

                // Get all span nodes
                XmlNodeList spanNodes = xmlDocument.SelectNodes("//span");

                // Loop through all span nodes
                foreach (XmlNode spanNode in spanNodes) {
                    attributes.Clear();
                    values.Clear();

                    // Get the span node's attributes
                    XmlAttributeCollection spanAttributes = spanNode.Attributes;
                    foreach (XmlAttribute spanAttribute in spanAttributes) {
                        // Add the attribute to the list
                        attributes.Add(spanAttribute.Name);
                        values.Add(spanAttribute.Value);
                    }

                    // Declare array variables
                    string[] attributesArray = new string[attributes.Count];
                    string[] valuesArray = new string[values.Count];

                    //Append valöues to array
                    for (int i = 0; i < attributes.Count; i++) {
                        attributesArray[i] = attributes[i];
                        valuesArray[i] = values[i];
                    }

                    // Create new TextElement
                    TextElement textElement = new TextElement(id, spanNode.InnerText, attributesArray, valuesArray);
                    contents.Children.Add(textElement);
                }
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
            
        }

        private void txt_title_MouseDown(object sender, MouseButtonEventArgs e) {
            if(Mouse.LeftButton == MouseButtonState.Pressed) {
                DragMove();
            }
        }

        private void cb_editorVisible_Checked(object sender, RoutedEventArgs e) { brdr_editor.Visibility = Visibility.Visible; }
        private async void cb_editorVisible_Unchecked(object sender, RoutedEventArgs e) {
            await Task.Delay(70);
            brdr_editor.Visibility = Visibility.Collapsed;
        }

        private void btn_closeEditor_Click(object sender, RoutedEventArgs e) { cb_editorVisible.IsChecked = false; }
        private void btn_openEditor_Click(object sender, RoutedEventArgs e) { cb_editorVisible.IsChecked = true; }

        private void btn_closeNote_Click(object sender, RoutedEventArgs e) { Close(); }

        private void btn_addText_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument noteXML = new XmlDocument();
            noteXML.Load(file);
            //new xml node
            XmlElement span = noteXML.CreateElement("span");
            span.InnerText = "test";

            XmlAttribute id = noteXML.CreateAttribute("id");
            id.Value = "0";
            //new xml attribute

            span.Attributes.Append(id);

            noteXML.DocumentElement.AppendChild(span);
            noteXML.Save(file);


            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(file);
            ExtractSpanNodesFromXML(xmlDocument);
        }

        public void AnimateColorChange(string newColor= "#FFFAE75C") {
            SolidColorBrush initialColor = (SolidColorBrush)brdr_postit.Background;

            // Set the initial color for the border
            brdr_postit.Background = initialColor;

            // Create a ColorAnimation to animate the color
            ColorAnimation colorAnimation = new ColorAnimation();
            colorAnimation.From = initialColor.Color;
            colorAnimation.To = (Color)ColorConverter.ConvertFromString(newColor);
            colorAnimation.Duration = TimeSpan.FromSeconds(1);  // Replace 2 with the duration of the animation in seconds

            // Apply the animation to the border's Background property
            Storyboard.SetTarget(colorAnimation, brdr_postit);
            Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("Background.Color"));

            // Create a Storyboard and add the ColorAnimation to it
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(colorAnimation);

            // Start the animation
            storyboard.Begin();
        }

        private void ell_bkgCol_gold_MouseDown(object sender, MouseButtonEventArgs e) { AnimateColorChange("#FFDFB200"); }
        private void ell_bkgCol_cyan_MouseDown(object sender, MouseButtonEventArgs e) { AnimateColorChange("#47bed2"); }
        private void ell_bkgCol_lemongreen_MouseDown(object sender, MouseButtonEventArgs e) { AnimateColorChange("#9dca53"); }
        private void ell_bkgCol_lightpurple_MouseDown(object sender, MouseButtonEventArgs e) { AnimateColorChange("#f87b8d"); }
        private void ell_bkgCol_orange_MouseDown(object sender, MouseButtonEventArgs e) { AnimateColorChange("#e6986a"); }
    }
}
