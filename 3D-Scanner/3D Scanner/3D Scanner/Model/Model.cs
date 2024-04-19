using _3D_Scanner.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Scanner.Model
{
    public class Model:ObserableObject
    {
		public static string Ext { get; } = ".mdl";
		private string _Name = "New model";

		public string Name
		{
			get { return _Name; }
			set
			{
				if (_Name != value)
				{
					_Name = value;
					NotifyPropertyChanged(nameof(Name));
				}
			}
		}

	}
}
