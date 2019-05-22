using System;
using System.Web.UI.WebControls;
using SiteServer.Plugin;
using SS.GovPublic.Core;
using SS.GovPublic.Core.Model;
using SS.GovPublic.Core.Provider;
using SS.GovPublic.Core.Utils;

namespace SS.GovPublic.Core.Pages
{
	public class ModalIdentifierRuleAdd : PageBase
	{
        public TextBox TbRuleName;
        public DropDownList DdlIdentifierType;
        public PlaceHolder PhAttributeName;
        public DropDownList DdlAttributeName;
        public PlaceHolder PhMinLength;
        public TextBox TbMinLength;
        public PlaceHolder PhFormatString;
        public TextBox TbFormatString;
        public TextBox TbSuffix;
        public PlaceHolder PhSequence;
        public TextBox TbSequence;
        public DropDownList DdlIsSequenceChannelZero;
        public DropDownList DdlIsSequenceDepartmentZero;
        public DropDownList DdlIsSequenceYearZero;

        private int _ruleId;

        public static string GetOpenWindowStringToAdd(int siteId)
        {
            return GovPublicUtils.GetOpenLayerString("添加规则", $"{nameof(ModalIdentifierRuleAdd)}.aspx?siteId={siteId}", 520, 640);
        }

        public static string GetOpenWindowStringToEdit(int siteId, int ruleId)
        {
            return GovPublicUtils.GetOpenLayerString("修改规则", $"{nameof(ModalIdentifierRuleAdd)}.aspx?siteId={siteId}&ruleId={ruleId}", 520, 640);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            _ruleId = TranslateUtils.ToInt(Request.QueryString["ruleId"]);

            if (IsPostBack) return;

            EIdentifierTypeUtils.AddListItems(DdlIdentifierType);

            var inputStyles = Main.ContentRepository.InputStyles;

            foreach (var tableColumn in inputStyles)
            {
                if (tableColumn.InputType == null ||
                    tableColumn.AttributeName == nameof(IContentInfo.Title) ||
                    tableColumn.AttributeName == nameof(IContentInfo.Color) ||
                    tableColumn.AttributeName == nameof(IContentInfo.Hot) ||
                    tableColumn.AttributeName == nameof(IContentInfo.Recommend) ||
                    tableColumn.AttributeName == nameof(IContentInfo.Top) ||
                    tableColumn.AttributeName == ContentAttribute.Content ||
                    tableColumn.AttributeName == ContentAttribute.DepartmentId ||
                    tableColumn.AttributeName == ContentAttribute.Description ||
                    tableColumn.AttributeName == ContentAttribute.ImageUrl ||
                    tableColumn.AttributeName == ContentAttribute.FileUrl ||
                    tableColumn.AttributeName == ContentAttribute.Identifier ||
                    tableColumn.AttributeName == ContentAttribute.Keywords ||
                    tableColumn.AttributeName == ContentAttribute.DocumentNo ||
                    tableColumn.AttributeName == ContentAttribute.IsAbolition ||
                    tableColumn.AttributeName == ContentAttribute.Publisher)
                {
                    continue;
                }
                DdlAttributeName.Items.Add(new ListItem(tableColumn.DisplayName + "(" + tableColumn.AttributeName + ")", tableColumn.AttributeName));
            }

            GovPublicUtils.AddListItems(DdlIsSequenceChannelZero);
            GovPublicUtils.AddListItems(DdlIsSequenceDepartmentZero);
            GovPublicUtils.AddListItems(DdlIsSequenceYearZero);

            GovPublicUtils.SelectSingleItemIgnoreCase(DdlIsSequenceChannelZero, true.ToString());
            GovPublicUtils.SelectSingleItemIgnoreCase(DdlIsSequenceDepartmentZero, false.ToString());
            GovPublicUtils.SelectSingleItemIgnoreCase(DdlIsSequenceYearZero, true.ToString());

            if (_ruleId > 0)
            {
                var ruleInfo = Main.IdentifierRuleRepository.GetIdentifierRuleInfo(_ruleId);
                if (ruleInfo != null)
                {
                    TbRuleName.Text = ruleInfo.RuleName;
                    GovPublicUtils.SelectSingleItemIgnoreCase(DdlIdentifierType, ruleInfo.IdentifierType);
                    GovPublicUtils.SelectSingleItemIgnoreCase(DdlAttributeName, ruleInfo.AttributeName);
                    TbMinLength.Text = ruleInfo.MinLength.ToString();
                    TbFormatString.Text = ruleInfo.FormatString;
                    TbSuffix.Text = ruleInfo.Suffix;
                    TbSequence.Text = ruleInfo.Sequence.ToString();

                    GovPublicUtils.SelectSingleItemIgnoreCase(DdlIsSequenceChannelZero, ruleInfo.IsSequenceChannelZero.ToString());
                    GovPublicUtils.SelectSingleItemIgnoreCase(DdlIsSequenceDepartmentZero, ruleInfo.IsSequenceDepartmentZero.ToString());
                    GovPublicUtils.SelectSingleItemIgnoreCase(DdlIsSequenceYearZero, ruleInfo.IsSequenceYearZero.ToString());
                }
            }

            DdlIdentifierType_SelectedIndexChanged(null, EventArgs.Empty);
        }

