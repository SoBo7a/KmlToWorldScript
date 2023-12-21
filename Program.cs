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
                Console.WriteLine($"Error: The specified KML file '{kmlFilePath}' does not exist.");
                return;
            }

            // Read KML file and get waypoints
            List<Waypoint> waypoints = KMLReader.ReadKML(kmlFilePath);

            // Generate the output XML file path in the "output" subfolder of the application directory
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

            Console.WriteLine($"Output XML file saved to: {outputXmlFilePath}");
        }
    }
}
