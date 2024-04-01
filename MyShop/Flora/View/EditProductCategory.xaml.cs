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
    /// Interaction logic for EditProductCategory.xaml
    /// </summary>
    public partial class EditProductCategory : UserControl
    {
        private EditProductCategoryVM editProductCategoryVM { get; set; }

        bool isImageChanged = false;
        public EditProductCategory()
        {
            InitializeComponent();
        }

        public EditProductCategory(PlantCategory plantCategory) : this()
        {
            DataContext = new EditProductCategoryVM(plantCategory);
            editProductCategoryVM = DataContext as EditProductCategoryVM;
        }


        private void EditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var view = DataContext as EditProductCategoryVM;
            string currentImagePath = string.Empty;
            string categoryName = myTextBoxName.Text;

            if (isImageChanged == true)
            {
                if (displayedImage.Source != null)
                {
                    Uri uri = new Uri(displayedImage.Source.ToString());
                    currentImagePath = uri.LocalPath;
                }

                if (view != null)
                {
                    string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string targetFileDirectory = Path.Combine(appDirectory, view.PlantCategory.CategoryImages);
                    try
                    {
                        if (File.Exists(currentImagePath))
                        {
                            displayedImage.Source = null;
                            var tmp_imagePath = view.PlantCategory.CategoryImages;
                            CopyImageToNewLocation(currentImagePath, targetFileDirectory);
                            view.PlantCategory.CategoryImages = tmp_imagePath;
                        }
                        else
                        {
                            MessageBox.Show("The source file does not exist.");
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show($"Failed to copy the file: {ex.Message}");
                    }
                }
            }

            // Update the category name
            view.PlantCategory.CategoryName = categoryName;

            // Update the category in the database
            try
            {
                view.UpdateCategoryInDatabase();
                MessageBox.Show("The plant category has been successfully edited.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update the category: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            isImageChanged = false;
        }

        public void CopyImageToNewLocation(string currentImagePath, string otherPath)
        {
            try
            {
                // Check if the file exists and is not locked by another process
                if (File.Exists(currentImagePath))
                {
                    using (FileStream sourceStream = new FileStream(currentImagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (FileStream destinationStream = new FileStream(otherPath, FileMode.Create, FileAccess.Write))
                        {
                            // Copy the file
                            sourceStream.CopyTo(destinationStream);
                        }
                    }
                }
                else
                {
                    // File does not exist
                    MessageBox.Show("The source file does not exist.");
                }
            }
            catch (IOException ex)
            {
                // Handle file access issues
                MessageBox.Show($"Failed to copy the file: {ex.Message}");
            }
        }


        private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

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
                isImageChanged = true;
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
    }
}
