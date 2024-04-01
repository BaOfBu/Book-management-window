using Flora.ViewModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Flora
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (DataContext is NavigationVM navigationVM)
            {
                navigationVM.BeforeViewChange += async (sender, args) =>
                {
                    await RunFadeOutAnimationAsync();
                    // Change the view only after the fade-out completes
                    navigationVM.CurrentView = new PlantProductVM(); // This changes the current view to the plant view.
                    RunFadeInAnimation();
                };
            }
        }
        private Task RunFadeOutAnimationAsync()
        {
            var tcs = new TaskCompletionSource<bool>();
            var fadeOutStoryboard = (Storyboard)Resources["FadeOutStoryboard"];
            fadeOutStoryboard.Completed += (s, args) => tcs.TrySetResult(true);
            fadeOutStoryboard.Begin(Pages);
            return tcs.Task;
        }

        private void RunFadeInAnimation()
        {
            var fadeInStoryboard = (Storyboard)Resources["FadeInStoryboard"];
            fadeInStoryboard.Begin(Pages);
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
