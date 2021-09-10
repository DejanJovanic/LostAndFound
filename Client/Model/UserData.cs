using Client.Model.Interfaces;
using System;

namespace Client.Model
{
    class UserData : IUserData
    {
        private static readonly Lazy<UserData>
        lazy =
        new Lazy<UserData>
            (() => new UserData());

        public static UserData Instance { get { return lazy.Value; } }
        private string username;
        private string password;
        private object lockObj;

        private UserData()
        {
            lockObj = new object();
            Username = "";
            Password = "";
        }
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                if (value != username)
                {
                    username = value;

                }
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (value != password)
                {
                    password = value;

                }
            }
        }

    }
}
