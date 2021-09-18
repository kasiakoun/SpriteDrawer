using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.ViewModels;
using SpriteDrawer;

namespace SpriteEditor.ViewModels.SpriteSheets
{
    public class FrameViewModel : MvxNotifyPropertyChanged
    {
        private bool _isShown;
        public Frame Model { get; }

        public AnimationViewModel Parent { get; }

        public bool IsShown
        {
            get => _isShown;
            set => SetProperty(ref _isShown, value);
        }

        public int X
        {
            get => Model.X;
            set
            {
                Model.X = value;
                RaisePropertyChanged(() => X);
            }
        }
        public int Y
        {
            get => Model.Y;
            set
            {
                Model.Y = value;
                RaisePropertyChanged(() => Y);
            }
        }

        public int Width
        {
            get => Model.Width;
            set
            {
                Model.Width = value;
                RaisePropertyChanged(() => Width);
            }
        }

        public int Height
        {
            get => Model.Height;
            set
            {
                Model.Height = value;
                RaisePropertyChanged(() => Height);
            }
        }

        public FrameViewModel(Frame model, AnimationViewModel parent)
        {
            Model = model;
            Parent = parent;
        }
    }
}
