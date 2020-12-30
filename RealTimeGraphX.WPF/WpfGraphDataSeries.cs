using System.Windows.Media;

namespace RealTimeGraphX.WPF
{
    /// <summary>
    /// Represents a WPF <see cref="IGraphDataSeries">data series</see>.
    /// </summary>
    /// <seealso cref="RealTimeGraphX.GraphObject" />
    /// <seealso cref="RealTimeGraphX.IGraphDataSeries" />
    public class WpfGraphDataSeries : GraphObject, IGraphDataSeries
    {
        private string _name;
        /// <summary>
        /// Gets or sets the series name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChangedAuto(); }
        }

        private float _strokeThickness;
        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        public float StrokeThickness
        {
            get
            {
                return _strokeThickness;
            }
            set
            {
                _strokeThickness = value;
                RaisePropertyChangedAuto();
            }
        }

        private bool _isVisible;
        /// <summary>
        /// Gets or sets a value indicating whether this series should be visible.
        /// </summary>
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; RaisePropertyChangedAuto(); }
        }

        private Color _stroke;
        /// <summary>
        /// Gets or sets the series stroke color.
        /// </summary>
        public Color Stroke
        {
            get { return _stroke; }
            set
            {
                _stroke = value;
                RaisePropertyChangedAuto();
            }
        }

        private Color _fill;
        /// <summary>
        /// Gets or sets the series fill brush.
        /// </summary>
        public Color Fill
        {
            get { return _fill; }
            set
            {
                _fill = value;
                RaisePropertyChangedAuto();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfGraphDataSeries"/> class.
        /// </summary>
        public WpfGraphDataSeries()
        {
            StrokeThickness = 1;
            IsVisible = true;
            Stroke = Colors.DodgerBlue;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to fill the series.
        /// </summary>
        public bool UseFill
        {
            get { return Fill != null; }
        }

        private object _currentValue;
        /// <summary>
        /// Gets the current value.
        /// </summary>
        public object CurrentValue
        {
            get { return _currentValue; }
            set { _currentValue = value; RaisePropertyChangedAuto(); }
        }
    }
}