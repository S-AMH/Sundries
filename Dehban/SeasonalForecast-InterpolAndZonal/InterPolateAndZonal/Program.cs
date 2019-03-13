using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OSGeo.GDAL;
using OSGeo.OGR;
using OSGeo.OSR;

namespace InterPolateAndZonal
{
    class Program
    {
        static void Main(string[] args)
        {
            GdalConfiguration.ConfigureGdal();
            GdalConfiguration.ConfigureOgr();
            Gdal.AllRegister();
            Ogr.RegisterAll();

            if (args.Length != 1)
            {
                Console.WriteLine("Error! Only 1 arguments should entered. First is shapefile name and other one is base tiff name.");
                Console.WriteLine("Usage: InterPolateAndZonal.exe [ShapeFileAddress]");
                Console.WriteLine("(Without barricades.)");
                return;
            }

            string shapeFile = args[0];

            if (!File.Exists(shapeFile))
            {
                Console.WriteLine("Error. ShapeFile does not exist.");
                return;
            }

            if (Directory.Exists(Path.GetFileNameWithoutExtension(shapeFile)))
                Directory.Delete(Path.GetFileNameWithoutExtension(shapeFile), true);

            string dirAddr = Directory.CreateDirectory(Path.GetFileNameWithoutExtension(shapeFile)).FullName;

            Parallel.For(1, 80,
                index =>
                {
                    Saga.interpolate(shapeFile, "m" + index.ToString(), Resource.Top, Resource.Left,
                    Resource.Right, Resource.Bottom, Resource.CellSize,
                    Path.Combine(dirAddr, index.ToString() + ".tif"));
                });

            string grids = "";
            for (int i = 1; i <= 79; i++)
                grids += "\""+ Path.Combine(dirAddr,i.ToString() + ".tif")+"\""+";";
            grids = grids.Substring(0, grids.Length - 1);

            Saga.zonal(grids, shapeFile, Path.Combine(dirAddr, "zonal.shp"));
            if (Directory.Exists(Path.Combine(dirAddr, "normalized")))
                Directory.Delete(Path.Combine(dirAddr, "normalized"), true);
            Directory.CreateDirectory(Path.Combine(dirAddr, "normalized"));
            for(int i = 1; i < 79; i ++)
                GDAL.substraction(Path.Combine(dirAddr, i.ToString() + ".tif"), Path.Combine(dirAddr, "79.tif"), Path.Combine(dirAddr, "normalized", i.ToString() + ".tif"));

        }
    }
}
