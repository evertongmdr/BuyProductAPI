using AutoMapper;

namespace BuyProductAPI.Profiles
{
    public class ProductsProfile: Profile
    {
        public ProductsProfile()
        {
            CreateMap<Entites.Product, Models.ProductDto>().ReverseMap();
        }
    }
}
