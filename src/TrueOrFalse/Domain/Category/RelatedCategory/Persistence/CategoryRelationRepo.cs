using MySql.Data.MySqlClient;
using NHibernate;
using Seedworks.Lib.Persistence;

public class CategoryRelationRepo : RepositoryDb<CategoryRelation>
{
    public CategoryRelationRepo(ISession session) : base(session){ }

    public List<CategoryRelation> GetAllRelations(IList<Category> allCategories)
    {
        var relations = new List<CategoryRelation>();

        using (MySqlConnection connection = new MySqlConnection(Settings.ConnectionString))
        {
            connection.Open();
            using (MySqlCommand command = new MySqlCommand("SELECT * FROM memucholive.relatedcategoriestorelatedcategories", connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var relation = new CategoryRelation
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Child = allCategories.FirstOrDefault(c => c.Id == Convert.ToInt32(reader["Category_id"])),
                            Parent = allCategories.FirstOrDefault(c => c.Id == Convert.ToInt32(reader["Related_id"])),
                            PreviousId = reader["Previous_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["Previous_id"]),
                            NextId = reader["Next_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["Next_id"])
                        };
                        relations.Add(relation);
                    }
                }
            }
        }

        return relations;
    }


}