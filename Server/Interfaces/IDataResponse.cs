namespace Server.Interfaces
{
    public interface IDataResponse
    {
        IValue CurrentValue { get; set; }
        string Response { get; set; }
    }
}
