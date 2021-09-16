using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using SpriteDrawer;
using SpriteEditor.ViewModels.SpriteSheets;

namespace SpriteEditor.ViewModels
{
    public class HomePageViewModel : MvxViewModel
    {
        private const string PathToSprite = @"e:\Projects\mk3\src\assets\data\sprites\cyrax.json";

        private readonly SpriteSheetConverter _loader = new SpriteSheetConverter();

        private object _selectedItem;
        private FrameViewModel _selectedFrame;
        private SpriteSheetViewModel _selectedSpriteSheet;
        private AnimationViewModel _selectedAnimation;

        private MvxCommand _saveCommand;
        private MvxCommand _addAnimationCommand;
        private MvxCommand _addFrameCommand;

        public List<SpriteSheetViewModel> SpriteSheets { get; private set; }

        public List<FrameViewModel> AvailableFrames => SpriteSheets[0].Animations.SelectMany(p => p.Frames).ToList();

        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                switch (_selectedItem)
                {
                    case SpriteSheetViewModel spriteSheet:
                        SelectedSpriteSheet = spriteSheet;
                        SelectedAnimation = null;
                        SelectedFrame = spriteSheet.Animations?.FirstOrDefault()?.Frames?.FirstOrDefault();
                        break;
                    case AnimationViewModel animation:
                        SelectedSpriteSheet = animation.Parent;
                        SelectedAnimation = animation;
                        SelectedFrame = animation.Frames.FirstOrDefault();
                        break;
                    case FrameViewModel frame:
                        SelectedSpriteSheet = frame.Parent.Parent;
                        SelectedAnimation = frame.Parent;
                        SelectedFrame = frame;
                        break;
                }
            }
        }

        public SpriteSheetViewModel SelectedSpriteSheet
        {
            get => _selectedSpriteSheet;
            set => SetProperty(ref _selectedSpriteSheet, value);
        }

        public AnimationViewModel SelectedAnimation
        {
            get => _selectedAnimation;
            set => SetProperty(ref _selectedAnimation, value);
        }

        public FrameViewModel SelectedFrame
        {
            get => _selectedFrame;
            set => SetProperty(ref _selectedFrame, value);
        }

        public BitmapImage SpriteBitmap { get; private set; }

        public Point InitialPosition { get; set; }

        public MvxCommand SaveCommand => _saveCommand ?? (new MvxCommand(SaveFile));
        public MvxCommand AddAnimationCommand => _addAnimationCommand ?? (new MvxCommand(AddAnimation));
        public MvxCommand AddFrameCommand => _addFrameCommand ?? (new MvxCommand(AddFrame));

        public void AddAnimation()
        {
            var emptyAnimation = CreateEmptyAnimation(SelectedSpriteSheet);
            SelectedSpriteSheet.Animations.Add(emptyAnimation);
            RaisePropertyChanged(() => AvailableFrames);
        }

        public void AddFrame()
        {
            var emptyFrame = CreateEmptyFrame(SelectedAnimation);
            SelectedAnimation.Frames.Add(emptyFrame);
            RaisePropertyChanged(() => AvailableFrames);
        }

        private AnimationViewModel CreateEmptyAnimation(SpriteSheetViewModel parent)
        {
            var emptyAnimationModel = new Animation
            {
                Frames = new List<Frame>(),
                Name = "empty"
            };

            return new AnimationViewModel(emptyAnimationModel, parent);
        }

        private FrameViewModel CreateEmptyFrame(AnimationViewModel parent)
        {
            var emptyFrameModel = new Frame
            {
                Height = 50,
                Width = 50
            };
            return new FrameViewModel(emptyFrameModel, parent);
        }

        public void SaveFile()
        {
            _loader.SaveSpriteSheetByPath(SpriteSheets[0].Model, PathToSprite);
        }

        public override async Task Initialize ()
        {
            await base.Initialize();

            var spriteSheet = _loader.LoadSpriteSheetByPath(PathToSprite);

            var spriteSheetViewModel = new SpriteSheetViewModel(spriteSheet);
            SpriteSheets = new List<SpriteSheetViewModel> { spriteSheetViewModel };
            SelectedItem = SpriteSheets[0];

            const string pathToImage = @"e:\Projects\mk3\src\assets\graphics\units\cyrax.png";
            SpriteBitmap = new BitmapImage(new Uri(pathToImage, UriKind.Absolute));

            InitialPosition = new Point(50, 50);
        }
    }
}
