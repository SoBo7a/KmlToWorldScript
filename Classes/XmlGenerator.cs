using System;
using System.Globalization;
using System.Xml;
using KmlToWorldScript.Classes;

namespace KmlToWorldScript.Classes
{
    class XmlGenerator
    {
        private class UserInputs
        {
            public string Descr { get; set; }
            public string ContainerTitle { get; set; }
            public bool IsOnGround { get; set; }
            public string LocalOffsetXYZ { get; set; }
            public string LocalOrientation { get; set; }
            public string AIType { get; set; }
            public string ContainerID { get; set; }
            public double GroundCruiseSpeed { get; set; }
            public double GroundTurnSpeed { get; set; }
            public double GroundTurnTime { get; set; }
            public bool YieldToUser { get; set; }
            public bool CanReverse { get; set; }
            public bool KeepLastHeading { get; set; }
            public bool WrapWaypoints { get; set; }
            public int CurrentWaypoint { get; set; }
            public bool BackupToFirst { get; set; }
            public bool AlwaysBackup { get; set; }
            public bool AltitudeIsAGL { get; set; }

        }

        public static XmlDocument GenerateXml(List<Waypoint> waypoints, string outputFileName)
        {
            XmlDocument xmlDoc = new XmlDocument();

            // Create the root element
            XmlElement root = xmlDoc.CreateElement("SimBase.Document");
            root.SetAttribute("Type", "MissionFile");
            root.SetAttribute("version", "1.0");
            xmlDoc.AppendChild(root);

            // Create Filename element
            XmlElement filenameElement = xmlDoc.CreateElement("Filename");
            filenameElement.InnerText = outputFileName;
            root.AppendChild(filenameElement);

            // Create WorldBase.Flight element
            XmlElement flightElement = xmlDoc.CreateElement("WorldBase.Flight");
            root.AppendChild(flightElement);

            // Create SimContain.Container element
            XmlElement containerElement = xmlDoc.CreateElement("SimContain.Container");
            containerElement.SetAttribute("InstanceID", "{" + Guid.NewGuid().ToString() + "}");
            flightElement.AppendChild(containerElement);

            // Get user inputs
            UserInputs userInputs = GetUserInputs();

            // Add values for SimContain.Container
            AddElementWithText(xmlDoc, containerElement, "Descr", userInputs.Descr);
            AddElementWithText(xmlDoc, containerElement, "ContainerTitle", userInputs.ContainerTitle);
            AddElementWithText(xmlDoc, containerElement, "IsOnGround", userInputs.IsOnGround.ToString());
            AddElementWithText(xmlDoc, containerElement, "LocalOffsetXYZ", userInputs.LocalOffsetXYZ);
            AddElementWithText(xmlDoc, containerElement, "LocalOrientation", userInputs.LocalOrientation);
            AddElementWithText(xmlDoc, containerElement, "AIType", userInputs.AIType);
            AddElementWithText(xmlDoc, containerElement, "ContainerID", userInputs.ContainerID);
            AddElementWithText(xmlDoc, containerElement, "SimplifiedSimMode", "AI");

            // Create ParameterAI element
            XmlElement parameterAIElement = xmlDoc.CreateElement("ParameterAI");
            containerElement.AppendChild(parameterAIElement);

            // Add values for ParameterAI
            AddElementWithText(xmlDoc, parameterAIElement, "GroundCruiseSpeed", userInputs.GroundCruiseSpeed.ToString("F3", CultureInfo.InvariantCulture));
            AddElementWithText(xmlDoc, parameterAIElement, "GroundTurnSpeed", userInputs.GroundTurnSpeed.ToString("F3", CultureInfo.InvariantCulture));
            AddElementWithText(xmlDoc, parameterAIElement, "GroundTurnTime", userInputs.GroundTurnTime.ToString("F3", CultureInfo.InvariantCulture));
            AddElementWithText(xmlDoc, parameterAIElement, "YieldToUser", userInputs.YieldToUser.ToString());
            AddElementWithText(xmlDoc, parameterAIElement, "CanReverse", userInputs.CanReverse.ToString());
            AddElementWithText(xmlDoc, parameterAIElement, "KeepLastHeading", userInputs.KeepLastHeading.ToString());

            // Create WaypointList element
            XmlElement waypointListElement = xmlDoc.CreateElement("WaypointList");
            parameterAIElement.AppendChild(waypointListElement);

            // Add default values for WaypointList
            AddElementWithText(xmlDoc, waypointListElement, "WrapWaypoints", userInputs.WrapWaypoints.ToString());
            AddElementWithText(xmlDoc, waypointListElement, "CurrentWaypoint", userInputs.CurrentWaypoint.ToString());
            AddElementWithText(xmlDoc, waypointListElement, "BackupToFirst", userInputs.BackupToFirst.ToString());
            AddElementWithText(xmlDoc, waypointListElement, "AlwaysBackup", userInputs.AlwaysBackup.ToString());
            AddElementWithText(xmlDoc, waypointListElement, "YieldToUser", userInputs.YieldToUser.ToString());

            // Create Waypoint elements
            foreach (Waypoint waypoint in waypoints)
            {
                XmlElement waypointElement = xmlDoc.CreateElement("Waypoint");
                waypointElement.SetAttribute("InstanceID", "{" + Guid.NewGuid().ToString() + "}");
                waypointListElement.AppendChild(waypointElement);

                // Add values for each Waypoint
                AddElementWithText(xmlDoc, waypointElement, "Descr", $"Waypoint {waypoints.IndexOf(waypoint) + 1}");
                AddElementWithText(xmlDoc, waypointElement, "WaypointID", waypoints.IndexOf(waypoint).ToString());
                AddElementWithText(xmlDoc, waypointElement, "WorldPosition", $"{waypoint.Latitude.ToString("G", CultureInfo.InvariantCulture)},{waypoint.Longitude.ToString("G", CultureInfo.InvariantCulture)},{waypoint.Altitude.ToString("+000000.0;-000000.0", CultureInfo.InvariantCulture)}");
                AddElementWithText(xmlDoc, waypointElement, "AltitudeIsAGL", userInputs.AltitudeIsAGL.ToString());
                AddElementWithText(xmlDoc, waypointElement, "IsTeleportEnable", "False");
            }

            return xmlDoc;
        }

