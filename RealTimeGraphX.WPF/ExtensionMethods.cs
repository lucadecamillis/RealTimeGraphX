using System.ComponentModel;
using System.Drawing;

namespace RealTimeGraphX.WPF
{
    /// <summary>
    /// Contains a collection of extension methods.
    /// </summary>
    internal static class ExtensionMethods
    {
        /// <summary>
        /// Converts this WPF color to a GDI color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        internal static Color ToGdiColor(this System.Windows.Media.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Determines whether this dependency object is running in design mode.
        /// </summary>
        /// <param name="obj">The object.</param>
        internal static bool IsInDesignMode(this System.Windows.DependencyObject obj)
        {
            return (DesignerProperties.GetIsInDesignMode(obj));
        }
    }
}