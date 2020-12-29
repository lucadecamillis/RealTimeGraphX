using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace RealTimeGraphX.WPF.Generators
{
    internal class GdiGenerator : IBitmapGenerator
    {
        private System.Windows.Media.Imaging.WriteableBitmap _writeable_bitmap;
        private System.Drawing.Bitmap _gdi_bitmap;
        private System.Drawing.Graphics _g;

        public void BeginDraw(System.Drawing.SizeF size, bool sizeChanged)
        {
            if (sizeChanged)
            {
                _writeable_bitmap = new System.Windows.Media.Imaging.WriteableBitmap((int)Math.Max(size.Width, 1), (int)Math.Max(size.Height, 1), 96.0, 96.0, System.Windows.Media.PixelFormats.Pbgra32, null);

                _gdi_bitmap = new System.Drawing.Bitmap(_writeable_bitmap.PixelWidth, _writeable_bitmap.PixelHeight,
                                             _writeable_bitmap.BackBufferStride,
                                             System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                                             _writeable_bitmap.BackBuffer);
            }

            _writeable_bitmap.Lock();

            _g = System.Drawing.Graphics.FromImage(_gdi_bitmap);
            _g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            _g.Clear(System.Drawing.Color.Transparent);
        }

        public void DrawSeries(WpfGraphDataSeries dataSeries, IEnumerable<System.Drawing.PointF> points)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddLines(points.ToArray());
            _g.DrawPath(GetPen(dataSeries), path);
            path.Dispose();
        }

        public System.Windows.Media.Imaging.BitmapSource EndDraw()
        {
            _writeable_bitmap.AddDirtyRect(new System.Windows.Int32Rect(0, 0, _writeable_bitmap.PixelWidth, _writeable_bitmap.PixelHeight));
            _writeable_bitmap.Unlock();

            var cloned = _writeable_bitmap.Clone();
            cloned.Freeze();

            _g.Dispose();

            return cloned;
        }

        public void FillSeries(WpfGraphDataSeries dataSeries, IEnumerable<System.Drawing.PointF> points, System.Drawing.SizeF size)
        {
            var brush = GetFill(dataSeries);

            if (brush is System.Drawing.Drawing2D.LinearGradientBrush)
            {
                var gradient = brush as System.Drawing.Drawing2D.LinearGradientBrush;
                gradient.ResetTransform();
                gradient.ScaleTransform(size.Width / gradient.Rectangle.Width, size.Height / gradient.Rectangle.Height);
            }

            _g.FillPolygon(brush, points.ToArray());
        }

        public void SetTransform(GraphTransform transform)
        {
            _g.TranslateTransform((float)transform.TranslateX, (float)transform.TranslateY);
            _g.ScaleTransform((float)transform.ScaleX, (float)transform.ScaleY);
        }

        private System.Drawing.Pen GetPen(WpfGraphDataSeries dataSeries)
        {
            return new System.Drawing.Pen(GetStroke(dataSeries), dataSeries.StrokeThickness);
        }

        private System.Drawing.Color GetStroke(WpfGraphDataSeries dataSeries)
        {
            if (dataSeries.Stroke != null)
            {
                return dataSeries.Stroke.ToGdiColor();
            }
            else
            {
                return System.Drawing.Color.Transparent;
            }
        }

        private System.Drawing.Brush GetFill(WpfGraphDataSeries dataSeries)
        {
            if (dataSeries.Fill != null)
            {
                return ToGdiBrush(dataSeries.Fill);
            }

            return null;
        }

        /// <summary>
        /// Converts this WPF brush to a GDI brush.
        /// </summary>
        /// <param name="brush">The brush.</param>
        /// <returns></returns>
        private Brush ToGdiBrush(System.Windows.Media.Brush brush)
        {
            if (brush.GetType() == typeof(System.Windows.Media.SolidColorBrush))
            {
                return new SolidBrush((brush as System.Windows.Media.SolidColorBrush).Color.ToGdiColor());
            }
            else if (brush.GetType() == typeof(System.Windows.Media.LinearGradientBrush))
            {
                System.Windows.Media.LinearGradientBrush b = brush as System.Windows.Media.LinearGradientBrush;

                double angle = Math.Atan2(b.EndPoint.Y - b.StartPoint.Y, b.EndPoint.X - b.StartPoint.X) * 180 / Math.PI;

                LinearGradientBrush gradient = new LinearGradientBrush(new Rectangle(0, 0, 200, 100), Color.Black, Color.Black, (float)angle);

                ColorBlend blend = new ColorBlend();

                List<Color> colors = new List<Color>();
                List<float> offsets = new List<float>();

                foreach (var stop in b.GradientStops)
                {
                    colors.Add(stop.Color.ToGdiColor());
                    offsets.Add((float)stop.Offset);
                }

                blend.Colors = colors.ToArray();
                blend.Positions = offsets.ToArray();

                gradient.InterpolationColors = blend;

                return gradient;
            }
            else
            {
                return new LinearGradientBrush(new PointF(0, 0), new Point(200, 100), Color.Black, Color.Black);
            }
        }
    }
}