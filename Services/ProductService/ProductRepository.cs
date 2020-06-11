using BuyProductAPI.Contexts;
using BuyProductAPI.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BuyProductAPI.Services.ProductService.ProductTypes;

namespace BuyProductAPI.Services.ProductService
{
    public class ProductRepository : IProductRepository
    {
        private readonly BuyProductContext _productContext;
        public ProductRepository(BuyProductContext productContext)
        {
            _productContext = productContext;
        }
        public async Task<bool> CreateProduct(Product product)
        {
            await _productContext.AddAsync(product);
            return await Save();
        }

        public async Task<bool> DeleteProduct(Product product)
        {
            _productContext.Remove(product);
            return await Save();
        }

        public async Task<Product> GetProduct(Guid productId)
        {
            if (productId == Guid.Empty)
                throw new ArgumentNullException(nameof(productId));

            return await _productContext.Products.Where(p => p.Id == productId).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Product>> GetProductsByIds(IEnumerable<Guid> productIds)
        {
            if (productIds == null)
                throw new ArgumentNullException(nameof(productIds));

            return await _productContext.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
        }

        public async Task<ICollection<Product>> GetProducts(ProductResourceParameters productResourceParameters)
        {
            if (productResourceParameters == null)
                throw new ArgumentNullException(nameof(productResourceParameters));

            var collection = _productContext.Products as IQueryable<Product>; /* http://www.macoratti.net/14/11/net_ieiq1.htm =>
            basicamente executa o select com todos os filtros*/

            if (!string.IsNullOrEmpty(productResourceParameters.SearchQuery))
            {
                var searchQuery = productResourceParameters.SearchQuery.Trim().ToLower();

                collection =  collection.Where(p => EF.Functions.Like(p.Name.ToLower(), $"%{searchQuery}%"));
            }

            if (!string.IsNullOrEmpty(productResourceParameters.Quantity))
            {
                int quantity;
                try
                {
                    quantity = int.Parse(productResourceParameters.Quantity.Trim());
                    collection =  collection.Where(p => p.Quantity < quantity);
                }
                catch (FormatException formatException)
                {
                    throw formatException;
                }
            }

            return await collection.ToListAsync();
        }
        public async Task<bool> ProductExists(Guid productId)
        {
            return await _productContext.Products.AnyAsync(p => p.Id == productId);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            _productContext.Update(product);
            return await Save();
        }


        public async Task<bool> Save()
        {
            int saved = await _productContext.SaveChangesAsync();
            return saved >= 0 ? true : false;
        }

        public async Task<bool> ExistsProductInAOrder(Guid productId)
        {
            return await _productContext.OderProducts.AnyAsync(op => op.ProductId == productId);
        }
    }
}
