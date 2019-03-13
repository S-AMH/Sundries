using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace InterPolateAndZonal
{
    class Saga
    {
        public static void interpolate(string _points, string _filed, string _Top, string _Left,
            string _Right, string _Buttom, string _CellSize, string _out)
        {
            Process cmd = new Process();
            cmd.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.FileName = @"cmd.exe";
            cmd.StartInfo.Arguments = @"/C" + Resource.Saga + " -f=s grid_gridding 1 " + "-POINTS " + _points + " -FIELD " + _filed + " -TARGET_USER_SIZE " +
                _CellSize + " -TARGET_USER_XMIN " + _Left + " -TARGET_USER_XMAX " + _Right +
                " -TARGET_USER_YMIN " + _Buttom + " -TARGET_USER_YMAX " + _Top + " -TARGET_OUT_GRID " + _out;
            cmd.Start();
            cmd.WaitForExit();
            cmd.StartInfo.Arguments = @"/C" + "gdal_translate -of GTIFF -ot FLOAT32 " + _out.Substring(0, _out.Length - 4) + ".sdat " + _out.Substring(0, _out.Length - 4) + ".tif";
            cmd.Start();
            cmd.WaitForExit();
        }

        public static void zonal(string _Grids, string _ShapeFile, string _Out)
        {
            Process cmd = new Process();
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.FileName = "cmd.exe";

            cmd.StartInfo.Arguments = @"/C" + Resource.Saga + " -f=s shapes_grid 0 " + " -SHAPES=" +
                _ShapeFile + " -GRIDS=" + _Grids + " -RESULT=" + _Out +
                " -RESAMPLING=1";
            Console.WriteLine(cmd.StartInfo.Arguments);
            cmd.Start();
            cmd.WaitForExit();

            cmd.StartInfo.Arguments = @"/C" + Resource.Saga + " -f=s io_table 0 -TABLE=" + _Out + " -SEPARATOR=2 -FILENAME=" + _Out.Substring(0,_Out.Length-4) + ".csv";
            cmd.Start();
            cmd.WaitForExit();
        }

    }
}
