using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using MongoDB.Bson;

namespace cse325_team7_project.Api.Binders;

/// <summary>
/// Registers the <see cref="ObjectIdModelBinder"/> for any action parameter typed as <see cref="ObjectId"/>.
/// </summary>
public sealed class ObjectIdModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(ObjectId))
        {
            return new BinderTypeModelBinder(typeof(ObjectIdModelBinder));
        }
        return null;
    }
}
