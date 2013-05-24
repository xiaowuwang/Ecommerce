using System;
using System.Linq;
using System.Web.Mvc;
using MrCMS.Web.Apps.Ecommerce.Entities.Discounts;
using MrCMS.Web.Apps.Ecommerce.Services.Discounts;
using NHibernate;
using MrCMS.Web.Apps.Ecommerce.Entities;

namespace MrCMS.Web.Apps.Ecommerce.Binders
{
    public class AddDiscountApplicationModelBinder : DiscountApplicationModelBinder
    {
        public AddDiscountApplicationModelBinder(ISession session, IDiscountApplicationService discountApplicationService)
            : base(session, discountApplicationService)
        {
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var modelTypeName = controllerContext.Controller.ValueProvider.GetValue("ApplicationOpt").AttemptedValue;
            var type = bindingContext.ModelType.Assembly.GetTypes().SingleOrDefault(x => x.IsSubclassOf(bindingContext.ModelType) && x.FullName == modelTypeName);

            bindingContext.ModelMetadata =
                ModelMetadataProviders.Current.GetMetadataForType(
                    () => CreateModel(controllerContext, bindingContext, type), type);

            var discountApplication = base.BindModel(controllerContext, bindingContext) as DiscountApplication;
            discountApplication.Id = 0;
            return discountApplication;
        }

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var obj = Activator.CreateInstance(modelType);
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, modelType);
            bindingContext.ModelMetadata.Model = obj;
            return obj;
        }
    }
}