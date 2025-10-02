namespace cse325_team7_project.Domain.Models;
public class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string MovieCollectionName { get; set; } = "Movies";
    public string UserCollectionName { get; set; } = "Users";
    public string ListCollectionName { get; set; } = "Lists";
}
