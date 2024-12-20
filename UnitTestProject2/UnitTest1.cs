using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using LearnPattern.Program.OrderManager;

namespace UnitTestProject2
{
    [TestClass]
    public class OrderManagerTests
    {
        private OrderManager _orderManager;

        [TestInitialize]
        public void Setup()
        {
            _orderManager = new OrderManager();
        }

        [TestMethod]
        public void CreateOrder_ValidOrderData_OrderAddedSuccessfully()
        {
            // Arrange
            var orderData = new Dictionary<string, string>
            {
                { "OrderId", "1" },
                { "CustomerName", "John Doe" },
                { "CustomerGroup", "individual" },
                { "Discount", "0.1" },
                { "ProductName", "Laptop" },
                { "Quantity", "2" },
                { "PriceEach", "500.00" },
                { "ShippingAddress", "city A" }
            };

            // Act
            _orderManager.CreateOrder(orderData);

            // Assert
            var orders = GetOrdersFromManager();
            Assert.AreEqual(1, orders.Count);
            Assert.AreEqual("John Doe", orders[0].Customer_Name);
        }

        [TestMethod]
        public void DeleteOrder_ExistingOrderId_OrderRemovedSuccessfully()
        {
            // Arrange
            CreateSampleOrder(1, "John Doe", "individual", 0.1, "Laptop", 2, 500.00, "city A");

            // Act
            _orderManager.DeleteOrder(1);

            // Assert
            var orders = GetOrdersFromManager();
            Assert.AreEqual(0, orders.Count);
        }

        [TestMethod]
        public void DeleteOrder_NonExistingOrderId_NoChangeInOrders()
        {
            // Arrange
            CreateSampleOrder(1, "John Doe", "individual", 0.1, "Laptop", 2, 500.00, "city A");

            // Act
            _orderManager.DeleteOrder(2);

            // Assert
            var orders = GetOrdersFromManager();
            Assert.AreEqual(1, orders.Count);
        }

        [TestMethod]
        public void ListOrder_EmptyOrderList_NoExceptionThrown()
        {
            // Act & Assert
            try
            {
                _orderManager.ListOrder();
            }
            catch (Exception)
            {
                Assert.Fail("ListOrder should not throw an exception when the order list is empty.");
            }
        }

        [TestMethod]
        public void CreateOrder_InvalidOrderType_ThrowsArgumentException()
        {
            // Arrange
            var orderData = new Dictionary<string, string>
            {
                { "OrderId", "1" },
                { "CustomerName", "Jane Doe" },
                { "CustomerGroup", "invalid" },
                { "Discount", "0.2" },
                { "ProductName", "Phone" },
                { "Quantity", "1" },
                { "PriceEach", "800.00" },
                { "ShippingAddress", "city A" }
            };

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => _orderManager.CreateOrder(orderData));
        }

        private void CreateSampleOrder(int orderId, string customerName, string customerGroup, double discount, string productName, int quantity, double priceEach, string shippingAddress)
        {
            var orderData = new Dictionary<string, string>
            {
                { "OrderId", orderId.ToString() },
                { "CustomerName", customerName },
                { "CustomerGroup", customerGroup },
                { "Discount", discount.ToString(CultureInfo.InvariantCulture) },
                { "ProductName", productName },
                { "Quantity", quantity.ToString() },
                { "PriceEach", priceEach.ToString(CultureInfo.InvariantCulture) },
                { "ShippingAddress", shippingAddress }
            };
            _orderManager.CreateOrder(orderData);
        }

        private List<IOrder> GetOrdersFromManager()
        {
            var field = typeof(OrderManager).GetField("_orders", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field.GetValue(_orderManager) as List<IOrder>;
        }
    }

    [TestClass]
    public class CsvReaderTests
    {
        [TestMethod]
        public void ReadCsv_ValidCsvFile_ReturnsCorrectData()
        {
            // Arrange
            string filePath = "test.csv";
            File.WriteAllLines(filePath, new[]
            {
                "OrderId,CustomerName,CustomerGroup,Discount,ProductName,Quantity,PriceEach,ShippingAddress",
                "1,John Doe,individual,0.1,Laptop,2,500.00,city A"
            });

            // Act
            var data = CsvReader.ReadCsv(filePath);

            // Assert
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual("John Doe", data[0]["CustomerName"]);

            // Cleanup
            File.Delete(filePath);
        }
    }

    [TestClass]
    public class OrderFactoryProviderTests
    {
        [TestMethod]
        public void GetFactory_ValidOrderType_ReturnsCorrectFactory()
        {
            // Arrange
            var orderType = "individual_inner_city";

            // Act
            var factory = OrderFactoryProvider.GetFactory(orderType);

            // Assert
            Assert.IsInstanceOfType(factory, typeof(individual_inner_city_ordersFactory));
        }

        [TestMethod]
        public void GetFactory_InvalidOrderType_ThrowsArgumentException()
        {
            // Arrange
            var orderType = "invalid_type";

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => OrderFactoryProvider.GetFactory(orderType));
        }
    }
}
