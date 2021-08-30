using MvvmCross.ViewModels;
using SpriteEditor.ViewModels;

namespace SpriteEditor
{
    public class CoreApp : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();
            RegisterAppStart<HomePageViewModel>();
        }
    }
}
