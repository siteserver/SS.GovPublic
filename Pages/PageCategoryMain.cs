using System;

namespace SS.GovPublic.Pages
{
    public class PageCategoryMain : PageBase
    {
        public static string GetRedirectUrl(int siteId)
        {
            return $"{nameof(PageCategoryMain)}.aspx?siteId={siteId}";
        }

        public void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
