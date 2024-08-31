using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MauiApplication.Helpers
{
    public static class ResourceHelper
    {
        public static void ExtractEmbeddedResource(string resourceName, string outputPath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var resourceStream = assembly.GetManifestResourceStream(resourceName);
            if (resourceStream == null)
            {
                Debug.WriteLine($"Resource '{resourceName}' not found.");
                throw new FileNotFoundException($"Resource '{resourceName}' not found.");
            }

            using var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
            resourceStream.CopyTo(fileStream);
            Debug.WriteLine($"Resource '{resourceName}' extracted to {outputPath}.");
        }
    }
}
