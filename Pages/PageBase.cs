using System;
using System.Web;
using System.Web.UI;
using SS.GovPublic.Core;

namespace SS.GovPublic.Pages
{
    public class PageBase : Page
    {
        public int SiteId { get; private set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SiteId = Convert.ToInt32(Request.QueryString["siteId"]);

            if (!Main.Instance.AdminApi.IsSiteAuthorized(SiteId))
            {
                HttpContext.Current.Response.Write("<h1>未授权访问</h1>");
                HttpContext.Current.Response.End();
            }
        }
    }
}
