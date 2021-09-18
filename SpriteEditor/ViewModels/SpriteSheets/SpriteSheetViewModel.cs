using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
            Animations = new MvxObservableCollection<AnimationViewModel>(model.Animations.Select(p => new AnimationViewModel(p, this)));
            Animations.CollectionChanged += AnimationsOnCollectionChanged;
        }

        private void AnimationsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var newAnimation = e.NewItems[0] as AnimationViewModel;
                Model.Animations.Add(newAnimation.Model);
            }
        }
    }
}
