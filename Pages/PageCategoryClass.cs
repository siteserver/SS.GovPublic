using System;
using System.Web.UI.WebControls;
using SS.GovPublic.Core;
using SS.GovPublic.Model;
using SS.GovPublic.Provider;

namespace SS.GovPublic.Pages
{
    public class PageCategoryClass : PageBase
    {
        public DataGrid DgContents;
        public Button BtnAdd;

        public static string GetRedirectUrl(int siteId)
        {
            return $"{nameof(PageCategoryClass)}.aspx?siteId={siteId}";
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (Request.QueryString["Delete"] != null && Request.QueryString["Id"] != null)
            {
                var id = Convert.ToInt32(Request.QueryString["Id"]);
                try
                {
                    CategoryClassDao.Delete(id);
                    LtlMessage.Text = Utils.GetMessageHtml("成功删除分类法", true);
                }
                catch (Exception ex)
                {
                    LtlMessage.Text = Utils.GetMessageHtml($"删除分类法失败，{ex.Message}", false);
                }
            }
            else if ((Request.QueryString["Up"] != null || Request.QueryString["Down"] != null) && Request.QueryString["ClassCode"] != null)
            {
                var classCode = Request.QueryString["ClassCode"];
                var isDown = Request.QueryString["Down"] != null;
                if (isDown)
                {
                    CategoryClassDao.UpdateTaxisToUp(SiteId, classCode);
                }
                else
                {
                    CategoryClassDao.UpdateTaxisToDown(SiteId, classCode);
                }
            }

            DgContents.DataSource = CategoryClassDao.GetCategoryClassInfoList(SiteId);
            DgContents.ItemDataBound += DgContents_ItemDataBound;
            DgContents.DataBind();

            BtnAdd.Attributes.Add("onclick", ModalCategoryClassAdd.GetOpenWindowStringToAdd(SiteId));
        }

        private void DgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var classInfo = (CategoryClassInfo) e.Item.DataItem;

            var ltlClassName = (Literal)e.Item.FindControl("ltlClassName");
            var ltlClassCode = (Literal)e.Item.FindControl("ltlClassCode");
            var hlUpLinkButton = (HyperLink)e.Item.FindControl("hlUpLinkButton");
            var hlDownLinkButton = (HyperLink)e.Item.FindControl("hlDownLinkButton");
            var ltlIsEnabled = (Literal)e.Item.FindControl("ltlIsEnabled");
            var ltlEditUrl = (Literal)e.Item.FindControl("ltlEditUrl");
            var ltlDeleteUrl = (Literal)e.Item.FindControl("ltlDeleteUrl");

            if (!classInfo.IsSystem)
            {
                ltlClassName.Text = $@"<a href=""{PageCategory.GetRedirectUrl(SiteId, classInfo.ClassCode)}"" target=""category"">{classInfo.ClassName}</a>";
            }
            else if (ECategoryClassTypeUtils.Equals(ECategoryClassType.Channel, classInfo.ClassCode))
            {
                ltlClassName.Text = $@"{classInfo.ClassName}";
            }
            else if (ECategoryClassTypeUtils.Equals(ECategoryClassType.Department, classInfo.ClassCode))
            {
                ltlClassName.Text = $@"{classInfo.ClassName}";
            }

            ltlClassCode.Text = classInfo.ClassCode;
            ltlIsEnabled.Text = classInfo.IsEnabled ? "启用" : "禁用";

            hlUpLinkButton.NavigateUrl = $"{GetRedirectUrl(SiteId)}&Up={true}&ClassCode={classInfo.ClassCode}";

            hlDownLinkButton.NavigateUrl = $"{GetRedirectUrl(SiteId)}&Down={true}&ClassCode={classInfo.ClassCode}";

            ltlEditUrl.Text =
                $@"<a href='javascript:;' onclick=""{ModalCategoryClassAdd.GetOpenWindowStringToEdit(
                    SiteId, classInfo.Id)}"">编辑</a>";

            if (classInfo.IsSystem) return;

            var urlDelete = $"{GetRedirectUrl(SiteId)}&Delete={true}&Id={classInfo.Id}";
            ltlDeleteUrl.Text =
                $@"<a href=""{urlDelete}"" onClick=""javascript:return confirm('此操作将删除分类法“{classInfo.ClassName}”及其分类项，确认吗？');"">删除</a>";
        }
    }
}
