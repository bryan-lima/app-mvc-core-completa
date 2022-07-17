using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DevIO.App.Extensions
{
    public class MoedaAttribute : ValidationAttribute
    {
        #region Protected Methods

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                decimal _moeda = Convert.ToDecimal(value, new CultureInfo("pt-BR"));
            }
            catch (Exception)
            {
                return new ValidationResult("Moeda em formato inválido");
            }

            return ValidationResult.Success;
        }

        #endregion Protected Methods
    }

    public class MoedaAttributeAdapter : AttributeAdapterBase<MoedaAttribute>
    {
        #region Public Constructors

        public MoedaAttributeAdapter(MoedaAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        {

        }

        #endregion Public Constructors

        #region Public Methods

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context is null)
                throw new ArgumentException(nameof(context));

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-moeda", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-number", GetErrorMessage(context));
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return "Moeda em formato inválido";
        }

        #endregion Public Methods
    }

    public class MoedaValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        #region Private Fields

        private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();

        #endregion Private Fields

        #region Public Methods

        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            if (attribute is MoedaAttribute moedaAttribute)
            {
                return new MoedaAttributeAdapter(moedaAttribute, stringLocalizer);
            }

            return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
        }

        #endregion Public Methods
    }
}