        private static UserInputs GetUserInputs()
        {
            Console.WriteLine();
            Console.WriteLine("Enter the description (press Enter for empty):");
            string descr = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Enter the name of the SimObject to use:");
            string containerTitle = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Enter 1 for IsOnGround=true (default), 2 for IsOnGround=false:");
            string isOnGroundInput = Console.ReadLine();
            bool isOnGround = string.IsNullOrEmpty(isOnGroundInput) || isOnGroundInput == "1";

            Console.WriteLine();
            Console.WriteLine("Enter the LocalOffsetXYZ (press Enter for default '0.000,0.000,0.000,0.000'):");
            string localOffsetXYZ = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(localOffsetXYZ))
            {
                localOffsetXYZ = "0.000,0.000,0.000,0.000";
            }

            Console.WriteLine();
            Console.WriteLine("Enter the LocalOrientation (press Enter for default '0.000,0.000,0.000,0.000'):");
            string localOrientation = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(localOrientation))
            {
                localOrientation = "0.000,0.000,0.000,0.000";
            }

            Console.WriteLine();
            Console.WriteLine("Please select the AIType:");
            string AIType = GetAIType();

            Console.WriteLine();
            Console.WriteLine("Enter the Container ID (press Enter for default '-1'):");
            string containerIDInput = Console.ReadLine();
            int containerID;
            if (!int.TryParse(containerIDInput, out containerID))
            {
                containerID = -1;
            }

