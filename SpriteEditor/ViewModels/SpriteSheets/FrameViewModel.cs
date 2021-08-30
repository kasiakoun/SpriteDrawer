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

        public bool IsShown
        {
            get => _isShown;
            set => SetProperty(ref _isShown, value);
        }

        public FrameViewModel(Frame model)
        {
            Model = model;
        }
    }
}
