using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterPolateAndZonal
{
    class GDAL
    {
        public static bool substraction(string first, string second, string output, double? minValue = 0.0)
        {

            try
            {
                using (var drv = Gdal.GetDriverByName("GTiff"))
                {
                    if (drv == null)
                        throw new Exception("Can't get GTiff driver.");

                    using (var firstInput = Gdal.Open(first, Access.GA_ReadOnly))
                    using (var secondInput = Gdal.Open(second, Access.GA_ReadOnly))
                    {
                        if (firstInput == null)
                            throw new Exception("Can't open GDAL dataset: " + first);
                        if (secondInput == null)
                            throw new Exception("Can't open GDAL dataset: " + second);
                        var options = new[] { "" };
                        using (var newDataset = drv.CreateCopy(output, firstInput, 0, options, null, "Sample_Sub"))
                        {
                            if (newDataset == null)
                                throw new Exception("Could not create destination dataset.");
                            using (var bandFirst = firstInput.GetRasterBand(1))
                            using (var bandSecond = secondInput.GetRasterBand(1))
                            using (var bandOut = newDataset.GetRasterBand(1))
                            {
                                double noData = -999000000;
                                bandOut.SetNoDataValue(-999000000);
                                var sizeX = bandOut.XSize;
                                var numLines = bandOut.YSize;
                                for (var i = 0; i < numLines; i++)
                                {
                                    var FirstScanline = new float[sizeX];
                                    var SecondScanline = new float[sizeX];

                                    var cplReturn = bandFirst.ReadRaster(0, i, sizeX - 1, 1, FirstScanline, sizeX, 1, 0, 0);
                                    if (cplReturn != CPLErr.CE_None)
                                        throw new Exception("band.ReadRaster failed: " + Gdal.GetLastErrorMsg());
                                    cplReturn = bandSecond.ReadRaster(0, i, sizeX - 1, 1, SecondScanline, sizeX, 1, 0, 0);
                                    if (cplReturn != CPLErr.CE_None)
                                        throw new Exception("band.ReadRaster failed: " + Gdal.GetLastErrorMsg());
                                    var outputLine = new List<float>();
                                    for (var j = 0; j < sizeX; j++)
                                    {
                                        double pixelValue;
                                        if (FirstScanline[j] != noData && SecondScanline[j] != noData)
                                        {
                                            pixelValue = FirstScanline[j] - SecondScanline[j];
                                            if (pixelValue < 0)
                                                pixelValue = 0;
                                            if (minValue.HasValue)
                                                pixelValue = Math.Max(pixelValue, minValue.GetValueOrDefault());
                                        }
                                        else
                                            pixelValue = noData;
                                        outputLine.Add((float)pixelValue);
                                    }
                                    cplReturn = bandOut.WriteRaster(0, i, sizeX - 1, 1, outputLine.ToArray(), sizeX, 1, 0, 0);
                                    if (cplReturn != CPLErr.CE_None)
                                        throw new Exception("band.WriteRaster failed: " + Gdal.GetLastErrorMsg());
                                    bandOut.FlushCache();
                                    newDataset.FlushCache();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Can't open GDAL datasets: " + first + " + " + second, e);
            }

            return true;
        }
    }
}
