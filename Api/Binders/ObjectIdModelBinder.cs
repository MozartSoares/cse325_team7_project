using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;
using cse325_team7_project.Api.Common;

namespace cse325_team7_project.Api.Binders;

/**binder: interecepts requests to transform a simple string into an ObjectId from the route or query string via ValueProviders
It looks for the parameter name (e.g. "id") in multiple sources, in order:
1. Route -> /movies/{id}
2. Query string -> /movies?id=...
3. Form data (POST forms)
4. Request body (for complex objects in JSON)
5. Headers (if configured)
It converts the string found (from route or query) into a valid ObjectId, or throws a standardized exception if invalid.
This ObjectIdModelBinder is only triggered when the controller parameter type is ObjectId.
*/
public sealed class ObjectIdModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

        var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        //if the value is not found, throw a bad request exception
        if (value == ValueProviderResult.None)
            throw new BadRequestException($"Missing parameter '{bindingContext.ModelName}'");

        var valueStr = value.FirstValue;
        //if the value is empty, throw a bad request exception
        if (string.IsNullOrWhiteSpace(valueStr))
            throw new BadRequestException($"Parameter '{bindingContext.ModelName}' is required");

        //if the value is not a valid ObjectId, throw a bad request exception
        if (!ObjectId.TryParse(valueStr, out var oid))
            throw new BadRequestException($"Parameter '{bindingContext.ModelName}' must be a valid ObjectId");

        bindingContext.Result = ModelBindingResult.Success(oid);
        return Task.CompletedTask;
    }
}
