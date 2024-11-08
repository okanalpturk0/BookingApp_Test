using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
public class Hotel
{
    public string Id { get; set; }
    public List<Room> Rooms { get; set; }
}
public class Room
{
    public string RoomType { get; set; }
}
public class Booking
{
    public string HotelId { get; set; }
    public DateTime First_datetime { get; set; }
    public DateTime Last_datetime { get; set; }
    public string RoomType { get; set; }
    public string RoomRate { get; set; }
}
public class Program
{
    private static List<Hotel> hotels;
    private static List<Booking> bookings;

    public static void Main(string[] args)
    {
        if (args.Length < 4 || args[0] != "--hotels" || args[2] != "--bookings") { }

        hotels = LoadHotels(args[1]);
        bookings = LoadBookings(args[3]);
        Console.WriteLine("Enter availability queries in the format below:" + "\n" + "Availability(Hotel ID, Start Date(yyyymmdd)-End Date(yyyymmdd), Room Type (For single SGL or for double DBL))");
        Console.WriteLine("Leave blank to exit.\n");

        while (true)
        {
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) break;

            if (ParseQuery(input, out string Hotel_Id, out DateTime Start_Date, out DateTime? End_Date, out string Room_Type))
            {
                int availability = CheckAvailability(Hotel_Id, Start_Date, End_Date ?? Start_Date, Room_Type);
                Console.WriteLine($"\nAvailability for {Room_Type} from {Start_Date:yyyyMMdd} to {(End_Date ?? Start_Date):yyyyMMdd}: {availability}\n");
            }
            else
            {
                Console.WriteLine("Invalid input. Please try again.");
            }
        }
    }


    private static List<Hotel> LoadHotels(string hotel_file_path)
    {
        string json = File.ReadAllText(hotel_file_path);
        return JsonConvert.DeserializeObject<List<Hotel>>(json);
    }
    private static List<Booking> LoadBookings(string booking_file_path)
    {
        string json = File.ReadAllText(booking_file_path);
        var Raw_Bookings = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);

        return Raw_Bookings.Select(b => new Booking
        {
            HotelId = b["hotelId"],
            First_datetime = DateTime.ParseExact(b["arrival"], "yyyyMMdd", null),
            Last_datetime = DateTime.ParseExact(b["departure"], "yyyyMMdd", null),
            RoomType = b["roomType"],
            RoomRate = b["roomRate"]
        }).ToList();
    }


    private static bool ParseQuery(string input, out string Hotel_Id, out DateTime Start_Date, out DateTime? End_Date, out string Room_Type)
    {
        Hotel_Id = string.Empty;
        Start_Date = DateTime.MinValue;
        End_Date = null;
        Room_Type = string.Empty;

        var match = System.Text.RegularExpressions.Regex.Match(input, @"^Availability\(([^,]+),\s*(\d{8})(?:-(\d{8}))?,\s*([A-Z]+)\)$");
        if (!match.Success) return false;

        Hotel_Id = match.Groups[1].Value;
        Start_Date = DateTime.ParseExact(match.Groups[2].Value, "yyyyMMdd", null);
        if (match.Groups[3].Success)
            End_Date = DateTime.ParseExact(match.Groups[3].Value, "yyyyMMdd", null);

        Room_Type = match.Groups[4].Value;
        return true;
    }
    private static int CheckAvailability(string Hotel_Id, DateTime Start_Date, DateTime End_Date, string Room_Type)
    {
        var hotel = hotels.FirstOrDefault(h => h.Id == Hotel_Id);
        if (hotel == null)
        {
            Console.WriteLine("Hotel not found.");
            return 0;
        }

        int TotalRooms = hotel.Rooms.Count(r => r.RoomType == Room_Type);
        int BookedRooms = 0;

        foreach (var booking in bookings.Where(b => b.HotelId == Hotel_Id && b.RoomType == Room_Type))
        {
            if ((Start_Date < booking.Last_datetime && End_Date >= booking.First_datetime))
            {
                BookedRooms++;
            }
        }
        return TotalRooms - BookedRooms;
    }
}
