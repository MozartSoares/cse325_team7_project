using cse325_team7_project.Domain.Models;
using MongoDB.Bson;

namespace cse325_team7_project.Api.Services.Interfaces;

/// <summary>
/// Contract for the user aggregate operations consumed by controllers.
/// </summary>
public interface IUserService
{
    /// <summary>Returns every user document.</summary>
    Task<IReadOnlyList<User>> List();

    /// <summary>Loads a user or throws when it does not exist.</summary>
    Task<User> Get(ObjectId id);

    /// <summary>Creates a new user ensuring email/username uniqueness.</summary>
    Task<User> Create(User user);

    /// <summary>Updates a user while keeping immutable metadata.</summary>
    Task<User> Update(ObjectId id, User update);

    /// <summary>Deletes a user and returns whether the operation removed a document.</summary>
    Task<bool> Delete(ObjectId id);
}
