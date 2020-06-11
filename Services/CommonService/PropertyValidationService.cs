using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BuyProductAPI.Services.CommonService
{
    // essa classe é utilizada com ShapeDate (Escolhe os atributos que irão retronar na request)
    public class PropertyValidationService : IPropertyValidationService
    {
        public bool HasValidProperties<T>(string fields)
        {
            if (string.IsNullOrEmpty(fields))
                return true;

            var fieldsArray = fields.Split(",");

            foreach (var field in fieldsArray)
            {
                var propetyName = field.Trim();
                var propertyNameExists = typeof(T).GetProperty(propetyName,
                    BindingFlags.IgnoreCase |
                    BindingFlags.Public |
                    BindingFlags.Instance);

                if (propertyNameExists == null)
                    return false;

            }
            return true;
        }
    }
}
