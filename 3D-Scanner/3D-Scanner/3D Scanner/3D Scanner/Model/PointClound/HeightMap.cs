using _3D_Scanner.Core;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Color = System.Windows.Media.Color;

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
        public MeshGeometry3D Plane = new MeshGeometry3D();

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

                for (int row = 0; row < Raw.Z.Count(); row++)
                {
                    var ScaleRange = Core.NumbericListExterntion.FindMinMax(Raw.Z[row]);

                    for (int column = 0; column < Raw.Z[row].Count(); column++)
                    {
                        double heightLevel = -5.5 + (Raw.Z[row][column] - ScaleRange.min) * 40;
                        int white = (int)(1 / (1 + Math.Pow(Math.E, 0 - heightLevel)) * 254);

                        var color = System.Drawing.Color.FromArgb(white, white, white);
                        bm.SetPixel(column, row, color);
                    }
                }
                var Xrange = Core.NumbericListExterntion.FindMinMax(Raw.X);
                var Yrange = Core.NumbericListExterntion.FindMinMax(Raw.Y);
                int Width = (int)(Xrange.max - Xrange.min);
                int Height = (int)(Yrange.max - Yrange.min);

                var bResize = new Bitmap(bm, Width * 10, Height);
                bResize.Save("resize.png");
                var image = BitmapExterntion.Bitmap2BitmapImage(bResize);
                this.HeightMapImage = image;
                return image;
            }
        }


        public Model3D Model { get; set; }
        public PointsVisual3D pointsVisual3D { get; set; }
        public void Create3DModel()
        {
            // Create a model group
            var modelGroup = new Model3DGroup();

            //// Create a mesh builder and add a box to it
            //var meshBuilder = new MeshBuilder(false, false);
            //var mesh1Builder = new MeshBuilder(false, false);
            //var mesh2Builder = new MeshBuilder(false, false);
            //var mesh3Builder = new MeshBuilder(false, false);
            //var mesh4Builder = new MeshBuilder(false, false);
            //var mesh5Builder = new MeshBuilder(false, false);
            //var mesh6Builder = new MeshBuilder(false, false);
            //var mesh7Builder = new MeshBuilder(false, false);
            //var mesh8Builder = new MeshBuilder(false, false);
            var insideMaterial = MaterialHelper.CreateMaterial(Colors.Black);

            if (Raw == null) return;
            else
            {
                var listPoint = Raw.GetPoints();

                Point3DCollection dataList = Raw.GetPointsCollections();

                //read from ply file and append the positions to the dataList. i.e. dataList.Add(new Point3D(x,y,x));
                pointsVisual3D = new PointsVisual3D { Color = Colors.Red, Size = 10 };

                pointsVisual3D.Points = dataList;
                //    Bitmap bm = new Bitmap(@"D:\TNG\1.Project\4.3D scanner\1.Document\image.jpg");
                //    var XSize = NumbericListExterntion.FindMinMax(listPoint.Select(o => o.X).ToList());
                //    var YSize = NumbericListExterntion.FindMinMax(listPoint.Select(o => o.Y).ToList());
                //    Bitmap resize = new Bitmap(bm, (int)(XSize.max - XSize.min), (int)(YSize.max - YSize.min));


                //    var height = NumbericListExterntion.FindMinMax(listPoint.Select(o => o.Z).ToList());
                //    var delta = height.max - height.min;

                //    for (int i = 0; i < listPoint.Count; i++)
                //    {
                //        Console.WriteLine("Progress: {0}%", Math.Round((double)i / listPoint.Count * 100, 3));
                //        var meshBuilder = new MeshBuilder(false, false);
                //        meshBuilder.AddBox(listPoint[i], 0.02, 0.2, listPoint[i].Z*2);
                //        var color = bm.GetPixel((int)(listPoint[i].X - XSize.min), (int)(listPoint[i].Y - YSize.min));
                //        var Material = MaterialHelper.CreateMaterial(Color.FromArgb(255, color.R, color.G, color.B));
                //        var mesh = meshBuilder.ToMesh(true);
                //        modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = Material, BackMaterial = insideMaterial });

                //        //if (listPoint[i].Z > delta * 0.9)
                //        //    meshBuilder.AddBox(listPoint[i], 0.15, 0.2, listPoint[i].Z);
                //        //else if (listPoint[i].Z > delta * 0.8)
                //        //    mesh1Builder.AddBox(listPoint[i], 0.15, 0.2, listPoint[i].Z);
                //        //else if (listPoint[i].Z > delta * 0.7)
                //        //    mesh2Builder.AddBox(listPoint[i], 0.15, 0.2, listPoint[i].Z);
                //        //else if (listPoint[i].Z > delta * 0.6)
                //        //    mesh3Builder.AddBox(listPoint[i], 0.15, 0.2, listPoint[i].Z);
                //        //else if (listPoint[i].Z > delta * 0.5)
                //        //    mesh4Builder.AddBox(listPoint[i], 0.15, 0.2, listPoint[i].Z);
                //        //else if (listPoint[i].Z > delta * 0.4)
                //        //    mesh5Builder.AddBox(listPoint[i], 0.15, 0.2, listPoint[i].Z);
                //        //else if (listPoint[i].Z > delta * 0.2)
                //        //    mesh6Builder.AddBox(listPoint[i], 0.15, 0.2, listPoint[i].Z);
                //        //else if (listPoint[i].Z > delta * 0.1)
                //        //    mesh7Builder.AddBox(listPoint[i], 0.15, 0.2, listPoint[i].Z);
                //        //else
                //        //    mesh8Builder.AddBox(listPoint[i], 0.15, 0.2, listPoint[i].Z);
                //    }
                //}
                //// Create a mesh from the builder (and freeze it)
                ////var mesh = meshBuilder.ToMesh(true);
                ////var mesh1 = mesh1Builder.ToMesh(true);
                ////var mesh2 = mesh2Builder.ToMesh(true);
                ////var mesh3 = mesh3Builder.ToMesh(true);
                ////var mesh4 = mesh4Builder.ToMesh(true);
                ////var mesh5 = mesh5Builder.ToMesh(true);
                ////var mesh6 = mesh6Builder.ToMesh(true);
                ////var mesh7 = mesh7Builder.ToMesh(true);
                ////var mesh8 = mesh8Builder.ToMesh(true);

                //// Create some materials
                ////var Material =  MaterialHelper.CreateMaterial(Color.FromArgb(255, 0, 255, 0));
                ////var Material1 = MaterialHelper.CreateMaterial(Color.FromArgb(255, 10, 225, 0));
                ////var Material2 = MaterialHelper.CreateMaterial(Color.FromArgb(255, 20, 200, 0));
                ////var Material3 = MaterialHelper.CreateMaterial(Color.FromArgb(255, 30, 180, 0));
                ////var Material4 = MaterialHelper.CreateMaterial(Color.FromArgb(255, 40, 150, 0));
                ////var Material5 = MaterialHelper.CreateMaterial(Color.FromArgb(255, 50, 120, 0));
                ////var Material6 = MaterialHelper.CreateMaterial(Color.FromArgb(255, 80, 100, 0));
                ////var Material7 = MaterialHelper.CreateMaterial(Color.FromArgb(255, 180, 50, 5));
                ////var Material8 = MaterialHelper.CreateMaterial(Color.FromArgb(255, 255, 0, 10));

                ////var insideMaterial = MaterialHelper.CreateMaterial(Colors.Black);

                //// Add 3 models to the group (using the same mesh, that's why we had to freeze it)
                ////modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = Material, BackMaterial = insideMaterial });
                ////modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh1, Material = Material1, BackMaterial = insideMaterial });
                ////modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh2, Material = Material2, BackMaterial = insideMaterial });
                ////modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh3, Material = Material3, BackMaterial = insideMaterial });
                ////modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh4, Material = Material4, BackMaterial = insideMaterial });
                ////modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh5, Material = Material5, BackMaterial = insideMaterial });
                ////modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh6, Material = Material6, BackMaterial = insideMaterial });
                ////modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh7, Material = Material7, BackMaterial = insideMaterial });
                ////modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh8, Material = Material8, BackMaterial = insideMaterial });
                ////// Set the property, which will be bound to the Content property of the ModelVisual3D (see MainWindow.xaml)
                //this.Model = modelGroup;
            }
        }
    }
}
