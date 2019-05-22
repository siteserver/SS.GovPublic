using System;
using System.Web.UI.WebControls;
using SS.GovPublic.Core;
using SS.GovPublic.Core.Model;
using SS.GovPublic.Core.Provider;
using SS.GovPublic.Core.Utils;

namespace SS.GovPublic.Core.Pages
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
            return GovPublicUtils.GetOpenLayerString("添加分类法", $"{nameof(ModalCategoryClassAdd)}.aspx?siteId={siteId}", 300, 460);
        }

        public static string GetOpenWindowStringToEdit(int siteId, int classId)
        {
            return GovPublicUtils.GetOpenLayerString("编辑分类法", $"{nameof(ModalCategoryClassAdd)}.aspx?siteId={siteId}&classId={classId}", 300, 460);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            _classId = TranslateUtils.ToInt(Request.QueryString["classId"]);

            if (IsPostBack) return;

            if (_classId <= 0) return;

            var classInfo = Main.CategoryClassRepository.GetCategoryClassInfo(_classId);

            TbClassName.Text = classInfo.ClassName;
            TbClassCode.Text = classInfo.ClassCode;
            TbClassCode.Enabled = false;
            GovPublicUtils.SelectSingleItemIgnoreCase(DdlIsEnabled, classInfo.IsEnabled.ToString());
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
                    categoryClassInfo = Main.CategoryClassRepository.GetCategoryClassInfo(_classId);
                    if (categoryClassInfo != null)
                    {
                        categoryClassInfo.ClassName = TbClassName.Text;
                        categoryClassInfo.ClassCode = TbClassCode.Text;
                        categoryClassInfo.IsEnabled = Convert.ToBoolean(DdlIsEnabled.SelectedValue);
                        categoryClassInfo.Description = TbDescription.Text;
                    }
                    Main.CategoryClassRepository.Update(categoryClassInfo);

                    isChanged = true;
                }
                catch (Exception ex)
                {
                    LtlMessage.Text = GovPublicUtils.GetMessageHtml($"分类法修改失败：{ex.Message}！", false);
                }
            }
            else
            {
                var classNameList = Main.CategoryClassRepository.GetClassNameList(SiteId);
                var classCodeList = Main.CategoryClassRepository.GetClassCodeList(SiteId);
                if (classNameList.IndexOf(TbClassName.Text) != -1)
                {
                    LtlMessage.Text = GovPublicUtils.GetMessageHtml("分类法添加失败，分类法名称已存在！", false);
                }
                else if (classCodeList.IndexOf(TbClassCode.Text) != -1)
                {
                    LtlMessage.Text = GovPublicUtils.GetMessageHtml("分类法添加失败，分类代码已存在！", false);
                }
                else
                {
                    try
                    {
                        categoryClassInfo = new CategoryClassInfo
                        {
                            SiteId = SiteId,
                            ClassCode = TbClassCode.Text,
                            ClassName = TbClassName.Text,
                            IsSystem = false,
                            IsEnabled = Convert.ToBoolean(DdlIsEnabled.SelectedValue),
                            ContentAttributeName = string.Empty,
                            Taxis = 0,
                            Description = TbDescription.Text
                        };

                        Main.CategoryClassRepository.Insert(categoryClassInfo);

                        isChanged = true;
                    }
                    catch (Exception ex)
                    {
                        LtlMessage.Text = GovPublicUtils.GetMessageHtml($"分类法添加失败：{ex.Message}！", false);
                    }
                }
            }

            if (isChanged)
            {
                GovPublicUtils.CloseModalPage(Page);
            }
        }
    }
}
