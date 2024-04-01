using Flora.ViewModel;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
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

            if (isImageChanged)
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
                            // Set displayedImage.Source to null before copying the file
                            displayedImage.Source = null;

                            // Force the command to the UI thread to be processed to release the file handle
                            Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);
                            GC.Collect(); // Force a garbage collection to release the file
                            GC.WaitForPendingFinalizers(); // Wait for the finalizers to complete


                            try
                            {
                                // Copy the image to the new location
                                CopyImageToNewLocation(currentImagePath, targetFileDirectory);
                            }
                            catch (IOException ex)
                            {
                                MessageBox.Show($"Failed to copy the file: {ex.Message}");
                                return; // Exit the method if an exception occurs
                            }

                            DisplayImage(targetFileDirectory);

                            // Update the category images path
                            view.PlantCategory.CategoryImages = targetFileDirectory;


                        }
                        else
                        {
                            MessageBox.Show("The source file does not exist.");
                            return; // Exit the method if the source file does not exist
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show($"Failed to copy the file: {ex.Message}");
                        return; // Exit the method if an exception occurs
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
            myTitle.Text = categoryName;
            // Reset the isImageChanged flag
            isImageChanged = false;
        }

        public void CopyImageToNewLocation(string currentImagePath, string targetFileDirectory)
        {
            // Attempt to copy the file with retries
            int maxRetries = 3;
            int delayOnRetry = 1000;

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    // ... existing check for File.Exists ...

                    // Use a FileStream with FileMode.OpenOrCreate
                    using (FileStream sourceStream = File.Open(currentImagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (FileStream destinationStream = File.Open(targetFileDirectory, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                        {
                            sourceStream.CopyTo(destinationStream);
                        }
                    }

                    break; // If the copy is successful, break out of the loop
                }
                catch (IOException ex)
                {
                    if (i < (maxRetries - 1))
                    {
                        // Wait before trying again
                        System.Threading.Thread.Sleep(delayOnRetry);
                    }
                    else
                    {
                        // This is the last attempt - rethrow the exception
                        throw;
                    }
                }
            }
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
            // Giải phóng hình ảnh hiện tại trước khi gán hình ảnh mới
            displayedImage.Source = null;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filePath);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            displayedImage.Source = bitmap;
            displayedImage.Visibility = Visibility.Visible;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            var view = DataContext as EditProductCategoryVM;
            if (view != null)
            {
                myTextBoxName.Text = (view.categoryName);
                myTitle.Text = (view.categoryName);
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string targetFileDirectory = Path.Combine(appDirectory, view.PlantCategory.CategoryImages);
                DisplayImage(targetFileDirectory);
            }
        }
        private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete this item?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var viewModel = DataContext as EditProductCategoryVM;
                viewModel?.DeleteCategoryFromDatabase();
                var navigationVM = GetNavigationVMFromMainWindow();
                if (navigationVM != null)
                {
                    navigationVM.NavigateBack();
                }
            }
            else
            {

            }
        }
    }
}
