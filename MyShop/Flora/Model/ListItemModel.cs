using System.Windows.Input;

namespace Flora.Model
{
    internal class ListItemModel
    {
        public string Text { get; set; }
        public string Icon { get; set; }
        public ICommand ItemCommand { get; set; }

    }
}
