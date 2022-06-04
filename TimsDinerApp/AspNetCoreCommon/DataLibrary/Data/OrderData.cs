using Dapper;
using DataLibrary.Db;
using DataLibrary.Models;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary.Data
{
    public class OrderData : IOrderData
    {
        private readonly IDataAccess _dataAccess;
        private readonly ConnectionStringData _connectionStringData;

        public OrderData(IDataAccess dataAccess, ConnectionStringData connectionStringData)
        {
            _dataAccess = dataAccess;
            _connectionStringData = connectionStringData;
        }

        public async Task<int> CreateOrder(Order order)
        {
            var dynamicParameters = new DynamicParameters();

            dynamicParameters.Add("OrderName", order.OrderName);
            dynamicParameters.Add("OrderDate", order.OrderDate);
            dynamicParameters.Add("FoodId", order.FoodId);
            dynamicParameters.Add("Quantity", order.Quantity);
            dynamicParameters.Add("Total", order.Total);
            dynamicParameters.Add("Id", DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("dbo.spOrders_Insert",
                                       dynamicParameters,
                                       _connectionStringData.SqlConnectionName);

            return dynamicParameters.Get<int>("Id");
        }

        public Task<int> UpdateOrderName(int orderId, string orderName)
        {
            return _dataAccess.SaveData("dbo.spOrders_UpdateName",
                                        new { Id = orderId, OrderName = orderName },
                                        _connectionStringData.SqlConnectionName);
        }

        public Task<int> DeleteOrder(int orderId)
        {
            return _dataAccess.SaveData("dbo.spOrders_Delete",
                                        new { Id = orderId },
                                        _connectionStringData.SqlConnectionName);
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            var records = await _dataAccess.LoadData<Order, dynamic>("dbo.spOrders_GetById", new
            {
                Id = orderId
            },
            _connectionStringData.SqlConnectionName);

            return records.FirstOrDefault();
        }
    }
}