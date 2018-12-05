using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DezignSpiration.Controls
{
    public partial class ColorView : Grid
    {
        bool isSelected;
        Models.Color color = new Models.Color();
        Command selectedCommand = new Command(() => { });

        public Models.Color Color
        {
            get => color;
            set
            {
                color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public Command SelectedCommand
        {
            get => selectedCommand;
            set
            {
                selectedCommand = value;
                OnPropertyChanged(nameof(SelectedCommand));
            }
        }


        public ColorView()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}
