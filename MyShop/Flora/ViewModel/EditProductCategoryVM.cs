namespace Flora.ViewModel
{
    class EditProductCategoryVM : Utilities.ViewModelBase
    {
        public PlantCategory plantCategory { get; set; }
        public EditProductCategoryVM()
        {

            plantCategory = new PlantCategory()
            {
                CategoryId = 1,
                CategoryImages = "/Images/ProductTypes/1.png",
                CategoryName = "Vegetable",
                Plants = null
            };
        }
    }
}
