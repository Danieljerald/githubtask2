#nullable disable
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;


namespace Digitalrestaurantorderplatform.Models;

public class ProductModel
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
        
    
    public static DataTable getProductDetails(string filterItem)
    {

        using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
        {
            sqlConnection.Open();
            if(filterItem==null)
            {
                
                SqlCommand sqlCommand = new SqlCommand("SELECT * from menu where status='visible' ", sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);   
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
            else if(filterItem=="*")
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT * from menu where status='visible' ", sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);   
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
            else
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT * from menu where categories=@value and status='visible' ", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@value",filterItem);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);   
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }  
        }

    }
    public static int addCartItems(string addCart,string name)
    {
        MenuModel menuModel=new MenuModel();
        try
        {
            using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
            {    
                
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("select * from menu where foodname=@value",sqlConnection);
                sqlCommand.Parameters.AddWithValue("@value",addCart);
                SqlDataReader sqlDataReader= sqlCommand.ExecuteReader();
                while(sqlDataReader.Read())
                {
                    menuModel.foodname=sqlDataReader[0].ToString();
                    menuModel.amount=(int)sqlDataReader[1];
                    menuModel.image=(byte[])sqlDataReader[3];   
                }
            }
            using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
            {    
                
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("select * from cartlist where foodname=@value and username=@value2",sqlConnection);
                sqlCommand.Parameters.AddWithValue("@value",menuModel.foodname);
                sqlCommand.Parameters.AddWithValue("@value2",name);
                SqlDataReader sqlDataReader= sqlCommand.ExecuteReader();
                while(sqlDataReader.HasRows)
                {
                    return 2;
                }
            }
            using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand=new SqlCommand("insert into cartlist values(@value,@value2,@value3,@value4,@value5)",sqlConnection);
                sqlCommand.Parameters.AddWithValue("@value",menuModel.foodname);
                sqlCommand.Parameters.AddWithValue("@value7",menuModel.foodname);
                sqlCommand.Parameters.AddWithValue("@value2",menuModel.amount);
                sqlCommand.Parameters.AddWithValue("@value3",1);
                sqlCommand.Parameters.AddWithValue("@value4",name);
                sqlCommand.Parameters.AddWithValue("@value5",menuModel.image);
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
    public static DataTable getCartList(string name)
    {
        using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("SELECT * from cartlist where username=@value", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@value",name);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        } 
    }
    public static void deleteCart(string foodname,string name)
    {
        using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand=new SqlCommand("delete from cartlist where foodname= @value and username=@value2", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@value",foodname);
            sqlCommand.Parameters.AddWithValue("@value2",name);
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
    public static void increaseQuantity(string foodname,string name)
    {
        try
        {
            using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand=new SqlCommand("update cartlist set quantity=quantity+1 where username=@value and foodname=@value2 ", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@value",name);
                sqlCommand.Parameters.AddWithValue("@value2",foodname);
                sqlCommand.ExecuteNonQuery();
            }
        }
        catch (SystemException)
        {
            Console.WriteLine("Query not found");
        }
        
    }
    public static void decreaseQuantity(string foodname,string name)
    {
        int count=0;
        using (SqlConnection sqlConnection = new SqlConnection("Data source=ASPIRE1528; Database = userdetails; Integrated security=SSPI"))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand=new SqlCommand("select quantity from cartlist where username=@value and foodname=@value2", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@value",name);
            sqlCommand.Parameters.AddWithValue("@value2",foodname);
            SqlDataReader sqlDataReader= sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                count=Convert.ToInt32(sqlDataReader["quantity"].ToString());
            }
        }
        using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
        {
            if(count>1)
            {
                sqlConnection.Open();
                SqlCommand sqlCommand=new SqlCommand("update cartlist set quantity=quantity-1 where username=@value and foodname=@value2 ", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@value",name);
                sqlCommand.Parameters.AddWithValue("@value2",foodname);
                sqlCommand.ExecuteNonQuery();
            }
        }    
    }
    public static int getTotalPrice(string username)
    {
        int count=0;
        try
        {
            using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand=new SqlCommand("select sum(amount*quantity) from cartlist where username=@value", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@value",username);
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
    public static int getProductTotalPrice(string username)
    {
        int count=0;
        try
        {
            using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand=new SqlCommand("select amount*quantity from cartlist where username=@value", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@value",username);
                count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            }
            return count;
        }
        catch (SystemException)
        { 
            Console.WriteLine("Error in fetching product total price");
            return 0;
        }
        
    }
    
}
