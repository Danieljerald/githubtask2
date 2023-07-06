using System;
using System.Data;
using System.Data.SqlClient;
#nullable disable
namespace Digitalrestaurantorderplatform.Models;

public class AdminModel
{
    public static String getConnectionString()
        {
             var build = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfiguration configuration = build.Build();

            String connectionString = Convert.ToString(configuration.GetConnectionString("DefaultConnection"));
            if(connectionString != null)
                return connectionString;
            return "";
        }
    //thread method
    static void Print()
    {
        Console.WriteLine("Changes successful");
    }
     public static DataTable getOrderList()
    {
        using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("SELECT * from orderlist order by datetime ", sqlConnection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        } 

    }
    public static void changeStatus(string orderid)
    {
        using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("update orderlist set status='Dispatched' where orderid=@value", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@value",orderid);
            sqlCommand.ExecuteNonQuery();
        }
    } 
    public static void updateStatus(string orderid)
    {
        using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("update orderlist set status='Delivered' where orderid=@value", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@value",orderid);
            sqlCommand.ExecuteNonQuery();
        }
    }
    public static void updateMenu(MenuModel menu)
    {
        
        try
        {           
            using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("insert into menu values(@value,@value2,@value3,@value4,@value5,@value6)", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@value",menu.foodname);
                sqlCommand.Parameters.AddWithValue("@value2",menu.amount);
                sqlCommand.Parameters.AddWithValue("@value3",menu.caption);
                sqlCommand.Parameters.AddWithValue("@value4",menu.image);
                sqlCommand.Parameters.AddWithValue("@value5",menu.categories);
                sqlCommand.Parameters.AddWithValue("@value6","visible");
                sqlCommand.ExecuteNonQuery();
            }
        }
        catch (SqlException)
        {
            
            Console.WriteLine("values not entered");
        }    
        
        
    }
    public static DataTable viewMenu()
    {          
        using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("select * from menu", sqlConnection);
            DataTable dataTable=new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }
    }
    public static void updateVisibility(string visibility,int productid)
    {
        using (SqlConnection sqlConnection = new SqlConnection("Data source=ASPIRE1528; Database = userdetails; Integrated security=SSPI"))
        {
            //creating thread
            Thread thread = new Thread(new ThreadStart(Print));
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("update menu set status=@value where productid=@value2", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@value",visibility);
            sqlCommand.Parameters.AddWithValue("@value2",productid);
            sqlCommand.ExecuteNonQuery();
            //thread start here
            thread.Start();
        }

    }
    public static void deleteFood(int productid)
    {
        using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("delete from menu where productid=@value", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@value",productid);
            sqlCommand.ExecuteNonQuery();
        }

    }
    public static DataTable fetchReport()
    {
        using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("select * from orderlist where status ='Delivered' and  (datetime between DATEADD(day,-30,GETDATE()) and datetime) order by datetime desc", sqlConnection);
            SqlDataAdapter sqlDataAdapter=new SqlDataAdapter(sqlCommand);
            DataTable dataTable=new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }
    }
    public static DataTable downloadReport(string empname)
    {

        using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("select * from orderlist where status ='Delivered'and username=@value  and  (datetime between DATEADD(day,-30,GETDATE()) and datetime) order by datetime desc", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@value",empname);
            SqlDataAdapter sqlDataAdapter=new SqlDataAdapter(sqlCommand);
            DataTable dataTable=new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }
    }

}