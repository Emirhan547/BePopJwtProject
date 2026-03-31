using Microsoft.AspNetCore.Mvc;

namespace BePopJwt.WebUI.ViewComponents.UILayout
{
    public class _UILayoutScriptViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
