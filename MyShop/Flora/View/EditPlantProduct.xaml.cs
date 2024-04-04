using Flora.ViewModel;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Flora.View
{
    /// <summary>
    /// Interaction logic for EditPlantProduct.xaml
    /// </summary>
    public partial class EditPlantProduct : UserControl
    {
        private EditPlantProductVM editPlantProductVM { get; set; }
        private bool isImageChanged = false;
        public EditPlantProduct()
        {
            InitializeComponent();

        }
        public EditPlantProduct(Plant plant) : this()
        {

            DataContext = new EditPlantProductVM(plant);
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
            var view = DataContext as EditPlantProductVM;
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

        private void EditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var view = DataContext as EditPlantProductVM;
            string currentImagePath = string.Empty;
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
                    string targetFileDirectory = Path.Combine(appDirectory, view.Plant.PlantImage);
                    string dbFileDirectory = view.Plant.PlantImage;

                    string basePath = AppDomain.CurrentDomain.BaseDirectory;
                    string imagesDirectory = Path.GetFullPath(Path.Combine(basePath, @"..\..\..\"));
                    string imageFilePath = Path.Combine(imagesDirectory, view.Plant.PlantImage);
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

                            MessageBox.Show(imageFilePath);
                            try
                            {
                                // Copy the image to the new location
                                CopyImageToNewLocation(currentImagePath, targetFileDirectory);
                                CopyImageToNewLocation(currentImagePath, imageFilePath);
                                DisplayImage(targetFileDirectory);
                            }
                            catch (IOException ex)
                            {
                                MessageBox.Show($"Failed to copy the file: {ex.Message}");
                                return;
                            }



                            view.Plant.PlantImage = dbFileDirectory;

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
                isImageChanged = false;
            }
            view.Plant.Name = myTextBoxName.Text;
            var selectedCategoryId = (int?)myComboBoxProductType.SelectedValue;

            if (selectedCategoryId.HasValue)
            {
                view.Plant.CategoryId = selectedCategoryId.Value;
            }
            view.Plant.Description = myTextBoxDescription.Text;
            view.Plant.StockQuantity = int.Parse(myTextBoxNumberOfProduct.Text);
            view.Plant.Price = decimal.Parse(myTextBoxPrice.Text);
            try
            {
                view.SaveChangesAsync();
                MessageBox.Show("The plant category has been successfully edited.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update the category: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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
        private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete this item?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var viewModel = DataContext as EditPlantProductVM;



                viewModel?.DeletePlantAsync();

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
