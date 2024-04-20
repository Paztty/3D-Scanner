using _3D_Scanner.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace _3D_Scanner.Model
{
    public class RawData:ObserableObject
    {
		private List<List<double>> _X = new List<List<double>>();

		public List<List<double>> X
		{
			get { return _X; }
			set
			{
				if (_X != value)
				{
					_X = value;
					NotifyPropertyChanged(nameof(X));
					}
			}
		}
		private List<List<double>> _Y = new List<List<double>>();

		public List<List<double>> Y
		{
			get { return _Y; }
			set
			{
				if (_Y != value)
				{
					_Y = value;
					NotifyPropertyChanged(nameof(Y));
					}
			}
		}
		private List<List<double>> _Z = new List<List<double>>();

        public List<List<double>> Z
		{
			get { return _Z; }
			set
			{
				if (_Z != value)
				{
					_Z = value;
					NotifyPropertyChanged(nameof(Z));
					}
			}
		}

		public List<Point3D> Points = new List<Point3D>();
		public List<Point3D> GetPoints()
		{
            List<Point3D> Points = new List<Point3D>();
			var minZ = NumbericListExterntion.FindMinMax(Z);
			for (int row = 0; row < X.Count; row += 1)
			{
				var minZRow = NumbericListExterntion.FindMinMax(Z[row]);
                for (int column = 0; column < X[row].Count; column += 5)
				{
					Points.Add(new Point3D(X[row][column], Y[row][column]/10,(Z[row][column] - (minZRow.min - minZ.min) - minZ.min)));
				}
			}
			return Points;
        }

    }
}
