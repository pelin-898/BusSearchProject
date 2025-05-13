using BusSearch.Application.Constants;
using Microsoft.AspNetCore.Mvc;

namespace BusSearch.WebUI.Controllers
{
    public abstract class BaseController : Controller
    {
        protected void SetErrorMessage(string message)
        {
            TempData[ErrorMessages.ErrorMessage] = message;
        }
    }
}
