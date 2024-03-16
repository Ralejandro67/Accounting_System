using Microsoft.AspNetCore.Mvc.Filters;
using PupuseriaSalvadorena.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace PupuseriaSalvadorena.Filtros
{
    public class FiltroAutentificacion : ActionFilterAttribute
    {
        public string[] RolAcceso { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string? usuario = context.HttpContext.Session.GetString("Usuario");

            if (string.IsNullOrEmpty(usuario))
            {
                context.Result = new RedirectToActionResult("IniciarSesion", "Home", null);
            }
            else
            {
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var user = JsonSerializer.Deserialize<Usuario>(usuario, options);

                if (user != null && !string.IsNullOrEmpty(user.NombreRol) && RolAcceso != null)
                {
                    bool acceso = RolAcceso.Contains(user.NombreRol);

                    if (acceso)
                    {
                        base.OnActionExecuting(context);
                    }
                    else
                    {
                        context.Result = new RedirectToActionResult("Index", "Home", null);
                    }
                }
                else
                {
                    context.Result = new RedirectToActionResult("Index", "Home", null);
                }
            }
        }
    }
}
