#nullable disable
using System;
using System.Data.SqlClient;


namespace Digitalrestaurantorderplatform.Models
{
    public class RegisterModel
    {
        static Dictionary<string, string[]> userlist = new Dictionary<string, string[]>();

        static RegisterModel()
        {
            using (SqlConnection connection = new SqlConnection(getConnectionString()))
            {
                try
                {
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand("select * from customerlist", connection);
                    SqlDataReader sqlReader = sqlCommand.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        string[] userdata = new string[2];
                        userdata[0] = sqlReader["email"].ToString();
                        userdata[1] = sqlReader["password"].ToString();
                        userlist.Add(sqlReader["name"].ToString(), userdata);
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
        public static int signUpValidation(SignUpModel signupmodel)
        {
            string username = signupmodel.name;
            string email = signupmodel.email;
            string password = signupmodel.password;
            string cpassword = signupmodel.cpassword;
            bool usernameflag = true;
            bool emailflag = true;
            foreach (KeyValuePair<string, string[]> item in userlist)
            {
                if (string.Equals(username, item.Key))
                {
                    usernameflag = false;
                }
            }
            foreach (KeyValuePair<string, string[]> item in userlist)
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
                        userlist.Add(username, userdata);
                        return 1;
                    }
                }
                return 4;
            }
            return 2;
        }

        public static int loginValidation(LoginModel loginmodel)
        {
            string username = loginmodel.name;
            string password = loginmodel.password;
            bool loginflag = false;
            string adminUsername="admin@123";
            string adminPassword="admin@321";
            if (String.Equals(username,adminUsername) && String.Equals(password,adminPassword) )
            {
                return 3;
            }
            else
            {
                foreach (KeyValuePair<string, string[]> item in userlist)
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
        public static int ResetPassword(SignUpModel details)
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
            foreach (KeyValuePair<string, string[]> item in userlist)
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