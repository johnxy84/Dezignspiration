using System;
using Xamarin.Forms;
namespace DezignSpiration.Controls
{
    public class FlexEditor : Editor
    {
        public FlexEditor()
        {
            TextChanged += (sender, e) =>
            {
                // Force editor to resize as text changes
                InvalidateMeasure();
            };
        }
    }
}
