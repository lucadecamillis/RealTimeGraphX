using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RealTimeGraphX.WPF.Generators
{
    internal class WpfGenerator : IBitmapGenerator
    {
        private RenderTargetBitmap bitmap;
        private DrawingVisual visual;
        private DrawingContext context;

        public void BeginDraw(System.Drawing.SizeF size, bool sizeChanged)
        {
            if (sizeChanged)
            {
                int pixelWidth = (int)Math.Max(size.Width, 1);
                int pixelHeight = (int)Math.Max(size.Height, 1);
                double dpiX = 96.0;
                double dpiY = 96.0;
                PixelFormat pixelFormat = PixelFormats.Pbgra32;

                this.bitmap = new RenderTargetBitmap(pixelWidth, pixelHeight, dpiX, dpiY, pixelFormat);

                this.visual = new DrawingVisual();
            }

            this.context = this.visual.RenderOpen();
            this.bitmap.Clear();
        }

        public void DrawSeries(WpfGraphDataSeries dataSeries, IEnumerable<System.Drawing.PointF> points)
        {
            Pen pen = GetPen(dataSeries);

            IEnumerable<Point> wpfPoints = points.Select(x => new Point(x.X, x.Y));

            Point start = wpfPoints.FirstOrDefault();
            List<LineSegment> segments = new List<LineSegment>();

            foreach (Point other in wpfPoints.Skip(1))
            {
                segments.Add(new LineSegment(other, true));
            }
            PathFigure figure = new PathFigure(start, segments, false); //true if closed
            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(figure);

            context.DrawGeometry(Brushes.Transparent, pen, geometry);
        }

        public BitmapSource EndDraw()
        {
            this.bitmap.Render(this.visual);

            var cloned = bitmap.Clone();
            cloned.Freeze();

            this.context.Close();

            return cloned;
        }

        public void FillSeries(WpfGraphDataSeries dataSeries, IEnumerable<System.Drawing.PointF> points, System.Drawing.SizeF size)
        {
            // TODO
        }

        public void SetTransform(GraphTransform transform)
        {
            Transform tr = new TranslateTransform(transform.TranslateX, transform.TranslateY);
            Transform sc = new ScaleTransform(transform.ScaleX, transform.ScaleY);

            this.context.PushTransform(tr);
            this.context.PushTransform(sc);
        }

        private Pen GetPen(WpfGraphDataSeries dataSeries)
        {
            SolidColorBrush brush = new SolidColorBrush(dataSeries.Stroke);
            return new Pen(brush, dataSeries.StrokeThickness);
        }
    }
}