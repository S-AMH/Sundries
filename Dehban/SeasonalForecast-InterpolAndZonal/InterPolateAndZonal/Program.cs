using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InterPolateAndZonal
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length != 3)
            {
                Console.WriteLine("Error! Only 2 arguments should entered. First is shapefile name and other one is base tiff name.");
                Console.WriteLine("Usage: InterPolateAndZonal.exe [ShapeFileAddress] [BaseTiff Address]");
                Console.WriteLine("(Without barricades.)");
                return;
            }

            string shapeFile = args[1];
            string baseTif = args[2];

            if (!File.Exists(shapeFile))
            {
                Console.WriteLine("Error. ShapeFile does not exist.");
                return;
            }
            if(!File.Exists(baseTif))
            {
                Console.WriteLine("Error. Base Tiff does not exist.");
                return;
            }

            if (Directory.Exists(Path.GetFileNameWithoutExtension(shapeFile)))
                Directory.Delete(Path.GetFileNameWithoutExtension(shapeFile), true);
            Directory.CreateDirectory(Path.GetFileNameWithoutExtension(shapeFile));


        }
    }
}
