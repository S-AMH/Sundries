using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleLogger;
using System.IO;

namespace ATG
{
    static public class atgMethods
    {
        /// <summary>
        ///  Converts an integer to string with fixed digit count by adding zeros to left.
        /// </summary>
        /// <param name="_number">Number which should be converted to string with fixed digit count.</param>
        /// <param name="_digitsCount">Digit Counts for Corresponding integer.</param>
        /// <returns>String Corresponding to _number integer with _digitCount digits.</returns>
        /// <exception cref="System.ArgumentException">Thrown When <c>_number</c> digit counts is lower than <c>_digitsCount</c></exception>
        static public string xDigitNum(int _number, int _digitsCount)
        {
            if (_number.ToString().Length > _digitsCount)
                throw new ArgumentException("Error: Operation is not valid. digitCounts is lower than number's length.", "_digitCount");

            string result = "";
            for (int i = 0; i < _digitsCount - _number.ToString().Length; i++)
                result += "0";
            return result + _number.ToString();
        }
        static public void Copy(string sourceDirectory, string targetDirectory)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
