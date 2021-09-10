using System.ComponentModel.DataAnnotations;

namespace Client.Model
{
    class DisplayPersonPassword : DisplayPerson
    {
        private string password;
        [Required]
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        public DisplayPersonPassword() : base() { }
    }
}
