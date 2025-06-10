using PictureReducerShared;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;

class BatchImageResizer
{
    [SupportedOSPlatform("windows")]
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: BatchImageResizer - Incorrect parameters passed");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            return;
        }

        string sourcePath = args[0];
        string destinationPath = args[1];

        if (!Directory.Exists(sourcePath))
        {
            Console.WriteLine($"Error: Source directory does not exist: {sourcePath}");
            return;
        }

        if (!Directory.Exists(destinationPath))
        {
            try
            {
                Directory.CreateDirectory(destinationPath);
                Console.WriteLine($"Created destination directory: {destinationPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Could not create destination directory: {destinationPath}");
                Console.WriteLine(ex.Message);
                return;
            }
        }

        string[] imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tif" };
        string[] imageFiles;

        try
        {
            imageFiles = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories)
                                  .Where(f => imageExtensions.Contains(Path.GetExtension(f).ToLower()))
                                  .ToArray();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading source directory:");
            Console.WriteLine(ex.Message);
            return;
        }

        Console.WriteLine($"Found {imageFiles.Length} image(s) to process...");

        foreach (string inputFile in imageFiles)
        {
            try
            {
                string relativePath = Path.GetRelativePath(sourcePath, inputFile);
                string destinationFilePath = Path.Combine(destinationPath, Path.GetDirectoryName(relativePath));
                string outputFile = Path.Combine(destinationFilePath, Path.GetFileNameWithoutExtension(inputFile) + "_resized.jpg");

                Directory.CreateDirectory(destinationFilePath); // Ensure subfolder exists

                ImageResizer.ResizeImage(inputFile, outputFile, 800, 600, 85L);
                Console.WriteLine($"Resized: {relativePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing {inputFile}: {ex.Message}");
                continue;
            }
        }

        Console.WriteLine("Batch resizing complete.");
    }
}