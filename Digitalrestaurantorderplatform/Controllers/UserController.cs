using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Digitalrestaurantorderplatform.Models;
using System.Data;
#nullable disable

namespace Digitalrestaurantorderplatform.Controllers;

[Log]
public class UserController:Controller
{
    public class DuplicateItemFoundException : Exception  
    {  
        public DuplicateItemFoundException(String msg) : base(msg)  
        {  
        }  
    }
    static void checkCart(int check)  
    {  
        if (check==2)  
        {  
            throw new DuplicateItemFoundException("Duplicate item found item not added into database");  
        }  
    }

    private IConfiguration _configuration;
    public UserController(IConfiguration configuration)
    {
        _configuration=configuration;
    }
    [HttpGet]
    public IActionResult Shop(string filterItem)
    {
        ViewBag.admin=HttpContext.Session.GetString("admin");
        
        if (HttpContext.Session.GetString("user")!="")
        {
            ViewBag.user=HttpContext.Session.GetString("user");
            
        }
        else
        {
            HttpContext.Session.SetString("user","");
        }
        ProductModel productModel=new ProductModel(_configuration);
        DataTable dataTable=productModel.getProductDetails(filterItem);
        return View(dataTable);
        
        
    }
    
    public IActionResult addCart(string addCart)
    {
        if (HttpContext.Session.GetString("user")=="")
        {
            return RedirectToAction("Login","Home");   
        }
        else
        {
            ViewBag.user=HttpContext.Session.GetString("user");
            
            string username=HttpContext.Session.GetString("user");
            ProductModel productModel=new ProductModel(_configuration);
            int check=productModel.addCartItems(addCart,username);
            try
            {
                checkCart(check);
                return RedirectToAction("Shop");
            }
            catch (DuplicateItemFoundException duplicateitemfoundexception)
            {
                Console.WriteLine(duplicateitemfoundexception);
                return RedirectToAction("Shop");
            } 
        } 
    }
    public IActionResult Cart(string foodname)
    {
        
        ViewBag.admin=HttpContext.Session.GetString("admin");

        if(HttpContext.Session.GetString("user")!="")
        {
            ViewBag.user=HttpContext.Session.GetString("user");
            string username=HttpContext.Session.GetString("user").ToString();
            ProductModel productModel=new ProductModel(_configuration);
            ViewBag.totalPrice=productModel.getTotalPrice(username);
            DataTable datatable=productModel.getCartList(username);
            return View(datatable);
        }
        else
        {
            return RedirectToAction("Login","Home");
        }

    }
    public IActionResult removeCart(string foodname)
    {
        ViewBag.user=HttpContext.Session.GetString("user");
        string username=HttpContext.Session.GetString("user").ToString();
        ProductModel productModel=new ProductModel(_configuration);
        productModel.deleteCart(foodname,username);
        return RedirectToAction("Cart");
    }
    public IActionResult modifyQuantity(string foodname,string operation)
    {
        ViewBag.user=HttpContext.Session.GetString("user");
        string username=HttpContext.Session.GetString("user").ToString();
        if (operation=="minus")
        {
            ProductModel productModel=new ProductModel(_configuration);
            productModel.decreaseQuantity(foodname,username);
        }
        else
        {
            ProductModel productModel=new ProductModel(_configuration);
            productModel.increaseQuantity(foodname,username);
        }
        return RedirectToAction("Cart");
    }
    [HttpGet]
    public IActionResult DeliveryDetails()
    {
        ViewBag.admin=HttpContext.Session.GetString("admin");

        ViewBag.user=HttpContext.Session.GetString("user");
        return View();
    }
    [HttpPost]
    public IActionResult DeliveryDetails(OrderDetailsModel orderdetails)
    {
        ViewBag.user=HttpContext.Session.GetString("user");
        ViewBag.admin=HttpContext.Session.GetString("admin");
        string username=HttpContext.Session.GetString("user").ToString();
        OrderModel orderModel=new OrderModel(_configuration);
        int check=orderModel.getDeliveryDetails(username,orderdetails);
        if (check==1)
        {
            return RedirectToAction("ViewOrder");
        }
        else
        {
            return View();
        }
        
    }
    [HttpGet]
    public IActionResult ViewOrder(string sort)
    {
        ViewBag.admin=HttpContext.Session.GetString("admin");

        string name=HttpContext.Session.GetString("user");
        ViewBag.user=HttpContext.Session.GetString("user");
        OrderModel orderModel=new OrderModel(_configuration);
        DataTable datatable=orderModel.viewOrderList(name,sort);
        return View(datatable);
    }
    

    public IActionResult CancelOrder(string orderId,string foodname)
    {
        string name=HttpContext.Session.GetString("user");
        OrderModel orderModel=new OrderModel(_configuration);
        orderModel.removeOrder(name,orderId,foodname);
        return RedirectToAction("ViewOrder");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}