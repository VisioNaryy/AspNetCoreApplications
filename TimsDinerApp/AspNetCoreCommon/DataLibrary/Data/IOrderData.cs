using DataLibrary.Models;
using System.Threading.Tasks;

namespace DataLibrary.Data
{
    public interface IOrderData
    {
        Task<int> CreateOrder(Order order);
        Task<int> DeleteOrder(int orderId);
        Task<Order> GetOrderById(int orderId);
        Task<int> UpdateOrderName(int orderId, string orderName);
    }
}