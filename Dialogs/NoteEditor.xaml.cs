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
            try {
                // Declare temporary lists
                List<string> attributes = new List<string>();
                List<string> values = new List<string>();

                // Extract all span nodes from the XML file

                // Get all span nodes
                XmlNodeList spanNodes = xmlDocument.SelectNodes("//span");

                // Loop through all span nodes
                foreach (XmlNode spanNode in spanNodes) {
                    // Get the span node's attributes
                    XmlAttributeCollection spanAttributes = spanNode.Attributes;
                    foreach (XmlAttribute spanAttribute in spanAttributes) {
                        // Add the attribute to the list
                        attributes.Add(spanAttribute.Name);
                        values.Add(spanAttribute.Value);
                    }
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
                TextElement textElement = new TextElement(id, title, attributesArray, valuesArray);
                contents.Children.Add(textElement);
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
    }
}
