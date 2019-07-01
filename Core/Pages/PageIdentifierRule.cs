using System;
using System.Web.UI.WebControls;
using SS.GovPublic.Core;
using SS.GovPublic.Core.Model;
using SS.GovPublic.Core.Provider;
using SS.GovPublic.Core.Utils;

namespace SS.GovPublic.Core.Pages
{
    public class PageIdentifierRule : PageBase
    {
        public Literal LtlPreview;
        public DataGrid DgContents;
        public Button BtnAdd;

        public static string GetRedirectUrl(int siteId)
        {
            return GovPublicUtils.GetPluginUrl($"pages/{nameof(PageIdentifierRule)}.aspx?siteId={siteId}");
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Delete"] != null)
            {
                var ruleId = TranslateUtils.ToInt(Request.QueryString["RuleID"]);
                Main.IdentifierRuleRepository.Delete(ruleId);
                LtlMessage.Text = GovPublicUtils.GetMessageHtml("成功删除规则", true);
            }
            else if ((Request.QueryString["Up"] != null || Request.QueryString["Down"] != null) && Request.QueryString["RuleID"] != null)
            {
                var ruleId = TranslateUtils.ToInt(Request.QueryString["RuleID"]);
                var isDown = Request.QueryString["Down"] != null;
                if (isDown)
                {
                    Main.IdentifierRuleRepository.UpdateTaxisToUp(ruleId, SiteId);
                }
                else
                {
                    Main.IdentifierRuleRepository.UpdateTaxisToDown(ruleId, SiteId);
                }
            }

            if (IsPostBack) return;

            LtlPreview.Text = PublicManager.GetPreviewIdentifier(SiteId);

            DgContents.DataSource = Main.IdentifierRuleRepository.GetRuleInfoList(SiteId);
            DgContents.ItemDataBound += DgContents_ItemDataBound;
            DgContents.DataBind();

            BtnAdd.Attributes.Add("onclick", ModalIdentifierRuleAdd.GetOpenWindowStringToAdd(SiteId));
        }

        private void DgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var ruleInfo = (IdentifierRuleInfo)e.Item.DataItem;
            var identifierType = EIdentifierTypeUtils.GetEnumType(ruleInfo.IdentifierType);

            var ltlIndex = (Literal)e.Item.FindControl("ltlIndex");
            var ltlRuleName = (Literal)e.Item.FindControl("ltlRuleName");
            var ltlIdentifierType = (Literal)e.Item.FindControl("ltlIdentifierType");
            var ltlMinLength = (Literal)e.Item.FindControl("ltlMinLength");
            var ltlSuffix = (Literal)e.Item.FindControl("ltlSuffix");
            var hlUpLinkButton = (HyperLink)e.Item.FindControl("hlUpLinkButton");
            var hlDownLinkButton = (HyperLink)e.Item.FindControl("hlDownLinkButton");
            var ltlSettingUrl = (Literal)e.Item.FindControl("ltlSettingUrl");
            var ltlEditUrl = (Literal)e.Item.FindControl("ltlEditUrl");
            var ltlDeleteUrl = (Literal)e.Item.FindControl("ltlDeleteUrl");

            ltlIndex.Text = (e.Item.ItemIndex + 1).ToString();
            ltlRuleName.Text = ruleInfo.RuleName;
            ltlIdentifierType.Text = EIdentifierTypeUtils.GetText(identifierType);
            ltlMinLength.Text = ruleInfo.MinLength.ToString();
            ltlSuffix.Text = ruleInfo.Suffix;

            var redirectUrl = GetRedirectUrl(SiteId);

            hlUpLinkButton.NavigateUrl = $"{redirectUrl}&up={true}&ruleId={ruleInfo.Id}";

            hlDownLinkButton.NavigateUrl = $"{redirectUrl}&down={true}&ruleId={ruleInfo.Id}";

            //if (ruleInfo.IdentifierType == EIdentifierType.Department)
            //{
            //    var urlSetting = PageGovPublicDepartment.GetRedirectUrl(SiteId);
            //    ltlSettingUrl.Text = $@"<a href=""{urlSetting}"">机构分类设置</a>";
            //}
            //else if (ruleInfo.IdentifierType == EGovPublicIdentifierType.Channel)
            //{
            //    ltlSettingUrl.Text = $@"<a href=""{PageGovPublicChannel.GetRedirectUrl(SiteId)}"">主题分类设置</a>";
            //}

            ltlEditUrl.Text =
                $@"<a href='javascript:;' onclick=""{ModalIdentifierRuleAdd.GetOpenWindowStringToEdit(
                    SiteId, ruleInfo.Id)}"">编辑</a>";

            ltlDeleteUrl.Text =
                $@"<a href=""{redirectUrl}&delete={true}&ruleId={ruleInfo.Id}"" onClick=""javascript:return confirm('此操作将删除规则“{ruleInfo.RuleName}”，确认吗？');"">删除</a>";
        }
    }
}
