using Client.Model.Interfaces;

namespace Client.Model
{
    class ItemReturnValue : IItemReturnValue
    {
        public IItem DatabaseValue { get; set; }
        public IItem SubmitedValue { get; set; }
        public Response Response { get; set; }
    }
}
