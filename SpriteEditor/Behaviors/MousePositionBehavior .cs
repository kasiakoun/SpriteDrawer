using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace SpriteEditor.Behaviors
{
    public class MousePositionBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            nameof(Position), typeof(Point), typeof(MousePositionBehavior), new PropertyMetadata(default(Point)));

        public Point Position
        {
            get => (Point)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public static readonly DependencyProperty MultiplierProperty = DependencyProperty.Register(
            nameof(Multiplier), typeof(double), typeof(MousePositionBehavior), new PropertyMetadata(default(double)));

        public double Multiplier
        {
            get => (double)GetValue(MultiplierProperty);
            set => SetValue(MultiplierProperty, value);
        }

        protected override void OnAttached()
        {
            AssociatedObject.MouseMove += AssociatedObjectOnMouseMove;
        }

        private void AssociatedObjectOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            var pos = mouseEventArgs.GetPosition(AssociatedObject);
            Position = new Point(pos.X / Multiplier, pos.Y / Multiplier);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseMove -= AssociatedObjectOnMouseMove;
        }
    }
}
