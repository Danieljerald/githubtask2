using System;
using System.Data;
using System.Data.SqlClient;
#nullable disable
using Digitalrestaurantorderplatform.IRepository;
using Digitalrestaurantorderplatform.Models;
namespace Digitalrestaurantorderplatform.Repository;
class AdminModel:IAdminModel
{
    private readonly string _connectionString;
    public AdminModel(IConfiguration configuration)
    {
        _connectionString=configuration["ConnectionStrings:DefaultConnection"];
    }
    //thread method
    static void Print()
    {
        Console.WriteLine("Changes successful");
    }
    public DataTable getOrderList()
    {
        using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("OrderListStoredProcedure", sqlConnection);
            sqlCommand.CommandType=CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@operation","getOrderList");
            sqlCommand.Parameters.AddWithValue("@action","selectOrderList");
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        } 

    }
    public void changeStatus(string orderid)
    {
        using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("AdminModel", sqlConnection);
            sqlCommand.CommandType=CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@operation","changeStatus");
            sqlCommand.Parameters.AddWithValue("@action","updateOrderList");
            sqlCommand.Parameters.AddWithValue("@orderId",orderid);
            sqlCommand.ExecuteNonQuery();
        }
    } 
    public void updateStatus(string orderid)
    {
        using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("AdminModel", sqlConnection);
            sqlCommand.CommandType=CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@operation","updateStatus");
            sqlCommand.Parameters.AddWithValue("@action","updateToDelivered");
            sqlCommand.Parameters.AddWithValue("@orderId",orderid);
            sqlCommand.ExecuteNonQuery();
        }
    }
    public void updateMenu(MenuModel menu)
    {
        
        try
        {           
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("AdminModel", sqlConnection);
                sqlCommand.CommandType=CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@operation","updateMenu");
                sqlCommand.Parameters.AddWithValue("@action","insertMenu");
                sqlCommand.Parameters.AddWithValue("@foodName",menu.foodname);
                sqlCommand.Parameters.AddWithValue("@amount",menu.amount);
                sqlCommand.Parameters.AddWithValue("@caption",menu.caption);
                sqlCommand.Parameters.AddWithValue("@image",menu.image);
                sqlCommand.Parameters.AddWithValue("@categories",menu.categories);
                sqlCommand.Parameters.AddWithValue("@status","visible");
                sqlCommand.ExecuteNonQuery();
            }
        }
        catch (SqlException)
        {
            
            Console.WriteLine("values not entered");
        }    
        
        
    }
    public DataTable viewMenu()
    {          
        using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("productlist", sqlConnection);
            sqlCommand.CommandType=CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@operation","viewMenu");
            sqlCommand.Parameters.AddWithValue("@action","selectMenu");
            DataTable dataTable=new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }
    }
    public void updateVisibility(string visibility,int productid)
    {
        using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
        {
            //creating thread
            Thread thread = new Thread(new ThreadStart(Print));
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("AdminModel", sqlConnection);
            sqlCommand.CommandType=CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@operation","updateVisibility");
            sqlCommand.Parameters.AddWithValue("@action","updateAdmin");
            sqlCommand.Parameters.AddWithValue("@status",visibility);
            sqlCommand.Parameters.AddWithValue("@productId",productid);
            sqlCommand.ExecuteNonQuery();
            //thread start here
            thread.Start();
        }

    }
    public void deleteFood(int productid)
    {
        using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("AdminModel", sqlConnection);
            sqlCommand.CommandType=CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@operation","deleteFood");
            sqlCommand.Parameters.AddWithValue("@action","deleteMenu");
            sqlCommand.Parameters.AddWithValue("@productId",productid);
            sqlCommand.ExecuteNonQuery();
        }

    }
    public DataTable fetchReport()
    {
        using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("AdminModel", sqlConnection);
            sqlCommand.CommandType=CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@operation","fetchReport");
            sqlCommand.Parameters.AddWithValue("@action","getReport");
            SqlDataAdapter sqlDataAdapter=new SqlDataAdapter(sqlCommand);
            DataTable dataTable=new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }
    }
    public DataTable downloadReport(string empname)
    {

        using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("AdminModel", sqlConnection);
            sqlCommand.CommandType=CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@operation","downloadReport");
            sqlCommand.Parameters.AddWithValue("@action","getReport");
            sqlCommand.Parameters.AddWithValue("@userName",empname);
            SqlDataAdapter sqlDataAdapter=new SqlDataAdapter(sqlCommand);
            DataTable dataTable=new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }
    }

}