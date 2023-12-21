using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ConsoleAppRestaurantTableReservationManager
{
    [TestClass]
    public class ReservationManagerTests
    {
        [TestMethod]
        public void AddRestaurant_ValidInput_ShouldAddRestaurant()
        {
            var manager = new ReservationManager();

            manager.AddRestaurant("A", 10);

            Assert.AreEqual(1, manager.restaurants.Count);
            Assert.AreEqual("A", manager.restaurants[0].Name);
            Assert.AreEqual(10, manager.restaurants[0].Tables.Length);
        }

        [TestMethod]
        public void AddRestaurant_EmptyName_ShouldThrowArgumentException()
        {
            var manager = new ReservationManager();

            Assert.ThrowsException<ArgumentException>(() => manager.AddRestaurant("", 10));
        }

        [TestMethod]
        public void BookTable_ValidInput_ShouldBookTable()
        {
            var manager = new ReservationManager();
            manager.AddRestaurant("A", 10);

            var result = manager.BookTable("A", DateTime.Now, 3);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void BookTable_InvalidRestaurantName_ShouldThrowArgumentException()
        {
            var manager = new ReservationManager();

            Assert.ThrowsException<ArgumentException>(() => manager.BookTable("NonExistentRestaurant", DateTime.Now, 3));
        }

        [TestMethod]
        public void SortRestaurantsByAvailability_ValidInput_ShouldSortRestaurants()
        {
            var manager = new ReservationManager();
            manager.AddRestaurant("A", 10);
            manager.AddRestaurant("B", 5);

            manager.SortRestaurantsByAvailability(DateTime.Now);

            Assert.AreEqual("A", manager.restaurants[0].Name);
            Assert.AreEqual("B", manager.restaurants[1].Name);
        }
    }
}