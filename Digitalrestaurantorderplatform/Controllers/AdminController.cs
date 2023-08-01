using System.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Digitalrestaurantorderplatform.Models;
namespace Digitalrestaurantorderplatform.Controllers;
using System.Net.Http;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

#nullable disable

[Log]
public class AdminController : Controller
{
    private AppDbContext _database;
    private IConfiguration _configuration;
   public AdminController(AppDbContext database,IConfiguration configuration)
   {
         _database=database;
         _configuration=configuration;
   }


    public IActionResult AdminIndex()
    {
        List<SignUpModel> userList=_database.customerlist.ToList();
        ViewBag.customerList=userList;
        var customerlist=_database.customerlist.ToList();
        int count=customerlist.Count;
        ViewBag.count=count;

        HttpClient httpclient=new HttpClient();
        string apiurl="http://localhost:5163/api/Feedback";
        var apiresponse=httpclient.GetAsync(apiurl).Result;
        var listoffeedback=apiresponse.Content.ReadAsAsync<IEnumerable<FeedbackModel>>().Result;
        return View(listoffeedback);

    }
    public IActionResult clearFeedbacks()
    {
        HttpClient httpclient=new HttpClient();
        string apiurl="http://localhost:5163/api/Feedback";
        var apiresponse=httpclient.DeleteAsync(apiurl).Result;
        return RedirectToAction("AdminIndex","Admin");
    }
    public IActionResult ManageOrder(string orderid , string orderstatus)
    {
        AdminModel adminModel=new AdminModel(_configuration);
        if (orderid!=null && orderstatus!=null)
        {
            if (orderstatus=="dispatched")
            {
                adminModel.changeStatus(orderid);
            }
            else
            {
                adminModel.updateStatus(orderid);  
            }
        }
        DataTable datatable=adminModel.getOrderList(); 
        return View(datatable);
    }
    [HttpGet]
    public IActionResult ManageProduct()
    {
        return View();
    }
    [HttpPost]
    public IActionResult ManageProduct(MenuModel menu)
    {
        foreach (var file in Request.Form.Files)
        {
            MemoryStream memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            menu.image = memoryStream.ToArray();
        }
        AdminModel adminModel=new AdminModel(_configuration);
        adminModel.updateMenu(menu);
        return RedirectToAction("Menu");
    }
    [HttpGet]
    public IActionResult Menu(string visibility,int productid)
    {
        AdminModel adminModel=new AdminModel(_configuration);
        if (visibility!=null)
        {  
            adminModel.updateVisibility(visibility,productid);
        }
        DataTable datatable=adminModel.viewMenu();
        return View(datatable);
    }
    public IActionResult MenuDelete(int productid)
    {
        AdminModel adminModel=new AdminModel(_configuration);
        adminModel.deleteFood(productid);
        return RedirectToAction("Menu");
    }
    public IActionResult DeliveryReport()
    {
        AdminModel adminModel=new AdminModel(_configuration);
        DataTable datatable=adminModel.fetchReport();
        return View(datatable);
    }
    public ActionResult DownloadReport(string empname)
    {
        try
        {
            AdminModel adminModel=new AdminModel(_configuration);
            DataTable datatable =adminModel.downloadReport(empname);

            var memoryStream = new MemoryStream();
            var document = new Document(PageSize.A4, 50, 50, 25, 25);
            var writer = PdfWriter.GetInstance(document, memoryStream);
          
            document.Open();
            var table = new PdfPTable(6);

            table.AddCell("Foodname");
            table.AddCell("Quantity");
            table.AddCell("Total price");
            table.AddCell("Username");
            table.AddCell("Date and Time");
            table.AddCell("Order Id");

            foreach (DataRow item in datatable.Rows)
            {
                table.AddCell(item[0].ToString());
                table.AddCell(item[1].ToString());
                table.AddCell(item[2].ToString());
                table.AddCell(item[8].ToString());
                table.AddCell(item[9].ToString());
                table.AddCell(item[10].ToString());

            }

            document.Add(table);
            document.Close();

            byte[] pdfBytes = memoryStream.ToArray();

            return new FileContentResult(pdfBytes, "application/pdf");
        }
        catch (Exception)
        {
            // Handle the exception
            return Content("Unable to Generate pdf");
        } 
    }
}