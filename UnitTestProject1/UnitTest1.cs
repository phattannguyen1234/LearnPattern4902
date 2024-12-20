using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static LearnPattern.Program;
using Moq;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
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

            Assert.AreEqual(1, _orderManager._orders.Count());
            Assert.AreEqual("John Doe", _orderManager._orders[0].Customer_Name);
        }
        [TestMethod]
        public void DeleteOrder_OrderExists_OrderRemoved()
        {
            // Arrange 
            var orderManager = new OrderManager();
            var orderMock = new Mock<IOrder>();
            orderMock.SetupGet(o => o.Order_Id).Returns(1);
            orderManager._orders.Add(orderMock.Object); // Thêm đơn hàng vào danh sách
                                                        // Act
            orderManager.DeleteOrder(1);
            // Assert 
            Assert.AreEqual(0, orderManager._orders.Count); // Danh sách trống
        }

        [TestMethod]
        public void DeleteOrder_OrderDoesNotExist_NoChangeInOrders()
        {
            // Arrange
            var orderManager = new OrderManager();
            var orderMock = new Mock<IOrder>();
            orderMock.SetupGet(o => o.Order_Id).Returns(1);
            orderManager._orders.Add(orderMock.Object); // Thêm đơn hàng vào danh sách

            // Act
            orderManager.DeleteOrder(99); // Xóa với ID không tồn tại

            // Assert
            Assert.AreEqual(1, orderManager._orders.Count); // Danh sách không thay đổi
            Assert.AreEqual(1, orderManager._orders[0].Order_Id);
        }

        [TestMethod]
        public void DeleteOrder_OrderExists_PrintsSuccessMessage()
        {
            // Arrange
            var orderManager = new OrderManager();
            var orderMock = new Mock<IOrder>();
            orderMock.SetupGet(o => o.Order_Id).Returns(1);
            orderManager._orders.Add(orderMock.Object);
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                orderManager.DeleteOrder(1);

                // Assert
                var result = sw.ToString().Trim();
                Assert.AreEqual("Order with ID 1 has been successfully deleted.", result);
            }
            
        }
    }

    
}
