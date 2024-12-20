using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Xml.Schema;

namespace LearnPattern
{
	public class Program
	{
		static void Main()
        {

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            
            OrderManager manager = new OrderManager();

            
            Menu menu = new Menu(manager);
            menu.ShowMenu();
            
            string Remain = Console.ReadLine();
		}

        public class Menu
        {
            private OrderManager _orderManager; 

            public Menu(OrderManager orderManager)
            {
                _orderManager = orderManager;
            }

            public void ShowMenu()
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("=== Order Management Menu ===");
                    Console.WriteLine("1. Load and Process Orders from CSV");
                    Console.WriteLine("2. Display All Orders");
                    Console.WriteLine("3. Add New Order");
                    Console.WriteLine("4. Delete Order by ID");
                    Console.WriteLine("5. Export Orders to CSV");
                    Console.WriteLine("6. Exit");
                    Console.Write("Enter your choice: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            LoadOrders();
                            break;
                        case "2":
                            _orderManager.ListOrder();
                            Console.WriteLine("\nPress Enter to return to menu...");
                            Console.ReadLine();
                            break;
                        case "3":
                            AddOrder();
                            break;
                        case "4":
                            DeleteOrder();
                            break;
                        case "5":
                            ExportOrders();
                            break;
                        case "6":
                            Console.WriteLine("Exiting the program. Goodbye!");
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Try again.");
                            Console.ReadLine();
                            break;
                    }
                }
            }

            private void LoadOrders()
            {
                try
                {
                    Console.Write("Enter the CSV file path: ");
                    string filePath = Console.ReadLine();

                    var ordersData = CsvReader.ReadCsv(filePath);

                    foreach (var orderData in ordersData)
                    {
                        _orderManager.CreateOrder(orderData);
                    }

                    Console.WriteLine("Orders loaded and processed successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading orders: {ex.Message}");
                }
                Console.WriteLine("Press Enter to return to menu...");
                Console.ReadLine();
            }

            private void AddOrder()
            {
                try
                {
                    Console.WriteLine("=== Add New Order ===");
                    var orderData = OrderInput.CollectOrderData();

                    _orderManager.CreateOrder(orderData);

                    Console.WriteLine("Order added successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding order: {ex.Message}");
                }
                Console.WriteLine("Press Enter to return to menu...");
                Console.ReadLine();
            }

            private void DeleteOrder()
            {
                try
                {
                    Console.WriteLine("=== Delete Order by ID ===");
                    Console.Write("Enter Order ID to delete: ");
                    string input = Console.ReadLine();

                    if (int.TryParse(input, out int orderId))
                    {
                        _orderManager.DeleteOrder(orderId);
                    }
                    else
                    {
                        Console.WriteLine("Invalid Order ID. Please enter a numeric value.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting order: {ex.Message}");
                }
                Console.WriteLine("Press Enter to return to menu...");
                Console.ReadLine();
            }

            private void ExportOrders()
            {
                try
                {
                    // Console.Write("Enter the file path to export orders (e.g., E:\\exported_orders.csv): ");
                    Console.Write("The CSV Export File has been saved at E:\\bin\\products.csv\n");
                    string filePath = "E:\\bin\\products.csv";

                    // Gọi phương thức từ OrderManager để xuất dữ liệu
                    _orderManager.ExportOrdersToCsv(filePath);

                    Console.WriteLine("Orders exported successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error exporting orders: {ex.Message}");
                }

                Console.WriteLine("Press Enter to return to menu...");
                Console.ReadLine();
            }
        }


        public interface IOrder
		{
			int Order_Id {  get; set; }
			string Customer_Name { get; set; }
			string Customer_Group { get; set; }
			double Discount { get; set; }
			string Product_Name {  get; set; }
			int Quantity {  get; set; }
			double Price_Each { get; set; }
			double Shipping_Fee {  get; }
			string Shipping_Address { get; set; }
			double Provisional_Amount { get; }
			double Payment {  get; }
            string Order_Type { get; set; }

		}

		public class individual_inner_city_orders : IOrder
		{
			public int Order_Id { get; set; }
			public string Customer_Name { get; set; }
			public string Customer_Group { get; set; }
			public double Discount { get; set; }
			public string Product_Name { get; set; }
			public int Quantity { get; set; }
			public double Price_Each { get; set; }
			public double Shipping_Fee => 10000;
			public string Shipping_Address { get; set; }
			//
			public double Provisional_Amount => Quantity * Price_Each;
			public double Payment => Quantity * Price_Each * (1 - Discount) + Shipping_Fee;
            //
            public string Order_Type { get; set; } = "individual_inner_city_order";
		}

