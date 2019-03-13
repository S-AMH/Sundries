using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SeasonalForecastDataDownloadLinkGenerator
{
    class Program
    {
        static string twoDigit(int n)
        {
            if (n < 10)
                return "0" + n.ToString();
            else
                return n.ToString();
        }
        static void Main(string[] args)
        {

            int tmp = 00;
            Console.WriteLine("Enter Start Year:");
            tmp = Convert.ToInt32(Console.ReadLine());
            int startYear = tmp;
            Console.WriteLine("Enter Start Month:");
            tmp = Convert.ToInt32(Console.ReadLine());
            int startMonth = tmp;
            Console.WriteLine("Enter Start Day:");
            tmp = Convert.ToInt32(Console.ReadLine());
            int startDay = tmp;
            Console.WriteLine("Enter End Year:");
            tmp = Convert.ToInt32(Console.ReadLine());
            int endYear = tmp;
            Console.WriteLine("Enter Duration in Month:");
            tmp = Convert.ToInt32(Console.ReadLine());
            int duration = tmp;

            const string fluxMainString = @"ftp://nomads.ncdc.noaa.gov/CFS/cfs_reforecast_6-hourly_9mon_flxf/";
            const string pgbfMainString = @"ftp://nomads.ncdc.noaa.gov/CFS/cfs_reforecast_6-hourly_9mon_pgbf/";

            Directory.CreateDirectory(startYear.ToString());
            Directory.SetCurrentDirectory(startYear.ToString());

            for(int i = startYear; i < endYear+1; i++)
            {
                var startDate = new DateTime(i, startMonth, startDay, 00, 00, 00);
                var currentDate = startDate;
                var flxWriter = new StreamWriter("flx" + i.ToString() + ".txt");
                var pgbfWriter = new StreamWriter("pgbf" + i.ToString() + ".txt");

                while(currentDate < startDate.AddMonths(duration))
                {
                    string flx = fluxMainString;
                    string pgbf = pgbfMainString;

                    flx += i.ToString() + @"/" + i.ToString()+twoDigit(startMonth) + @"/" + i.ToString()+twoDigit(startMonth)+twoDigit(startDay);
                    flx += @"/" + "flxf" + currentDate.Year.ToString() + twoDigit(currentDate.Month) + twoDigit(currentDate.Day) +
                        twoDigit(currentDate.Hour) + ".01." + i.ToString() + twoDigit(startMonth) + twoDigit(startDay) + "00.grb2";
                    pgbf +=  i.ToString() + @"/" + i.ToString() + twoDigit(startMonth) + @"/" + i.ToString() + twoDigit(startMonth) + twoDigit(startDay);
                    pgbf += @"/" + "pgbf" + currentDate.Year.ToString() + twoDigit(currentDate.Month) + twoDigit(currentDate.Day) +
                        twoDigit(currentDate.Hour) + ".01." + i.ToString() + twoDigit(startMonth) + twoDigit(startDay) + "00.grb2";
                    flxWriter.WriteLine(flx);
                    pgbfWriter.WriteLine(pgbf);
                    currentDate = currentDate.AddHours(06);
                }
                flxWriter.Close();
                pgbfWriter.Close();
            }
            Console.WriteLine("Job Finished. Output is stored in " + startYear.ToString() + " Directory.");
            Console.ReadKey();
        }
    }
}
