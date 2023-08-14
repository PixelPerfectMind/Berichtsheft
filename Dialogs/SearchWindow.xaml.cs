using Berichtsheft.UserControls;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Berichtsheft.Dialogs {

    public partial class SearchWindow : Window {
        public SearchWindow() {
            InitializeComponent();
        }

        private async void btn_close_Click(object sender, RoutedEventArgs e) {
            await Task.Delay(70);
            Close();
        }

        private void txt_title_MouseDown(object sender, MouseButtonEventArgs e) {
            if(Mouse.LeftButton == MouseButtonState.Pressed) {
                DragMove();
            }
        }

        private void txt_search_TextChanged(object sender, TextChangedEventArgs e) {
            pb_search.Visibility = Visibility.Collapsed;
            cb_windowSizeAnimation.IsChecked = false;
        }

        private void btn_search_Click(object sender, RoutedEventArgs e) {
            pb_search.Visibility = Visibility.Visible;
            cb_windowSizeAnimation.IsChecked = true;

            sp_searchResults.Children.Clear();
            
            Classes.FileActions fileActions = new Classes.FileActions();
            try {
                IndexFolderRecursive(fileActions.path);
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.txt_message.Text = "Die Suche wurde aufgrund eines Fehlers abgebrochen.";
                exceptionWindow.ShowDialog();
            }

            pb_search.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Searches for file names and contents in the specified folder and all subfolders
        /// </summary>
        public void IndexFolderRecursive(string path) {

            // Add all files in current folder to list
            foreach(var file in Directory.GetFiles(path)) {

                // Check if the file name contains the search text
                if(file.Replace(path + "\\", "").Contains(txt_search.Text)) {
                    SearchItem searchItem = new SearchItem(file.Replace(path + "\\", ""), file);
                    sp_searchResults.Children.Add(searchItem);
                }

                // Check if the file contents contain the search text
                if(cb_searchFileContents.IsChecked == true) {
                    if(File.ReadAllText(file).Contains(txt_search.Text)) {
                        SearchItem searchItem = new SearchItem(file.Replace(path + "\\", ""), file);
                        sp_searchResults.Children.Add(searchItem);
                    }
                }
            }

            // Loop through all folders in current folder
            foreach(var folder in Directory.GetDirectories(path)) {
                // Call this function again for each folder
                IndexFolderRecursive(folder);
            }
        }

    }
}