        public class individual_outer_city_orders : IOrder
        {
            public int Order_Id { get; set; }
            public string Customer_Name { get; set; }
            public string Customer_Group { get; set; }
            public double Discount { get; set; }
            public string Product_Name { get; set; }
            public int Quantity { get; set; }
            public double Price_Each { get; set; }
            public double Shipping_Fee => 45000;
            public string Shipping_Address { get; set; }
            //
            public double Provisional_Amount => Quantity * Price_Each;
            public double Payment => Quantity * Price_Each * (1 - Discount) + Shipping_Fee;
            //
            public string Order_Type { get; set; } = "individual_outer_city_order";
        }

        public class partner_inner_city_orders : IOrder
        {
            public int Order_Id { get; set; }
            public string Customer_Name { get; set; }
            public string Customer_Group { get; set; }
            public double Discount { get; set; }
            public string Product_Name { get; set; }
            public int Quantity { get; set; }
            public double Price_Each { get; set; }
            public double Shipping_Fee => 65000;
            public string Shipping_Address { get; set; }
            //
            public double Provisional_Amount => Quantity * Price_Each;
            public double Payment => Quantity * Price_Each * (1 - Discount) + Shipping_Fee;
            //
            public string Order_Type { get; set; } = "partner_inner_city_order";
            
        }

        public class partner_outer_city_orders : IOrder
        {
            public int Order_Id { get; set; }
            public string Customer_Name { get; set; }
            public string Customer_Group { get; set; }
            public double Discount { get; set; }
            public string Product_Name { get; set; }
            public int Quantity { get; set; }
            public double Price_Each { get; set; }
            public double Shipping_Fee => 120000;
            public string Shipping_Address { get; set; }
            //
            public double Provisional_Amount => Quantity * Price_Each;
            public double Payment => Quantity * Price_Each * (1 - Discount) + Shipping_Fee;
            //
            public string Order_Type { get; set; } = "partner_outer_city_order";
        }

        public abstract class OrderFactory
		{
			public abstract IOrder CreateOrder();
		}
		
		public class individual_inner_city_ordersFactory : OrderFactory
		{
			public override IOrder CreateOrder()
			{
				return new individual_inner_city_orders();
			}
		}
        public class individual_outer_city_ordersFactory : OrderFactory
        {
            public override IOrder CreateOrder()
            {
                return new individual_outer_city_orders();
            }
        }

        public class partner_inner_city_ordersFactory : OrderFactory
        {
            public override IOrder CreateOrder()
            {
                return new partner_inner_city_orders();
            }
        }
        public class partner_outer_city_ordersFactory : OrderFactory
        {
            public override IOrder CreateOrder()
            {
                return new partner_outer_city_orders();
            }
        }

        public static class OrderFactoryProvider
		{
			public static OrderFactory GetFactory(string orderType)
			{
				switch (orderType.ToLower())
				{
					case "individual_inner_city":
						return new individual_inner_city_ordersFactory();
                    case "individual_outer_city":
                        return new individual_outer_city_ordersFactory();
                    case "partner_inner_city":
                        return new partner_inner_city_ordersFactory();
                    case "partner_outer_city":
                        return new partner_outer_city_ordersFactory();
                    default:
							throw new ArgumentException("Invalid order type");
				}
			}
		}

		public class OrderManager
		{
			public readonly List<IOrder> _orders = new List<IOrder>();

            public void CreateOrder(Dictionary<string, string> orderData)
			{
                //
                string orderType = OrderClassifier.ClassifyOrder(orderData);
                //
                int orderId = int.Parse(orderData["OrderId"]);
                string customerName = orderData["CustomerName"];
                string customerGroup = orderData["CustomerGroup"];
                double discount = double.Parse(orderData["Discount"], CultureInfo.InvariantCulture);
                string productName = orderData["ProductName"];
                int quantity = int.Parse(orderData["Quantity"]);
                double priceEach = double.Parse(orderData["PriceEach"], CultureInfo.InvariantCulture);
                string shippingAddress = orderData["ShippingAddress"];
                //
                
                //
                OrderFactory factory = OrderFactoryProvider.GetFactory(orderType);
				IOrder order = factory.CreateOrder();

				order.Order_Id = orderId;
				order.Customer_Name = customerName;
                order.Customer_Group = customerGroup;
				order.Discount = discount;
				order.Product_Name = productName;
				order.Quantity = quantity;
				order.Price_Each = priceEach;
                order.Shipping_Address = shippingAddress;
                //
				_orders.Add(order);
			}

