namespace Client.Model.Interfaces
{
    interface IItemReturnValue
    {
        IItem DatabaseValue { get; set; }
        IItem SubmitedValue { get; set; }
        Response Response { get; set; }
    }

    public enum Response
    {
        OK,
        INVALIDDATA,
        CONFLICT,
        NOTLOGGEDIN,
    }
}
