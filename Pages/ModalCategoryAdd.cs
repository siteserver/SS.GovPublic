using System;
using System.Web.UI.WebControls;
using SS.GovPublic.Core;
using SS.GovPublic.Model;

namespace SS.GovPublic.Pages
{
	public class ModalCategoryAdd : PageBase
	{
	    public Literal LtlMessage;
        public TextBox TbCategoryName;
        public TextBox TbCategoryCode;
        public PlaceHolder PhParentId;
        public DropDownList DdlParentId;
        public TextBox TbSummary;

        private string _classCode = string.Empty;
        private int _categoryId;

        public static string GetOpenWindowStringToAdd(int siteId, string classCode)
        {
            return Utils.GetOpenLayerString("添加节点", Main.Instance.PluginApi.GetPluginUrl($"{nameof(ModalCategoryAdd)}.aspx?siteId={siteId}&classCode={classCode}"), 500, 460);
        }

        public static string GetOpenWindowStringToEdit(int siteId, string classCode, int categoryId)
        {
            return Utils.GetOpenLayerString("修改节点", Main.Instance.PluginApi.GetPluginUrl($"{nameof(ModalCategoryAdd)}.aspx?siteId={siteId}&classCode={classCode}&categoryId={categoryId}"), 520, 460);
        }

		public void Page_Load(object sender, EventArgs e)
        {
            _classCode = Request.QueryString["ClassCode"];
            _categoryId = Utils.ToInt(Request.QueryString["categoryId"]);

            if (IsPostBack) return;

            if (_categoryId == 0)
            {
                DdlParentId.Items.Add(new ListItem("<无上级节点>", "0"));

                var categoryIdList = Main.CategoryDao.GetCategoryIdList(SiteId, _classCode);
                var isLastNodeArray = new bool[categoryIdList.Count];
                foreach (var theCategoryId in categoryIdList)
                {
                    var categoryInfo = Main.CategoryDao.GetCategoryInfo(theCategoryId);
                    var listitem = new ListItem(Utils.GetSelectOptionText(categoryInfo.CategoryName, categoryInfo.ParentsCount, categoryInfo.IsLastNode, isLastNodeArray), theCategoryId.ToString());
                    DdlParentId.Items.Add(listitem);
                }
            }
            else
            {
                PhParentId.Visible = false;
            }

            if (_categoryId != 0)
            {
                var categoryInfo = Main.CategoryDao.GetCategoryInfo(_categoryId);

                TbCategoryName.Text = categoryInfo.CategoryName;
                TbCategoryCode.Text = categoryInfo.CategoryCode;
                DdlParentId.SelectedValue = categoryInfo.ParentId.ToString();
                TbSummary.Text = categoryInfo.Summary;
            }
        }

        public void Submit_OnClick(object sender, EventArgs e)
        {
            if (_categoryId == 0)
            {
                var categoryInfo = new CategoryInfo
                {
                    SiteId = SiteId,
                    ClassCode = _classCode,
                    CategoryName = TbCategoryName.Text,
                    CategoryCode = TbCategoryCode.Text,
                    ParentId = Utils.ToInt(DdlParentId.SelectedValue),
                    Summary = TbSummary.Text
                };

                Main.CategoryDao.Insert(categoryInfo);
            }
            else
            {
                var categoryInfo = Main.CategoryDao.GetCategoryInfo(_categoryId);

                categoryInfo.CategoryName = TbCategoryName.Text;
                categoryInfo.CategoryCode = TbCategoryCode.Text;
                categoryInfo.Summary = TbSummary.Text;

                Main.CategoryDao.Update(categoryInfo);
            }

            LtlMessage.Text = Utils.GetMessageHtml("分类设置成功！", true);

            Utils.CloseModalPage(Page);
        }
	}
}
