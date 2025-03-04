﻿using CustomerChurmPrediction.Entities.OrderEntity;
using static CustomerChurmPrediction.Utils.CollectionName;
using MongoDB.Driver;
using CustomerChurmPrediction.Entities.ProductEntity;
using MongoDB.Driver.Linq;
using static CustomerChurmPrediction.Services.OrderService;
using CustomerChurmPrediction.Entities.OrderEntity.Model;
using Microsoft.AspNetCore.SignalR;

namespace CustomerChurmPrediction.Services
{
    public interface IOrderService : IBaseService<Order>
    {
        /// <summary>
        /// Получить список заказов по id компаниии
        /// </summary>
        public Task<List<OrderModel>> GetByCompanyIdAsync(string companyId, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Получить список заказов по id пользователя
        /// </summary>
        public Task<List<OrderModel>> GetByUserIdAsync(string userId, CancellationToken? cancellationToken = default);
    }

    public class OrderService(
        IMongoClient client,
        IConfiguration config,
        ILogger<OrderService> logger,
        IWebHostEnvironment _environment,
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService) 
        : BaseService<Order>(client, config, logger, _environment, _hubContext, _userConnectionService, Orders), IOrderService
    {
        public async Task<List<OrderModel>> GetByCompanyIdAsync(string companyId, CancellationToken? cancellationToken = default)
        {
            if (string.IsNullOrEmpty(companyId))
                throw new ArgumentNullException($"Parameter {nameof(companyId)} is null");
            try
            {
                var orderFilter = Builders<Order>.Filter.ElemMatch(order => order.Items, item => item.CompanyId == companyId);
                var companyOrders = await FindAllAsync(orderFilter, cancellationToken);

                // id продуктов
                List<string> productIds = companyOrders
                    .SelectMany(order => order.Items.Select(i => i.ProductId))
                    .Distinct()
                    .ToList();

                var productFilter = Builders<Product>.Filter.In(product => product.Id, productIds);
                var productCollection = Database.GetCollection<Product>(Products);

                var productList = await (await productCollection.FindAsync(productFilter)).ToListAsync();
                var productDict = productList.ToDictionary(p => p.Id);

                var orderModels = companyOrders.Select(order => new OrderModel
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    OrderStatus = order.OrderStatus,
                    TotalPrice = order.TotalPrice,
                    Items = order.Items.Where(item => item.CompanyId == companyId).Select(item =>
                    {
                        var product = productDict.GetValueOrDefault(item.ProductId);
                        return new OrderItemModel
                        {
                            ProductId = item.ProductId,
                            ProductName = product?.Name ?? "Неизвестный продукт",
                            ProductImageUrl = product?.ImageSrcs?.FirstOrDefault() ?? "",
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            TotalPrice = item.TotalPrice
                        };
                    }).ToList()
                }).ToList();

                return orderModels;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<OrderModel>> GetByUserIdAsync(string userId, CancellationToken? cancellationToken = default)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException();
            try
            {
                var filter = Builders<Order>.Filter.Eq(order => order.UserId, userId);

                List<Order> userOrders = await FindAllAsync(filter, cancellationToken);

                List<string> productIds = userOrders
                    .SelectMany(order => order.Items.Select(i => i.ProductId))
                    .Distinct()
                    .ToList();

                var productFilter = Builders<Product>.Filter.In(product => product.Id, productIds);
                var productCollection = Database.GetCollection<Product>(Products);

                var productList = await (await productCollection.FindAsync(productFilter)).ToListAsync();
                var productDict = productList.ToDictionary(p => p.Id);

                List<OrderModel> orderModels = userOrders.Select(order => new OrderModel
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    OrderStatus = order.OrderStatus,
                    TotalPrice = order.TotalPrice,
                    Items = order.Items.Select(item =>
                    {
                        var product = productDict.GetValueOrDefault(item.ProductId);
                        return new OrderItemModel
                        {
                            ProductId = item.ProductId,
                            ProductName = product?.Name ?? "Неизвестный продукт",
                            ProductImageUrl = product?.ImageSrcs?.FirstOrDefault() ?? "",
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            TotalPrice = item.TotalPrice
                        };
                    }).ToList()
                }).ToList();

                return orderModels;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    
}
