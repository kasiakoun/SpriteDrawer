using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using SpriteEditor.ViewModels;

namespace SpriteEditor.Views
{
    /// <summary>
    /// Interaction logic for HomePageView.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(HomePageViewModel))]
    public partial class HomePageView : MvxWpfView
    {
        public HomePageView()
        {
            InitializeComponent();
        }

        private void CanvasItemsControl_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var listBox = sender as ListBox;

            var selectedListBoxItem = listBox?.ItemContainerGenerator
                .ContainerFromItem(listBox.Items.CurrentItem) as ListBoxItem;
            if (selectedListBoxItem == null) return;

            var left = Canvas.GetLeft(selectedListBoxItem);
            var top = Canvas.GetTop(selectedListBoxItem);

            switch (e.Key)
            {
                case Key.Left:
                    Canvas.SetLeft(selectedListBoxItem, left - 1);
                    break;
                case Key.Right:
                    Canvas.SetLeft(selectedListBoxItem, left + 1);
                    break;
                case Key.Up:
                    Canvas.SetTop(selectedListBoxItem, top - 1);
                    break;
                case Key.Down:
                    Canvas.SetTop(selectedListBoxItem, top + 1);
                    break;
            }

            e.Handled = true;
        }

        private void CanvasItemsControl_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox == null) return;

            listBox.Focus();
            Debug.WriteLine(DateTime.Now);
        }
    }
}
