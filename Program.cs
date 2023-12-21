using System;
using System.Collections.Generic;
using System.IO;
using KmlToWorldScript.Classes;

namespace KmlToWorldScript
{
    class Program
    {
        static void Main()
        {
            // Ask the user for the KML file path
            Console.WriteLine("Enter the path of the KML file:");
            string kmlFilePath = Console.ReadLine();

            // Check if the provided KML file exists
            if (!File.Exists(kmlFilePath))
            {
                Console.WriteLine();
                Console.WriteLine($"ERROR: The specified KML file '{kmlFilePath}' does not exist.");
                Console.WriteLine();
                Console.WriteLine("Press any key to close the application...");
                Console.ReadKey();
                return;
            }

            // Read KML file and get waypoints
            List<Waypoint> waypoints = KMLReader.ReadKML(kmlFilePath);

            // Generate the output XML file path in the "output" subfolder of the application directory
            Console.WriteLine();
            Console.WriteLine("Enter the name for the generated XML-File (without extension):");
            string outputFileName = Console.ReadLine();

            // Ensure the ".xml" extension is present in the filename
            if (!outputFileName.EndsWith(".xml"))
            {
                outputFileName += ".xml";
            }

            string outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output");
            string outputXmlFilePath = Path.Combine(outputDirectory, outputFileName);

            // Ensure the "output" subfolder exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Generate XML and save it to the specified file
            XmlGenerator.GenerateAndSaveXml(waypoints, outputXmlFilePath);

            Console.WriteLine();
            Console.WriteLine($"Output XML file saved to: {outputXmlFilePath}");

            // Wait for user input or count down and close the application
            Console.WriteLine();
            Console.WriteLine("Press any key to exit or wait for 10 seconds...");
            DateTime endTime = DateTime.Now.AddSeconds(10);

            while (DateTime.Now < endTime && !Console.KeyAvailable)
            {
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
