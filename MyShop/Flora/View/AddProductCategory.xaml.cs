using Flora.ViewModel;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Flora.View
{
    /// <summary>
    /// Interaction logic for AddProductCategory.xaml
    /// </summary>
    public partial class AddProductCategory : UserControl
    {
        private AddProductCategoryVM addProductCategoryVM { get; set; }
        public AddProductCategory()
        {
            InitializeComponent();
            DataContext = new AddProductCategoryVM();
            addProductCategoryVM = DataContext as AddProductCategoryVM;
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
        private void TextBox_LostFocus_Generic(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Enter text here...";
                textBox.FontStyle = FontStyles.Normal;
                textBox.Foreground = Brushes.Gray;
            }
        }
        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            var navigationVM = GetNavigationVMFromMainWindow();
            if (navigationVM != null)
            {
                navigationVM.ProductsCommand.Execute(null);
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
                DisplayImage(fileName);
                textBlockStatus.Text = Path.GetFileName(fileName);
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
            //// Check if the category name is null or empty
            if (string.IsNullOrWhiteSpace(myTextBoxName.Text) || myTextBoxName.Text == "Enter text here...")
            {
                MessageBox.Show("Please enter a category name.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //// Check if an image has been loaded
            if (displayedImage.Source is not BitmapImage bitmapImage)
            {
                MessageBox.Show("Please select an image to add with the new category.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var categoryName = myTextBoxName.Text;
            var categoryId = myTextBoxCategory.Text.ToString();
            if (displayedImage.Source is BitmapImage)
            {
                try
                {
                    // Construct the path to the target directory
                    string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string targetDirectory = Path.Combine(appDirectory, "Images", "ProductTypes");
                    string targetFileName = "ProductCategory" + categoryId + ".png";
                    string targetPath = Path.Combine(targetDirectory, targetFileName);

                    string basePath = AppDomain.CurrentDomain.BaseDirectory;
                    string imagesDirectory = Path.GetFullPath(Path.Combine(basePath, @"..\..\..\Images\ProductTypes"));
                    string imageFilePath = Path.Combine(imagesDirectory, targetFileName);
                    // Ensure the target directory exists
                    Directory.CreateDirectory(targetDirectory);

                    //// Save the image
                    SaveImage(bitmapImage, targetPath);
                    SaveImage(bitmapImage, imageFilePath);

                    addProductCategoryVM.CategoryName = categoryName;
                    addProductCategoryVM.CategoryImages = targetPath.Replace(appDirectory, "");
                    await addProductCategoryVM.SaveCategoryAsync();
                    MessageBox.Show("The plant category have been successfully added.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Clear();

                }
                catch (Exception ex)
                {
                    textBlockStatus.Text = "Failed to save image.";
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
            myTextBoxName.Text = "Enter text here...";
            myTextBoxName.Text = "Enter text here...";
            myTextBoxName.FontStyle = FontStyles.Normal;
            myTextBoxName.Foreground = Brushes.Gray;

            displayedImage.Source = null;
            displayedImage.Visibility = Visibility.Collapsed;

            uploadInstructionsStackPanel.Visibility = Visibility.Visible;
            textBlockStatus.Text = "Drop files here to upload";
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {

            myTextBoxName.Text = "Enter text here...";
            myTextBoxName.Text = "Enter text here...";
            myTextBoxName.FontStyle = FontStyles.Normal;
            myTextBoxName.Foreground = Brushes.Gray;

            displayedImage.Source = null;
            displayedImage.Visibility = Visibility.Collapsed;

            uploadInstructionsStackPanel.Visibility = Visibility.Visible;
            textBlockStatus.Text = "Drop files here to upload";
        }
    }
}
