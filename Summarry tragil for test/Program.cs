using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Configuration;
namespace Summarry_tragil_for_test
{
    class Program
    {
        //static IFirebaseClient firebaseClient;
        
        static void Main(string[] args)
        {

            DAOFireBaseProvider DAO = new DAOFireBaseProvider();
            DAOMSSQLProvider ssmsDao = new DAOMSSQLProvider();
            Customer c = new Customer()
            {
                Id = 1,
                Name = "Tal",
                Country= "Israel",
                Age=28
            };
            Order o = new Order()
            {
                Id = 1,
                Customer_Id = 1,
                Price = 5000,
                Date = "15.2.19"
            };
            //DAO.AddCustomer(c);
            // DAO.GetAllCustomers();
            //DAO.GetAllOredersByCustomerId(3);
            //ssmsDao.AddCustomer(c);
            // ssmsDao.GetAllCustomers();
            //ssmsDao.GetAllOredersByCustomerId(2);
            //ssmsDao.GetCustomerById(1);
            // ssmsDao.RemoveCustomer(5);
            //ssmsDao.GetAllOrderCustomer();
            // DAO.GetCustomerById(1);
            // DAO.GetCustomerById(2);
            // DAO.RemoveCustomer(2);
            //DAO.AddOrder(o);
           // DAO.AddCustomer(c);
            DAO.GetAllOrderCustomer();
        }
    }
}
