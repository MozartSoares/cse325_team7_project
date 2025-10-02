using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;
using cse325_team7_project.Api.Common;

namespace cse325_team7_project.Api.Binders;

/// <summary>
/// Converts route/query values into MongoDB <see cref="ObjectId"/> instances and raises
/// <see cref="BadRequestException"/> when the supplied value is missing or invalid.
/// Triggered automatically whenever a controller action declares an <see cref="ObjectId"/> parameter.
/// It looks for the parameter name (e.g. "id") in multiple sources, in order:
/// 1. Route -> /movies/{id}
/// 2. Query string -> /movies?id=...
/// 3. Form data (POST forms)
/// 4. Request body (for complex objects in JSON)
/// 5. Headers (if configured)
/// </summary>
public sealed class ObjectIdModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

        var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        // Abort early if the parameter is absent in the route/query/form providers.
        if (value == ValueProviderResult.None)
            throw new BadRequestException($"Missing parameter '{bindingContext.ModelName}'");

        var valueStr = value.FirstValue;
        // Empty strings are treated as missing data.
        if (string.IsNullOrWhiteSpace(valueStr))
            throw new BadRequestException($"Parameter '{bindingContext.ModelName}' is required");

        // Parse the hexadecimal value; reject malformed inputs.
        if (!ObjectId.TryParse(valueStr, out var oid))
            throw new BadRequestException($"Parameter '{bindingContext.ModelName}' must be a valid ObjectId");

        bindingContext.Result = ModelBindingResult.Success(oid);
        return Task.CompletedTask;
    }
}
