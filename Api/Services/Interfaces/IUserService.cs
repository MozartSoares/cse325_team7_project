using cse325_team7_project.Domain.Models;
using MongoDB.Bson;

namespace cse325_team7_project.Api.Services.Interfaces;

public interface IUserService
{
    Task<IReadOnlyList<User>> List();
    Task<User> Get(ObjectId id);
    Task<User> Create(User user);
    Task<User> Update(ObjectId id, User update);
    Task<bool> Delete(ObjectId id);
}
