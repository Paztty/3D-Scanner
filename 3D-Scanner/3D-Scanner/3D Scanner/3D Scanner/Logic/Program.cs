﻿using _3D_Scanner.Core;
using _3D_Scanner.Model;
using _3D_Scanner.Model.PointClound;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;

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
			HeightMap.Raw = null;
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

		public Program()
		{

        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public Model3D Model { get; set; }
    }
}
