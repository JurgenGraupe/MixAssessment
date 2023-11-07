using JurgenMixAssessment.Models;

namespace JurgenMixAssessment
{
    public class GridSpatialHash
    {
        private Dictionary<(int, int), List<Vehicle>> grid = new Dictionary<(int, int), List<Vehicle>>();
        private double cellSize;
        
        public GridSpatialHash(double cellSize)
        {
            this.cellSize = cellSize;
        }

        private (int, int) GetGridCell(float latitude, float longitude)
        {
            return ((int)(latitude / cellSize), (int)(longitude / cellSize));
        }

        public void AddVehicle(Vehicle vehicle)
        {
            var cell = GetGridCell(vehicle.Latitude, vehicle.Longitude);            
            if (grid.ContainsKey(cell))
            {
                grid[cell].Add(vehicle);                          
            } else
            {                    
                grid[cell] = new List<Vehicle>
                {
                    vehicle
                };
            }
        }

        public Vehicle FindNearestVehicle(float latitude, float longitude)
        {
            var cell = GetGridCell(latitude, longitude);
            Vehicle closestVehicle = null;
            double closestDistance = double.MaxValue;
            
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    var checkCell = (cell.Item1 + i, cell.Item2 + j);
                    if (grid.TryGetValue(checkCell, out var list))
                    {
                        foreach (var coord in list)
                        {
                            double distance = Haversine(latitude, longitude, coord.Latitude, coord.Longitude);
                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                closestVehicle = coord;
                            }
                        }
                    }
                }
            }

            return closestVehicle;
        }
       
        private double Haversine(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371;
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);
            lat1 = ToRadians(lat1);
            lat2 = ToRadians(lat2);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));                 
            return R * c;
        }

        private double ToRadians(double deg)
        {
            return deg * (Math.PI / 180);
        }

        public void LoadVehiclesFromBinaryFile(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 62464))
            using (BinaryReader reader = new BinaryReader(fs))
            {                
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {                    
                    int vehicleId = reader.ReadInt32();
                    string vehicleRegistration = ReadNullTerminatedString(reader);
                    float latitude = reader.ReadSingle();
                    float longitude = reader.ReadSingle();
                    ulong recordedTimeUTC = reader.ReadUInt64();
                    
                    AddVehicle(new Vehicle {
                        VehicleId = vehicleId,
                        VehicleRegistration = vehicleRegistration,
                        Latitude = latitude,
                        Longitude = longitude,
                        RecordedTimeUTC = recordedTimeUTC
                    });
                }
            }
        }

        private string ReadNullTerminatedString(BinaryReader reader)
        {
            List<byte> bytes = new List<byte>();
            byte b;
            while ((b = reader.ReadByte()) != 0)
            {
                bytes.Add(b);
            }
            return "1234";// Encoding.ASCII.GetString(bytes.ToArray());
        }
    }
}
