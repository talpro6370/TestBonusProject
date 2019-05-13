using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Configuration;
using FireSharp;
using Newtonsoft;
using Newtonsoft.Json;

namespace Summarry_tragil_for_test
{
    class DAOFireBaseProvider : IDAOProvider
    {
        static IFirebaseClient firebaseClient;
        
        static IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret =ConfigurationManager.AppSettings["secret"],
            BasePath = ConfigurationManager.AppSettings["url"]
        };
        static DAOFireBaseProvider()
        {
            firebaseClient = new FireSharp.FirebaseClient(config);
        }
        public bool AddCustomer(Customer c)
        {
            try
            {
                FirebaseResponse res = firebaseClient.Get($"Customer/{c.Id}");
                var res2 = res.ResultAs<Customer>();
            }
            catch(NullReferenceException n)
            {
                PushResponse response = firebaseClient.Push($"Customer/{c.Id}", c);
                Customer result = response.ResultAs<Customer>();
                return true;
            }
            return false;
        }
        public bool AddOrder(Order o)
        {
            try
            {
                FirebaseResponse res = firebaseClient.Get($"Order/{o.Id}");
                var res2 = res.ResultAs<Order>();
            }
            catch (NullReferenceException n)
            {
                Console.WriteLine(n.Message);
                return false;
            }
            PushResponse response = firebaseClient.Push("Order/4", o);
            Order result = response.ResultAs<Order>();
            if (result != null)
                return true;
            return false;
        }

        public List<Customer> GetAllCustomers()
        {
            //List<Customer> resList = new List<Customer>();
            FirebaseResponse response = firebaseClient.Get("Customer");
            var result = response.ResultAs<List<Customer>>();
            result.ForEach(m => Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(result)));
            return result;
        }

        public List<CustomerOrder> GetAllOrderCustomer()
        {
            var Customers = GetAllCustomers();
            var Orders = GetAllOrders();
            Console.WriteLine("Each customer and his orders:");
            var innerJoin = Customers.Join(Orders,
                c => c.Id,
                o => o.Customer_Id,
                (c, o) => new CustomerOrder()
                {
                    Customer_Id = c.Id,
                    Price = o.Price,
                    Date = o.Date,
                    Customer_Name = c.Name
                }).ToList();
            innerJoin.ForEach(m => Console.WriteLine(JsonConvert.SerializeObject(m)));
            return innerJoin;
        }

        public List<Order> GetAllOrders()
        {
            FirebaseResponse response = firebaseClient.Get("Order");
            var result = response.ResultAs<List<Order>>();
            result.ForEach(m => Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(result)));
            return result;
        }

        public List<Order> GetAllOredersByCustomerId(int customerId)
        {
            FirebaseResponse response = firebaseClient.Get($"Order/{customerId}");
            
            var result = response.ResultAs<List<Order>>();
            
            result.ForEach((m => Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(result))));
            return result;
        }

        public Customer GetCustomerById(int customerId)
        {
            FirebaseResponse response = firebaseClient.Get($"Customer/{customerId}");
            var result = response.ResultAs<Customer>();
            return result;
        }

        public Order GetOrderById(int orderId)
        {
            FirebaseResponse response = firebaseClient.Get($"Order/{orderId}");
            var result = response.ResultAs<Order>();
            return result;
        }

        public bool RemoveCustomer(int customerId)
        {
            if (GetAllOredersByCustomerId(customerId) != null) return false; //If this customer 
                                                                            //has orders-
                                                                           //dont remove customer.
            FirebaseResponse response = firebaseClient.Delete($"Customer/{customerId}");
            if (response.Body.Contains($"Customer/{customerId}") == true) return false;
            return true;
        }

        public bool RemoveOrder(int orderId)
        {
            FirebaseResponse response = firebaseClient.Delete($"Order/{orderId}");
            if (response.Body.Contains($"Order/{orderId}") == true) return false;
            return true;
        }

        public bool UpdateCustomer(Customer c)
        {
            FirebaseResponse response = firebaseClient.Update($"Customer/{c.Id}", c);
            var result = response.ResultAs<Customer>();
            if (result.Name == c.Name) return true;
            return false;
        }

        public bool UpdateOrder(Order o)
        {
            try
            {
                FirebaseResponse res = firebaseClient.Get($"Customer/{o.Customer_Id}");
                var res2 = res.ResultAs<Customer>();
            }
            catch (NullReferenceException n)
            {
                Console.WriteLine(n.Message);
                return false;
            }
            FirebaseResponse response = firebaseClient.Update($"Order/{o.Id}", o);
            var result = response.ResultAs<Order>(); 
            if (result.Customer_Id == o.Customer_Id) return true;
            return false;
        }
    }
}
