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
using SpriteEditor.Helpers;
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
        private double _startLeft;
        private double _startTop;
        private double _startWidth;
        private double _startHeight;
        private TransformOperation _transformOperation;

        private void ListBoxItem_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var listBoxItem = sender as ListBoxItem;
            if (listBoxItem == null) return;

            var touchedPoint = Mouse.GetPosition(listBoxItem);
            _transformOperation = GetTransformOperationByPosition(touchedPoint, listBoxItem.ActualWidth, listBoxItem.ActualHeight);

            _draggableListBoxItem = listBoxItem;
            _startLeft = Canvas.GetLeft(_draggableListBoxItem);
            _startTop = Canvas.GetTop(_draggableListBoxItem);

            var frameRectangle = GetFrameRectangle(_draggableListBoxItem);
            _startWidth = frameRectangle.Width;
            _startHeight = frameRectangle.Height;
        }

        private void ListBox_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _draggableListBoxItem = null;
        }

        private void ListBox_OnMouseMove(object sender, MouseEventArgs e)
        {
            var listBox = sender as ListBox;

            if (_draggableListBoxItem == null) return;
            if (listBox == null) return;

            var newPoint = Mouse.GetPosition(listBox);
            var frameRectangle = GetFrameRectangle(_draggableListBoxItem);
            TransformRectangleByCurrentPosition(newPoint, frameRectangle);

            e.Handled = true;
        }

        private void TransformRectangleByCurrentPosition(Point newPoint, Rectangle frameRectangle)
        {
            var leftOffset = Math.Round(newPoint.X - _startPoint.X) / ScaleSlider.Value;
            var topOffset = Math.Round(newPoint.Y - _startPoint.Y) / ScaleSlider.Value;

            var newLeft = _startLeft + leftOffset;
            var newTop = _startTop + topOffset;
            if (_transformOperation == TransformOperation.LeftTopCorner)
            {
                if (_startWidth - leftOffset > 0)
                {
                    Canvas.SetLeft(_draggableListBoxItem, newLeft);
                    frameRectangle.Width = _startWidth - leftOffset;
                }
                if (_startHeight - topOffset > 0)
                {
                    Canvas.SetTop(_draggableListBoxItem, newTop);
                    frameRectangle.Height = _startHeight - topOffset;
                }
            }
            else if (_transformOperation == TransformOperation.RightTopCorner)
            {
                if (_startWidth + leftOffset > 0)
                {
                    frameRectangle.Width = _startWidth + leftOffset;
                }
                if (_startHeight - topOffset > 0)
                {
                    Canvas.SetTop(_draggableListBoxItem, newTop);
                    frameRectangle.Height = _startHeight - topOffset;
                }
            }
            else if (_transformOperation == TransformOperation.RightBottomCorner)
            {
                if (_startWidth + leftOffset > 0)
                {
                    frameRectangle.Width = _startWidth + leftOffset;
                }
                if (_startHeight + topOffset > 0)
                {
                    frameRectangle.Height = _startHeight + topOffset;
                }
            }
            else if (_transformOperation == TransformOperation.LeftBottomCorner)
            {
                if (_startWidth - leftOffset > 0)
                {
                    Canvas.SetLeft(_draggableListBoxItem, newLeft);
                    frameRectangle.Width = _startWidth - leftOffset;
                }
                if (_startHeight + topOffset > 0)
                {
                    frameRectangle.Height = _startHeight + topOffset;
                }
            }
            else if (_transformOperation == TransformOperation.LeftSide)
            {
                if (_startWidth - leftOffset <= 0) return;

                Canvas.SetLeft(_draggableListBoxItem, newLeft);
                frameRectangle.Width = _startWidth - leftOffset;
            }
            else if (_transformOperation == TransformOperation.RightSide)
            {
                if (_startWidth + leftOffset <= 0) return;

                frameRectangle.Width = _startWidth + leftOffset;
            }
            else if (_transformOperation == TransformOperation.TopSide)
            {
                if (_startHeight - topOffset <= 0) return;

                Canvas.SetTop(_draggableListBoxItem, newTop);
                frameRectangle.Height = _startHeight - topOffset;
            }
            else if (_transformOperation == TransformOperation.BottomSide)
            {
                if (_startHeight + topOffset <= 0) return;

                frameRectangle.Height = _startHeight + topOffset;
            }
            else if (_transformOperation == TransformOperation.Center)
            {
                Canvas.SetLeft(_draggableListBoxItem, newLeft);
                Canvas.SetTop(_draggableListBoxItem, newTop);
            }
        }

        private Rectangle GetFrameRectangle(ListBoxItem listBoxItem)
        {
            var contentPresenter = VisualChildHelper.FindVisualChild<ContentPresenter>(listBoxItem);
            var contentTemplate = contentPresenter.ContentTemplate;

            return (Rectangle)contentTemplate.FindName("FrameRectangle", contentPresenter);
        }

        private void ListBox_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox == null) return;

            _startPoint = Mouse.GetPosition(listBox);
        }

        private void ListBox_OnMouseLeave(object sender, MouseEventArgs e)
        {
            var canvas = sender as Canvas;
            if (canvas == null) return;

            _draggableListBoxItem = null;
            e.Handled = true;
        }

        private void ListBoxItem_OnMouseMove(object sender, MouseEventArgs e)
        {
            var listBoxItem = sender as ListBoxItem;
            if (listBoxItem == null) return;
            if (_draggableListBoxItem != null) return;

            var touchedPoint = Mouse.GetPosition(listBoxItem);

            listBoxItem.Cursor = GetCursorByPosition(touchedPoint, listBoxItem.ActualWidth, listBoxItem.ActualHeight);
        }

        private Cursor GetCursorByPosition(Point touchedPoint, double actualWidth, double actualHeight)
        {
            var transformOperation = GetTransformOperationByPosition(touchedPoint, actualWidth, actualHeight);
            return transformOperation switch
            {
                TransformOperation.LeftSide => Cursors.SizeWE,
                TransformOperation.RightSide => Cursors.SizeWE,
                TransformOperation.TopSide => Cursors.SizeNS,
                TransformOperation.BottomSide => Cursors.SizeNS,
                TransformOperation.LeftTopCorner => Cursors.SizeNWSE,
                TransformOperation.RightTopCorner => Cursors.SizeNESW,
                TransformOperation.RightBottomCorner => Cursors.SizeNWSE,
                TransformOperation.LeftBottomCorner => Cursors.SizeNESW,
                _ => Cursors.Hand
            };
        }

        private TransformOperation GetTransformOperationByPosition(Point touchedPoint, double actualWidth, double actualHeight)
        {
            const int toleranceValue = 5;
            if (touchedPoint.X < toleranceValue && touchedPoint.Y < toleranceValue)
            {
                return TransformOperation.LeftTopCorner;
            }
            if (actualWidth - touchedPoint.X < toleranceValue && touchedPoint.Y < toleranceValue)
            {
                return TransformOperation.RightTopCorner;
            }
            if (actualWidth - touchedPoint.X < toleranceValue && actualHeight - touchedPoint.Y < toleranceValue)
            {
                return TransformOperation.RightBottomCorner;
            }
            if (touchedPoint.X < toleranceValue && actualHeight - touchedPoint.Y < toleranceValue)
            {
                return TransformOperation.LeftBottomCorner;
            }
            if (actualWidth - touchedPoint.X < toleranceValue)
            {
                return TransformOperation.RightSide;
            }
            if (touchedPoint.X < toleranceValue)
            {
                return TransformOperation.LeftSide;
            }
            if (actualHeight - touchedPoint.Y < toleranceValue)
            {
                return TransformOperation.BottomSide;
            }
            if (touchedPoint.Y < toleranceValue)
            {
                return TransformOperation.TopSide;
            }

            return TransformOperation.Center;
        }
    }
}
