using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Timeliner.Uwp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        readonly Dictionary<string, Type> TagPagePairs = new Dictionary<string, Type>()
        {
            {"Event", typeof(EventPage) },
            {"Era", typeof(EraPage) },
            {"Group", typeof(GroupPage) },
            {"Query", typeof(GroupPage) },
            {"Settings", typeof(SettingsPage) }
        };

        private bool TryGoBack()
        {
            if (!ContentFrame.CanGoBack) return false;

            if (MainNav.IsPaneOpen &&
                (MainNav.DisplayMode == muxc.NavigationViewDisplayMode.Compact ||
                MainNav.DisplayMode == muxc.NavigationViewDisplayMode.Minimal))
                return false;

            ContentFrame.GoBack();
            return true;
        }

        private void MainNav_Navigate(
            string tag,
            Windows.UI.Xaml.Media.Animation.NavigationTransitionInfo info)
        {
            Type nextPage = null;
            if (TagPagePairs.ContainsKey(tag)) nextPage = TagPagePairs[tag];
            var prePage = ContentFrame.CurrentSourcePageType;
            if (nextPage != null && !Type.Equals(prePage, nextPage))
                ContentFrame.Navigate(nextPage, null, info);
        }

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void MainNav_BackRequested(muxc.NavigationView sender, muxc.NavigationViewBackRequestedEventArgs args)
        {
            TryGoBack();
        }

        private void MainNav_ItemInvoked(muxc.NavigationView sender, muxc.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked) MainNav_Navigate("Settings", args.RecommendedNavigationTransitionInfo);
            else if (args.InvokedItemContainer != null) MainNav_Navigate(args.InvokedItemContainer.Tag.ToString(), args.RecommendedNavigationTransitionInfo);
        }
    }
}
