using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmCross.ViewModels;
using SpriteDrawer;

namespace SpriteEditor.ViewModels.SpriteSheets
{
    public class SpriteSheetViewModel : MvxNotifyPropertyChanged
    {
        public SpriteSheet Model { get; }

        public MvxObservableCollection<AnimationViewModel> Animations { get; }

        public SpriteSheetViewModel(SpriteSheet model)
        {
            Model = model;
            Animations = new MvxObservableCollection<AnimationViewModel>(model.Animations.Select(p => new AnimationViewModel(p)));
        }
    }
}
