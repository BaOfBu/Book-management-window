using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Flora.Model;
using Flora.View;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace Flora.ViewModel
{
    class ProductVM : Utilities.ViewModelBase
    {
        public List<string> PagesNumberList { get; set; }
        public List<string> SortTypeList { get;set; }

        public ProductVM() {
            PagesNumberList = new List<string> { "8", "16", "24", "32", "64", "96" };
            SortTypeList = new List<string>  {   "Sort by name ascending",
                                                "Sort by name descending",
                                             };
        } 
    }
}
