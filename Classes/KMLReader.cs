using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using KmlToWorldScript.Classes;

namespace KmlToWorldScript.Classes
{
    class KMLReader
    {
        public static List<Waypoint> ReadKML(string filePath)
        {
            List<Waypoint> waypoints = new List<Waypoint>();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                nsmgr.AddNamespace("kml", "http://www.opengis.net/kml/2.2");

                XmlNode coordinatesNode = xmlDoc.SelectSingleNode("//kml:coordinates", nsmgr);

                if (coordinatesNode != null)
                {
                    string[] waypointsStr = coordinatesNode.InnerText.Trim().Split(' ');

                    foreach (string waypointStr in waypointsStr)
                    {
                        string[] coordinates = waypointStr.Split(',');

                        if (coordinates.Length == 3 &&
                            double.TryParse(coordinates[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double latitude) &&
                            double.TryParse(coordinates[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double longitude) &&
                            double.TryParse(coordinates[2], NumberStyles.Any, CultureInfo.InvariantCulture, out double altitude))
                        {
                            Waypoint wp = new Waypoint
                            {
                                Latitude = latitude,
                                Longitude = longitude,
                                Altitude = altitude
                            };
                            waypoints.Add(wp);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Invalid waypoint format: {waypointStr}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading KML file: {ex.Message}");
            }

            return waypoints;
        }
    }
}