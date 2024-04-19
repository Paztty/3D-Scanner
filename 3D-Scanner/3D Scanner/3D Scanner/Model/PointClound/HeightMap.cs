using _3D_Scanner.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace _3D_Scanner.Model.PointClound
{
    public class HeightMap : ObserableObject
    {
        //Variable
        private RawData _Raw = new RawData();

        public RawData Raw
        {
            get { return _Raw; }
            set
            {
                if (_Raw != value)
                {
                    _Raw = value;
                    NotifyPropertyChanged(nameof(Raw));
                }
            }
        }

        private BitmapImage _HeightMapImage = new BitmapImage();

        public BitmapImage HeightMapImage
        {
            get { return _HeightMapImage; }
            set
            {
                if (_HeightMapImage != value)
                {
                    _HeightMapImage = value;
                    NotifyPropertyChanged(nameof(HeightMapImage));
                }
            }
        }

        public HeightMap() { }

        public BitmapImage CreateHeightMap()
        {
            if (Raw == null) return null;
            else
            {
                var bm = new Bitmap(Raw.Z[0].Count(), Raw.Z.Count());

                var beginRange = NumbericListExterntion.FindMinMax(Raw.Z[0]);
                var endRange = NumbericListExterntion.FindMinMax(Raw.Z[Raw.Z.Count() - 1]);
                double minScale = ((endRange.min - beginRange.min) / Raw.Z.Count());
                double minBase = Math.Min(beginRange.min, endRange.min);
                for (int row = 0; row < Raw.Z.Count(); row++)
                {
                    var ScaleRange = Core.NumbericListExterntion.FindMinMax(Raw.Z[row]);
                    var Delta = ScaleRange.max - ScaleRange.min;

                    for (int column = 0; column < Raw.Z[row].Count(); column++)
                    {
                        double heightLevel = -5.5 + (Raw.Z[row][column] - (minBase + row * minScale)) * 20;
                        int white = (int)(1 / (1 + Math.Pow(Math.E, 0 - heightLevel)) * 254);

                        var color = Color.FromArgb(white, white, white);
                        bm.SetPixel(column, row, color);
                    }
                }
                var Xrange = Core.NumbericListExterntion.FindMinMax(Raw.X);
                var Yrange = Core.NumbericListExterntion.FindMinMax(Raw.Y);
                int Width = (int)(Xrange.max - Xrange.min);
                int Height = (int)(Yrange.max - Yrange.min);

                var bResize = new Bitmap(bm, Width*10, Height);
                bResize.Save("resize.png");
                var image = BitmapExterntion.Bitmap2BitmapImage(bResize);
                this.HeightMapImage = image;
                return image;
            }
        }


    }
}
