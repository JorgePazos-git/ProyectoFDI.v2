using Microsoft.AspNetCore.Mvc;

namespace ProyectoFDI.v2.Extension
{
    public enum NotificationType
    {
        Success,
        Error,
        Info,
        Question
    }
    public class BaseController : Controller
    {
        public void BasicNotification(string msj, NotificationType type, string title = "")
        {
            TempData["notification"] = $"Swal.fire('{title}','{msj}', '{type.ToString().ToLower()}')";
        }
    }
}
