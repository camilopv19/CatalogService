using DataAccessLayer.Entities;

namespace BusinessLogicLayer
{
    public interface IMessageService
    {
        string Publish(Item item);
    }
}