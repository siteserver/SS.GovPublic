using System;
using System.Web.UI.WebControls;
using SS.GovPublic.Core;
using SS.GovPublic.Model;
using SS.GovPublic.Provider;

namespace SS.GovPublic.Pages
{
    public class ModalCategoryClassAdd : PageBase
    {
        protected TextBox TbClassName;
        protected TextBox TbClassCode;
        protected DropDownList DdlIsEnabled;
        protected TextBox TbDescription;

        private int _classId;

        public static string GetOpenWindowStringToAdd(int siteId)
        {
            return Utils.GetOpenLayerString("添加分类法", $"{nameof(ModalCategoryClassAdd)}.aspx?siteId={siteId}", 300, 460);
        }

        public static string GetOpenWindowStringToEdit(int siteId, int classId)
        {
            return Utils.GetOpenLayerString("编辑分类法", $"{nameof(ModalCategoryClassAdd)}.aspx?siteId={siteId}&classId={classId}", 300, 460);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            _classId = Utils.ToInt(Request.QueryString["classId"]);

            if (IsPostBack) return;

            if (_classId <= 0) return;

            var classInfo = CategoryClassDao.GetCategoryClassInfo(_classId);

            TbClassName.Text = classInfo.ClassName;
            TbClassCode.Text = classInfo.ClassCode;
            TbClassCode.Enabled = false;
            Utils.SelectSingleItemIgnoreCase(DdlIsEnabled, classInfo.IsEnabled.ToString());
            if (classInfo.IsSystem)
            {
                DdlIsEnabled.Enabled = false;
            }
            TbDescription.Text = classInfo.Description;
        }

        public void Submit_OnClick(object sender, EventArgs e)
        {
            var isChanged = false;
            CategoryClassInfo categoryClassInfo;

            if (_classId > 0)
            {
                try
                {
                    categoryClassInfo = CategoryClassDao.GetCategoryClassInfo(_classId);
                    if (categoryClassInfo != null)
                    {
                        categoryClassInfo.ClassName = TbClassName.Text;
                        categoryClassInfo.ClassCode = TbClassCode.Text;
                        categoryClassInfo.IsEnabled = Convert.ToBoolean(DdlIsEnabled.SelectedValue);
                        categoryClassInfo.Description = TbDescription.Text;
                    }
                    CategoryClassDao.Update(categoryClassInfo);

                    isChanged = true;
                }
                catch (Exception ex)
                {
                    LtlMessage.Text = Utils.GetMessageHtml($"分类法修改失败：{ex.Message}！", false);
                }
            }
            else
            {
                var classNameList = CategoryClassDao.GetClassNameList(SiteId);
                var classCodeList = CategoryClassDao.GetClassCodeList(SiteId);
                if (classNameList.IndexOf(TbClassName.Text) != -1)
                {
                    LtlMessage.Text = Utils.GetMessageHtml("分类法添加失败，分类法名称已存在！", false);
                }
                else if (classCodeList.IndexOf(TbClassCode.Text) != -1)
                {
                    LtlMessage.Text = Utils.GetMessageHtml("分类法添加失败，分类代码已存在！", false);
                }
                else
                {
                    try
                    {
                        categoryClassInfo = new CategoryClassInfo(0, SiteId, TbClassCode.Text, TbClassName.Text, false, Convert.ToBoolean(DdlIsEnabled.SelectedValue), string.Empty, 0, TbDescription.Text);

                        CategoryClassDao.Insert(categoryClassInfo);

                        isChanged = true;
                    }
                    catch (Exception ex)
                    {
                        LtlMessage.Text = Utils.GetMessageHtml($"分类法添加失败：{ex.Message}！", false);
                    }
                }
            }

            if (isChanged)
            {
                Utils.CloseModalPage(Page);
            }
        }
    }
}
