using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace AssetInventory.Pages.Assets
{
    public class IndexModel : PageModel
    {
        public List<Asset> listAsset = new();
        public void OnGet()
        {
            try
            {
                string connectionString = "Server=tcp:rdlmssqlserver.database.windows.net,1433;Initial Catalog=rdlassetinventory;Persist Security Info=False;User ID=rdladmin;Password=rdl@12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new(connectionString)) 
                {
                    connection.Open();
                    String sql = "SELECT * FROM RDLAssetInventory";
                    using(SqlCommand command = new(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader()) 
                        {
                            while (reader.Read())
                            {
                                Asset asset = new Asset();
                                asset.id = "" + reader.GetInt32(0);
                                asset.Device_name = reader.GetString(1);
                                asset.Device_IP = reader.GetString(2);
                                asset.Location = reader.GetString(3);
                                asset.created_at = reader.GetDateTime(4).ToString();

                                listAsset.Add(asset);
                            }                            
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class Asset 
    {
        public string id;
        public string Device_name;
        public string Device_IP;
        public string Location;
        public string created_at;
    }
}
