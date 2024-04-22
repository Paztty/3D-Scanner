using _3D_Scanner.Core;
using _3D_Scanner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _3D_Scanner.User_Control
{
    /// <summary>
    /// Interaction logic for Viewwer3D.xaml
    /// </summary>
    public partial class Viewwer3D : UserControl
    {
        public Viewwer3D()
        {
            InitializeComponent();
        }

        public void Viewwer3D_ViewerKeyDown(object sender, EventArgs even)
        {
            var e = (sender as KeyEventArgs);
            switch (e.Key)
            {
                case Key.Up:
                    CameraPhi += CameraDPhi;
                    if (CameraPhi > Math.PI / 2.0) CameraPhi = Math.PI / 2.0;
                    break;
                case Key.Down:
                    CameraPhi -= CameraDPhi;
                    if (CameraPhi < -Math.PI / 2.0) CameraPhi = -Math.PI / 2.0;
                    break;
                case Key.Left:
                    CameraTheta += CameraDTheta;
                    break;
                case Key.Right:
                    CameraTheta -= CameraDTheta;
                    break;
                case Key.Add:
                case Key.OemPlus:
                    CameraR -= CameraDR;
                    if (CameraR < CameraDR) CameraR = CameraDR;
                    break;
                case Key.Subtract:
                case Key.OemMinus:
                    CameraR += CameraDR;
                    break;
            }
            // Update the camera's position.
            PositionCamera();
        }

        // The main object model group.
        private Model3DGroup MainModel3Dgroup = new Model3DGroup();

        // The camera.
        private PerspectiveCamera TheCamera;

        // The camera's current location.
        private double CameraPhi = Math.PI / 6.0;       // 30 degrees
        private double CameraTheta = Math.PI / 6.0;     // 30 degrees
#if SURFACE2
        private double CameraR = 3.0;
#else
        private double CameraR = 100.0;
#endif

        // The change in CameraPhi when you press the up and down arrows.
        private const double CameraDPhi = 0.1;

        // The change in CameraTheta when you press the left and right arrows.
        private const double CameraDTheta = 0.1;

        // The change in CameraR when you press + or -.
        private const double CameraDR = 10;

        // Create the scene.
        // MainViewport is the Viewport3D defined
        // in the XAML code that displays everything.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Give the camera its initial position.
            TheCamera = new PerspectiveCamera();
            TheCamera.FieldOfView = 60;
            MainViewport.Camera = TheCamera;
            PositionCamera();

            // Define lights.
            DefineLights();

            // Create the model.
            DefineModel(MainModel3Dgroup);

            // Add the group of models to a ModelVisual3D.
            ModelVisual3D model_visual = new ModelVisual3D();
            model_visual.Content = MainModel3Dgroup;

            // Display the main visual to the viewportt.
            MainViewport.Children.Add(model_visual);
        }

        // Define the lights.
        private void DefineLights()
        {
            AmbientLight ambient_light = new AmbientLight(Colors.Gray);
            DirectionalLight directional_light =
                new DirectionalLight(Colors.Gray, new Vector3D(-1.0, -3.0, -2.0));
            MainModel3Dgroup.Children.Add(ambient_light);
            MainModel3Dgroup.Children.Add(directional_light);
        }

#if SURFACE2
        private const double xmin = -1.5;
        private const double xmax = 1.5;
        private const double dx = 0.03;
        private const double zmin = -1.5;
        private const double zmax = 1.5;
        private const double dz = 0.03;
#else
        private const double xmin = -13;
        private const double xmax = 13;
        private const double dx = 0.1;
        private const double zmin = -45;
        private const double zmax = 45;
        private const double dz = 0.1;
#endif

        private const double texture_xscale = (xmax - xmin);
        private const double texture_zscale = (zmax - zmin);

        // Add the model to the Model3DGroup.
        private void DefineModel(Model3DGroup model_group)
        {
            // Make a mesh to hold the surface.
            MeshGeometry3D mesh = new MeshGeometry3D();

            // Make the surface's points and triangles.
            for (double x = xmin; x <= xmax - dx; x += dx)
            {
                for (double z = zmin; z <= zmax - dz; z += dx)
                {
                    // Make points at the corners of the surface
                    // over (x, z) - (x + dx, z + dz).
                    Point3D p00 = new Point3D(x, F(x, z), z);
                    Point3D p10 = new Point3D(x + dx, F(x + dx, z), z);
                    Point3D p01 = new Point3D(x, F(x, z + dz), z + dz);
                    Point3D p11 = new Point3D(x + dx, F(x + dx, z + dz), z + dz);

                    // Add the triangles.
                    AddTriangle(mesh, p00, p01, p11);
                    AddTriangle(mesh, p00, p11, p10);
                }
            }
            Console.WriteLine(mesh.Positions.Count + " points");
            Console.WriteLine(mesh.TriangleIndices.Count / 3 + " triangles");
            Console.WriteLine();

            // Make the surface's material using an image brush.
            ImageBrush grid_brush = new ImageBrush();
            grid_brush.ImageSource =
                new BitmapImage(new Uri("D:\\TNG\\1.Project\\4.3D scanner\\1.Document\\image.jpg", UriKind.Relative));
            DiffuseMaterial grid_material = new DiffuseMaterial(grid_brush);

            // Make the mesh's model.
            GeometryModel3D surface_model = new GeometryModel3D(mesh, grid_material);

            // Make the surface visible from both sides.
            surface_model.BackMaterial = grid_material;

            // Add the model to the model groups.
            model_group.Children.Add(surface_model);
        }

        // The function that defines the surface we are drawing.
        private double F(double x, double z)
        {
#if SURFACE2
            const double two_pi = 2 * 3.14159265;
            double r2 = x * x + z * z;
            double r = Math.Sqrt(r2);
            double theta = Math.Atan2(z, x);
            return Math.Exp(-r2) * Math.Sin(two_pi * r) * Math.Cos(3 * theta);
#else
            double r2 = x * x + z * z;
            return 8 * Math.Cos(r2 / 2) / (2 + r2);
#endif
        }

        // Add a triangle to the indicated mesh.
        // If the triangle's points already exist, reuse them.
        private void AddTriangle(MeshGeometry3D mesh, Point3D point1, Point3D point2, Point3D point3)
        {
            // Get the points' indices.
            int index1 = AddPoint(mesh.Positions, mesh.TextureCoordinates, point1);
            int index2 = AddPoint(mesh.Positions, mesh.TextureCoordinates, point2);
            int index3 = AddPoint(mesh.Positions, mesh.TextureCoordinates, point3);

            // Create the triangle.
            mesh.TriangleIndices.Add(index1);
            mesh.TriangleIndices.Add(index2);
            mesh.TriangleIndices.Add(index3);
        }

        // A dictionary to hold points for fast lookup.
        private Dictionary<Point3D, int> PointDictionary =
            new Dictionary<Point3D, int>();

        // If the point already exists, return its index.
        // Otherwise create the point and return its new index.
        private int AddPoint(Point3DCollection points,
            PointCollection texture_coords, Point3D point)
        {
            // If the point is in the point dictionary,
            // return its saved index.
            if (PointDictionary.ContainsKey(point))
                return PointDictionary[point];

            // We didn't find the point. Create it.
            points.Add(point);
            PointDictionary.Add(point, points.Count - 1);

            // Set the point's texture coordinates.
            texture_coords.Add(
                new Point(
                    (point.X - xmin) * texture_xscale,
                    (point.Z - zmin) * texture_zscale));

            // Return the new point's index.
            return points.Count - 1;
        }

        // Adjust the camera's position.
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    CameraPhi += CameraDPhi;
                    if (CameraPhi > Math.PI / 2.0) CameraPhi = Math.PI / 2.0;
                    break;
                case Key.Down:
                    CameraPhi -= CameraDPhi;
                    if (CameraPhi < -Math.PI / 2.0) CameraPhi = -Math.PI / 2.0;
                    break;
                case Key.Left:
                    CameraTheta += CameraDTheta;
                    break;
                case Key.Right:
                    CameraTheta -= CameraDTheta;
                    break;
                case Key.Add:
                case Key.OemPlus:
                    CameraR -= CameraDR;
                    if (CameraR < CameraDR) CameraR = CameraDR;
                    break;
                case Key.Subtract:
                case Key.OemMinus:
                    CameraR += CameraDR;
                    break;
            }

            // Update the camera's position.
            PositionCamera();
        }

        // Position the camera.
        private void PositionCamera()
        {
            // Calculate the camera's position in Cartesian coordinates.
            double y = CameraR * Math.Sin(CameraPhi);
            double hyp = CameraR * Math.Cos(CameraPhi);
            double x = hyp * Math.Cos(CameraTheta);
            double z = hyp * Math.Sin(CameraTheta);
            TheCamera.Position = new Point3D(x, y, z);

            // Look toward the origin.
            TheCamera.LookDirection = new Vector3D(-x,- y, -z);

            // Set the Up direction.
            TheCamera.UpDirection = new Vector3D(0, 1, 0);

            // Console.WriteLine("Camera.Position: (" + x + ", " + y + ", " + z + ")");
        }

        public void CreateView(RawData raw, BitmapImage brush, int widthCount, int lenghtCount)
        {
            MainModel3Dgroup.Children.Clear();
            MainViewport.Children.Clear();
            // Give the camera its initial position.
            TheCamera = new PerspectiveCamera();
            TheCamera.FieldOfView = 90;
            MainViewport.Camera = TheCamera;
            PositionCamera();

            // Define lights.
            DefineLights();

            // Create the model.
            DefineModel(raw, brush, widthCount, lenghtCount);

            // Add the group of models to a ModelVisual3D.
            ModelVisual3D model_visual = new ModelVisual3D();
            model_visual.Content = MainModel3Dgroup;

            // Display the main visual to the viewportt.
            MainViewport.Children.Add(model_visual);
        }

        private void DefineModel(RawData raw, BitmapImage brush, int widthCount, int lenghtCount)
        {
            PointDictionary = new Dictionary<Point3D, int>();
            // Make a mesh to hold the surface.
            MeshGeometry3D mesh = new MeshGeometry3D();
            Core.NumbericListExterntion.clean = 0;
            for (int row = 0; row < raw.Z.Count(); row++)
            {
                var ScaleRange = Core.NumbericListExterntion.FindMinMax(raw.Z[row]);
                for (int column = 0; column < raw.Z[row].Count(); column++)
                {
                    var z = raw.Z[row][column] - ScaleRange.min;
                    raw.Z[row][column] = Core.NumbericListExterntion.KalmanFilter(z) * 50;
                }
            }
            var rangeX = Core.NumbericListExterntion.FindMinMax(raw.X);
            var rangeY = Core.NumbericListExterntion.FindMinMax(raw.Y);

            var centerX = (rangeX.max - rangeX.min)/2 + rangeX.min;
            var centerY = (rangeY.max - rangeY.min)/2 + rangeY.min;

            // Make the surface's points and triangles.
            for (int y = 0; y < raw.Z.Count() - 1; y++)
            {
                for (int x = 0; x < raw.Z[0].Count() - 2 ; x++)
                {
                    try
                    {
                        //Point3D p00 = new Point3D(x * dx + xmin, y , (Z[y][x] - 80) * 10);// current point
                        //Point3D p10 = new Point3D(x * dx + dx + xmin, y , (Z[y][x + 1] - 80) * 10);
                        //Point3D p01 = new Point3D(x * dx + xmin, y * dx + dx , (Z[y + 1][x] - 80) * 10);
                        //Point3D p11 = new Point3D(x * dx + dx + xmin, y * dx + dx , (Z[y + 1][x + 1] - 80) * 10);

                        Point3D p00 = new Point3D((raw.X[y][x] - centerX) * 10, raw.Z[y][x], raw.Y[y][x] - centerY);// current point
                        Point3D p10 = new Point3D((raw.X[y][x + 1] - centerX) * 10, raw.Z[y][x + 1], raw.Y[y][x + 1] - centerY);
                        Point3D p01 = new Point3D((raw.X[y + 1][x] - centerX) * 10, raw.Z[y + 1][x], raw.Y[y+ 1][x] - centerY);
                        Point3D p11 = new Point3D((raw.X[y + 1][x + 1] - centerX) * 10, raw.Z[y + 1][x + 1], raw.Y[y + 1][x + 1] - centerY);

                        AddTriangle(mesh, p00, p01, p11);
                        AddTriangle(mesh, p00, p11, p10);
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                }
            }
            Console.WriteLine(mesh.Positions.Count + " points");
            Console.WriteLine(mesh.TriangleIndices.Count / 3 + " triangles");
            Console.WriteLine();

            // Make the surface's material using an image brush.
            ImageBrush grid_brush = new ImageBrush();
            grid_brush.ImageSource = brush;

            ScaleTransform transform = new ScaleTransform();
            transform.ScaleX = -1;
            transform.CenterX = 0.5;
            grid_brush.RelativeTransform = transform;

            //new BitmapImage(new Uri("D:\\TNG\\1.Project\\4.3D scanner\\1.Document\\Grid.png", UriKind.Relative));
            DiffuseMaterial grid_material = new DiffuseMaterial(new SolidColorBrush(Colors.Gray));

            // Make the mesh's model.
            GeometryModel3D surface_model = new GeometryModel3D(mesh, grid_material);

            // Make the surface visible from both sides.
            surface_model.BackMaterial = new DiffuseMaterial(grid_brush);

            // Add the model to the model groups.
            //MainModel3Dgroup.Children.Clear();
            MainModel3Dgroup.Children.Add(surface_model);
        }
    }
}
