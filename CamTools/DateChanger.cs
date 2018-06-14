using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System.Globalization;

namespace CamTools
{
    public class DateChanger
    {
        public static void ShowHelp()
        {
            Console.WriteLine("Usage: CAMTOOLS DATEFROMEXIF <file or path>");
            return;
        }

        public void Execute(string[] args)
        {
            if (args.Length == 0)
            {
                DateChanger.ShowHelp();
                return;
            }
            Change(args[0]);
        }

        private void Change(string path)
        { 
            if (System.IO.Directory.Exists(path))
            {
                foreach (var file in System.IO.Directory.GetFiles(path))
                {
                    if (System.IO.Path.GetExtension(file).ToLowerInvariant() == ".jpg")
                    {
                        ChangeFile(file);
                    }
                }
            }
            else if (System.IO.File.Exists(path))
            {
                ChangeFile(path);
            }
            else
            {
                Console.WriteLine("Path or file does not exist");
            }
        }

        private void ChangeFile(string imagePath)
        {
            IEnumerable<Directory> directories = ImageMetadataReader.ReadMetadata(imagePath);
            var subIfdDirectory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();//?.Tags.FirstOrDefault(t => t.Type == 306);
            var dateTakenString = subIfdDirectory?.GetDescription(ExifDirectoryBase.TagDateTime);
            if (dateTakenString == null)
            {
                Console.WriteLine($"No date found in image '{System.IO.Path.GetFileName(imagePath)}'");
                return;
            }

            DateTime dateTaken;
            if (!DateTime.TryParseExact(dateTakenString, "yyyy:MM:dd HH:mm:ss",
                CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTaken))
            {
                Console.WriteLine($"Error parsing datetime '{dateTakenString}' from image '{System.IO.Path.GetFileName(imagePath)}'");
                return;
            }

            try
            {
                System.IO.File.SetLastWriteTime(imagePath, dateTaken);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error chaning image date '{e.Message}'");
                return;
            }

            Console.WriteLine($"Changed image '{System.IO.Path.GetFileName(imagePath)}' date to '{dateTakenString}'");

            // 306
            return;
        }
    }
}
