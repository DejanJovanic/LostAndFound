namespace Client.Model.Interfaces
{
    public interface IPerson
    {
        bool IsAdmin { get; set; }
        string LastName { get; set; }
        string Name { get; set; }
        string Username { get; set; }
    }

    public enum PersonReturnCode
    {
        OK,
        USERNAMETAKEN,
        USERNOTFOUND,
        INVALIDRIGHT,
        BADUSERSUPPLIED,
        NOTLOGGEDIN,
    }
}