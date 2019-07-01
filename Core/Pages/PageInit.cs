using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.GovPublic.Core;
using SS.GovPublic.Core.Utils;

namespace SS.GovPublic.Core.Pages
{
    public class PageInit : Page
    {
        public CheckBox CbIsNameToIndex;
        public TextBox TbChannelNames;
        public Literal LtlScript;

        public int SiteId { get; private set; }
        private string _redirectUrl;

        public string UrlModalChannelSelect
            => Main.UtilsApi.GetAdminUrl($"cms/modalchannelselect.aspx?siteId={SiteId}");

        public static string GetRedirectUrl(int siteId, string redirectUrl)
        {
            return GovPublicUtils.GetPluginUrl(
                $"pages/{nameof(PageInit)}.aspx?siteId={siteId}&redirectUrl={HttpUtility.UrlEncode(redirectUrl)}");
        }

        public void Page_Load(object sender, EventArgs e)
        {
            var request = SiteServer.Plugin.Context.AuthenticatedRequest;
            SiteId = request.GetQueryInt("siteId");
            _redirectUrl = request.GetQueryString("redirectUrl");

            if (!request.AdminPermissions.HasSitePermissions(SiteId, Main.PluginId))
            {
                HttpContext.Current.Response.Write("<h1>未授权访问</h1>");
                HttpContext.Current.Response.End();
            }

            if (IsPostBack) return;

            var channelInfoList = PublicManager.GetPublicChannelInfoList(SiteId);

            if (channelInfoList.Count > 0)
            {
                HttpContext.Current.Response.Redirect(_redirectUrl);
                HttpContext.Current.Response.End();
            }
        }

        private List<string> GetChannelIndexList()
        {
            var indexList = new List<string>();
            var channelIdList = Main.ChannelApi.GetChannelIdList(SiteId);
            foreach (var channelId in channelIdList)
            {
                var channelInfo = Main.ChannelApi.GetChannelInfo(SiteId, channelId);
                if (!string.IsNullOrEmpty(channelInfo?.IndexName))
                {
                    if (!indexList.Contains(channelInfo.IndexName))
                    {
                        indexList.Add(channelInfo.IndexName);
                    }
                }
            }
            return indexList;
        }

        public void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged;
            var parentChannelId = TranslateUtils.ToInt(Request.Form["channelId"]);
            if (parentChannelId == 0)
            {
                parentChannelId = SiteId;
            }

            try
            {
                if (string.IsNullOrEmpty(TbChannelNames.Text))
                {
                    LtlScript.Text = $"<script>{AlertUtils.Error("操作错误", "请填写需要添加的栏目名称")}</script>";
                    return;
                }

                var insertedChannelIdHashtable = new Hashtable { [1] = parentChannelId }; //key为栏目的级别，1为第一级栏目

                var channelNameArray = TbChannelNames.Text.Split('\n');
                List<string> indexNameList = null;
                foreach (var item in channelNameArray)
                {
                    if (string.IsNullOrEmpty(item)) continue;

                    //count为栏目的级别
                    var count = (GovPublicUtils.GetStartCount('－', item) == 0) ? GovPublicUtils.GetStartCount('-', item) : GovPublicUtils.GetStartCount('－', item);
                    var channelName = item.Substring(count, item.Length - count);
                    var channelIndex = string.Empty;
                    count++;

                    if (!string.IsNullOrEmpty(channelName) && insertedChannelIdHashtable.Contains(count))
                    {
                        if (CbIsNameToIndex.Checked)
                        {
                            channelIndex = channelName.Trim();
                        }

                        if (GovPublicUtils.Contains(channelName, "(") && GovPublicUtils.Contains(channelName, ")"))
                        {
                            var length = channelName.IndexOf(')') - channelName.IndexOf('(');
                            if (length > 0)
                            {
                                channelIndex = channelName.Substring(channelName.IndexOf('(') + 1, length);
                                channelName = channelName.Substring(0, channelName.IndexOf('('));
                            }
                        }
                        channelName = channelName.Trim();
                        channelIndex = channelIndex.Trim(' ', '(', ')');
                        if (!string.IsNullOrEmpty(channelIndex))
                        {
                            if (indexNameList == null)
                            {
                                indexNameList = GetChannelIndexList();
                            }
                            if (indexNameList.IndexOf(channelIndex) != -1)
                            {
                                channelIndex = string.Empty;
                            }
                            else
                            {
                                indexNameList.Add(channelIndex);
                            }
                        }

                        var parentId = (int)insertedChannelIdHashtable[count];
                        var parentNodeInfo = Main.ChannelApi.GetChannelInfo(SiteId, parentId);

                        var channelInfo = Main.ChannelApi.NewInstance(SiteId);

                        channelInfo.ParentId = parentId;
                        channelInfo.ChannelName = channelName;
                        channelInfo.IndexName = channelIndex;
                        channelInfo.ContentModelPluginId = Main.PluginId;
                        channelInfo.ChannelTemplateId = parentNodeInfo.ChannelTemplateId;
                        channelInfo.ContentTemplateId = parentNodeInfo.ContentTemplateId;

                        var insertedChannelId = Main.ChannelApi.Insert(SiteId, channelInfo);
                        insertedChannelIdHashtable[count + 1] = insertedChannelId;
                    }
                }

                isChanged = true;
            }
            catch (Exception ex)
            {
                isChanged = false;
                LtlScript.Text = $"<script>{AlertUtils.Error("操作错误", ex.Message)}</script>";
            }

            if (isChanged)
            {
                LtlScript.Text = $"<script>{AlertUtils.Success("操作成功", "您已成功添加栏目，点击确认返回", "确 认", $"location.href = '{_redirectUrl}'")}</script>";
            }
        }
    }
}