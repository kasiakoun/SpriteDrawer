using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        public SpriteSheetViewModel Parent { get; }

        public string Name
        {
            get => Model.Name;
            set
            {
                Model.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public AnimationViewModel(Animation model, SpriteSheetViewModel parent)
        {
            Model = model;
            Parent = parent;
            Frames = new MvxObservableCollection<FrameViewModel>(model.Frames?.Select(p => new FrameViewModel(p, this)));
            Frames.CollectionChanged += FramesOnCollectionChanged;
        }

        private void FramesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var newFrame = e.NewItems[0] as FrameViewModel;
                Model.Frames.Add(newFrame.Model);
            }
        }
    }
}
