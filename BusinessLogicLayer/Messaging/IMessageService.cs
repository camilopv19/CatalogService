using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Messaging
{
    public interface IMessageService
    {
        string Publish(Item item);
    }
}