using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SiteServer.Plugin;
using SS.GovPublic.Core;
using SS.GovPublic.Core.Model;

namespace SS.GovPublic.Core.Pages
{
    public class PageBase : Page
    {
        public Literal LtlMessage;

        public int SiteId { get; private set; }

        public List<IChannelInfo> ChannelInfoList { get; private set; }

        public IAuthenticatedRequest AuthRequest { get; private set; }

        private ConfigInfo _configInfo;

        public ConfigInfo ConfigInfo => _configInfo ?? (_configInfo = Main.ConfigApi.GetConfig<ConfigInfo>(Main.PluginId, SiteId) ?? new ConfigInfo());

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            AuthRequest = SiteServer.Plugin.Context.AuthenticatedRequest;
            SiteId = AuthRequest.GetQueryInt("siteId");

            if (!AuthRequest.AdminPermissions.HasSitePermissions(SiteId, Main.PluginId))
            {
                HttpContext.Current.Response.Write("<h1>未授权访问</h1>");
                HttpContext.Current.Response.End();
            }

            ChannelInfoList = PublicManager.GetPublicChannelInfoList(SiteId);

            if (ChannelInfoList.Count == 0)
            {
                HttpContext.Current.Response.Redirect(PageInit.GetRedirectUrl(SiteId, Request.RawUrl));
                HttpContext.Current.Response.End();
            }
        }
    }
}
