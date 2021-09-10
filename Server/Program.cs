using Server.Database;
using Server.Model;
using System;
using System.ServiceModel;

namespace Server
{
    class Program
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main()
        {
            using (var context = new LostAndFoundContext())
            {
                context.Database.CreateIfNotExists();

            }
            InitializeDB();
            ServiceHost loginHost = new ServiceHost(typeof(LoginService));
            ServiceHost dataHost = new ServiceHost(typeof(DataService));
            ServiceHost userHost = new ServiceHost(typeof(UserService));
            loginHost.Open();
            dataHost.Open();
            userHost.Open();
            log.Info("Service successfully started");
            Console.WriteLine("Service Opened");
            Console.Read();
            loginHost.Close();
            dataHost.Close();
            userHost.Close();
            log.Info("Service successfully closed");

        }
        static void InitializeDB()
        {
            if (!Database.Database.Instance.DoesUserExist("admin", "admin"))
            {
                using (var repo = new UsersRepo())
                {
                    repo.AddUserServer(new DatabasePerson() { IsAdmin = true, LastName = "admin", Name = "admin", Password = "admin", Username = "admin" });
                    log.Info("Default admin inserted");
                }
            }
            log.Info("Default admin is already in database");
        }
    }


}
