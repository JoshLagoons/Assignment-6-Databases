using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Assignment6_DD
{
    public static class Constants
    {
        public static string Folder = @"C:\Users\13129\Desktop\Assignment6_DD\data\";
        public static string output = @"C:\Users\13129\Desktop\Assignment6_DD\data\Output.txt";
        public static string dirPath => Path.Combine(Directory.GetCurrentDirectory(), Folder);
        public static string database => "[dbo].[Produce]";
    }
}
