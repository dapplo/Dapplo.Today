using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.CaliburnMicro.NotifyIconWpf;
using Dapplo.CaliburnMicro.NotifyIconWpf.ViewModels;
using Dapplo.Today.Configuration;
using MahApps.Metro.IconPacks;

namespace Dapplo.Today.Ui.ViewModels
{
    /// <inheritdoc />
    [Export(typeof(ITrayIconViewModel))]
    public class SystemTrayContextMenuViewModel : TrayIconViewModel
    {
        private readonly IContextMenuTranslations _contextMenuTranslations;
        private readonly IEnumerable<Lazy<IMenuItem>> _contextMenuItems;

        /// <inheritdoc />
        [ImportingConstructor]
        public SystemTrayContextMenuViewModel(IContextMenuTranslations contextMenuTranslations,
            [ImportMany("contextmenu", typeof(IMenuItem))]
            IEnumerable<Lazy<IMenuItem>> contextMenuItems)
        {
            _contextMenuTranslations = contextMenuTranslations;
            _contextMenuItems = contextMenuItems;
        }

        /// <inheritdoc />
        protected override void OnActivate()
        {
            base.OnActivate();

            // Set the title of the icon (the ToolTipText) to our IContextMenuTranslations.Title
            _contextMenuTranslations.CreateDisplayNameBinding(this, nameof(IContextMenuTranslations.Title));

            var items = new List<IMenuItem>();

            // Lazy values
            items.AddRange(_contextMenuItems.Select(lazy => lazy.Value));

            var titleItem = new MenuItem
            {
                Id = "A_Title",
                Style = MenuItemStyles.Title,
                Icon = new PackIconMaterial
                {
                    Kind = PackIconMaterialKind.Calendar,
                    Background = Brushes.White,
                    Foreground = Brushes.Black
                }
            };
            _contextMenuTranslations.CreateDisplayNameBinding(titleItem, nameof(IContextMenuTranslations.Title));
            titleItem.ApplyIconForegroundColor(Brushes.DarkRed);
            items.Add(titleItem);
            items.Add(new MenuItem
            {
                Style = MenuItemStyles.Separator,
                Id = "Y_Separator"
            });

            // Create an exit item
            var exitItem = new ClickableMenuItem
            {
                Id = "Z_Exit",
                Icon = new PackIconMaterial
                {
                    Kind = PackIconMaterialKind.Close,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Stretch
                },
                ClickAction = clickedItem =>
                {
                    Application.Current.Shutdown();
                }
            };
            exitItem.ApplyIconForegroundColor(Brushes.DarkRed);
            _contextMenuTranslations.CreateDisplayNameBinding(exitItem, nameof(IContextMenuTranslations.Exit));
            items.Add(exitItem);

            ConfigureMenuItems(items);

            // Make sure the margin is set, do this AFTER the icon are set
            items.ApplyIconMargin(new Thickness(2, 2, 2, 2));

            SetIcon(new PackIconMaterial
            {
                Kind = PackIconMaterialKind.Calendar,
                Background = Brushes.White,
                Foreground = Brushes.Black
            });
            Show();
        }
    }
}
