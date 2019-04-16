using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleLogger;
using System.IO;
using ATG;

namespace DBRawDataCollectionMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            const string DB = @"C:\FWS\DB\rawData\GFS\0.13";
            DateTime startDate;
            DateTime endDate;
            DateTime currentDate;
            string start;
            string end;
            string filePath;
            string var;
            List<string> variables = new List<string>();

            SimpleLog.SetLogFile(logDir: ".//Logs", writeText: false);
            SimpleLog.Info("Program Started.");
            Console.WriteLine("Please type start date: (yyyy/MM/dd)");
            start = Console.ReadLine();

            SimpleLog.Info("start Date Entered is: " + start);

            Console.WriteLine("Please type end date: (yyyy/MM/dd)");
            end = Console.ReadLine();

            SimpleLog.Info("end date entered is: " + end);

            Console.WriteLine("Please enter filePath to save data(press enter to save to current directory)");
            filePath = Console.ReadLine();
            Console.WriteLine("Please enter variables sperated with \",\". \n" +
                "Avaliables are: APCP, APCP-12hr, APCP-24hr, CumulicativeRaster3hr \n" +
                " CumulicativeRaster3hrAPCP, Daily-APCP, Daily-RAIN, RAIN, RAIN-12hr, RAIN-24hr, SNOW, SOIL, TEMP");
            var = Console.ReadLine();

            SimpleLog.Info("requested variables are: " + var);

            if (filePath == "/n")
                filePath = Directory.GetCurrentDirectory();

            SimpleLog.Info("save directory is: " + filePath);

            startDate = new DateTime(
                Convert.ToInt32(start.Split('/')[0]),
                Convert.ToInt32(start.Split('/')[1]),
                Convert.ToInt32(start.Split('/')[2]));
            endDate = new DateTime(
                Convert.ToInt32(end.Split('/')[0]),
                Convert.ToInt32(end.Split('/')[1]),
                Convert.ToInt32(end.Split('/')[2]));

            variables = var.Split(',').ToList();

            currentDate = startDate;

            while(currentDate <= endDate)
            {
                for(int i = 0; i <= variables.Count; i ++)
                {
                    string[] run = { "00", "06", "12", "18" };
                    foreach (var r in run)
                    {
                        try
                        {
                            atgMethods.Copy(
                                Path.Combine(DB, currentDate.Year.ToString(),
                                atgMethods.xDigitNum(currentDate.Month, 2),
                                atgMethods.xDigitNum(currentDate.Day, 2), r, "3hr", variables[i]),
                                Path.Combine(filePath, currentDate.Year.ToString(),
                                atgMethods.xDigitNum(currentDate.Month, 2),
                                atgMethods.xDigitNum(currentDate.Day, 2), r, "3hr", variables[i])
                                );

                            SimpleLog.Info(Path.Combine(DB, currentDate.Year.ToString(),
                                atgMethods.xDigitNum(currentDate.Month, 2),
                                atgMethods.xDigitNum(currentDate.Day, 2), r, "3hr", variables[i])
                                + " Copied successfully.");
                        }
                        catch(Exception e)
                        {
                            SimpleLog.Error("Follwoing error Occured." + e.Message);
                            Console.WriteLine("Follwoing error Occured." + e.Message);
                        }
                        
                    }
                }
                currentDate = currentDate.AddDays(1);
            }
        }
    }
}
