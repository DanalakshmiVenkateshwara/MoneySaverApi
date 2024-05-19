using System;
using System.IO;

namespace MoneySaver.Utilities.Config
{
    public static class FileUtilities
    {
        public static string SaveBase64FileToFolder(string userFolderName, string base64String, string fileName)
        {
            // Path to save the folder and file
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), userFolderName);

            // Create the directory if it doesn't exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Combine the folder path with the file name to get the full file path
            string filePath = Path.Combine(folderPath, fileName);

            try
            {
                // Decode the Base64 string to a byte array
                byte[] fileBytes = Convert.FromBase64String(base64String);

                // Write the byte array to a file
                File.WriteAllBytes(filePath, fileBytes);

                Console.WriteLine($"File saved to {filePath}");
                return filePath;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"The provided Base64 string is not valid for file {fileName}.");
                Console.WriteLine(ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while writing the file {fileName}.");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred while processing the file {fileName}.");
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }
}
