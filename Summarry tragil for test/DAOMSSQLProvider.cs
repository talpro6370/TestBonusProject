using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarry_tragil_for_test
{
    class DAOMSSQLProvider : IDAOProvider
    {
        static TirgulForTestEntities entity = new TirgulForTestEntities();
        public bool AddCustomer(Customer c)
        {
            int len = entity.Customers.Count();
            entity.Customers.Add(c);
            if (entity.Customers.Count()!=len&& entity.Customers.Count()!=0)
            return true;
            entity.SaveChanges();
            return false;
        }

        public bool AddOrder(Order o)
        {
            int len = entity.Orders.Count();
            entity.Orders.Add(o);
            if (entity.Orders.Count() != len && entity.Orders.Count() != 0)
                return true;
            entity.SaveChanges();
            return false;
        }

        public List<Customer> GetAllCustomers()
        {
            List<Customer> list = entity.Customers.ToList();
            return list ;
        }

        public List<CustomerOrder> GetAllOrderCustomer()
        {
            Console.WriteLine("Each customer and his orders:");

            var innerJoin= entity.Orders.Join(entity.Customers, order => order.Customer_Id, customer => customer.Id, (order, customer) => new CustomerOrder()
            {
                Customer_Id = customer.Id,
                Price=order.Price,
                Date=order.Date,
                Customer_Name = customer.Name
            }).ToList();
            innerJoin.ForEach(m => Console.WriteLine(JsonConvert.SerializeObject(m)));
            return innerJoin;
        }

        public List<Order> GetAllOrders()
        {
            List<Order> list = entity.Orders.ToList();
            return list;
        }

        public List<Order> GetAllOredersByCustomerId(int customerId)
        {
            List<Order> ordersRes = new List<Order>();
            ordersRes = entity.Orders.ToList();
            ordersRes = (from o in entity.Orders where o.Customer_Id == customerId select o).ToList();
            return ordersRes;
        }

        public Customer GetCustomerById(int customerId)
        {
            List<Customer> custs = new List<Customer>();
            Customer c = new Customer();
            custs = (from cust in entity.Customers where cust.Id == customerId select cust).ToList();
            c = custs[0];
            return c;
        }

        public Order GetOrderById(int orderId)
        {
            List<Order> custs = new List<Order>();
            Order o = new Order();
            custs = (from ords in entity.Orders where ords.Id == orderId select ords).ToList();
            o = custs[0];
            return o;
        }

        public bool RemoveCustomer(int customerId)
        {
            int len = entity.Customers.Count();
            var custs = (from c in entity.Customers where c.Id == customerId select c).ToList();
            entity.Customers.Remove(custs[0]);
            entity.SaveChanges();
            if (len != entity.Customers.Count())
            return true;
            return false;
        }

        public bool RemoveOrder(int orderId)
        {
            int len = entity.Orders.Count();
            var ords = (from o in entity.Orders where o.Id == orderId select o).ToList();
            entity.Orders.Remove(ords[0]);
            entity.SaveChanges();
            if (len != entity.Orders.Count())
                return true;
            return false;
        }

        public bool UpdateCustomer( Customer c)
        {
            entity.Customers.Take(1).FirstOrDefault().Name = c.Name;
            entity.Customers.Take(1).FirstOrDefault().Country = c.Country;
            entity.Customers.Take(1).FirstOrDefault().Age= c.Age;
            entity.SaveChanges();
            var list = entity.Customers.ToList();
            if (list[0].Age == c.Age) return true;
            return false;
        }

        public bool UpdateOrder(Order o)
        {
            entity.Orders.Take(1).FirstOrDefault().Customer_Id = o.Customer_Id;
            entity.Orders.Take(1).FirstOrDefault().Price = o.Price;
            entity.Orders.Take(1).FirstOrDefault().Date= o.Date;
            entity.SaveChanges();
            var list = entity.Orders.ToList();
            if (list[0].Customer_Id == o.Customer_Id) return true;
            return false;
        }
    }
}
