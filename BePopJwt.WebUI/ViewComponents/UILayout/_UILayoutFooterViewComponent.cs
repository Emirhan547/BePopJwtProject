using Microsoft.AspNetCore.Mvc;

namespace BePopJwt.WebUI.ViewComponents.UILayout
{
    public class _UILayoutFooterViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
