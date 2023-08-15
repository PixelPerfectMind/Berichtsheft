using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Berichtsheft.Dialogs {

    public partial class Settings : Window {
        public Settings() {
            InitializeComponent();
            LoadSavedSettings();
        }

        private async void btn_closeWindow_Click(object sender, RoutedEventArgs e) {
            await Task.Delay(200);
            this.Close();
        }

        private void btn_minimizeWindow_Click(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Minimized;
        }

        private void txt_windowTitle_MouseDown(object sender, MouseButtonEventArgs e) {
            if(Mouse.LeftButton == MouseButtonState.Pressed) { DragMove(); }
        }

        public void LoadSavedSettings() {
              // Load the saved settings
            try {
                // Load the saved settings
                if(Properties.Settings.Default.useDashboardBackground) {
                    cb_useBlurEffect.IsEnabled = true;
                    cb_useBlurEffect.IsChecked = Properties.Settings.Default.blurBackground;
                    if(Properties.Settings.Default.internalBackgroundImage != "") {
                        img_backgroundPreview.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Backgrounds/" + Properties.Settings.Default.internalBackgroundImage.Replace("img/", "") + ".jpg"));
                    } else {
                        img_backgroundPreview.Source = new BitmapImage(new Uri(Properties.Settings.Default.externalBackgroundPath));
                    }
                } else {
                    cb_useBlurEffect.IsEnabled = false;
                    cb_useBlurEffect.IsChecked = false;
                    img_backgroundPreview.Source = null;
                }
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        /// <summary>
        /// Use blur effect for the background of the MainWindow
        /// </summary>
        private void cb_useBlurEffect_Checked(object sender, RoutedEventArgs e) {
            Properties.Settings.Default.blurBackground = true;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Don't use blur effect for the background of the MainWindow
        /// </summary>
        private void cb_useBlurEffect_Unchecked(object sender, RoutedEventArgs e) {
            Properties.Settings.Default.blurBackground = false;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Use no background image for the MainWindow
        /// </summary>
        private void rb_background_none_Checked(object sender, RoutedEventArgs e) {
            cb_useBlurEffect.IsChecked = false;
            cb_useBlurEffect.IsEnabled = false;
            img_backgroundPreview.Source = null;
            Properties.Settings.Default.useDashboardBackground = false;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Use a pre-defined background image for the MainWindow
        /// </summary>
        private void BackgroundImageSelected(string name) {
            try {
                Properties.Settings.Default.useDashboardBackground = true;
                cb_useBlurEffect.IsEnabled = true;
                cb_useBlurEffect.IsChecked = true;
                img_backgroundPreview.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Backgrounds/" + name + ".jpg"));
                Properties.Settings.Default.internalBackgroundImage = "img/" + name;
                Properties.Settings.Default.Save();
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }

        private void rb_background_blossoms_Checked(object sender, RoutedEventArgs e) { BackgroundImageSelected("blossoms"); }
        private void rb_background_geysir_Checked(object sender, RoutedEventArgs e) { BackgroundImageSelected("geysir"); }
        private void rb_background_wtrfall1_Checked(object sender, RoutedEventArgs e) { BackgroundImageSelected("wtrfall1"); }
        private void rb_background_wtrfall2_Checked(object sender, RoutedEventArgs e) { BackgroundImageSelected("wtrfall2"); }

        /// <summary>
        /// Use a custom background image for the MainWindow
        /// </summary>
        private void rb_background_custom_Checked(object sender, RoutedEventArgs e) {
            try { 
                Properties.Settings.Default.useDashboardBackground = true;
                cb_useBlurEffect.IsEnabled = true;
                cb_useBlurEffect.IsChecked = true;
                System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog {
                    Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
                };
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    img_backgroundPreview.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                    Properties.Settings.Default.externalBackgroundPath = openFileDialog.FileName;
                    Properties.Settings.Default.internalBackgroundImage = "";
                    Properties.Settings.Default.Save();
                }
            } catch (Exception ex) {
                ExceptionWindow exceptionWindow = new ExceptionWindow(ex);
                exceptionWindow.ShowDialog();
            }
        }
    }
}