            public void DeleteOrder(int orderId)
            {
                var order = _orders.FirstOrDefault(o => o.Order_Id == orderId);
                if (order != null)
                {
                    _orders.Remove(order);
                    Console.WriteLine($"Order with ID {orderId} has been successfully deleted.");
                }
                else
                {
                    Console.WriteLine($"Order with ID {orderId} not found.");
                }
            }

            public void ListOrder()
			{
				foreach (var order in _orders)
				{
                    Console.WriteLine(
                        "----------------------------------------\n" +
                        $"Order ID:          {order.Order_Id}\n" +
                        $"Customer Name:     {order.Customer_Name}\n" +
                        $"Customer Group:    {order.Customer_Group}\n" +
                        $"Discount:          {order.Discount:P}\n" +
                        $"Product Name:      {order.Product_Name}\n" +
                        $"Quantity:          {order.Quantity}\n" +
                        $"Price Each:        {order.Price_Each.ToString("C", new CultureInfo("en-US"))}\n" +
                        $"Shipping Fee:      {order.Shipping_Fee.ToString("C", new CultureInfo("en-US"))}\n" +
                        $"Shipping Address:  {order.Shipping_Address}\n" +
                        $"Provisional Amount:{order.Provisional_Amount.ToString("C", new CultureInfo("en-US"))}\n" +
                        $"Payment:           {order.Payment.ToString("C", new CultureInfo("en-US"))}\n" +
                        $"Order Type:        {order.Order_Type}\n" +
                        "----------------------------------------"
                    );
                }
			}

            public static class OrderClassifier
            {
                public static string ClassifyOrder(Dictionary<string, string> orderData)
                {
                    if (orderData["CustomerGroup"] == "individual" &&
                        new[] { "city A", "city B" }.Contains(orderData["ShippingAddress"]))
                    {
                        return "individual_inner_city";
                    }
                    else if (orderData["CustomerGroup"] == "individual" &&
                        new[] { "city C", "city D" }.Contains(orderData["ShippingAddress"]))
                    {
                        return "individual_outer_city";
                    }
                    else if (orderData["CustomerGroup"] == "partner" &&
                        new[] { "city A", "city B" }.Contains(orderData["ShippingAddress"]))
                    {
                        return "partner_inner_city";
                    }
                    else if (orderData["CustomerGroup"] == "partner" &&
                        new[] { "city C", "city D" }.Contains(orderData["ShippingAddress"]))
                    {
                        return "partner_outer_city";
                    }
                    else
                    {
                        throw new ArgumentException("Invalid classification criteria.");
                    }
                }
            }

            public void ExportOrdersToCsv(string filePath)
            {
                try
                {
                    using (var writer = new StreamWriter(filePath))
                    using (var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        // Ghi header (tên cột)
                        csv.WriteHeader<IOrder>();
                        csv.NextRecord();

                        // Ghi từng order vào file
                        foreach (var order in _orders)
                        {
                            csv.WriteRecord(order);
                            csv.NextRecord();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to export orders to CSV.", ex);
                }
            }

        }
        public class CsvReader
        {
            public static List<Dictionary<string, string>> ReadCsv(string filePath)
            {
                var orders = new List<Dictionary<string, string>>();
                var lines = File.ReadAllLines(filePath);

                var headers = lines[0].Split(',');

                for (int i = 1; i < lines.Length; i++)
                {
                    var values = lines[i].Split(',');
                    var orderData = new Dictionary<string, string>();

                    for (int j = 0; j < headers.Length; j++)
                    {
                        orderData[headers[j]] = values[j];
                    }
                    orders.Add(orderData);
                }

                return orders;
            }
        }

        public class OrderInput
        {
            public static Dictionary<string, string> CollectOrderData()
            {
                var orderData = new Dictionary<string, string>();

                Console.Write("Enter Order ID: ");
                orderData["OrderId"] = Console.ReadLine();

                Console.Write("Enter Customer Name: ");
                orderData["CustomerName"] = Console.ReadLine();

                Console.Write("Enter Customer Group (individual/partner): ");
                orderData["CustomerGroup"] = Console.ReadLine();

                Console.Write("Enter Discount (as a decimal, e.g., 0.1 for 10%): ");
                orderData["Discount"] = Console.ReadLine();

                Console.Write("Enter Product Name: ");
                orderData["ProductName"] = Console.ReadLine();

                Console.Write("Enter Quantity: ");
                orderData["Quantity"] = Console.ReadLine();

                Console.Write("Enter Price Each: ");
                orderData["PriceEach"] = Console.ReadLine();

                Console.Write("Enter Shipping Address: ");
                orderData["ShippingAddress"] = Console.ReadLine();

                return orderData;
            }
        }


    }
}