        public void DdlIdentifierType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var identifierType = EIdentifierTypeUtils.GetEnumType(DdlIdentifierType.SelectedValue);
            if (identifierType == EIdentifierType.Department || identifierType == EIdentifierType.Channel)
            {
                PhAttributeName.Visible = false;
                PhFormatString.Visible = false;
                PhMinLength.Visible = true;
                PhSequence.Visible = false;
            }
            else if (identifierType == EIdentifierType.Sequence)
            {
                PhAttributeName.Visible = false;
                PhFormatString.Visible = false;
                PhMinLength.Visible = true;
                PhSequence.Visible = true;
            }
            else if (identifierType == EIdentifierType.Attribute)
            {
                PhAttributeName.Visible = true;
                PhFormatString.Visible = true;
                PhMinLength.Visible = true;
                PhSequence.Visible = false;
            }
        }

        public void Submit_OnClick(object sender, EventArgs e)
        {
            var ruleInfoList = Main.IdentifierRuleRepository.GetRuleInfoList(SiteId);
				
			if (_ruleId > 0)
			{
                var ruleInfo = Main.IdentifierRuleRepository.GetIdentifierRuleInfo(_ruleId);
                var identifierType = EIdentifierTypeUtils.GetEnumType(ruleInfo.IdentifierType);

                ruleInfo.RuleName = TbRuleName.Text;
                ruleInfo.IdentifierType = DdlIdentifierType.SelectedValue;
                ruleInfo.MinLength = TranslateUtils.ToInt(TbMinLength.Text);
                ruleInfo.Suffix = TbSuffix.Text;
                ruleInfo.FormatString = TbFormatString.Text;
                ruleInfo.AttributeName = DdlAttributeName.SelectedValue;
                ruleInfo.Sequence = TranslateUtils.ToInt(TbSequence.Text);

                if (identifierType == EIdentifierType.Sequence)
                {
                    ruleInfo.IsSequenceChannelZero = TranslateUtils.ToBool(DdlIsSequenceChannelZero.SelectedValue);
                    ruleInfo.IsSequenceDepartmentZero = TranslateUtils.ToBool(DdlIsSequenceDepartmentZero.SelectedValue);
                    ruleInfo.IsSequenceYearZero = TranslateUtils.ToBool(DdlIsSequenceYearZero.SelectedValue);
                }

                foreach (var identifierRuleInfo in ruleInfoList)
                {
                    if (identifierRuleInfo.Id == ruleInfo.Id) continue;
                    if (identifierType != EIdentifierType.Attribute && identifierRuleInfo.IdentifierType == ruleInfo.IdentifierType)
                    {
                        LtlMessage.Text = GovPublicUtils.GetMessageHtml("规则修改失败，本类型规则只能添加一次！", false);
                        return;
                    }
                    if (identifierRuleInfo.RuleName == TbRuleName.Text)
                    {
                        LtlMessage.Text = GovPublicUtils.GetMessageHtml("规则修改失败，规则名称已存在！", false);
                        return;
                    }
                }

                Main.IdentifierRuleRepository.Update(ruleInfo);
            }
			else
			{
                var identifierType = EIdentifierTypeUtils.GetEnumType(DdlIdentifierType.SelectedValue);

                foreach (var thrRuleInfo in ruleInfoList)
                {
                    var ruleIdentifierType = EIdentifierTypeUtils.GetEnumType(thrRuleInfo.IdentifierType);
                    if (identifierType != EIdentifierType.Attribute && identifierType == ruleIdentifierType)
                    {
                        LtlMessage.Text = GovPublicUtils.GetMessageHtml("规则添加失败，本类型规则只能添加一次！", false);
                        return;
                    }
                    if (thrRuleInfo.RuleName == TbRuleName.Text)
                    {
                        LtlMessage.Text = GovPublicUtils.GetMessageHtml("规则添加失败，规则名称已存在！", false);
                        return;
                    }
                }

                var ruleInfo = new IdentifierRuleInfo
                {
                    SiteId = SiteId,
                    RuleName = TbRuleName.Text,
                    IdentifierType = EIdentifierTypeUtils.GetValue(identifierType),
                    MinLength = TranslateUtils.ToInt(TbMinLength.Text),
                    Suffix = TbSuffix.Text,
                    FormatString = TbFormatString.Text,
                    AttributeName = DdlAttributeName.SelectedValue,
                    Sequence = TranslateUtils.ToInt(TbSequence.Text)
                };

                if (identifierType == EIdentifierType.Sequence)
                {
                    ruleInfo.IsSequenceChannelZero = TranslateUtils.ToBool(DdlIsSequenceChannelZero.SelectedValue);
                    ruleInfo.IsSequenceDepartmentZero = TranslateUtils.ToBool(DdlIsSequenceDepartmentZero.SelectedValue);
                    ruleInfo.IsSequenceYearZero = TranslateUtils.ToBool(DdlIsSequenceYearZero.SelectedValue);
                }

                Main.IdentifierRuleRepository.Insert(ruleInfo);
            }

            GovPublicUtils.CloseModalPage(Page);
        }
	}
}
