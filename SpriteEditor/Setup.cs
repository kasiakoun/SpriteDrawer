using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.ViewModels;

namespace SpriteEditor
{
    public class Setup : MvxWpfSetup<CoreApp>
    {
        protected override ILoggerProvider CreateLogProvider()
        {
            return null;
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            return null;
        }
    }
}
