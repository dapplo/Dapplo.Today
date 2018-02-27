using System.ComponentModel;
using Dapplo.Language;

namespace Dapplo.Today.Configuration
{
    /// <summary>
    /// All the translations for the context menu
    /// </summary>
    [Language("ContextMenu")]
    public interface IContextMenuTranslations : ILanguage, INotifyPropertyChanged
    {
        /// <summary>
        /// Translation for the exit entry in the context menu
        /// </summary>
        [DefaultValue("Exit")]
        string Exit { get; }

        /// <summary>
        /// Translation for the title in the context menu
        /// </summary>
        [DefaultValue("Today")]
        string Title { get; }
    }
}
