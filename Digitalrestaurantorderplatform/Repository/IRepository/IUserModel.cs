using System.Data;
using Digitalrestaurantorderplatform.Models;
using Digitalrestaurantorderplatform.Repository;
namespace Digitalrestaurantorderplatform.IRepository;

interface IUserModel
{
    int signUpValidation(SignUpModel signupmodel);
    int loginValidation(LoginModel loginmodel,string adminUsername,string adminPassword);
    int ResetPassword(SignUpModel details);
    // DataTable getProductDetails(string filterItem);
    // int addCartItems(string foodName,string name);
    // DataTable getCartList(string name);
    // void deleteCart(string foodname,string name);

    // void increaseQuantity(string foodname,string name);

    // void decreaseQuantity(string foodname,string name);
    // int getTotalPrice(string username);







}