using _3D_Scanner.Core;
using _3D_Scanner.Model;
using _3D_Scanner.Model.PointClound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Scanner.Logic
{
    public class Program:ObserableObject
    {
		private HeightMap _HeightMap = new HeightMap();

		public HeightMap HeightMap
		{
			get { return _HeightMap; }
			set
			{
				if (_HeightMap != value)
				{
					_HeightMap = value;
					NotifyPropertyChanged(nameof(HeightMap));
				}
			}
		}

		public void ImportData()
		{
            var Raw = PointCloundImport.Import();
			HeightMap.Raw = Raw;
			Console.WriteLine("Debug point");
		}
		public void CreateHeightMap()
		{
			if(HeightMap.Raw != null)
			{
				HeightMap.CreateHeightMap();
			}
		}
	}
}
