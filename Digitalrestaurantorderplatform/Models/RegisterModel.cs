#nullable disable
using System;
using System.Data;
using System.Data.SqlClient;


namespace Digitalrestaurantorderplatform.Models
{
    class RegisterModel
    {

        Dictionary<string, string[]> userList = new Dictionary<string, string[]>();
        public RegisterModel(IConfiguration configuration)
        {
            using (SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]))
            {
                try
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand("productlist",sqlConnection);
                    sqlCommand.CommandType=CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@operation","constructor");
                    sqlCommand.Parameters.AddWithValue("@action","selectUsers");
                    SqlDataReader sqlReader = sqlCommand.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        
                        string[] userdata = new string[2];
                        userdata[0] = sqlReader["email"].ToString();
                        userdata[1] = sqlReader["password"].ToString();
                        if (!userList.ContainsKey(sqlReader["name"].ToString()))
                        {
                            userList.Add(sqlReader["name"].ToString(), userdata);
                        }
                    }
                }
                catch (SqlException sqlException)
                {
                    Console.WriteLine("Datebase error" + sqlException);
                }
                catch (SystemException systemException)
                {
                    Console.WriteLine("Error" + systemException);
                }
            }
        }

        
        internal int signUpValidation(SignUpModel signupmodel)
        {
            string username = signupmodel.name;
            string email = signupmodel.email;
            string password = signupmodel.password;
            string cpassword = signupmodel.cpassword;
            bool usernameflag = true;
            bool emailflag = true;
            foreach (KeyValuePair<string, string[]> item in userList)
            {
                if (string.Equals(username, item.Key))
                {
                    usernameflag = false;
                }
            }
            foreach (KeyValuePair<string, string[]> item in userList)
            {
                if (String.Equals(email, item.Value[0]))
                {
                    emailflag = false;
                }

            }
            if (usernameflag == true)
            {
                if (String.Equals(password, cpassword))
                {
                    if (emailflag == true)
                    {
                        string[] userdata = new string[2];
                        userdata[0] = email;
                        userdata[1] = password;
                        userList.Add(username, userdata);
                        return 1;
                    }
                }
                return 4;
            }
            return 2;
        }

        internal int loginValidation(LoginModel loginmodel,string adminUsername,string adminPassword)
        {
            
            string username = loginmodel.name;
            string password = loginmodel.password;
            bool loginflag = false;

            if (String.Equals(username,adminUsername) && String.Equals(password,adminPassword) )
            {
                return 3;
            }
            else
            {
                foreach (KeyValuePair<string, string[]> item in userList)
                {
                    if (String.Equals(username, item.Key) && String.Equals(password, item.Value[1]))
                    {
                        loginflag = true;
                    }
                }
                if (loginflag == true)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }

        }
        internal int ResetPassword(SignUpModel details)
        {
            string username = details.name;
            string email = details.email;
            string password = details.password;
            string cpassword = details.cpassword;

            bool loginflag = false;
            if (!String.Equals(password,cpassword))
            {
                return 3;
            }
            foreach (KeyValuePair<string, string[]> item in userList)
            {
                if (String.Equals(username, item.Key) && String.Equals(email, item.Value[0]))
                {
                    loginflag = true;
                }
            }
            if (loginflag == true)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
    }
}