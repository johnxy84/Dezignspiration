using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace DezignSpiration.Controls
{
    public partial class ColorsList : FlexLayout
    {
        public Models.Color SelectedColor
        {
            get => GetValue(SelectedColorProperty) as Models.Color;
            set
            {
                SetValue(SelectedColorProperty, value);
            }
        }

        public static readonly BindableProperty SelectedColorProperty = BindableProperty.Create(
            propertyName: "SelectedColor",
            returnType: typeof(Models.Color),
            declaringType: typeof(ColorsList),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                //if(bindable is ColorsList rootView && newvalue is Models.Color selectedColor)
                //{
                //    rootView.UpdateSelectedColor(selectedColor);
                //}
            });

        public ObservableCollection<Models.Color> ColorsSource { get; set; }

        public static readonly BindableProperty ColorsSourceProperty = BindableProperty.Create(
            propertyName: "ColorsSource",
            returnType: typeof(ObservableCollection<Models.Color>),
            declaringType: typeof(ColorsList),
            defaultValue: new ObservableCollection<Models.Color>(),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                //Get our rootview and color source
                if (bindable is ColorsList rootView && newValue is ObservableCollection<Models.Color> newColors)
                {
                    // Clear all the children views
                    //TODO: Modify this view to respond to only new change

                    rootView.Children.Clear();
                    for (int i = 0; i < newColors.Count; i++)
                    {
                        Models.Color color = newColors[i];
                        var colorView = new ColorView
                        {
                            Color = color,
                            SelectedCommand = new Command(selectedColor =>
                           {
                               rootView.UpdateSelectedColor((Models.Color)selectedColor);
                               ColorView oldSelectedView = null;
                               ColorView newSelectedView = null;

                               foreach (var view in rootView.Children)
                               {
                                   if (view is ColorView currentView)
                                   {
                                       if (currentView.IsSelected)
                                       {
                                           oldSelectedView = currentView;
                                       }
                                       if (currentView.Color.Id == color.Id)
                                       {
                                           newSelectedView = currentView;
                                       }
                                   }
                               }
                               oldSelectedView.IsSelected = false;
                               newSelectedView.IsSelected = true;
                               oldSelectedView = newSelectedView;
                           })
                        };

                        // Set the first element to selected
                        if (i == 0)
                        {
                            colorView.IsSelected = true;
                            rootView.UpdateSelectedColor(color);
                        }

                        // Insert view to rootlayout
                        rootView.Children.Add(colorView);
                    }
                }
            });

        public ColorsList()
        {
            InitializeComponent();
        }

        public void UpdateSelectedColor(Models.Color color)
        {
            SelectedColor = color;
        }

    }
}
