using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace PuzzleGame
{
    class Piece : Grid
    {
        #region attributes
        Path path;
        ImageSource imageSource;
        string imageUri;
        double x, y;
        int index;
        #endregion

        #region constructor
        public Piece(ImageSource imageSource, double x, double y)
        {
            ImageUri = imageUri;
            X = x;
            Y = y;

            path = new Path();
            
            path.Fill = new ImageBrush()
            {
                ImageSource = imageSource,
                Stretch = Stretch.Uniform,
                ViewportUnits = BrushMappingMode.Absolute,
                Viewport = new Rect(0, 0, 100, 100),
                ViewboxUnits = BrushMappingMode.Absolute,
                Viewbox = new Rect(
                    x * 100,
                    y * 100,
                    100,
                    100
                    )
            };
            path.Data = new RectangleGeometry(new Rect(0, 0, 100, 100));
            this.Children.Add(path);
        }
        #endregion

        #region methods

        #endregion

        #region properties
        public string ImageUri { get { return imageUri; } set { imageUri = value; } }
        public double X { get { return x; } set { x = value; } }
        public double Y { get { return y; } set { y = value; } }
        public int Index { get { return index; } set { index = value; } }
        #endregion

    }
}
