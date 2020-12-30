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
            if(dataSeries.UseFill)
            {
                // Handled by FillSeries
                return;
            }

            Geometry pathGeometry = CreatePath(points, closePath: false);

            Pen pen = GetPen(dataSeries);

            context.DrawGeometry(Brushes.Transparent, pen, pathGeometry);
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
            Geometry pathGeometry = CreatePath(points, closePath: false);

            Pen pen = GetPen(dataSeries);
            Brush brush = new SolidColorBrush(dataSeries.Fill);

            // TODO: scale transform the gradient (brush)
            //    gradient.ResetTransform();
            //    gradient.ScaleTransform(size.Width / gradient.Rectangle.Width, size.Height / gradient.Rectangle.Height);

            this.context.DrawGeometry(brush, pen, pathGeometry);
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

        private PathGeometry CreatePath(IEnumerable<System.Drawing.PointF> points, bool closePath)
        {
            IEnumerable<Point> wpfPoints = points.ToWpfPoints();

            Point start = wpfPoints.FirstOrDefault();

            IList<LineSegment> segments = wpfPoints
                .Skip(1)
                .Select(s => new LineSegment(s, true))
                .ToList();

            PathFigure figure = new PathFigure(start, segments, closed: closePath); // true if closed
            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(figure);

            return geometry;
        }
    }
}