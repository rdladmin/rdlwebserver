using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace AssetInventory.Pages.Assets
{
    public class CreateModel : PageModel
    {
        public Asset asset = new();
        public String errorMessage = "";
        public String SuccessMessage = "";

        public void OnGet()
        {
        }

        public void OnPost() 
        {
            asset.Device_name = Request.Form["Device_name"];
            asset.Device_IP = Request.Form["Device_IP"];
            asset.Location = Request.Form["location"];

            if (asset.Device_name.Length == 0 || asset.Device_IP.Length == 0 || asset.Location.Length ==0)
            {
                errorMessage = "All the Fields are Required";
                return;
            }

            //Save the new asset into database
            try
            {
                string connectionString = "Server=tcp:rdlmssqlserver.database.windows.net,1433;Initial Catalog=rdlassetinventory;Persist Security Info=False;User ID=rdladmin;Password=rdl@12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO RDLAssetInventory" +
                        "(Device_name,Device_IP,Location) VALUES" +
                        "(@Device_name,@Device_IP,@Location);";
                    using (SqlCommand command = new(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Device_name", asset.Device_name);
                        command.Parameters.AddWithValue("@Device_IP", asset.Device_IP);
                        command.Parameters.AddWithValue("@Location", asset.Location);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            asset.Device_name = ""; asset.Device_IP = ""; asset.Location = "";
            SuccessMessage = "New Asset Added Succesfully";

            Response.Redirect("/Assets/Index");
        }
    }
}
