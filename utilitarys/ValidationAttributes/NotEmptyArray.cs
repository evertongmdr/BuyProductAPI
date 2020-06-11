using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BuyProductAPI.utilitarys.ValidationAttributes
{
    [AttributeUsage(
    AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false)]
    public class NotEmptyArray : ValidationAttribute
    {
        public const string DefaultErrorMessage = "O atributo {0}  não pode ser vázio";
        public NotEmptyArray() : base(DefaultErrorMessage) { }

        public override bool IsValid(object value)
        {

            if (value is null)
                return true; // quem vai fazer esse tratamento o requerid por isso retorna true
           
            return ((ICollection)value).Count ==0 ?false: true;      
        }
    }
}
