using System.Diagnostics;
using JurgenMixAssessment;
using JurgenMixAssessment.Models;

class Program
{
    public static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Console.WriteLine($"Starting data extraction.");

        var spatialHash = new GridSpatialHash(0.01);
        spatialHash.LoadVehiclesFromBinaryFile("..\\VehiclePositions_DataFile\\VehiclePositions.dat");

        stopwatch.Stop();
        Console.WriteLine($"Data extraction complete: {stopwatch.Elapsed}\n");

        var SearchCoordinates = new List<Coordinate> {
                new Coordinate { latitude = 34.544909F, Longitude = -102.100843F },
                new Coordinate { latitude = 32.345544F, Longitude = -99.123124F },
                new Coordinate { latitude = 33.234235F, Longitude = -100.214124F },
                new Coordinate { latitude = 35.195739F, Longitude = -95.348899F },
                new Coordinate { latitude = 31.895839F, Longitude = -97.789573F },
                new Coordinate { latitude = 32.895839F, Longitude = -101.789573F },
                new Coordinate { latitude = 34.115839F, Longitude = -100.225732F },
                new Coordinate { latitude = 32.335839F, Longitude = -99.992232F },
                new Coordinate { latitude = 33.535339F, Longitude = -94.792232F },
                new Coordinate { latitude = 32.234235F, Longitude = -100.222222F },
            };

        stopwatch.Restart();

        foreach (var coordinate in SearchCoordinates)
        {
            var nearestVehicle = spatialHash.FindNearestVehicle(coordinate.latitude, coordinate.Longitude);
            Console.WriteLine($"Nearest vehicle to {coordinate.latitude}, {coordinate.Longitude}: {nearestVehicle.VehicleRegistration} at {nearestVehicle.Latitude} {nearestVehicle.Longitude}");
        }

        stopwatch.Stop();
        Console.WriteLine($"\nSearch execution time: {stopwatch.Elapsed}");
        //Console.WriteLine($"{spatialHash.GetCalculationCount()} Calculations performed.");
    }
}