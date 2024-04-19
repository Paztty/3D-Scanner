using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Drawing.Imaging;
using System.IO;

namespace _3D_Scanner.Core
{

    public class ObserableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class NumbericListExterntion
    {
        public static (double min, double max) FindMinMax(List<List<double>> doubles)
        {
            double min = double.MaxValue;
            double max = double.MinValue;
            foreach (var line in doubles)
            {
                foreach (var num in line)
                {
                    min = min > num ? num : min;
                    max = max > num ? max : num;
                }
            }
            return (min, max);
        }
        public static (double min, double max) FindMinMax(List<double> doubles)
        {
            double min = double.MaxValue;
            double max = double.MinValue;

            foreach (var num in doubles)
            {
                min = min > num ? num : min;
                max = max > num ? max : num;
            }
            return (min, max);
        }
        static double clean = 0;
        public static double KalmanFilter(double noisy)
        {
            double K = 0.115;  // noise 0 < K < 1
            clean += K * (noisy - clean);
            return clean;
        }
    }

    public static class BitmapExterntion
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public static BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }

}
