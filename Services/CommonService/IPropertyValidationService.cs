namespace BuyProductAPI.Services.CommonService
{
    public interface IPropertyValidationService
    {
        bool HasValidProperties<T>(string fields);
    }
}