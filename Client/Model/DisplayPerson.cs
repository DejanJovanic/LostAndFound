using Client.Model.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Client.Model
{
    public class DisplayPerson : ValidatableBindableBase, IPerson
    {

        private string name;
        [Required]
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private string lastName;
        [Required]
        public string LastName
        {
            get { return lastName; }
            set { SetProperty(ref lastName, value); }
        }

        private string username;
        [Required]
        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }

        private bool isAdmin;
        [Required]

        public bool IsAdmin
        {
            get { return isAdmin; }
            set { SetProperty(ref isAdmin, value); }
        }

        public DisplayPerson()
        {
            IsAdmin = false;
        }
    }
}
