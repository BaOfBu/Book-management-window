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
            myComboBoxProductType.IsEditable = false;
            myComboBoxProductType.SelectedIndex = 0;
            myComboBoxProductType.IsReadOnly = true;
        }
        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }
        private void TextBox_GotFocus_Generic(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && (textBox.Text == "Enter number here..." || textBox.Text == "Enter text here..."))
            {
                textBox.Text = ""; // Clear the placeholder text when the TextBox gains focus
                textBox.Foreground = System.Windows.Media.Brushes.Black; // Change text color back to default
                textBox.FontStyle = FontStyles.Normal;
            }
        }

        private void TextBox_Number_Decimal_LostFocus_Generic(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Enter text here...";
                textBox.FontStyle = FontStyles.Italic;
                textBox.Foreground = Brushes.Gray;
            }

            // This is the input from the user
            string input = textBox.Text;

            // Adjusting the regex pattern to match decimal numbers (including optional decimal points)
            // This pattern allows optional leading digits, a decimal point, and more digits after the decimal point.
            // It also matches integers without a decimal point.
            Regex regex = new Regex(@"^\d*(\.\d+)?$");

            bool isValidDecimal = regex.IsMatch(input) || input == "Enter text here...";

            if (!isValidDecimal && !string.IsNullOrEmpty(input))
            {
                // Show warning if the input is not a valid decimal number and not the placeholder text
                warningTextBlockPrice.Visibility = Visibility.Visible;
            }
            else
            {
                // Hide warning if the input is a valid decimal or the textbox is empty/has placeholder text
                warningTextBlockPrice.Visibility = Visibility.Collapsed;
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
            var view = DataContext as AddPlantProductVM;
            var navigationVM = GetNavigationVMFromMainWindow();
            if (navigationVM != null)
            {
                if (view.PlantCategory != null)
                {
                    navigationVM.NavigateToWithParameter(typeof(PlantProductVM), view.PlantCategory);
                }
                else
                {
                    navigationVM.ProductsCommand.Execute(null);
                }
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
        private async void AddNewProductCategoryType_Click(object sender, RoutedEventArgs e)
        {
            var view = DataContext as AddPlantProductVM;
            if (view != null)
            {
                string Name = myTextBoxName.Text;

                string Description = myTextBoxDescription.Text;

                int StockQuantity = (int.Parse(myTextBoxNumberOfProduct.Text));

                decimal price;
                decimal Price;
                if (decimal.TryParse(myTextBoxPrice.Text, out price))
                {
                    Price = price;
                }
                else
                {
                    Price = 0;
                }

                int _categoryId = -1;
                var selectedCategoryId = (int?)myComboBoxProductType.SelectedValue;

                if (selectedCategoryId.HasValue)
                {
                    _categoryId = selectedCategoryId.Value;
                }

                string PlantImage;
                //// Check if an image has been loaded
                if (displayedImage.Source is not BitmapImage bitmapImage)
                {
                    MessageBox.Show("Please select an image to add with the new product.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (displayedImage.Source is BitmapImage)
                {
                    try
                    {
                        // Construct the path to the target directory
                        string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        string targetDirectory = Path.Combine(appDirectory, "Images", "PlantProducts");
                        string targetFileName = "PlantProduct" + view.NextPlantId + ".png";
                        string targetPath = Path.Combine(targetDirectory, targetFileName);

                        string basePath = AppDomain.CurrentDomain.BaseDirectory;
                        string imagesDirectory = Path.GetFullPath(Path.Combine(basePath, @"..\..\..\Images\PlantProducts"));
                        string imageFilePath = Path.Combine(imagesDirectory, targetFileName);
                        // Ensure the target directory exists
                        Directory.CreateDirectory(targetDirectory);

                        //// Save the image
                        SaveImage(bitmapImage, targetPath);
                        SaveImage(bitmapImage, imageFilePath);

                        view.Name = Name;
                        view.PlantImage = targetPath.Replace(appDirectory, "");
                        view.StockQuantity = StockQuantity;
                        view.Price = Price;
                        view.Description = Description;
                        view.CategoryId = (int)_categoryId;

                        await view.SavePlantAsync();
                        MessageBox.Show("The plant category have been successfully added.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        Clear();
                    }
                    catch (Exception ex)
                    {
                        textBlockStatus.Text = "Failed to save image.";
                    }
                }
            }
        }
        private void SaveImage(BitmapImage sourceImage, string outputPath)
        {
            // Ensure the source image is loaded
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(sourceImage.UriSource));

            using (var fileStream = new FileStream(outputPath, FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }
        private void Clear()
        {
            myTextBoxName.Text = "Enter text here";
            myComboBoxProductType.Text = string.Empty;
            displayedImage.Source = null;
            displayedImage.Visibility = Visibility.Collapsed;
            uploadInstructionsStackPanel.Visibility = Visibility.Visible;
            myTextBoxPrice.Text = "Enter number here";
            myTextBoxNumberOfProduct.Text = "Enter number here";
            myTextBoxDescription.Text = "Enter text here";
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            myTextBoxName.Text = "Enter text here";
            myComboBoxProductType.Text = string.Empty;
            displayedImage.Source = null;
            displayedImage.Visibility = Visibility.Collapsed;
            uploadInstructionsStackPanel.Visibility = Visibility.Visible;
            myTextBoxPrice.Text = "Enter number here";
            myTextBoxDescription.Text = "Enter text here";
            myTextBoxNumberOfProduct.Text = "Enter number here";
        }

    }
}
