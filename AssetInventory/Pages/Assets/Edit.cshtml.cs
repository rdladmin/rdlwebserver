using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AssetInventory.Pages.Assets
{
    public class EditModel : PageModel
    {
        public Asset asset = new();
        public String errorMessage = "";
        public String SuccessMessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                string connectionString = "Server=tcp:rdlmssqlserver.database.windows.net,1433;Initial Catalog=rdlassetinventory;Persist Security Info=False;User ID=rdladmin;Password=rdl@12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

                using (SqlConnection connection = new(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM RDLAssetInventory WHERE id=@id";
                    using (SqlCommand command = new(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                asset.id = "" + reader.GetInt32(0);
                                asset.Device_name = reader.GetString(1);
                                asset.Device_IP = reader.GetString(2);
                                asset.Location = reader.GetString(3);
                            }
                        }
                    }
                }

            }
            catch (Exception ex) 
            { 
                errorMessage = ex.Message;
                return;
            }
        }


        public void OnPost() 
        { 
            asset.id = Request.Form["id"];
            asset.Device_name = Request.Form["Device_name"];
            asset.Device_IP = Request.Form["Device_IP"];
            asset.Location = Request.Form["Location"];

            if (asset.Device_name.Length == 0 || asset.Device_IP.Length == 0 || asset.Location.Length == 0)
            {
                errorMessage = "All the Fields are Required";
                return;
            }

            try
            {
                string connectionString = "Server=tcp:rdlmssqlserver.database.windows.net,1433;Initial Catalog=rdlassetinventory;Persist Security Info=False;User ID=rdladmin;Password=rdl@12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

                using (SqlConnection connection = new(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE RDLAssetInventory SET Device_name=@Device_name, Device_IP=@Device_IP, Location=@Location WHERE id=@id";

                    using (SqlCommand command = new(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Device_name", asset.Device_name);
                        command.Parameters.AddWithValue("@Device_IP", asset.Device_IP);
                        command.Parameters.AddWithValue("@Location", asset.Location);
                        command.Parameters.AddWithValue("@id", asset.id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Assets/Index");
        }
    }
}
