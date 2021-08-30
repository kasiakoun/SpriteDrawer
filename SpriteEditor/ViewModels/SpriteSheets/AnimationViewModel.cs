using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmCross.ViewModels;
using SpriteDrawer;

namespace SpriteEditor.ViewModels.SpriteSheets
{
    public class AnimationViewModel : MvxNotifyPropertyChanged
    {
        public Animation Model { get; }

        public MvxObservableCollection<FrameViewModel> Frames { get; }

        public AnimationViewModel(Animation model)
        {
            Model = model;
            Frames = new MvxObservableCollection<FrameViewModel>(model.Frames.Select(p => new FrameViewModel(p)));
        }
    }
}
