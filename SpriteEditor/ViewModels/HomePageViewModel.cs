using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Ookii.Dialogs.Wpf;
using SpriteDrawer;
using SpriteEditor.ViewModels.SpriteSheets;

namespace SpriteEditor.ViewModels
{
    public class HomePageViewModel : MvxViewModel
    {
        private readonly SpriteSheetConverter _loader = new SpriteSheetConverter();

        private MvxObservableCollection<SpriteSheetViewModel> _spriteSheets;

        private object _selectedItem;
        private FrameViewModel _selectedFrame;
        private SpriteSheetViewModel _selectedSpriteSheet;
        private AnimationViewModel _selectedAnimation;

        private string _lastAssetsPath;

        private MvxCommand _saveCommand;
        private MvxCommand _openCommand;
        private MvxCommand _addSpriteCommand;
        private MvxCommand _addAnimationCommand;
        private MvxCommand _addFrameCommand;

        public MvxObservableCollection<SpriteSheetViewModel> SpriteSheets
        {
            get => _spriteSheets;
            private set => SetProperty(ref _spriteSheets, value);
        }

        public List<FrameViewModel> AvailableFrames => SelectedSpriteSheet?.Animations?.SelectMany(p => p.Frames).ToList();

        public readonly Dictionary<string, BitmapImage> BitmapImageDictionary = new Dictionary<string, BitmapImage>();

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
            set
            {
                SetProperty(ref _selectedSpriteSheet, value);
                RaisePropertyChanged(() => AvailableFrames);
            }
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

        public Point InitialPosition { get; set; }

        public MvxCommand SaveCommand => _saveCommand ?? (new MvxCommand(SaveFile));
        public MvxCommand OpenCommand => _openCommand ?? (new MvxCommand(OpenFolder));
        public MvxCommand AddSpriteCommand => _addSpriteCommand ?? (new MvxCommand(AddSprite));
        public MvxCommand AddAnimationCommand => _addAnimationCommand ?? (new MvxCommand(AddAnimation));
        public MvxCommand AddFrameCommand => _addFrameCommand ?? (new MvxCommand(AddFrame));

        public void OpenFolder()
        {
            var folderDialog = new VistaFolderBrowserDialog();
            if (!folderDialog.ShowDialog().GetValueOrDefault(false)) return;

            _lastAssetsPath = GetAssetsPath(folderDialog.SelectedPath);
            if (_lastAssetsPath == null)
            {
                MessageBox.Show("This path does not contain assets directory",
                    "Invalid path",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            var directories = Directory
                .GetDirectories(folderDialog.SelectedPath, string.Empty, SearchOption.AllDirectories)
                .Select(p => new DirectoryInfo(p))
                .ToList();
            var spriteSheets = new List<SpriteSheetViewModel>();
            foreach (var directory in directories)
            {
                var files = directory.GetFiles("*.json");
                var createdSpriteSheets = GetSpriteSheets(files);
                spriteSheets.AddRange(createdSpriteSheets);
            }

            if (spriteSheets.Count == 0) return;

            foreach (var spriteSheet in spriteSheets)
            {
                FillBitmap(spriteSheet);
            }

            SpriteSheets = new MvxObservableCollection<SpriteSheetViewModel>(spriteSheets);
            SelectedItem = SpriteSheets[0];
        }

        private bool FillBitmap(SpriteSheetViewModel spriteSheetViewModel)
        {
            if (!BitmapImageDictionary.ContainsKey(spriteSheetViewModel.ImagePath))
            {
                var pathToImage = Path.Combine(_lastAssetsPath, spriteSheetViewModel.ImagePath);
                try
                {
                    var bitmap = new BitmapImage(new Uri(pathToImage, UriKind.Absolute));
                    BitmapImageDictionary[spriteSheetViewModel.ImagePath] = bitmap;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return false;
                }
            }

            spriteSheetViewModel.SpriteImage = BitmapImageDictionary[spriteSheetViewModel.ImagePath];

            return true;
        }

        private string GetAssetsPath(string path)
        {
            if (path == null) return null;

            var pathName = Path.GetFileName(path);
            if (pathName == "assets") return path;

            return GetAssetsPath(Path.GetDirectoryName(path));
        }

        private List<SpriteSheetViewModel> GetSpriteSheets(FileInfo[] jsonFiles)
        {
            var spriteSheets = new List<SpriteSheetViewModel>();
            foreach (var jsonFile in jsonFiles)
            {
                var spriteSheetViewModel = CreateSpriteSheetByPath(jsonFile.FullName);
                if (spriteSheetViewModel == null) continue;
                spriteSheets.Add(spriteSheetViewModel);
            }

            return spriteSheets;
        }

        private SpriteSheetViewModel CreateSpriteSheetByPath(string jsonPath)
        {
            try
            {
                var spriteSheet = _loader.LoadSpriteSheetByPath(jsonPath);
                var spriteSheetViewModel = new SpriteSheetViewModel(spriteSheet)
                {
                    PathToFile = jsonPath
                };


                return spriteSheetViewModel;
            }
            catch
            {
                return null;
            }
        }

        public void AddSprite()
        {
            var emptySprite = CreateEmptySprite();
            SpriteSheets.Add(emptySprite);
            RaisePropertyChanged(() => AvailableFrames);
        }

        public void AddAnimation()
        {
            if (SelectedSpriteSheet == null) return;

            var emptyAnimation = CreateEmptyAnimation(SelectedSpriteSheet);
            SelectedSpriteSheet.Animations.Add(emptyAnimation);
            RaisePropertyChanged(() => AvailableFrames);
        }

        public void AddFrame()
        {
            if (SelectedAnimation == null) return;

            var emptyFrame = CreateEmptyFrame(SelectedAnimation);
            SelectedAnimation.Frames.Add(emptyFrame);
            RaisePropertyChanged(() => AvailableFrames);
        }

        private SpriteSheetViewModel CreateEmptySprite()
        {
            var emptySprite = new SpriteSheet
            {
                Animations = new List<Animation>(),
                Name = "empty"
            };

            return new SpriteSheetViewModel(emptySprite);
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
            if (SelectedSpriteSheet == null) return;

            if (string.IsNullOrEmpty(SelectedSpriteSheet.PathToFile))
            {
                var fileDialog = new VistaSaveFileDialog
                {
                    FileName = SelectedSpriteSheet.Name,
                    DefaultExt = ".json",
                    Filter = "JavaScript Object Notation (.json)|*.json"
                };
                var result = fileDialog.ShowDialog();
                if (!result.GetValueOrDefault(false)) return;
            }

            _loader.SaveSpriteSheetByPath(SelectedSpriteSheet.Model, SelectedSpriteSheet.PathToFile);
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            InitialPosition = new Point(50, 50);
        }
    }
}
