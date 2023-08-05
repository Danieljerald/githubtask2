using System.Data;
using Digitalrestaurantorderplatform.Models;
namespace Digitalrestaurantorderplatform.IRepository;

public interface IAdminModel
{
    DataTable getOrderList();
    void changeStatus(string orderid);
    void updateStatus(string orderid);
    void updateMenu(MenuModel menu);
    DataTable viewMenu();
    void updateVisibility(string visibility,int productid);
    void deleteFood(int productid);
    DataTable fetchReport();
    DataTable downloadReport(string empname);

}