using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

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

        /// <summary>
        /// Stream a list of gdi points into a list of WPF points
        /// </summary>
        /// <param name="gdiPoints"></param>
        /// <returns></returns>
        internal static IEnumerable<System.Windows.Point> ToWpfPoints(this IEnumerable<System.Drawing.PointF> gdiPoints)
        {
            if (gdiPoints == null)
            {
                return Enumerable.Empty<System.Windows.Point>();
            }

            return gdiPoints.Select(p => new System.Windows.Point(x: p.X, y: p.Y));
        }
    }
}