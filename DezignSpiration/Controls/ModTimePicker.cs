using System;
using Xamarin.Forms;
namespace DezignSpiration.Controls
{
    public class ModTimePicker : TimePicker
    {
        public ModTimePicker()
        {
            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == TimeProperty.PropertyName)
                {
                    // Redraw Control to get more space
                    InvalidateMeasure();
                }
            };
        }
    }
}
