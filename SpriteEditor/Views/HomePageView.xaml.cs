﻿using System;
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

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;

            if (textBox == null) return;
            if (_editingTextBlock == null) return;

            if (e.Key == Key.Enter)
            {
                _editingTextBlock.Visibility = Visibility.Visible;
                _editingTextBlock = null;
            }
        }

        private TextBlock _editingTextBlock;

        private void UIElement_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;

            if (textBlock == null) return;
            if (e.LeftButton != MouseButtonState.Pressed || e.ClickCount < 2) return;

            _editingTextBlock = textBlock;
            _editingTextBlock.Visibility = Visibility.Collapsed;

            e.Handled = true;
        }

        private void UIElement_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            if (textBox == null) return;
            if (!Convert.ToBoolean(e.NewValue)) return;

            textBox.Focus();
            textBox.CaretIndex = textBox.Text.Length;
        }

        private void UIElement_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;

            if (textBox == null) return;
            if (_editingTextBlock == null) return;

            _editingTextBlock.Visibility = Visibility.Visible;
            _editingTextBlock = null;
        }

        private ListBoxItem _draggableListBoxItem;
        private Point _startPoint;
        private double _left;
        private double _top;

        private void ListBoxItem_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var listBoxItem = sender as ListBoxItem;
            if (listBoxItem == null) return;

            _draggableListBoxItem = listBoxItem;
            _left = Canvas.GetLeft(_draggableListBoxItem);
            _top = Canvas.GetTop(_draggableListBoxItem);
        }
        private void ListBoxItem_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _draggableListBoxItem = null;
        }

        private void Canvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            var canvas = sender as Canvas;

            if (_draggableListBoxItem == null) return;
            if (canvas == null) return;

            var newPoint = Mouse.GetPosition(canvas);

            var newLeft = _left + Math.Round(newPoint.X - _startPoint.X);
            var newTop = _top + Math.Round(newPoint.Y - _startPoint.Y);

            Canvas.SetLeft(_draggableListBoxItem, newLeft);
            Canvas.SetTop(_draggableListBoxItem, newTop);

            e.Handled = true;
        }

        private void Canvas_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = sender as Canvas;
            if (canvas == null) return;

            _startPoint = Mouse.GetPosition(canvas);
        }

        private void Canvas_OnMouseLeave(object sender, MouseEventArgs e)
        {
            var canvas = sender as Canvas;
            if (canvas == null) return;

            _draggableListBoxItem = null;
            e.Handled = true;
        }
    }
}
