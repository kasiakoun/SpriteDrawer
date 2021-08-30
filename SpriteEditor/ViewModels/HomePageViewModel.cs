using System;
using System.Collections.Generic;
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

        private object _selectedItem;
        private FrameViewModel _selectedFrame;
        private MvxCommand _saveCommand;
        private readonly SpriteSheetConverter _loader = new SpriteSheetConverter();
        public List<SpriteSheetViewModel> SpriteSheets { get; private set; }

        public List<FrameViewModel> AvailableFrames => SpriteSheets[0].Animations.SelectMany(p => p.Frames).ToList();

        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                SelectedFrame = _selectedItem switch
                {
                    SpriteSheetViewModel spriteSheet => spriteSheet.Animations[0].Frames[0],
                    AnimationViewModel animation => animation.Frames[0],
                    FrameViewModel frame => frame,
                    _ => throw new ArgumentException()
                };
            }
        }

        public FrameViewModel SelectedFrame
        {
            get => _selectedFrame;
            set => SetProperty(ref _selectedFrame, value);
        }

        public BitmapImage SpriteBitmap { get; private set; }

        public Point InitialPosition { get; set; }

        public MvxCommand SaveCommand => _saveCommand ?? (new MvxCommand(SaveFile));

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
