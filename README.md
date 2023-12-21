# KmlToWorldScript

This application is developed for generating WorldScripts in Microsoft Flight Simulator 2020 from KML files. It simplifies the process of creating script files for SimObjects, enabling them to follow waypoints defined by coordinates in the KML file. 
The tool offers flexibility by allowing users to modify various parameters within the generated WorldScript. Additionally, users can perform further customizations manually after the initial file generation.

I highly recommend watching this [tutorial](https://www.youtube.com/watch?v=bCjIvzLjmd4) by Federico Pinotti, as it served as a key source of inspiration for the development of this tool. 
The tutorial provides detailed insights on waypoint animations for MSFS and inspired the creation of this application, aiming to speed up the process outlined in the video.


## Usage

To use the MSFS WorldScript Generator, follow these steps:

1. Download: Obtain the compiled executable (*.exe) from the latest release.
2. Run the App:
	- Double-click the executable file to run the application.			
	- Alternatively, open a terminal, navigate to the directory containing the executable, and run it from the command line.
3. Provide KML File Path:	
	- When prompted, enter the full path of the KML file containing the waypoints.
4. Follow Console Prompts:
	- The application will guide you through providing various parameters for the WorldScript. Follow the console prompts to input the required information.
5. Completion:
	- After entering all required data, the tool will generate the WorldScript XML file.
	- The XML file is saved in the "output" subfolder within the application directory.
	- You can manually customize the generated script file further if needed.
	- Copy the generated XML file into your Microsoft Flight Simulator project. Place it in the "PackageSources/Scenarii/My-Script/" directory, which should already be added to your project.
	- Open your project in the MSFS-SDK.
	- You can now use the WorldScript Object within the editor for further integration into your project.


## Modifiable Values

When running the script, you will be prompted to provide the following inputs:

- **Descr**: Description of the WorldScript.
- **ContainerTitle**: Name of the SimObject to use with the Script.
- **IsOnGround**: Specify if the SimObject is fixed on the ground (`true`) or in the air (`false`).
- **LocalOffsetXYZ**: Local offset of SimObject to WorldScript-placement in XYZ coordinates (default: '0.000,0.000,0.000,0.000').
- **LocalOrientation**: Local orientation of the SimObject to WorldScript-placement (default: '0.000,0.000,0.000,0.000').
- **AIType**: Type of AI to use. [List of available AITypes](https://docs.flightsimulator.com/html/Content_Configuration/Environment/Living_World/Airport_Services/Service_And_IdleWorker_Script_Definitions.htm?rhhlterm=groundcruisespeed&rhsearch=GroundCruiseSpeed) (Currently only tested with "GroundVehicle" AIType) 
- **ContainerID**: Container ID (default: '-1').
- **GroundCruiseSpeed**: The speed (in Knots) for this AI (default: '35.000').
- **GroundTurnSpeed**: AI slows to this speed in Knots when making sharp turns (default: '10.000').
- **GroundTurnTime**: AI attempts to make 90° turns in this amount of time. (default: '3.500').
- **YieldToUser**: Whether the AI will stop moving when getting too close to the User aircraft or not (`true` or `false`, default: `true`).
- **CanReverse**: AI can reverse (`true` or `false`, default: `false`).
- **KeepLastHeading**: AI will try to reach the heading defined by its last two waypoints before reaching its last waypoint (`true` or `false`, default: `false`).
- **WrapWaypoints**: AI will move to the first waypoint and restart the route if it has reached the last waypoint (`true` or `false`, default: `true`).
- **CurrentWaypoint**: Select the starting waypoint (Waypoints are indexed by 0) (default: '0').
- **BackupToFirst**: AI tries to reverse to the first waypoint (`true` or `false`, default: `false`).
- **AlwaysBackup**: AI always moves in reverse (`true` or `false`, default: `false`).
- **AltitudeIsAGL**: Defines if waypoint altitude is AGL or not (`true` or `false`, default: `true`).
