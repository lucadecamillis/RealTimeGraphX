using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace RealTimeGraphX.WPF
{
    /// <summary>
    /// Abstract bitmap generation from the graph surface
    /// </summary>
    public interface IBitmapGenerator
    {
        void BeginDraw(System.Drawing.SizeF size, bool sizeChanged);

        void SetTransform(GraphTransform transform);

        void DrawSeries(
            WpfGraphDataSeries dataSeries,
            IEnumerable<System.Drawing.PointF> points);

        void FillSeries(
            WpfGraphDataSeries dataSeries,
            IEnumerable<System.Drawing.PointF> points,
            System.Drawing.SizeF size);

        BitmapSource EndDraw();
    }
}