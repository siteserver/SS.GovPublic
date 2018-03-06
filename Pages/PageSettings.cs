using System;
using System.Web.UI.WebControls;
using SiteServer.Plugin;
using SS.GovPublic.Core;
using SS.GovPublic.Model;

namespace SS.GovPublic.Pages
{
    public class PageSettings : PageBase
    {
        public Literal LtlMessage;
        public DropDownList DdlGovPublicChannelId;
        public DropDownList DdlIsPublisherRelatedDepartmentId;

        private ConfigInfo _configInfo;

        public void Page_Load(object sender, EventArgs e)
        {
            _configInfo = Main.Instance.GetConfigInfo(SiteId);

            if (IsPostBack) return;

            AddListItemsForGovPublic(DdlGovPublicChannelId.Items);
            Utils.SelectSingleItem(DdlGovPublicChannelId, _configInfo.GovPublicChannelId.ToString());
            Utils.SelectSingleItem(DdlIsPublisherRelatedDepartmentId, _configInfo.GovPublicIsPublisherRelatedDepartmentId.ToString());
        }

        private void AddListItemsForGovPublic(ListItemCollection listItemCollection)
        {
            var channelIdList = Main.Instance.ChannelApi.GetChannelIdList(SiteId);
            var channelCount = channelIdList.Count;
            var isLastChannelArray = new bool[channelCount];
            foreach (var channelId in channelIdList)
            {
                var nodeInfo = Main.Instance.ChannelApi.GetChannelInfo(SiteId, channelId);
                var listItem = new ListItem(Utils.GetSelectOptionText(nodeInfo.ChannelName, nodeInfo.ParentsCount, nodeInfo.IsLastNode, isLastChannelArray),
                    nodeInfo.Id.ToString());
                if (nodeInfo.ContentModelPluginId != Main.Instance.Id)
                {
                    listItem.Value = "0";
                    listItem.Attributes.Add("disabled", "disabled");
                    listItem.Attributes.Add("style", "background-color:#f0f0f0;color:#9e9e9e");
                }
                listItemCollection.Add(listItem);
            }
        }

        public void Submit_OnClick(object sender, EventArgs e)
        {
            if (!Page.IsPostBack || !Page.IsValid) return;

            var channelId = Utils.ToInt(DdlGovPublicChannelId.SelectedValue);
            var nodeInfo = Main.Instance.ChannelApi.GetChannelInfo(SiteId, channelId);
            if (nodeInfo == null || nodeInfo.ContentModelPluginId != Main.Instance.Id)
            {
                DdlGovPublicChannelId.Items.Clear();
                AddListItemsForGovPublic(DdlGovPublicChannelId.Items);
                Utils.SelectSingleItem(DdlGovPublicChannelId, _configInfo.GovPublicChannelId.ToString());

                LtlMessage.Text = Utils.GetMessageHtml("信息公开设置修改失败，主题分类根栏目必须选择信息公开类型栏目！", false);
                return;
            }

            _configInfo.GovPublicChannelId = channelId;
            _configInfo.GovPublicIsPublisherRelatedDepartmentId = Utils.ToBool(DdlIsPublisherRelatedDepartmentId.SelectedValue);

            Main.Instance.ConfigApi.SetConfig(SiteId, _configInfo);
            LtlMessage.Text = Utils.GetMessageHtml("信息公开设置修改成功！", true);
        }
    }
}