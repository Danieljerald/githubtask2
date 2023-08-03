using System;
using System.Data;
using System.Data.SqlClient;
#nullable disable
namespace Digitalrestaurantorderplatform.Models;

class OrderModel
{
    private readonly string _connectionString;
    private IConfiguration _configuration;
    internal OrderModel(IConfiguration configuration)
    {
        _configuration=configuration;
        _connectionString=configuration["ConnectionStrings:DefaultConnection"];
    }
    internal int getDeliveryDetails(string userName,OrderDetailsModel orderDetails)
    {
        bool flag=false;
        string name=orderDetails.name;
        string empId=orderDetails.empId;
        string address=orderDetails.address;
        string mobileNo=orderDetails.mobileNo;
        
        List<string[]> productList=new List<string[]>();

        using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("EditCartList", sqlConnection);
            sqlCommand.CommandType=CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@operation","getCartList");
            sqlCommand.Parameters.AddWithValue("@action","selectCartList");
            sqlCommand.Parameters.AddWithValue("@name",userName);
            SqlDataReader sqlDataReader=sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string[] cartItems=new string[2];
                cartItems[0]=sqlDataReader["foodname"].ToString();
                cartItems[1]=sqlDataReader["quantity"].ToString();
                productList.Add(cartItems);
            }  
        }
        ProductModel productModel=new ProductModel(_configuration);
        int totalPrice=productModel.getTotalPrice(userName);
        using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
        {
            try
            {
                Random random = new Random();
                String str = "abcdefghijklmnopqrstuvwxyz0123456789";
                String randomstring = "";
                    for (int i = 0; i < 8; i++)
                    {
                        // Selecting a index randomly
                        int x = random.Next(str.Length);
                        randomstring = randomstring + str[x];
                    }
                
                foreach(string[] item in productList)
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand("OrderListStoredProcedure", sqlConnection);
                    sqlCommand.CommandType=CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@operation","getDeliveryDetails");
                    sqlCommand.Parameters.AddWithValue("@action","insertOrderList");
                    sqlCommand.Parameters.AddWithValue("@foodName",item[0]);
                    sqlCommand.Parameters.AddWithValue("@quantity",Convert.ToInt16(item[1]));
                    sqlCommand.Parameters.AddWithValue("@totalPrice",totalPrice);
                    sqlCommand.Parameters.AddWithValue("@name",name);
                    sqlCommand.Parameters.AddWithValue("@address",address);
                    sqlCommand.Parameters.AddWithValue("@mobile",mobileNo);
                    sqlCommand.Parameters.AddWithValue("@empid",empId);
                    sqlCommand.Parameters.AddWithValue("@status","In process");
                    sqlCommand.Parameters.AddWithValue("@userName",userName);
                    sqlCommand.Parameters.AddWithValue("@dateTime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    sqlCommand.Parameters.AddWithValue("@orderId",randomstring);

                    sqlCommand.ExecuteNonQuery(); 
                    sqlConnection.Close();
                }

                flag=true;
            }
            catch (SqlException sqlexception)
            {
                
                Console.WriteLine("Order didn't placed"+ sqlexception);
                return 0;
            }
            
        }
        if (flag)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("EditCartList", sqlConnection);
                sqlCommand.CommandType=CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@operation","getDeliveryDetails");
                sqlCommand.Parameters.AddWithValue("@action","deleteCartList");
                sqlCommand.Parameters.AddWithValue("@name",userName);
                sqlCommand.ExecuteNonQuery();

            }
        }
        return 1;
    }

    internal DataTable viewOrderList(string username,string sort)
    {
        if (sort=="All")
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("OrderListStoredProcedure", sqlConnection);
                sqlCommand.CommandType=CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@operation","viewOrderList");
                sqlCommand.Parameters.AddWithValue("@action","selectAllOrderList");
                sqlCommand.Parameters.AddWithValue("@userName",username);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }  
        }
        else if (sort=="last1day")
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("OrderListStoredProcedure", sqlConnection);
                sqlCommand.CommandType=CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@operation","viewOrderList");
                sqlCommand.Parameters.AddWithValue("@action","selectOneDay");
                sqlCommand.Parameters.AddWithValue("@userName",username);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }  
        }
        else
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("OrderListStoredProcedure", sqlConnection);
                sqlCommand.CommandType=CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@operation","viewOrderList");
                sqlCommand.Parameters.AddWithValue("@action","selectTenDays");
                sqlCommand.Parameters.AddWithValue("@userName",username);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }  
        }
    }
    internal void removeOrder(string username,string orderId,string foodname)
    {
        using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();               
                SqlCommand sqlCommand = new SqlCommand("OrderListStoredProcedure", sqlConnection);
                sqlCommand.CommandType=CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@operation","removeOrder");
                sqlCommand.Parameters.AddWithValue("@action","deleteOrder");
                sqlCommand.Parameters.AddWithValue("@userName",username);
                sqlCommand.Parameters.AddWithValue("@orderId",orderId);
                sqlCommand.Parameters.AddWithValue("@foodName",foodname);
                sqlCommand.ExecuteNonQuery();

            }
    }

    
}

