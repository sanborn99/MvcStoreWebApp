using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using P1DbContext.Models;
using BusinessLayer;
using Xunit;
using System.Collections.Generic;
using System.Configuration;



namespace P1.Tests
{
    public class UnitTest1
    {

        //create the in-memory Db
        DbContextOptions<P1DbClass> options = new DbContextOptionsBuilder<P1DbClass>().UseInMemoryDatabase(databaseName: "TestingDb").Options;
		//DbContextOptions<P1DbClass> options2 = new DbContextOptionsBuilder<P1DbClass>().UseSqlServer(Configuration.GetConnectionString("DefaultConnection")).Options;
		//DbContextOptions<P1DbClass> options2 = new DbContextOptionsBuilder<P1DbClass>().UseSqlServer("Server=localhost\\SQLEXPRESS;Database=P1Db;Trusted_Connection=True;").Options;


		[Fact]
		public void AddingCustomerSuccesfully()		// Test 1
		{
			// arrange
			Customer testCustomer = new Customer()
			{
				FirstName = "Jester",
				LastName = "Teester",
				UserName = "Clown77",
				Password = "jingle"
			};


			bool result = false;
			Customer CustomerResult;


			// act
			using (var context = new P1DbClass(options))
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();

				BusinessModel testBusinessModel = new BusinessModel(context);

				result = testBusinessModel.CreateAccount(testCustomer);

				//assert
				Assert.True(result);
			}
		}

		[Fact]
		public void NewCustomerCorrectValue()		// Test 2
		{
			// arange
			Customer testCustomer = new Customer()
			{
				FirstName = "Jester",
				LastName = "Teester",
				UserName = "Clown77",
				Password = "jingle"
			};

			Customer CustomerResult;


			// act
			using (var context = new P1DbClass(options))
			{
				context.Database.EnsureDeleted();// delete any Db fro a previous test
				context.Database.EnsureCreated();// create anew the Db... you will need ot seed it again.
				context.Customers.Add(testCustomer);
				context.SaveChanges();
				CustomerResult = context.Customers.Where(x => x.FirstName == "Jester").FirstOrDefault();
			}

			// assert
			Assert.Equal(CustomerResult, testCustomer);
		}

        [Fact]
        public void LocationListReturnVal()						// Test 3
        {
			//arange
			Location locationOne = new Location()
			{
				LocationName = "East Street Shop",
				LocationAddress = "123 East Street",
				LocationPhoneNumber = "111-111-1111",
			};
			Location locationTwo = new Location()
			{
				LocationName = "West Street Shop",
				LocationAddress = "123 West Street",
				LocationPhoneNumber = "111-111-1111",
			};
			Location locationThree = new Location()
			{
				LocationName = "North Street Shop",
				LocationAddress = "123 North Street",
				LocationPhoneNumber = "111-111-1111",
			};

			List<Location> testLocationList = new List<Location>();

			//act
			using (var context = new P1DbClass(options))
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();

				BusinessModel testBusinessModel = new BusinessModel(context);

				context.Locations.Add(locationOne);
				context.Locations.Add(locationTwo);
				context.Locations.Add(locationThree);
				context.SaveChanges();

				testLocationList = testBusinessModel.GetLocationsList();
			}

