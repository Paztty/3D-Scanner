using _3D_Scanner.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


	}
}
