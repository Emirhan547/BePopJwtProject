using Microsoft.AspNetCore.Mvc;

namespace BePopJwt.WebUI.ViewComponents.UILayout
{
    public class _UILayoutHeaderViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
             return View();
        }
    }
}