            Console.WriteLine();
            Console.WriteLine("Enter the GroundCruiseSpeed (press Enter for default '35.000'):");
            string groundCruiseSpeedInput = Console.ReadLine();
            double groundCruiseSpeed;
            if (string.IsNullOrWhiteSpace(groundCruiseSpeedInput))
            {
                groundCruiseSpeed = 35.000;
            }
            else
            {
                groundCruiseSpeedInput = groundCruiseSpeedInput.Contains(".") ? groundCruiseSpeedInput : $"{groundCruiseSpeedInput}.000";
                if (!double.TryParse(groundCruiseSpeedInput, NumberStyles.Any, CultureInfo.InvariantCulture, out groundCruiseSpeed))
                {
                    groundCruiseSpeed = 35.000;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Enter the GroundTurnSpeed (press Enter for default '10.000'):");
            string groundTurnSpeedInput = Console.ReadLine();
            double groundTurnSpeed;
            if (string.IsNullOrWhiteSpace(groundTurnSpeedInput))
            {
                groundTurnSpeed = 10.000;
            }
            else
            {
                groundTurnSpeedInput = groundTurnSpeedInput.Contains(".") ? groundTurnSpeedInput : $"{groundTurnSpeedInput}.000";
                if (!double.TryParse(groundTurnSpeedInput, NumberStyles.Any, CultureInfo.InvariantCulture, out groundTurnSpeed))
                {
                    groundTurnSpeed = 10.000;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Enter the GroundTurnTime (press Enter for default '3.500'):");
            string groundTurnTimeInput = Console.ReadLine();
            double groundTurnTime;
            if (string.IsNullOrWhiteSpace(groundTurnTimeInput))
            {
                groundTurnTime = 3.500;
            }
            else
            {
                groundTurnTimeInput = groundTurnTimeInput.Contains(".") ? groundTurnTimeInput : $"{groundTurnTimeInput}.000";
                if (!double.TryParse(groundTurnTimeInput, NumberStyles.Any, CultureInfo.InvariantCulture, out groundTurnTime))
                {
                    groundTurnTime = 3.500;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Enter 1 for YieldToUser=true (default), 2 for YieldToUser=false:");
            string yieldInput = Console.ReadLine();
            bool yieldToUser = string.IsNullOrEmpty(yieldInput) || yieldInput == "1";

            Console.WriteLine();
            Console.WriteLine("Enter 1 for CanReverse=true, 2 for CanReverse=false (default):");
            string canReverseInput = Console.ReadLine();
            bool canReverse = !string.IsNullOrEmpty(canReverseInput) && canReverseInput == "1";

            Console.WriteLine();
            Console.WriteLine("Enter 1 for KeepLastHeading=true (default), 2 for KeepLastHeading=false:");
            string keepLastHeadingInput = Console.ReadLine();
            bool keepLastHeading = string.IsNullOrEmpty(keepLastHeadingInput) || keepLastHeadingInput == "1";
            
            Console.WriteLine();
            Console.WriteLine("Enter 1 for WrapWaypoints=true (default), 2 for WrapWaypoints=false:");
            string wrapWaypointsInput = Console.ReadLine();
            bool wrapWaypoints = string.IsNullOrEmpty(wrapWaypointsInput) || wrapWaypointsInput == "1";

            Console.WriteLine();
            Console.WriteLine("Enter the CurrentWaypoint (press Enter for default '0'):");
            string currentWaypointInput = Console.ReadLine();
            int currentWaypoint;
            if (!int.TryParse(currentWaypointInput, out currentWaypoint))
            {
                currentWaypoint = 0;
            }

            Console.WriteLine();
            Console.WriteLine("Enter 1 for BackupToFirst=true, 2 for BackupToFirst=false (default):");
            string backupToFirstInput = Console.ReadLine();
            bool backupToFirst = !string.IsNullOrEmpty(backupToFirstInput) && backupToFirstInput == "1";

            Console.WriteLine();
            Console.WriteLine("Enter 1 for AlwaysBackup=true, 2 for AlwaysBackup=false (default):");
            string alwaysBackupInput = Console.ReadLine();
            bool alwaysBackup = !string.IsNullOrEmpty(alwaysBackupInput) && alwaysBackupInput == "1";

            Console.WriteLine();
            Console.WriteLine("Enter 1 for (Waypoint) AltitudeIsAGL=true (default), 2 for AltitudeIsAGL=false:");
            string altitudeIsAGLInput = Console.ReadLine();
            bool altitudeIsAGL = string.IsNullOrEmpty(altitudeIsAGLInput) || altitudeIsAGLInput == "1";

            // Create and return the UserInputs object
            return new UserInputs
            {
                Descr = descr,
                ContainerTitle = containerTitle,
                IsOnGround = isOnGround,
                LocalOffsetXYZ = localOffsetXYZ,
                LocalOrientation = localOrientation,
                AIType = AIType,
                ContainerID = containerID.ToString(),
                GroundCruiseSpeed = groundCruiseSpeed,
                GroundTurnSpeed = groundTurnSpeed,
                GroundTurnTime = groundTurnTime,
                YieldToUser = yieldToUser,
                CanReverse = canReverse,
                KeepLastHeading = keepLastHeading,
                WrapWaypoints = wrapWaypoints,
                CurrentWaypoint = currentWaypoint,
                BackupToFirst = backupToFirst,
                AlwaysBackup = alwaysBackup,
                AltitudeIsAGL = altitudeIsAGL,
            };
        }

        private static string GetAIType()
        {
            Console.WriteLine("Select an AI Type:");
            Console.WriteLine("1. None");
            Console.WriteLine("2. Airplane");
            Console.WriteLine("3. Helicopter");
            Console.WriteLine("4. WanderBoat");
            Console.WriteLine("5. GroundVehicle");
            Console.WriteLine("6. FuelTruck");
            Console.WriteLine("7. PushBack");
            Console.WriteLine("8. SmallPushBack");
            Console.WriteLine("9. BaggageCart");
            Console.WriteLine("10. BaggageLoader");
            Console.WriteLine("11. CateringTruck");
            Console.WriteLine("12. BoardingRamp");
            Console.WriteLine("13. GroundPowerUnit");
            Console.WriteLine("14. VehicleFollower");
            Console.WriteLine("15. AirportAmbient");
            Console.WriteLine("16. IdleWorker");
            Console.WriteLine("17. AirplanePlayback");
            Console.WriteLine("18. Boat");
            Console.WriteLine("19. Animal");
            Console.WriteLine("20. FlyingAnimal");
            Console.WriteLine("21. Human");
            Console.WriteLine("22. Aircraft_Pilot");
            Console.WriteLine("23. Marshaller");
            Console.WriteLine("24. Jetway");
            Console.WriteLine("25. Linked_Object");

            // Default to GroundVehicle
            string defaultAIType = "GroundVehicle";

            // Get the user's choice
            string userInput = Console.ReadLine();

            // Map user input to the corresponding AI type or use the default
            switch (userInput)
            {
                case "1":
                    return "None";
                case "2":
                    return "Airplane";
                case "3":
                    return "Helicopter";
                case "4":
                    return "WanderBoat";
                case "5":
                    return "GroundVehicle";
                case "6":
                    return "FuelTruck";
                case "7":
                    return "PushBack";
                case "8":
                    return "SmallPushBack";
                case "9":
                    return "BaggageCart";
                case "10":
                    return "BaggageLoader";
                case "11":
                    return "CateringTruck";
                case "12":
                    return "BoardingRamp";
                case "13":
                    return "GroundPowerUnit";
                case "14":
                    return "VehicleFollower";
                case "15":
                    return "AirportAmbient";
                case "16":
                    return "IdleWorker";
                case "17":
                    return "AirplanePlayback";
                case "18":
                    return "Boat";
                case "19":
                    return "Animal";
                case "20":
                    return "FlyingAnimal";
                case "21":
                    return "Human";
                case "22":
                    return "Aircraft_Pilot";
                case "23":
                    return "Marshaller";
                case "24":
                    return "Jetway";
                case "25":
                    return "Linked_Object";

                default:
                    Console.WriteLine();
                    Console.WriteLine($"Invalid input. Defaulting to {defaultAIType}");
                    return defaultAIType;
            }
        }

        private static void AddElementWithText(XmlDocument xmlDoc, XmlElement parentElement, string elementName, string elementText)
        {
            XmlElement element = xmlDoc.CreateElement(elementName);
            element.InnerText = elementText;
            parentElement.AppendChild(element);
        }

        public static void GenerateAndSaveXml(List<Waypoint> waypoints, string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            XmlDocument xmlDoc = GenerateXml(waypoints, fileName);

            try
            {
                xmlDoc.Save(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine($"Error saving XML file: {ex.Message}");
            }
        }
    }
}
