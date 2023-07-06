using System;
using System.Data;
using System.Data.SqlClient;
#nullable disable
namespace Digitalrestaurantorderplatform.Models;

public class OrderModel
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
    public static int getDeliveryDetails(string username,OrderDetailsModel orderDetails)
    {
        bool flag=false;
        string name=orderDetails.name;
        string empId=orderDetails.empId;
        string address=orderDetails.address;
        string mobileNo=orderDetails.mobileNo;
        
        int totalPrice=ProductModel.getProductTotalPrice(username);
        List<string[]> productList=new List<string[]>();

        using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("SELECT * from cartlist where username=@value", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@value",username);
            SqlDataReader sqlDataReader=sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string[] cartItems=new string[2];
                cartItems[0]=sqlDataReader["foodname"].ToString();
                cartItems[1]=sqlDataReader["quantity"].ToString();
                productList.Add(cartItems);
            }  
        }
        using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
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
                    SqlCommand sqlCommand = new SqlCommand("insert into orderlist values(@value,@value2,@value3,@value4,@value5,@value6,@value7,@value8,@value9,@value10,@value11)", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@value",item[0]);
                    sqlCommand.Parameters.AddWithValue("@value2",Convert.ToInt16(item[1]));
                    sqlCommand.Parameters.AddWithValue("@value3",totalPrice);
                    sqlCommand.Parameters.AddWithValue("@value4",name);
                    sqlCommand.Parameters.AddWithValue("@value5",address);
                    sqlCommand.Parameters.AddWithValue("@value6",mobileNo);
                    sqlCommand.Parameters.AddWithValue("@value7",empId);
                    sqlCommand.Parameters.AddWithValue("@value8","In process");
                    sqlCommand.Parameters.AddWithValue("@value9",username);
                    sqlCommand.Parameters.AddWithValue("@value10",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    sqlCommand.Parameters.AddWithValue("@value11",randomstring);

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
            using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("delete from cartlist where username=@value", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@value",username);
                sqlCommand.ExecuteNonQuery();

            }
        }
        return 1;
    }

    public static DataTable viewOrderList(string username,string sort)
    {
        if (sort=="All")
        {
            using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * from orderlist where username=@value order by datetime desc ", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@value",username);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }  
        }
        else if (sort=="last1day")
        {
            using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * from orderlist where username=@value and (datetime between DATEADD(day,-1,GETDATE()) and datetime) order by datetime desc ", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@value",username);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }  
        }
        else
        {
            using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * from orderlist where username=@value and (datetime between DATEADD(day,-10,GETDATE()) and datetime) order by datetime desc", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@value",username);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }  
        }
    }
    public static void removeOrder(string username,string orderId,string foodname)
    {
        using (SqlConnection sqlConnection = new SqlConnection(getConnectionString()))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("delete from orderlist where username=@value and orderid=@value2 and foodname=@value3", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@value",username);
                sqlCommand.Parameters.AddWithValue("@value2",orderId);
                sqlCommand.Parameters.AddWithValue("@value3",foodname);
                sqlCommand.ExecuteNonQuery();

            }
    }

    
}

