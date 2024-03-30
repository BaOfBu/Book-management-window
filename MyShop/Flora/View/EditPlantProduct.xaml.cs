using Flora.ViewModel;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Flora.View
{
    /// <summary>
    /// Interaction logic for EditPlantProduct.xaml
    /// </summary>
    public partial class EditPlantProduct : UserControl
    {
        private EditPlantProductVM editPlantProductVM { get; set; }
        public EditPlantProduct()
        {
            InitializeComponent();
            editPlantProductVM = DataContext as EditPlantProductVM;
        }

        private void TextBox_GotFocus_Generic(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "Enter text here..." && textBox.Foreground == Brushes.Gray)
            {
                textBox.Text = "";
                textBox.FontStyle = FontStyles.Normal;
                textBox.Foreground = Brushes.Black;
            }
        }

        private void TextBox_LostFocus_Generic(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Enter text here...";
                textBox.FontStyle = FontStyles.Italic;
                textBox.Foreground = Brushes.Gray;
            }
        }
        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            var navigationVM = GetNavigationVMFromMainWindow();
            if (navigationVM != null)
            {
                navigationVM.NavigateBack();
            }
        }
        private NavigationVM GetNavigationVMFromMainWindow()
        {
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow.DataContext is NavigationVM navigationVM)
            {
                return navigationVM;
            }
            return null;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg;*.png;*.webp)|*.jpg;*.png;*.webp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                DisplayImage(fileName);
                textBlockStatus.Text = Path.GetFileName(fileName);
            }
        }
        private void DisplayImage(string filePath)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filePath);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            displayedImage.Source = bitmap;
            displayedImage.Visibility = Visibility.Visible;
        }

        private void EditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }


        private void TextBox_Number_LostFocus_Generic(object sender, System.Windows.RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Enter text here...";
                textBox.FontStyle = FontStyles.Italic;
                textBox.Foreground = Brushes.Gray;
            }
            string input = textBox.Text;
            Regex regex = new Regex(@"^\d+$");
            bool isValidNumber = regex.IsMatch(input);

            if (!isValidNumber && !string.IsNullOrEmpty(input) && input != "Enter text here...")
            {
                warningTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                warningTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
