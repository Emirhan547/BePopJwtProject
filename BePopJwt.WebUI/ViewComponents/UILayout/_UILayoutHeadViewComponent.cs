using Microsoft.AspNetCore.Mvc;

namespace BePopJwt.WebUI.ViewComponents.UILayout
{
    public class _UILayoutHeadViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
