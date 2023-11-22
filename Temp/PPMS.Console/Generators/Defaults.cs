using System;
using System.IO;

namespace PPMS.Console.Generators
{
    public static class Defaults
    {
       private static string mainPath =
          Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"GeneratedProjectFiles");

       public static string MainPath
       {
           get { return mainPath; }
       }
        
    }
}