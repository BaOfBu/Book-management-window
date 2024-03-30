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
    /// Interaction logic for AddPlantProduct.xaml
    /// </summary>
    public partial class AddPlantProduct : UserControl
    {
        private AddPlantProductVM addPlantProductVM { get; set; }
        public AddPlantProduct()
        {
            InitializeComponent();
            addPlantProductVM = DataContext as AddPlantProductVM;
        }
        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

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

        private void TextBox_Number_LostFocus_Generic(object sender, RoutedEventArgs e)
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
        private void UploadArea_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
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
                textBlockStatus.Text = Path.GetFileName(fileName); // Update text to show file name
            }
        }
        private void UploadArea_Drop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
            {
                string fileName = files[0];
                DisplayImage(fileName); // Show the first dropped file as image
                textBlockStatus.Text = Path.GetFileName(fileName); // Update text to show file name
            }
        }
        private void DisplayImage(string filePath)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filePath);
            bitmap.CacheOption = BitmapCacheOption.OnLoad; // To prevent file locking
            bitmap.EndInit();

            displayedImage.Source = bitmap;
            displayedImage.Visibility = Visibility.Visible; // Show the image

            uploadInstructionsStackPanel.Visibility = Visibility.Collapsed; // Hide the upload instructions
        }
        private void AddNewProductCategoryType_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            myTextBoxName.Text = "Enter text here";
            myComboBoxProductType.Text = string.Empty;
            myComboBoxStatusOfProduct.Text = string.Empty;
            myTextBoxNumberOfProduct.Text = "Enter number here";
        }
    }
}
