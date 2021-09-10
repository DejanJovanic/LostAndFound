using System.IdentityModel.Selectors;
using System.ServiceModel;

namespace Server
{
    class IdentityValidator : UserNamePasswordValidator
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override void Validate(string userName, string password)
        {


            if (string.IsNullOrWhiteSpace(userName))
            {
                log.Fatal("Connection without provided username");
                throw new FaultException("Username has to be provided");
            }


            if (string.IsNullOrWhiteSpace(password))
            {
                log.Fatal("Connection without provided password");
                throw new FaultException("Password has to be provided");
            }


            if (!Database.Database.Instance.DoesUserExist(userName, password))
            {
                log.Fatal("Connection with invalid username-password combination");
                throw new FaultException("User with given username and password does not exist");
            }

        }
    }
}
