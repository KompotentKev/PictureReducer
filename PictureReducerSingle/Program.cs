using PictureReducerShared; // Assuming the shared library is referenced
using System.Runtime.Versioning;

class SingleImageResizer
{
    [SupportedOSPlatform("windows")]
    static void Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Usage: SingleImageResizer - Incorrect parameters passed");
            Console.WriteLine("Press anykey to exit");
            Console.ReadKey();
            return;
        }

        string sourcePath = args[0];
        string destinationPath = args[1];
        string fileName = args[2];

        string inputFile = Path.Combine(sourcePath, fileName);
        string outputFile = Path.Combine(destinationPath, Path.GetFileName(fileName));

        // Validate input file
        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Error: File does not exist: {inputFile}");
            return;
        }

        // Validate source and destination directories
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

        try
        {
            ImageResizer.ResizeImage(inputFile, outputFile, 800, 600, 85L);
            Console.WriteLine($"Successfully resized: {fileName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during image processing:");
            Console.WriteLine(ex.Message);
        }
    }
}
