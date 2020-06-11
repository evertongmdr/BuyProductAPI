using BuyProductAPI.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BuyProductAPI.Services.ProductService.ProductTypes;

namespace BuyProductAPI.Services.ProductService
{
    public interface IProductRepository
    {
        Task<Product> GetProduct(Guid productId);
        Task<ICollection<Product>> GetProductsByIds(IEnumerable<Guid> productIds);
        Task<ICollection<Product>> GetProducts(ProductResourceParameters productResourceParameters);
        Task<bool> ProductExists(Guid productId);
        Task<bool> CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(Product product);
        Task<bool> ExistsProductInAOrder(Guid productId);
        Task<bool> Save();

    }
}
