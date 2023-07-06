using Microsoft.AspNetCore.Mvc.Filters;
public class LogAttribute : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
        Log("OnActionExecuted", filterContext.RouteData);
    }
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        Log("OnActionExecuting", filterContext.RouteData);
    }
    public override void OnResultExecuted(ResultExecutedContext filterContext)
    {
        Log("OnResultExecuted", filterContext.RouteData);
    }
     public override void OnResultExecuting(ResultExecutingContext filterContext)
    {
        Log("OnResultExecuting", filterContext.RouteData);
    }
    private void Log(string methodName, RouteData routeData)
    {
        var controllerName= routeData.Values["controller"];
        var actionName= routeData.Values["action"];
        var message= methodName +" -Controller:"+ controllerName+",Action:"+actionName+"\n";
        Console.WriteLine(message);
    }

}