using System;
using Xamarin.Forms;
namespace DezignSpiration.Controls
{
    public class ModPicker : Picker
    {
        public ModPicker()
        {
            SelectedIndexChanged += (sender, e) =>
            {
                // Force picker to resize and redraw
                InvalidateMeasure();
            };


        }
    }
}
