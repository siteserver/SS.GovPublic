using System;
using System.Web.UI.WebControls;
using SS.GovPublic.Core.Controls;
using SS.GovPublic.Core;
using SS.GovPublic.Core.Provider;
using SS.GovPublic.Core.Utils;

namespace SS.GovPublic.Core.Pages
{
    public class PageAnalysis : PageBase
    {
        public DateTimeTextBox TbStartDate;
        public DateTimeTextBox TbEndDate;
        public DropDownList DdlChannelId;
        public Repeater RptContents;

        private int _nodeId;
        private int _totalCount;

        public static string GetRedirectUrl(int siteId)
        {
            return GovPublicUtils.GetPluginUrl($"pages/{nameof(PageAnalysis)}.aspx?siteId={siteId}");
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            TbStartDate.Text = string.Empty;
            TbEndDate.Now = true;

            var listItem = new ListItem("<<全部>>", "0");
            DdlChannelId.Items.Add(listItem);
            foreach (var nodeInfo in ChannelInfoList)
            {
                listItem = new ListItem(nodeInfo.ChannelName, nodeInfo.Id.ToString());
                DdlChannelId.Items.Add(listItem);
            }

            _totalCount = Main.ContentRepository.GetCount(SiteId, _nodeId, TbStartDate.DateTime, TbEndDate.DateTime);

            BindGrid();
        }

        private void RptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var departmentId = (int)e.Item.DataItem;

            var departmentInfo = DepartmentManager.GetDepartmentInfo(SiteId, departmentId);

            var ltlTrHtml = (Literal)e.Item.FindControl("ltlTrHtml");
            var ltlTarget = (Literal)e.Item.FindControl("ltlTarget");
            var ltlTotalCount = (Literal)e.Item.FindControl("ltlTotalCount");
            var ltlBar = (Literal)e.Item.FindControl("ltlBar");

            ltlTrHtml.Text =
                $@"<tr>";
            ltlTarget.Text = departmentInfo.DepartmentName;

            var count = _nodeId == 0
                ? Main.ContentRepository.GetCountByDepartmentId(SiteId, departmentId, TbStartDate.DateTime, TbEndDate.DateTime)
                : Main.ContentRepository.GetCountByDepartmentId(SiteId, departmentId, _nodeId, TbStartDate.DateTime,
                    TbEndDate.DateTime);

            ltlTotalCount.Text = count.ToString();

            ltlBar.Text = $@"<div class=""progress progress-success progress-striped"">
            <div class=""bar"" style=""width: {GetBarWidth(count, _totalCount)}%""></div>
          </div>";
        }

        private double GetBarWidth(int doCount, int totalCount)
        {
            double width = 0;
            if (totalCount > 0)
            {
                width = Convert.ToDouble(doCount) / Convert.ToDouble(totalCount);
                width = Math.Round(width, 2) * 100;
            }
            return width;
        }

        public void Analysis_OnClick(object sender, EventArgs e)
        {
            BindGrid();
        }

        public void BindGrid()
        {
            _nodeId = TranslateUtils.ToInt(DdlChannelId.SelectedValue);

            var departmentIdList = DepartmentManager.GetDepartmentIdList(SiteId);

            RptContents.DataSource = departmentIdList;
            RptContents.ItemDataBound += RptContents_ItemDataBound;
            RptContents.DataBind();
        }
    }
}
