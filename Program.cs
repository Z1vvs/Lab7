using System;
using System.Collections.Generic;
using System.Linq;

public class RestaurantReservationApp
{
    static void Main(string[] args)
    {
        try
        {
            var manager = new ReservationManager();
            manager.AddRestaurant("A", 10);
            manager.AddRestaurant("B", 5);

            var bookingResult1 = manager.BookTable("A", new DateTime(2023, 12, 25), 3);
            var bookingResult2 = manager.BookTable("A", new DateTime(2023, 12, 25), 3);

            Console.WriteLine(bookingResult1);
            Console.WriteLine(bookingResult2);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Application Error: " + ex.Message);
        }
    }
}

public class ReservationManager
{
    public List<Restaurant> restaurants;

    public ReservationManager()
    {
        restaurants = new List<Restaurant>();
    }

    public void AddRestaurant(string name, int tableCount)
    {
        try
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(nameof(name), "Restaurant name cannot be empty");
            }

            if (tableCount <= 0)
            {
                throw new ArgumentException(nameof(tableCount), "Table count must be greater than 0");
            }

            var restaurant = new Restaurant
            {
                Name = name,
                Tables = Enumerable.Range(0, tableCount).Select(_ => new RestaurantTable()).ToArray()
            };

            restaurants.Add(restaurant);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error adding restaurant: " + ex.Message);
        }
    }

    public List<string> FindAllFreeTables(DateTime date)
    {
        try
        {
            return restaurants
                .SelectMany(restaurant => restaurant.Tables
                    .Select((table, index) => new { Restaurant = restaurant, TableIndex = index, Table = table })
                    .Where(entry => !entry.Table.IsBooked(date))
                    .Select(entry => $"{entry.Restaurant.Name} - Table {entry.TableIndex + 1}")
                )
                .ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error finding free tables: " + ex.Message);
            return new List<string>();
        }
    }

    public bool BookTable(string restaurantName, DateTime date, int tableNumber)
    {
        try
        {
            var restaurant = restaurants.FirstOrDefault(r => r.Name == restaurantName);
            if (restaurant == null)
            {
                throw new ArgumentException(nameof(restaurantName), "Restaurant not found");
            }

            if (tableNumber < 0 || tableNumber >= restaurant.Tables.Length)
            {
                throw new ArgumentException(nameof(tableNumber), "Invalid table number");
            }

            return restaurant.Tables[tableNumber].Book(date);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error booking table: " + ex.Message);
            return false;
        }
    }

    public void SortRestaurantsByAvailability(DateTime date)
    {
        try
        {
            bool swapped;
            do
            {
                swapped = false;
                for (int i = 0; i < restaurants.Count - 1; i++)
                {
                    int availableTablesCurrent = CountAvailableTables(restaurants[i], date);
                    int availableTablesNext = CountAvailableTables(restaurants[i + 1], date);

                    if (availableTablesCurrent < availableTablesNext)
                    {
                        var temp = restaurants[i];
                        restaurants[i] = restaurants[i + 1];
                        restaurants[i + 1] = temp;
                        swapped = true;
                    }
                }
            } while (swapped);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error sorting restaurants: " + ex.Message);
        }
    }

    public int CountAvailableTables(Restaurant restaurant, DateTime date)
    {
        try
        {
            return restaurant.Tables.Count(table => !table.IsBooked(date));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error counting available tables: " + ex.Message);
            return 0;
        }
    }
}

public class Restaurant
{
    public string Name { get; set; }
    public RestaurantTable[] Tables { get; set; }
}

public class RestaurantTable
{
    private HashSet<DateTime> bookedDates;

    public RestaurantTable()
    {
        bookedDates = new HashSet<DateTime>();
    }

    public bool Book(DateTime date)
    {
        try
        {
            if (bookedDates.Contains(date))
            {
                return false;
            }

            bookedDates.Add(date);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error booking table: " + ex.Message);
            return false;
        }
    }

    public bool IsBooked(DateTime date)
    {
        try
        {
            return bookedDates.Contains(date);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error checking if table is booked: " + ex.Message);
            return false;
        }
    }
}