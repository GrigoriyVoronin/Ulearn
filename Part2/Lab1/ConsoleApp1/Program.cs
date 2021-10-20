using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal static class StringExtension
    {
        internal static double ToInt(this string str)
        {
            return int.Parse(str);
        }
    }

    public class MenuItem
    {
        public string Caption;
        public string HotKey;
        public MenuItem[] Items;
    }
    class Program
    {
         static void Main()
        {
            //1
            var city = new City();
            city.Name = "Ekaterinburg";
            city.Location = new GeoLocation();
            city.Location.Latitude = 56.50;
            city.Location.Longitude = 60.35;
            Console.WriteLine(
                "I love {0} located at ({1},{2})",
                city.Name,
                city.Location.Longitude.ToString(CultureInfo.InvariantCulture),
                city.Location.Latitude.ToString(CultureInfo.InvariantCulture));
            //2
            GenerateMenu();
            //3
            var arg1 = "100500";
            Console.WriteLine(arg1.ToInt() + "42".ToInt());
            //4
            GetAlbums(new List<FileInfo>(1));
            //5
            var filter = new SuperBeautyImageFilter();
            filter.ImageName = "Paris.jpg";
            filter.GaussianParameter = 0.4;
            filter.Run();
        }

        public static bool Check(string fileName)
        {
            return fileName.EndsWith(".mp3") || fileName.EndsWith(".wav");
        }

        public static bool Check(DirectoryInfo dir, List<string> list)
        {
            if (!list.Contains(dir.Name))
            {
                list.Add(dir.Name);
                return true;
            }
            return false;
        }

        public static List<DirectoryInfo> GetAlbums(List<FileInfo> files)
        {
            var b = new List<string>(files.Count);
            return files
                .Where(x => Check(x.Name))
                .Select(x => x.Directory)
                .Where(x =>Check(x, b))
                .ToList();
        }

        public static MenuItem[] GenerateMenu()
        {
            return new[]
            {
                new MenuItem 
                {
                    Caption="File",
                    HotKey="F",
                    Items = new[] 
                    {
                        new MenuItem {Caption="New",HotKey="N"},
                        new MenuItem {Caption="Save", HotKey="S"} 
                    } 
                },
                new MenuItem 
                {
                    Caption="Edit",
                    HotKey="E",
                    Items = new [] 
                    {
                        new MenuItem{Caption="Copy",HotKey="C"},
                        new MenuItem {Caption="Paste", HotKey="V"} 
                    } 
                } 
            };
        }
    }

    public class SuperBeautyImageFilter
    {
        public string ImageName;
        public double GaussianParameter;

        public void Run()
        {
            Console.WriteLine($"Processing {ImageName} with parameter {GaussianParameter}");
        }
    }
}
