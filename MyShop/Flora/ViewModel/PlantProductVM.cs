
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace Flora.ViewModel
{
    class PlantProductVM : Utilities.ViewModelBase, INotifyPropertyChanged
    {
        private int _pageSize = 8;
        private int _pageNumber = 1;
        private int _totalItemCount = 0;
        private string _currentSortOrder = string.Empty;
        private string _searchText = string.Empty;
        public List<string> PagesNumberList { get; } = new List<string> { "8", "16", "24", "32", "64", "96" };
        public List<string> SortTypeList { get; } = new List<string> {
            "Sort by name ascending",
            "Sort by name descending",
            "Sort by price ascending",
            "Sort by price descending"
        };
        public ObservableCollection<Plant> PlantList { get; set; }
        public PlantCategory PlantCategory { get; set; }
        private MyShopContext _shopContext;
        public string CurrentSortOrder
        {
            get => _currentSortOrder;
            set
            {
                if (_currentSortOrder != value)
                {
                    _currentSortOrder = value;
                    _pageNumber = 1;
                    OnPropertyChanged(nameof(CurrentSortOrder));
                    LoadPlantAsync();
                }
            }
        }
        private decimal? _minimumPrice;
        public decimal? MinimumPrice
        {
            get => _minimumPrice;
            set
            {
                if (_minimumPrice != value)
                {
                    _pageNumber = 1;
                    _minimumPrice = value;
                    OnPropertyChanged(nameof(MinimumPrice));
                    LoadPlantAsync();
                }
            }
        }

        private decimal? _maximumPrice;
        public decimal? MaximumPrice
        {
            get => _maximumPrice;
            set
            {
                if (_maximumPrice != value)
                {
                    _pageNumber = 1;
                    _maximumPrice = value;
                    OnPropertyChanged(nameof(MaximumPrice));
                    LoadPlantAsync();
                }
            }
        }
        public int TotalItemCount
        {
            get => _totalItemCount;
            set
            {
                if (_totalItemCount != value)
                {
                    _totalItemCount = value;
                    OnPropertyChanged(nameof(TotalItemCount));
                    LoadPlantAsync();
                }
            }
        }
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (_pageSize != value)
                {
                    _pageNumber = 1;
                    _pageSize = value;
                    OnPropertyChanged(nameof(PageSize));
                    LoadPlantAsync();
                }
            }
        }

        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                if (_pageNumber != value)
                {
                    _pageNumber = value;
                    OnPropertyChanged(nameof(PageNumber));
                    LoadPlantAsync();
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _pageNumber = 1;
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    LoadPlantAsync();
                }
            }
        }

        public PlantProductVM(PlantCategory category)
        {
            PlantList = new ObservableCollection<Plant>();
            _shopContext = new MyShopContext();
            PlantCategory = category;
            LoadPlantAsync();
        }
        private async void LoadPlantAsync()
        {
            try
            {
                PlantList.Clear();
                PlantList = await LoadAllPlantsAsync(_pageNumber, _pageSize);
                TotalItemCount = await CalculateTotalItemCountAsync();
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        public async Task<ObservableCollection<Plant>> LoadAllPlantsAsync(int pageNumber, int pageSize)
        {
            int categoryId = PlantCategory.CategoryId;
            int skip = (pageNumber - 1) * pageSize;
            IQueryable<Plant> query = _shopContext.Plants.Where(p => p.CategoryId == categoryId);

            // Filter categories based on SearchText
            if (!string.IsNullOrWhiteSpace(SearchText) && SearchText != "")
            {
                query = query.Where(c => c.Name.Contains(SearchText));
            }

            // Filter by price range
            if (_minimumPrice.HasValue)
            {
                query = query.Where(p => p.Price >= _minimumPrice);
            }
            if (_maximumPrice.HasValue)
            {
                query = query.Where(p => p.Price <= _maximumPrice);
            }

            switch (CurrentSortOrder)
            {
                case "Sort by name ascending":
                    query = query.OrderBy(c => c.Name);
                    break;
                case "Sort by name descending":
                    query = query.OrderByDescending(c => c.Name);
                    break;
                case "Sort by price ascending":
                    query = query.OrderBy(c => c.Price);
                    break;
                case "Sort by price descending":
                    query = query.OrderByDescending(c => c.Price);
                    break;
                default:
                    break;
            }
            var plants = await query
                                    .Skip(skip)
                                    .Take(pageSize)
                                    .ToListAsync();
            return new ObservableCollection<Plant>(plants);
        }
        public async Task<int> CalculateTotalItemCountAsync()
        {
            int categoryId = PlantCategory.CategoryId;

            IQueryable<Plant> query = _shopContext.Plants.Where(p => p.CategoryId == categoryId);

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(p => p.Name.Contains(SearchText));
            }

            // Filter by price range
            if (_minimumPrice.HasValue)
            {
                query = query.Where(p => p.Price >= _minimumPrice);
            }
            if (_maximumPrice.HasValue)
            {
                query = query.Where(p => p.Price <= _maximumPrice);
            }

            return await query.CountAsync();
        }
        public async Task ImportDataFromExcelAsync(string filePath)
        {
            try
            {
                // Call a method to read the Excel file and import data into the database
                await ImportPlantsFromExcel(filePath);

                // Notify user about successful import
                MessageBox.Show("Data imported successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Handle any errors that may occur during the import process
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ImportPlantsFromExcel(string filePath)
        {
            try
            {
                using (var document = SpreadsheetDocument.Open(filePath, false))
                {
                    var wbPart = document.WorkbookPart;
                    var tabs = wbPart.Workbook.Descendants<Sheet>();
                    var tab = tabs.FirstOrDefault(s => s.Name == "Plant");
                    if (tab != null)
                    {
                        var wsPart = (WorksheetPart)(wbPart.GetPartById(tab.Id));
                        var cells = wsPart.Worksheet.Descendants<Cell>();
                        int row = 2;
                        Cell plantIdCell = cells.FirstOrDefault(c => c?.CellReference == $"A{row}");
                        while (plantIdCell != null)
                        {
                            Debug.WriteLine("Hio");
                            string plantIdString = plantIdCell.InnerText;
                            var stringTable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                            if (stringTable != null)
                            {
                                int id = int.Parse(GetCellValue(wbPart, cells.FirstOrDefault(c => c?.CellReference == $"A{row}")));
                                string name = GetCellValue(wbPart, cells.FirstOrDefault(c => c?.CellReference == $"B{row}"));
                                decimal price = decimal.Parse(GetCellValue(wbPart, cells.FirstOrDefault(c => c?.CellReference == $"C{row}")));
                                string description = GetCellValue(wbPart, cells.FirstOrDefault(c => c?.CellReference == $"D{row}"));
                                int stockQuantity = int.Parse(GetCellValue(wbPart, cells.FirstOrDefault(c => c?.CellReference == $"E{row}")));
                                int categoryId = int.Parse(GetCellValue(wbPart, cells.FirstOrDefault(c => c?.CellReference == $"F{row}")));
                                string plantImage = GetCellValue(wbPart, cells.FirstOrDefault(c => c?.CellReference == $"G{row}"));

                                // Construct the path to the target directory
                                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                                string targetDirectory = Path.Combine(appDirectory, "Images", "PlantProducts");
                                string targetFileName = "PlantProduct" + id + ".png";
                                string targetPath = Path.Combine(targetDirectory, targetFileName);

                                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                                string imagesDirectory = Path.GetFullPath(Path.Combine(basePath, @"..\..\..\Images\PlantProducts"));
                                string imageFilePath = Path.Combine(imagesDirectory, targetFileName);
                                // Ensure the target directory exists
                                Directory.CreateDirectory(targetDirectory);

                                CopyImageToNewLocation(plantImage, targetPath);
                                CopyImageToNewLocation(plantImage, imageFilePath);

                                string dbImage = "Images/PlantProducts/" + "PlantProduct" + id + ".png";
                                await SavePlantToDatabase(id, name, price, description, stockQuantity, categoryId, dbImage);
                            }
                            row++;
                            plantIdCell = cells.FirstOrDefault(c => c?.CellReference == $"A{row}");
                        }
                        LoadPlantAsync();
                    }
                    else
                    {
                        MessageBox.Show("Sheet 'Plant' not found in the Excel file.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while importing plants from Excel: {ex.Message}");
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
                    Directory.CreateDirectory(Path.GetDirectoryName(targetFileDirectory));
                    using (FileStream sourceStream = File.Open(currentImagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (FileStream destinationStream = File.Open(targetFileDirectory, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                        {
                            sourceStream.CopyTo(destinationStream);
                        }
                    }

                    break;
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
        private string GetCellValue(WorkbookPart workbookPart, Cell cell)
        {
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                SharedStringTablePart stringTablePart = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                if (stringTablePart != null)
                {
                    SharedStringItem sharedStringItem = (SharedStringItem)stringTablePart.SharedStringTable.ElementAt(int.Parse(cell.InnerText));
                    return sharedStringItem.Text?.Text;
                }
            }
            return cell.InnerText;
        }
        private async Task SavePlantToDatabase(int id, string name, decimal price, string description, int stockQuantity, int categoryId, string plantImage)
        {
            try
            {
                // Check if the plant already exists in the database
                var existingPlant = await _shopContext.Plants.FirstOrDefaultAsync(p => p.PlantId == id);
                if (existingPlant != null)
                {
                    // Update the existing plant
                    existingPlant.Name = name;
                    existingPlant.Price = (decimal?)price;
                    existingPlant.Description = description;
                    existingPlant.StockQuantity = stockQuantity;
                    existingPlant.CategoryId = categoryId;
                    existingPlant.PlantImage = plantImage;
                }
                else
                {
                    // Create a new plant object
                    var newPlant = new Plant
                    {
                        PlantId = id,
                        Name = name,
                        Price = (decimal?)price,
                        Description = description,
                        StockQuantity = stockQuantity,
                        CategoryId = categoryId,
                        PlantImage = plantImage
                    };

                    // Add the new plant to the database context
                    _shopContext.Plants.Add(newPlant);
                }

                // Save changes to the database
                await _shopContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving plant to the database: {ex.Message}");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}