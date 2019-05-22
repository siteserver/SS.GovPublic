using System;
using System.Web.UI.WebControls;
using SS.GovPublic.Core;
using SS.GovPublic.Core.Model;
using SS.GovPublic.Core.Provider;
using SS.GovPublic.Core.Utils;

namespace SS.GovPublic.Core.Pages
{
    public class PageCategory : PageBase
    {
        public Repeater RptContents;
        public Button BtnAdd;
        public Button BtnDelete;

        public Literal LtlScripts;

        private CategoryClassInfo _categoryClassInfo;
        private int _currentCategoryId;

        public static string GetRedirectUrl(int siteId, string classCode)
        {
            return $"{nameof(PageCategory)}.aspx?siteId={siteId}&classCode={classCode}";
        }

        public static string GetRedirectUrl(int siteId, string classCode, int currentCategoryId)
        {
            return currentCategoryId != 0
                ? $"{nameof(PageCategory)}.aspx?siteId={siteId}&classCode={classCode}&currentCategoryId={currentCategoryId}"
                : GetRedirectUrl(siteId, classCode);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            var classCode = Request.QueryString["classCode"];
            _categoryClassInfo = Main.CategoryClassRepository.GetCategoryClassInfo(SiteId, classCode);

            if (Request.QueryString["Delete"] != null && Request.QueryString["CategoryIDCollection"] != null)
            {
                var categoryIdList = Request.QueryString["CategoryIDCollection"].Split(',');
                foreach (var categoryId in categoryIdList)
                {
                    Main.CategoryRepository.Delete(TranslateUtils.ToInt(categoryId));
                }
                LtlMessage.Text = GovPublicUtils.GetMessageHtml("成功删除所选节点", true);
            }
            else if (Request.QueryString["categoryId"] != null && (Request.QueryString["subtract"] != null || Request.QueryString["add"] != null))
            {
                var categoryId = TranslateUtils.ToInt(Request.QueryString["categoryId"]);
                var isSubtract = Request.QueryString["subtract"] != null;
                Main.CategoryRepository.UpdateTaxis(SiteId, _categoryClassInfo.ClassCode, categoryId, isSubtract);

                Response.Redirect(GetRedirectUrl(SiteId, _categoryClassInfo.ClassCode, categoryId));
                return;
            }

            if (IsPostBack) return;

            LtlScripts.Text += CategoryTreeItem.GetScript(SiteId, _categoryClassInfo.ClassCode, ECategoryLoadingType.List);

            if (Request.QueryString["CurrentCategoryID"] != null)
            {
                _currentCategoryId = TranslateUtils.ToInt(Request.QueryString["CurrentCategoryID"]);
                var onLoadScript = GetScriptOnLoad(_currentCategoryId);
                if (!string.IsNullOrEmpty(onLoadScript))
                {
                    LtlScripts.Text += onLoadScript;
                }
            }

            BtnAdd.Attributes.Add("onclick", ModalCategoryAdd.GetOpenWindowStringToAdd(SiteId, _categoryClassInfo.ClassCode));

            //BtnDelete.Attributes.Add("onclick",
            //    PageUtils.GetRedirectStringWithCheckBoxValueAndAlert(
            //        PageUtils.GetWcmUrl(nameof(PageCategory), new NameValueCollection
            //        {
            //            {"SiteId", SiteId.ToString()},
            //            {"ClassCode", _categoryClassInfo.ClassCode},
            //            {"Delete", true.ToString()},
            //        }), "CategoryIDCollection", "CategoryIDCollection", "请选择需要删除的节点！", "此操作将删除对应节点以及所有下级节点，确认删除吗？"));

            RptContents.DataSource = Main.CategoryRepository.GetCategoryIdListByParentId(SiteId, _categoryClassInfo.ClassCode, 0);
            RptContents.ItemDataBound += RptContents_ItemDataBound;
            RptContents.DataBind();
        }

        public string ClassName => _categoryClassInfo != null ? _categoryClassInfo.ClassName : string.Empty;

        public string GetScriptOnLoad(int currentCategoryId)
        {
            if (currentCategoryId == 0) return string.Empty;
            var categoryInfo = Main.CategoryRepository.GetCategoryInfo(currentCategoryId);
            if (categoryInfo == null) return string.Empty;
            string path;
            if (categoryInfo.ParentsCount <= 1)
            {
                path = currentCategoryId.ToString();
            }
            else
            {
                path = categoryInfo.ParentsPath.Substring(categoryInfo.ParentsPath.IndexOf(",", StringComparison.Ordinal) + 1) + "," + currentCategoryId;
            }
            return CategoryTreeItem.GetScriptOnLoad(path);
        }

        private void RptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var categoryId = (int)e.Item.DataItem;

            var categoryInfo = Main.CategoryRepository.GetCategoryInfo(categoryId);

            var ltlHtml = (Literal)e.Item.FindControl("ltlHtml");

            ltlHtml.Text = GetCategoryRowHtml(SiteId, categoryInfo, true, ECategoryLoadingType.List);
        }

        public static string GetCategoryRowHtml(int siteId, CategoryInfo categoryInfo, bool enabled, ECategoryLoadingType loadingType)
        {
            var treeItem = CategoryTreeItem.CreateInstance(categoryInfo, enabled);
            var title = treeItem.GetItemHtml(loadingType);

            var rowHtml = string.Empty;

            if (loadingType == ECategoryLoadingType.Tree || loadingType == ECategoryLoadingType.Select)
            {
                rowHtml = $@"
<tr treeItemLevel=""{categoryInfo.ParentsCount + 1}"">
	<td nowrap>
		{title}
	</td>
</tr>
";
            }
            else if (loadingType == ECategoryLoadingType.List)
            {
                var editUrl = string.Empty;
                var upLink = string.Empty;
                var downLink = string.Empty;
                var checkBoxHtml = string.Empty;

                if (enabled)
                {
                    editUrl =
                        $@"<a href=""javascript:;"" onclick=""{ModalCategoryAdd.GetOpenWindowStringToEdit(siteId, categoryInfo.ClassCode, categoryInfo.Id)}"">编辑</a>";

                    upLink = $@"<a href=""{GetRedirectUrl(siteId, categoryInfo.ClassCode)}&subtract={true}&categoryId={categoryInfo.Id}"">上升</a>";

                    downLink =
                        $@"<a href=""{GetRedirectUrl(siteId, categoryInfo.ClassCode)}&add={true}&categoryId={categoryInfo.Id}"">下降</a>";

                    checkBoxHtml =
                        $"<input type='checkbox' name='CategoryIDCollection' value='{categoryInfo.Id}' />";
                }

                rowHtml = $@"
<tr treeItemLevel=""{categoryInfo.ParentsCount + 1}"">
    <td>{title}</td>
    <td>{categoryInfo.CategoryCode}</td>
    <td class=""center"">{upLink}</td>
    <td class=""center"">{downLink}</td>
    <td class=""center"">{editUrl}</td>
    <td class=""center"">{checkBoxHtml}</td>
</tr>
";
            }
            return rowHtml;
        }
    }
}
