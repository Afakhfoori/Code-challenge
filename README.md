# Bunnings Code challenge 
This code has been created to address the challenge with details in this repo: https://github.com/tosumitagrawal/codingskills

# Tech Stach:
- C#, .Net Core 5 with MVC Application has been used to implement this code.
- Nunit has been used for testing purposes

# How to Run the project 
 - Open the solution file which is inside the src folder in Visual studio 2019 pro edition and run it with IISExpress
 OR
 - Run the project with .Net CLI, Navigate to src\BunningsCataloge folder and run "dotnet Run".
 
 P.S. How to install .Net SDK which includes the .Net CLI :https://docs.microsoft.com/en-us/dotnet/core/install/windows?tabs=net50
 
# Solution Structure:
-	Input and output folders which consist of sample input and output files, the output folder will be deleted each time the program run and a new output file will be created
-	BunningsCataloge project/folder: Is the main project which is using the MVC to implement the code and is the main project for running the solution, So always make sure that this is the startup project before running the solution in Visual Studio.
-	Domain project/folder: Is my domain folder which consist of the system entities
-	Services project/folder: Is my service project which is handling all the processes in the system and being tested using unit test project
-	TestMegaMerg project/folder: which is my unit test project and has couple of unit test for each services.

