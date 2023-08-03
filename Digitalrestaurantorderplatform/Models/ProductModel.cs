#nullable disable
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;


namespace Digitalrestaurantorderplatform.Models;

class ProductModel
{
    private readonly string connectionString;
    internal ProductModel(IConfiguration configuration)
    {
        connectionString=configuration["ConnectionStrings:DefaultConnection"];
    }
    internal DataTable getProductDetails(string filterItem)
    {

        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            sqlConnection.Open();
            if(filterItem==null)
            {
                
                SqlCommand sqlCommand = new SqlCommand("productlist", sqlConnection);

                sqlCommand.CommandType=CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@filterItem","null");
                sqlCommand.Parameters.AddWithValue("@operation","displayItem");
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);   
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
            else if(filterItem=="all")
            {
                SqlCommand sqlCommand = new SqlCommand("productlist", sqlConnection);
                
                sqlCommand.CommandType=CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@operation","displayItem");
                sqlCommand.Parameters.AddWithValue("@filterItem","all");
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);   
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
            else
            {
                SqlCommand sqlCommand = new SqlCommand("productlist", sqlConnection);
                
                sqlCommand.CommandType=CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@operation","displayItem");
                sqlCommand.Parameters.AddWithValue("@filterItem",filterItem);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);   
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }  
        }

    }
    internal int addCartItems(string foodName,string name)
    {
        MenuModel menuModel=new MenuModel();
        try
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {    
                
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("productlist",sqlConnection);
                sqlCommand.CommandType=CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@action","selectMenu");
                sqlCommand.Parameters.AddWithValue("@operation","getItemForCart");
                sqlCommand.Parameters.AddWithValue("@foodName",foodName);
                SqlDataReader sqlDataReader= sqlCommand.ExecuteReader();
                while(sqlDataReader.Read())
                {
                    menuModel.foodname=sqlDataReader[0].ToString();
                    menuModel.amount=(int)sqlDataReader[1];
                    menuModel.image=(byte[])sqlDataReader[3];   
                }
            }
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {    
                
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("productlist",sqlConnection);
                sqlCommand.CommandType=CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@operation","getItemForCart");
                sqlCommand.Parameters.AddWithValue("@action","selectCartList");
                sqlCommand.Parameters.AddWithValue("@foodName",menuModel.foodname);
                sqlCommand.Parameters.AddWithValue("@name",name);
                SqlDataReader sqlDataReader= sqlCommand.ExecuteReader();
                while(sqlDataReader.HasRows)
                {
                    return 2;
                }
            }
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand=new SqlCommand("productlist",sqlConnection);
                sqlCommand.CommandType=CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@action","insertCartList");
                sqlCommand.Parameters.AddWithValue("@operation","getItemForCart");
                sqlCommand.Parameters.AddWithValue("@foodName",menuModel.foodname);
                sqlCommand.Parameters.AddWithValue("@amount",menuModel.amount);
                sqlCommand.Parameters.AddWithValue("@quantity",1);
                sqlCommand.Parameters.AddWithValue("@name",name);
                sqlCommand.Parameters.AddWithValue("@image",menuModel.image);
                sqlCommand.ExecuteNonQuery();
                return 1;
            }
        }
        catch (SqlException)
        {  
            Console.WriteLine("Error in adding item");
            return 0;
        }
        catch (SystemException systemException)
        {
            Console.WriteLine("Error" + systemException);
            return 0;

        }
        
    }
    internal DataTable getCartList(string name)
    {
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("EditCartList", sqlConnection);
            sqlCommand.CommandType=CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@operation","getCartList");
            sqlCommand.Parameters.AddWithValue("@action","selectCartList");
            sqlCommand.Parameters.AddWithValue("@name",name);
            
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        } 
    }
    internal void deleteCart(string foodname,string name)
    {
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand=new SqlCommand("EditCartList", sqlConnection);
            sqlCommand.CommandType=CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@action","deleteCartList");
            sqlCommand.Parameters.AddWithValue("@operation","deleteCart");
            sqlCommand.Parameters.AddWithValue("@foodName",foodname);
            sqlCommand.Parameters.AddWithValue("@name",name);
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException sqlexception)
            {
                Console.WriteLine("Database error in deleting item"+ sqlexception);
            }
        }
    }
    internal void increaseQuantity(string foodname,string name)
    {
        try
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand=new SqlCommand("EditCartList", sqlConnection);
                sqlCommand.CommandType=CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@action","updateCartList");
                sqlCommand.Parameters.AddWithValue("@operation","increaseQuantity");
                sqlCommand.Parameters.AddWithValue("@foodName",foodname);
                sqlCommand.Parameters.AddWithValue("@name",name);
                sqlCommand.ExecuteNonQuery();
            }
        }
        catch (SystemException)
        {
            Console.WriteLine("Query not found");
        }
        
    }
    internal void decreaseQuantity(string foodname,string name)
    {
        int count=0;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand=new SqlCommand("EditCartList", sqlConnection);
            sqlCommand.CommandType=CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@action","selectCartList");
            sqlCommand.Parameters.AddWithValue("@operation","decreaseQuantity");
            sqlCommand.Parameters.AddWithValue("@foodName",foodname);
            sqlCommand.Parameters.AddWithValue("@name",name);
            SqlDataReader sqlDataReader= sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                count=Convert.ToInt32(sqlDataReader["quantity"].ToString());
            }
        }
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            if(count>1)
            {
                sqlConnection.Open();
                SqlCommand sqlCommand=new SqlCommand("EditCartList", sqlConnection);
                sqlCommand.CommandType=CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@action","updateCartList");
                sqlCommand.Parameters.AddWithValue("@operation","decreaseQuantity");
                sqlCommand.Parameters.AddWithValue("@foodName",foodname);
                sqlCommand.Parameters.AddWithValue("@name",name);
                sqlCommand.ExecuteNonQuery();
            }
        }    
    }
    internal int getTotalPrice(string username)
    {
        int count=0;
        try
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand=new SqlCommand("EditCartList", sqlConnection);
                sqlCommand.CommandType=CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@action","selectCartList");
                sqlCommand.Parameters.AddWithValue("@operation","getTotalPrice");
                sqlCommand.Parameters.AddWithValue("@name",username);
                count = Convert.ToInt32(sqlCommand.ExecuteScalar()); 
            }
            return count;
        }
        catch (SqlException sqlexception)
        {
            Console.WriteLine("Error in fetching total price"+ sqlexception);
            return 0;
        }
        catch(SystemException)
        {
            Console.WriteLine("No item in cart");
            return 0;
        }
        
    }
    // internal int getProductTotalPrice(string username)
    // {
    //     int count=0;
    //     try
    //     {
    //         using (SqlConnection sqlConnection = new SqlConnection(connectionString))
    //         {
    //             sqlConnection.Open();
    //             SqlCommand sqlCommand=new SqlCommand("EditCartList", sqlConnection);
    //             sqlCommand.CommandType=CommandType.StoredProcedure;
    //             sqlCommand.Parameters.AddWithValue("@action","selectCartList");
    //             sqlCommand.Parameters.AddWithValue("@operation","getProductTotalPrice");
    //             sqlCommand.Parameters.AddWithValue("@name",username);
    //             count = Convert.ToInt32(sqlCommand.ExecuteScalar());
    //         }
    //         return count;
    //     }
    //     catch (SystemException)
    //     { 
    //         Console.WriteLine("Error in fetching product total price");
    //         return 0;
    //     }
        
    // }
    
}
