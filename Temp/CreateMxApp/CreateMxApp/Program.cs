using OfficeOpenXml;

namespace CreateMxApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            Console.WriteLine("Hello, World!");
        }
    }
}