			//assert
			Assert.Equal(3, testLocationList.Count);
		}


		[Fact]
		public void GetLocationById()         // Test 4
		{
			//arange
			Location testLocationSetup = new Location()
			{
				LocationName = "West Street Shop",
				LocationAddress = "123 West Street",
				LocationPhoneNumber = "111-111-1111",
			};

			int testLocationId;
			Location testLocationMethodVal = new Location();
			Location testLocationQueryVal = new Location();

			//act
			using (var context = new P1DbClass(options))
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();

				BusinessModel testBusinessModel = new BusinessModel(context);

				context.Locations.Add(testLocationSetup);
				context.SaveChanges();

				testLocationId	= context.Locations.Max(x => x.LocationId);
				testLocationMethodVal = testBusinessModel.GetLocation(testLocationId);
				testLocationQueryVal = context.Locations.Where(x => x.LocationId == testLocationId).FirstOrDefault();
			}

			//assert
			Assert.Equal(testLocationQueryVal, testLocationMethodVal);
		}	   

		[Fact]
		public void CustomerListEmptyInput()            // Test 5
		{
			//arange
			Customer testCustomer1 = new Customer()
			{
				FirstName = "Mason",
				LastName = "Sanborn",
				UserName = "na",
				Password = "na"
			};
			Customer testCustomer2 = new Customer()
			{
				FirstName = "Sam",
				LastName = "Smith",
				UserName = "na",
				Password = "na"
			};
			Customer testCustomer3 = new Customer()
			{
				FirstName = "Sam",
				LastName = "Franks",
				UserName = "na",
				Password = "na"
			};
			Customer testCustomer4 = new Customer()
			{
				FirstName = "Jared",
				LastName = "Franks",
				UserName = "na",
				Password = "na"
			};

			List<Customer> testList = new List<Customer>();

			//act
			using (var context = new P1DbClass(options))
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();

				BusinessModel testBusinessModel = new BusinessModel(context);

				context.Customers.Add(testCustomer1);
				context.Customers.Add(testCustomer2);
				context.Customers.Add(testCustomer3);
				context.Customers.Add(testCustomer4);
				context.SaveChanges();

				testList = testBusinessModel.GetCustomerList("", "");
			}

			//assert
			Assert.Equal(4, testList.Count);
		}

		[Fact]
		public void CustomerListFirstNameInput()           // Test 6
		{
			//arange
			Customer testCustomer1 = new Customer()
			{
				FirstName = "Mason",
				LastName = "Sanborn",
				UserName = "na",
				Password = "na"
			};
			Customer testCustomer2 = new Customer()
			{
				FirstName = "Sam",
				LastName = "Smith",
				UserName = "na",
				Password = "na"
			};
			Customer testCustomer3 = new Customer()
			{
				FirstName = "Sam",
				LastName = "Franks",
				UserName = "na",
				Password = "na"
			};
			Customer testCustomer4 = new Customer()
			{
				FirstName = "Jared",
				LastName = "Franks",
				UserName = "na",
				Password = "na"
			};

			List<Customer> testList = new List<Customer>();

			//act
			using (var context = new P1DbClass(options))
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();

				BusinessModel testBusinessModel = new BusinessModel(context);

				context.Customers.Add(testCustomer1);
				context.Customers.Add(testCustomer2);
				context.Customers.Add(testCustomer3);
				context.Customers.Add(testCustomer4);
				context.SaveChanges();

				testList = testBusinessModel.GetCustomerList("Mason", "");
			}

			//assert
			Assert.Equal(1, testList.Count);
		}

		[Fact]
		public void CustomerListFirstNameMultiInput()           // Test 7
		{
			//arange
			Customer testCustomer1 = new Customer()
			{
				FirstName = "Mason",
				LastName = "Sanborn",
				UserName = "na",
				Password = "na"
			};
			Customer testCustomer2 = new Customer()
			{
				FirstName = "Sam",
				LastName = "Smith",
				UserName = "na",
				Password = "na"
			};
			Customer testCustomer3 = new Customer()
			{
				FirstName = "Sam",
				LastName = "Franks",
				UserName = "na",
				Password = "na"
			};
			Customer testCustomer4 = new Customer()
			{
				FirstName = "Jared",
				LastName = "Franks",
				UserName = "na",
				Password = "na"
			};

			List<Customer> testList = new List<Customer>();

			//act
			using (var context = new P1DbClass(options))
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();

				BusinessModel testBusinessModel = new BusinessModel(context);

				context.Customers.Add(testCustomer1);
				context.Customers.Add(testCustomer2);
				context.Customers.Add(testCustomer3);
				context.Customers.Add(testCustomer4);
				context.SaveChanges();

				testList = testBusinessModel.GetCustomerList("Sam", "");
			}

			//assert
			Assert.Equal(2, testList.Count);
		}

		[Fact]
		public void CustomerListLastNameMultiInput()           // Test 8
		{
			//arange
			Customer testCustomer1 = new Customer()
			{
				FirstName = "Mason",
				LastName = "Sanborn",
				UserName = "na",
				Password = "na"
			};
			Customer testCustomer2 = new Customer()
			{
				FirstName = "Sam",
				LastName = "Smith",
				UserName = "na",
				Password = "na"
			};
			Customer testCustomer3 = new Customer()
			{
				FirstName = "Sam",
				LastName = "Franks",
				UserName = "na",
				Password = "na"
			};
			Customer testCustomer4 = new Customer()
			{
				FirstName = "Jared",
				LastName = "Franks",
				UserName = "na",
				Password = "na"
			};

			List<Customer> testList = new List<Customer>();

			//act
			using (var context = new P1DbClass(options))
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();

				BusinessModel testBusinessModel = new BusinessModel(context);

				context.Customers.Add(testCustomer1);
				context.Customers.Add(testCustomer2);
				context.Customers.Add(testCustomer3);
				context.Customers.Add(testCustomer4);
				context.SaveChanges();

				testList = testBusinessModel.GetCustomerList("", "Franks");
			}

			//assert
			Assert.Equal(2, testList.Count);
		}

		[Fact]
		public void CustomerListSpecificInput()           // Test 9
		{
			//arange
			Customer testCustomer1 = new Customer()
			{
				FirstName = "Mason",
				LastName = "Sanborn",
				UserName = "na",
				Password = "na"
			};
			Customer testCustomer2 = new Customer()
			{
				FirstName = "Sam",
				LastName = "Smith",
				UserName = "na",
				Password = "na"
			};
			Customer testCustomer3 = new Customer()
			{
				FirstName = "Sam",
				LastName = "Franks",
				UserName = "na",
				Password = "na"
			};
			Customer testCustomer4 = new Customer()
			{
				FirstName = "Jared",
				LastName = "Franks",
				UserName = "na",
				Password = "na"
			};

			List<Customer> testList = new List<Customer>();

			//act
			using (var context = new P1DbClass(options))
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();

				BusinessModel testBusinessModel = new BusinessModel(context);

				context.Customers.Add(testCustomer1);
				context.Customers.Add(testCustomer2);
				context.Customers.Add(testCustomer3);
				context.Customers.Add(testCustomer4);
				context.SaveChanges();

				testList = testBusinessModel.GetCustomerList("Sam", "Franks");
			}

			//assert
			Assert.Equal(1, testList.Count);
		}

		[Fact]
		public void TestLoginTrue()           // Test 10
		{
			// arrange
			Customer testCustomer = new Customer()
			{
				FirstName = "Jester",
				LastName = "Teester",
				UserName = "Clown77",
				Password = "jingle"
			};
			bool result;

			// act
			//P1DbClass integrationContext = new P1DbClass();
			using (var context = new P1DbClass(options))
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();

				context.Customers.Add(testCustomer);
				context.SaveChanges();

				BusinessModel testBusinessModel = new BusinessModel(context);

				result = testBusinessModel.Login("Clown77", "jingle");

				//assert
				Assert.True(result);
			}
		}

		[Fact]
		public void TestLoginFalse()               // Test 11
		{
			// arrange
			Customer testCustomer = new Customer()
			{
				FirstName = "Jester",
				LastName = "Teester",
				UserName = "Clown77",
				Password = "jingle"
			};
			bool result;

			// act
			//P1DbClass integrationContext = new P1DbClass();
			using (var context = new P1DbClass(options))
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();

				context.Customers.Add(testCustomer);
				context.SaveChanges();

				BusinessModel testBusinessModel = new BusinessModel(context);

				result = testBusinessModel.Login("testfail", "testfail");

				//assert
				Assert.True(!result);
			}
		}

		[Fact]
		public void OrderDetailsTest()           // Test 12
		{
			// arrange
			Order testOrder1 = new Order()
			{
				OrderId =1,
				OrderTime = DateTime.Now,
				CustomerId = 2,
				LocationId = 1
			};
			Order testOrder2 = new Order()
			{
				OrderId = 2,
				OrderTime = DateTime.Now,
				CustomerId = 2,
				LocationId = 1
			};

			Order testOrderResult;

			// act
			//P1DbClass integrationContext = new P1DbClass();
			using (var context = new P1DbClass(options))
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();

				context.Orders.Add(testOrder1);
				context.Orders.Add(testOrder2);
				context.SaveChanges();

				BusinessModel testBusinessModel = new BusinessModel(context);

				testOrderResult = testBusinessModel.GetOrderDetails(1);

				//assert
				Assert.Equal(testOrder1, testOrderResult);
			}
		}

		[Fact]
		public void OrderDetailsFailTest()           // Test 13
		{
			// arrange
			Order testOrder1 = new Order()
			{
				OrderId = 1,
				OrderTime = DateTime.Now,
				CustomerId = 2,
				LocationId = 1
			};
			Order testOrder2 = new Order()
			{
				OrderId = 2,
				OrderTime = DateTime.Now,
				CustomerId = 2,
				LocationId = 1
			};

			Order testOrderResult;

			// act
			//P1DbClass integrationContext = new P1DbClass();
			using (var context = new P1DbClass(options))
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();

				context.Orders.Add(testOrder1);
				context.Orders.Add(testOrder2);
				context.SaveChanges();

				BusinessModel testBusinessModel = new BusinessModel(context);

				testOrderResult = testBusinessModel.GetOrderDetails(2);

				//assert
				Assert.NotEqual(testOrder1, testOrderResult);
			}
		}

		[Fact]
		public void OrderListReturnTest()           // Test 14
		{
			//arange
			Order testOrder1 = new Order()
			{
				OrderId = 1,
				OrderTime = DateTime.Now,
				CustomerId = 2,
				LocationId = 1
			};
			Order testOrder2 = new Order()
			{
				OrderId = 2,
				OrderTime = DateTime.Now,
				CustomerId = 2,
				LocationId = 1
			};

			List<Order> testOrderList = new List<Order>();

			//act
			using (var context = new P1DbClass(options))
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();

				BusinessModel testBusinessModel = new BusinessModel(context);

				context.Orders.Add(testOrder1);
				context.Orders.Add(testOrder2);
				context.SaveChanges();

				testOrderList = testBusinessModel.GetOrderList();
			}

			//assert
			Assert.Equal(2, testOrderList.Count);
		}

		[Fact]
		public void OrderListFalseTest()           // Test 15
		{
			//arange
			Order testOrder1 = new Order()
			{
				OrderId = 1,
				OrderTime = DateTime.Now,
				CustomerId = 2,
				LocationId = 1
			};
			Order testOrder2 = new Order()
			{
				OrderId = 2,
				OrderTime = DateTime.Now,
				CustomerId = 2,
				LocationId = 1
			};

			List<Order> testOrderList = new List<Order>();

			//act
			using (var context = new P1DbClass(options))
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();

				BusinessModel testBusinessModel = new BusinessModel(context);

				context.Orders.Add(testOrder1);
				context.Orders.Add(testOrder2);
				context.SaveChanges();

				testOrderList = testBusinessModel.GetOrderList();
			}

			//assert
			Assert.NotEqual(3, testOrderList.Count);
		}

		[Fact]
		public void LocationOrderListTest()           // Test 16
		{
			//arange
			Order testOrder1 = new Order()
			{
				OrderId = 1,
				OrderTime = DateTime.Now,
				CustomerId = 2,
				LocationId = 1
			};
			Order testOrder2 = new Order()
			{
				OrderId = 2,
				OrderTime = DateTime.Now,
				CustomerId = 2,
				LocationId = 1
			};
			Order testOrder3 = new Order()
			{
				OrderId = 3,
				OrderTime = DateTime.Now,
				CustomerId = 2,
				LocationId = 2
			};

			List<Order> testOrderList = new List<Order>();

			//act
			using (var context = new P1DbClass(options))
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();

				BusinessModel testBusinessModel = new BusinessModel(context);

				context.Orders.Add(testOrder1);
				context.Orders.Add(testOrder2);
				context.Orders.Add(testOrder3);
				context.SaveChanges();

				testOrderList = testBusinessModel.GetOrderList(1);
			}

			//assert
			Assert.Equal(2, testOrderList.Count);
		}


		[Fact]
		public void LocationOrderListTestSingle()           // Test 17
		{
			//arange
			Order testOrder1 = new Order()
			{
				OrderId = 1,
				OrderTime = DateTime.Now,
				CustomerId = 2,
				LocationId = 1
			};
			Order testOrder2 = new Order()
			{
				OrderId = 2,
				OrderTime = DateTime.Now,
				CustomerId = 2,
				LocationId = 1
			};
			Order testOrder3 = new Order()
			{
				OrderId = 3,
				OrderTime = DateTime.Now,
				CustomerId = 2,
				LocationId = 2
			};

			List<Order> testOrderList = new List<Order>();

			//act
			using (var context = new P1DbClass(options))
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();

				BusinessModel testBusinessModel = new BusinessModel(context);

				context.Orders.Add(testOrder1);
				context.Orders.Add(testOrder2);
				context.Orders.Add(testOrder3);
				context.SaveChanges();

				testOrderList = testBusinessModel.GetOrderList(2);
			}

			//assert
			Assert.Equal(1, testOrderList.Count);
		}

		[Fact]
		public void LocationOrderListTestSingleFalse()           // Test 18
		{
			//arange
			Order testOrder1 = new Order()
			{
				OrderId = 1,
				OrderTime = DateTime.Now,
				CustomerId = 2,
				LocationId = 1
			};
			Order testOrder2 = new Order()
			{
				OrderId = 2,
				OrderTime = DateTime.Now,
				CustomerId = 2,
				LocationId = 1
			};
			Order testOrder3 = new Order()
			{
				OrderId = 3,
				OrderTime = DateTime.Now,
				CustomerId = 2,
				LocationId = 2
			};

			List<Order> testOrderList = new List<Order>();

			//act
			using (var context = new P1DbClass(options))
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();

				BusinessModel testBusinessModel = new BusinessModel(context);

				context.Orders.Add(testOrder1);
				context.Orders.Add(testOrder2);
				context.Orders.Add(testOrder3);
				context.SaveChanges();

				testOrderList = testBusinessModel.GetOrderList(2);
			}

			//assert
			Assert.NotEqual(5, testOrderList.Count);
		}

		[Fact]
		public void GetCartTotalSingleItemTest()           // Test 19
		{
			//arange
			Product productOne = new Product()
			{
				ProductName = "Test1",
				ProductId = 99,
				Price = 4,
				Description = "na",
				Category = "Test"
			};


			Dictionary<Product, int> testCart = new Dictionary<Product, int>();
			testCart.Add(productOne, 3);

			decimal cartTotal = 0;

			//act
			using (var context = new P1DbClass(options))
			{
				BusinessModel testBusinessModel = new BusinessModel(context);

				cartTotal = testBusinessModel.GetCartTotal(testCart);
			}

			//assert
			Assert.Equal(12, cartTotal);
		}

		[Fact]
		public void GetCartTotalMultiItemTest()           // Test 20
		{
			//arange
			Product productOne = new Product()
			{
				ProductName = "Test1",
				ProductId = 99,
				Price = 4,
				Description = "na",
				Category = "Test"
			};
			Product productTwo = new Product()
			{
				ProductName = "Test2",
				ProductId = 98,
				Price = 5,
				Description = "na",
				Category = "Test"
			};

			Dictionary<Product, int> testCart = new Dictionary<Product, int>();
			testCart.Add(productOne, 3);
			testCart.Add(productTwo, 1);

			decimal cartTotal = 0;

			//act
			using (var context = new P1DbClass(options))
			{
				BusinessModel testBusinessModel = new BusinessModel(context);

				cartTotal = testBusinessModel.GetCartTotal(testCart);
			}

			//assert
			Assert.Equal(17, cartTotal);
		}


	} // end class
} // end namespace
