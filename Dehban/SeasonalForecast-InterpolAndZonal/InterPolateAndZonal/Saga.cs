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
            cmd.StartInfo.FileName = Resource.Saga;
            cmd.StartInfo.Arguments = " -f=s grid_gridding 1 " + _points + " " + _filed + " -TARGET_USER_SIZE " +
                _CellSize + " -TARGET_USER_XMIN " + _Left + " -TARGET_USER_XMAX " + _Right +
                " -TARGET_USER_YMIN " + _Buttom + " -TARGET_USER_YMAX " + _Top + " -TARGET_OUT_GRID " + _out;
            cmd.Start();
            cmd.WaitForExit();
        }

        public static void zonal(string _Grids, string _ShapeFile, string _Out)
        {
            Process cmd = new Process();
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.FileName = Resource.Saga;

            cmd.StartInfo.Arguments = " -f=s shapes_grid 2 -GRIDS=" + _Grids + " -POLYGONS=" +
                _ShapeFile + " -NAMING = 1 - METHOD = 3 - PARALLELIZED = 1 - RESULT = " + _Out +
                " - COUNT = 0 - MIN = 0 - MAX = 1 - RANGE = 0 - SUM = 0 - MEAN = 1 - VAR = 0" + 
                " - STDDEV = 0 - QUANTILE = 0";
            cmd.Start();
            cmd.WaitForExit();

            cmd.StartInfo.Arguments = " -f=s io_table 0 -TABLE=" + _Out + " -SEPARATOR=2 -FILENAME=" + Path.GetFileNameWithoutExtension(_Out) + ".csv";
            cmd.Start();
            cmd.WaitForExit();
        }
    }
}
