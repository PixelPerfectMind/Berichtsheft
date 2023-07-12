using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml;
using TextElement = Berichtsheft.UserControls.NoteUserControls.TextElement;
using FileOptions = Berichtsheft.Classes.FileActions;

namespace Berichtsheft.Dialogs {

    public partial class NoteEditor : Window {

        public int id;
        public string title;
        public string file;

        public string path = "";
        public NoteEditor(string title) {
            try {
                FileOptions fileOptions = new FileOptions();
                path = fileOptions.path + @"\Notes";

                InitializeComponent();

                this.title = title;
                txt_title.Text = title;
                this.file = path + @"\" + title + @".xml";

                brdr_editor.Visibility = Visibility.Collapsed;

                ExtractSpanNodesFromXML();                                                              // Extract the span nodes from the XML file

                XmlDocument xmlDocument = new XmlDocument();                                            // Load the XML file for getting the background color
                xmlDocument.Load(file);
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


        /// <summary>
        /// Refreshes the note's contents and extracts the span nodes from the XML file
        /// </summary>
        public void ExtractSpanNodesFromXML() {
            try {
                contents.Children.Clear();
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(file);

                List<string> attributes = new List<string>();
                List<string> values = new List<string>();

                XmlNodeList spanNodes = xmlDocument.SelectNodes("//span");                                  // Get all span nodes

                // Loop through all span nodes
                foreach (XmlNode spanNode in spanNodes) {                                                   // Loop through all span nodes
                    attributes.Clear();
                    values.Clear();

                    XmlAttributeCollection spanAttributes = spanNode.Attributes;
                    foreach (XmlAttribute spanAttribute in spanAttributes) {                                // Loop through all attributes
                        attributes.Add(spanAttribute.Name);                                                 // Add the attribute to the list
                        values.Add(spanAttribute.Value);
                    }

                    string[] attributesArray = new string[attributes.Count];                                // Declare array variables
                    string[] valuesArray = new string[values.Count];

                    for (int i = 0; i < attributes.Count; i++) {                                            // Loop through both lists and append the values to the arrays
                        attributesArray[i] = attributes[i];
                        valuesArray[i] = values[i];
                    }

                    
                    TextElement textElement = new TextElement(id, title, spanNode.InnerText,
                        attributesArray, valuesArray);                                                      // Create new TextElement, which contains the span node's contents
                    contents.Children.Add(textElement);                                                     // Add the TextElement to the contents stackpanel
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

        private void btn_addText_Click(object sender, RoutedEventArgs e) {
            try {
                //Create XML node
                XmlDocument noteXML = new XmlDocument();
                noteXML.Load(file);
                XmlElement span = noteXML.CreateElement("span");                                            // Create a new span node
                span.InnerText = "Notiztext";                                                               // Set text value

                XmlAttribute id = noteXML.CreateAttribute("id");                                            // Create new xml attribute...
                int newId = getLastNoteId() + 1;
                id.Value = newId.ToString();                                                                // ...and set its value

                span.Attributes.Append(id);                                                                 // Append the attributes to the span node
                noteXML.DocumentElement.AppendChild(span);                                                  // Append the span node to the document element
                noteXML.Save(file);
                ExtractSpanNodesFromXML();
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        /// <summary>
        /// returns the last note id from the XML file
        /// </summary>
        /// <returns></returns>
        public int getLastNoteId() {
            try {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(file);

                XmlNodeList spanNodes = xmlDocument.SelectNodes("//span");

                int lastId = 0;

                foreach (XmlNode spanNode in spanNodes) {                                                   // Loop through all span nodes, searching for the biggest id value
                    int id = Convert.ToInt32(spanNode.Attributes["id"].Value);
                    if (id > lastId) {
                        lastId = id;
                    }
                }
                return lastId;
            }
            catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
                return 0;
            }
        }

        /// <summary>
        /// Animates the color change of the Post-It-Border
        /// </summary>
        /// <param name="newColor"></param>
        public void AnimateColorChange(string newColor= "#FFFAE75C") {
            try {
                SolidColorBrush initialColor = (SolidColorBrush)brdr_postit.Background;                     // Get the initial color

                brdr_postit.Background = initialColor;                                                      // Set the initial color as the background

                ColorAnimation colorAnimation = new ColorAnimation {                                        // Create a ColorAnimation to animate the color change
                    From = initialColor.Color,
                    To = (Color)ColorConverter.ConvertFromString(newColor),
                    Duration = TimeSpan.FromSeconds(0.25)
                };

                
                Storyboard.SetTarget(colorAnimation, brdr_postit);                                          // Set the target of the animation to the border
                Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("Background.Color"));         // Apply the animation to the Background.Color property

                Storyboard storyboard = new Storyboard();                                                   // Create a new storyboard
                storyboard.Children.Add(colorAnimation);                                                    // Add the animation to the storyboard

                storyboard.Begin();                                                                         // Begin the animation
                SaveColor();
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }
        
        /// <summary>
        /// Saves the background color of the note to the XML file
        /// </summary>
        public async void SaveColor() {
            try {
                await Task.Delay(250);                                                                      // Wait for the animation to finish
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(file);
                XmlElement note = xmlDocument.DocumentElement;                                              // Get the root node
                note.SetAttribute("color", brdr_postit.Background.ToString());                              // Set the color attribute
                xmlDocument.Save(file);
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        private void ell_bkgCol_gold_MouseDown(object sender, MouseButtonEventArgs e) { AnimateColorChange("#FFDFB200"); }
        private void ell_bkgCol_cyan_MouseDown(object sender, MouseButtonEventArgs e) { AnimateColorChange("#47bed2"); }
        private void ell_bkgCol_lemongreen_MouseDown(object sender, MouseButtonEventArgs e) { AnimateColorChange("#9dca53"); }
        private void ell_bkgCol_lightpurple_MouseDown(object sender, MouseButtonEventArgs e) { AnimateColorChange("#f87b8d"); }
        private void ell_bkgCol_orange_MouseDown(object sender, MouseButtonEventArgs e) { AnimateColorChange("#e6986a"); }

        private void btn_addText_Copy_Click(object sender, RoutedEventArgs e) {
            try {
                XmlDocument noteXML = new XmlDocument();
                noteXML.Load(file);
                XmlElement span = noteXML.CreateElement("span");                                            // Create a new span node
                span.InnerText = "Titel";                                                                   // Set text value

                XmlAttribute id = noteXML.CreateAttribute("id");                                            // Create id attribute
                int newId = getLastNoteId() + 1;
                id.Value = newId.ToString();                                                                // Set the value of the id attribute

                XmlAttribute preset = noteXML.CreateAttribute("preset");
                preset.Value = "h2";

                span.Attributes.Append(id);                                                                 // Append the attributes to the span node
                span.Attributes.Append(preset);
                noteXML.DocumentElement.AppendChild(span);                                                  // Append the span node to the root node

                noteXML.Save(file);                                                                         // Save the XML file

                ExtractSpanNodesFromXML();                                                                  // Refresh the StackPanel
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        private void Window_Activated(object sender, EventArgs e) {
            try {
                ExtractSpanNodesFromXML();
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }
    }
}