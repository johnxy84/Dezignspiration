using DezignSpiration.Helpers;
using Xamarin.Forms;

namespace DezignSpiration.Pages
{
    public partial class QuotePage : ContentPage
    {
        public QuotePage()
        {
            InitializeComponent();

            SizeChanged += (sender, e) => {
                var orientation = Height > Width ? "Portrait" : "Landscape";
                VisualStateManager.GoToState(MainStack, orientation);
                VisualStateManager.GoToState(InfoView, orientation);
                VisualStateManager.GoToState(InfoText, orientation);
            };
        }
    }
}
