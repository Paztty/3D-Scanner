using _3D_Scanner.Core;
using _3D_Scanner.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace _3D_Scanner.Logic
{
    public static class PointCloundImport
    {
        public static RawData Import()
        {
            RawData raw = null;    
            OpenFileDialog  dlg = new OpenFileDialog();
            bool IsFileOpenned = (bool)dlg.ShowDialog();
            if (IsFileOpenned)
            {
                raw = new RawData();
                var lines = File.ReadAllLines(dlg.FileName);
                var linesAxis = "X";
                double lastnum = 0;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith(",,"))
                    {
                        switch (linesAxis)
                        {
                            case "X":
                                linesAxis = "Y";
                                break;
                            case "Y":
                                linesAxis = "Z";
                                NumbericListExterntion.clean = 0;
                                break;
                            default:
                                break;
                        }
                        continue;
                    }

                    switch (linesAxis)
                    {
                        case "X":
                            List<double> x_pos = new List<double>();
                            var listXPos = lines[i].Split(',').ToList();
                            foreach (var pos in listXPos)
                            {
                                if(Double.TryParse(pos, out double numPos))
                                {
                                    x_pos.Add(numPos);
                                    lastnum = numPos;
                                }
                                else
                                {
                                    x_pos.Add(lastnum);
                                }
                            }
                            raw.X.Add(x_pos);
                            break;
                        case "Y":
                            List<double> y_pos = new List<double>();
                            var listYPos = lines[i].Split(',').ToList();
                            foreach (var pos in listYPos)
                            {
                                if (Double.TryParse(pos, out double numPos))
                                {
                                    y_pos.Add(NumbericListExterntion.KalmanFilter(numPos));
                                    lastnum = numPos;
                                }
                                else
                                {
                                    y_pos.Add(lastnum);
                                }
                            }
                            raw.Y.Add(y_pos);
                            break;
                        case "Z":
                            List<double> z_pos = new List<double>();
                            var listZPos = lines[i].Split(',').ToList();
                            foreach (var pos in listZPos)
                            {
                                if (Double.TryParse(pos, out double numPos))
                                {
                                    z_pos.Add(NumbericListExterntion.KalmanFilter(numPos));
                                    lastnum = numPos;
                                }
                                else
                                {
                                    z_pos.Add(NumbericListExterntion.KalmanFilter(lastnum));
                                }
                            }
                            raw.Z.Add(z_pos);
                            break;
                        default:
                            break;
                    }
                }
            }
            return raw;
        }

        public static void ClearRawData( ref RawData data)
        {
            data.X.Clear();
            data.Y.Clear();
            data.Z.Clear();
        }
    }
}
