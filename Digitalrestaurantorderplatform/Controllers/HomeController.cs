using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Digitalrestaurantorderplatform.Models;
using System.Text;
using Newtonsoft.Json;
#nullable disable

namespace Digitalrestaurantorderplatform.Controllers;
[Log]
public class HomeController : Controller
{
    private AppDbContext _database;
    private readonly string _adminUsername,_adminPassword;

    private IConfiguration _configuration;
    public HomeController(AppDbContext database,IConfiguration configuration)
   {
        _database=database;
        _configuration=configuration;
        _adminUsername=configuration["AdminLogin:AdminId"];
        _adminPassword=configuration["AdminLogin:Password"];
   }
    public class SessionNotFoundException : Exception  
    {  
        public SessionNotFoundException(String msg) : base(msg)  
        {  

        }  
    }
    static void checkSession(string session)  
    {  
        if (string.IsNullOrEmpty(session))  
        {  
            throw new SessionNotFoundException("Session not set");  
        }  
    }

    [HttpGet]
    public IActionResult Index()
    {
        try
        {
            //Setting admin session as false
            HttpContext.Session.SetString("admin","false");
            ViewBag.admin=HttpContext.Session.GetString("admin");

            string session=HttpContext.Session.GetString("user");
            //calling user define exception
            checkSession(session); 
            ViewBag.user=HttpContext.Session.GetString("user"); 
            return View();
        }
        //user define exception
        catch (SessionNotFoundException sessionnotfoundexception)
        {
            Console.WriteLine(sessionnotfoundexception);
            HttpContext.Session.SetString("user",""); 
            return View(); 
        }
    }
    [HttpPost]
    public async Task <IActionResult> Index(FeedbackModel message)
    {   
        // Admin session
        ViewBag.admin=HttpContext.Session.GetString("admin");
        //user session
        ViewBag.user=HttpContext.Session.GetString("user");
        // Adding feedback in db
        if (HttpContext.Session.GetString("user")!="")
        { 
            message.username=HttpContext.Session.GetString("user");
            HttpClient httpClient =new HttpClient();
            string apiurl="http://localhost:5163/api/Feedback";
            var jsondata=JsonConvert.SerializeObject(message);
            var data=new StringContent(jsondata, Encoding.UTF8,"application/json");

            var httpresponse=httpClient.PostAsync(apiurl,data);
            var result=await httpresponse.Result.Content.ReadAsStringAsync();
            if (result=="true")
            {
                ViewBag.feedback="Feedback send successfully";
                return View();
            }
            return View();
        }
        else
        {
            return RedirectToAction("Login","Home");
        }
    }

    
    [HttpGet]
    public IActionResult Login()
    {
        HttpContext.Session.SetString("admin","false");
        ViewBag.admin=HttpContext.Session.GetString("admin");

        HttpContext.Session.SetString("user","");
        return View();
        
    }
    [HttpPost]
    public IActionResult Login(LoginModel login)
    {
        ViewBag.admin=HttpContext.Session.GetString("admin");
        RegisterModel registerModel=new RegisterModel(_configuration);
        int check=registerModel.loginValidation(login,_adminUsername,_adminPassword);
        if(check==1)
        {
            HttpContext.Session.SetString("user",login.name);
            return RedirectToAction("Index","Home");    
        }
        else if(check==2)
        {
            ViewBag.message="*Check username and password:)";
            return View("Login");
        }
        else if(check==3)
        {
            //Setting admin session true
            HttpContext.Session.SetString("admin","true");
            ViewBag.admin=HttpContext.Session.GetString("admin");

            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(10);
            Response.Cookies.Append("LastLoginTime",DateTime.Now.ToString(),cookieOptions);
            TempData["Time"]="Last Login:"+Request.Cookies["LastLoginTime"];

            return RedirectToAction("AdminIndex","Admin");
        }
        else
        {
            return View();
        }
    }
    [HttpGet]
    public IActionResult SignUp()
    {
        
        ViewBag.admin=HttpContext.Session.GetString("admin");
        return View();
    }
    [HttpPost]
     public IActionResult SignUp(SignUpModel register)
     {
        ViewBag.admin=HttpContext.Session.GetString("admin");

        RegisterModel registerModel=new RegisterModel(_configuration);
        int check;
        check=registerModel.signUpValidation(register);
        if(check == 1)
        {
            
            return RedirectToAction("UserDetails",register);
        }
        else if(check == 2)
        {
            ViewBag.message="*Check password";
            return View("SignUp");
        }
        else if ( check == 3)
        {
            
            ViewBag.message="*Check username";
            return View("SignUp");
        }
        else if ( check == 4)
        {
            ViewBag.message="*You already used this email";
            return View("SignUp");
        }
        else
        {
            return View();

        }
     }
     public IActionResult UserDetails(SignUpModel register)

     {
        ViewBag.admin=HttpContext.Session.GetString("admin");
        //Adding user accounts in EF table 
        _database.customerlist.Add(register);
        _database.SaveChanges();
        ViewBag.created="Account created successfully:)";
        return View("SignUp");
     }

    public IActionResult ForgotPassword()
    {
        ViewBag.admin=HttpContext.Session.GetString("admin");
        return View();
    }
    [HttpPost]
    public IActionResult ForgotPassword(SignUpModel details)
    {
        RegisterModel registerModel=new RegisterModel(_configuration);
        int check=registerModel.ResetPassword(details);
        if(check==1)
        {
            var profile=_database.customerlist.Find(details.name);
            profile.password=details.password;
            _database.customerlist.Update(profile);
            _database.SaveChanges();
            ViewBag.message="Password changed Successfully";
            return RedirectToAction("Login");
        }
        else if(check==3)
        {
            ViewBag.message="Password and conform password are not same";
            return View();
        }
        else
        {
           ViewBag.message="check email and username";
            return View();
        }
        
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}