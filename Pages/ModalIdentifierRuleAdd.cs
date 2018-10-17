using System;
using System.Web.UI.WebControls;
using SiteServer.Plugin;
using SS.GovPublic.Core;
using SS.GovPublic.Model;
using SS.GovPublic.Provider;

namespace SS.GovPublic.Pages
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
            return Utils.GetOpenLayerString("添加规则", $"{nameof(ModalIdentifierRuleAdd)}.aspx?siteId={siteId}", 520, 640);
        }

        public static string GetOpenWindowStringToEdit(int siteId, int ruleId)
        {
            return Utils.GetOpenLayerString("修改规则", $"{nameof(ModalIdentifierRuleAdd)}.aspx?siteId={siteId}&ruleId={ruleId}", 520, 640);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            _ruleId = Utils.ToInt(Request.QueryString["ruleId"]);

            if (IsPostBack) return;

            EIdentifierTypeUtils.AddListItems(DdlIdentifierType);

            var tableColumns = ContentDao.Columns;

            foreach (var tableColumn in tableColumns)
            {
                if (tableColumn.InputStyle == null ||
                    tableColumn.AttributeName == nameof(IContentInfo.Title) ||
                    tableColumn.AttributeName == nameof(IContentInfo.IsColor) ||
                    tableColumn.AttributeName == nameof(IContentInfo.IsHot) ||
                    tableColumn.AttributeName == nameof(IContentInfo.IsRecommend) ||
                    tableColumn.AttributeName == nameof(IContentInfo.IsTop) ||
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
                DdlAttributeName.Items.Add(new ListItem(tableColumn.InputStyle.DisplayName + "(" + tableColumn.AttributeName + ")", tableColumn.AttributeName));
            }

            Utils.AddListItems(DdlIsSequenceChannelZero);
            Utils.AddListItems(DdlIsSequenceDepartmentZero);
            Utils.AddListItems(DdlIsSequenceYearZero);

            Utils.SelectSingleItemIgnoreCase(DdlIsSequenceChannelZero, true.ToString());
            Utils.SelectSingleItemIgnoreCase(DdlIsSequenceDepartmentZero, false.ToString());
            Utils.SelectSingleItemIgnoreCase(DdlIsSequenceYearZero, true.ToString());

            if (_ruleId > 0)
            {
                var ruleInfo = IdentifierRuleDao.GetIdentifierRuleInfo(_ruleId);
                if (ruleInfo != null)
                {
                    TbRuleName.Text = ruleInfo.RuleName;
                    Utils.SelectSingleItemIgnoreCase(DdlIdentifierType, ruleInfo.IdentifierType);
                    Utils.SelectSingleItemIgnoreCase(DdlAttributeName, ruleInfo.AttributeName);
                    TbMinLength.Text = ruleInfo.MinLength.ToString();
                    TbFormatString.Text = ruleInfo.FormatString;
                    TbSuffix.Text = ruleInfo.Suffix;
                    TbSequence.Text = ruleInfo.Sequence.ToString();

                    Utils.SelectSingleItemIgnoreCase(DdlIsSequenceChannelZero, ruleInfo.IsSequenceChannelZero.ToString());
                    Utils.SelectSingleItemIgnoreCase(DdlIsSequenceDepartmentZero, ruleInfo.IsSequenceDepartmentZero.ToString());
                    Utils.SelectSingleItemIgnoreCase(DdlIsSequenceYearZero, ruleInfo.IsSequenceYearZero.ToString());
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
            var ruleInfoList = IdentifierRuleDao.GetRuleInfoList(SiteId);
				
			if (_ruleId > 0)
			{
                var ruleInfo = IdentifierRuleDao.GetIdentifierRuleInfo(_ruleId);
                var identifierType = EIdentifierTypeUtils.GetEnumType(ruleInfo.IdentifierType);

                ruleInfo.RuleName = TbRuleName.Text;
                ruleInfo.IdentifierType = DdlIdentifierType.SelectedValue;
                ruleInfo.MinLength = Utils.ToInt(TbMinLength.Text);
                ruleInfo.Suffix = TbSuffix.Text;
                ruleInfo.FormatString = TbFormatString.Text;
                ruleInfo.AttributeName = DdlAttributeName.SelectedValue;
                ruleInfo.Sequence = Utils.ToInt(TbSequence.Text);

                if (identifierType == EIdentifierType.Sequence)
                {
                    ruleInfo.IsSequenceChannelZero = Utils.ToBool(DdlIsSequenceChannelZero.SelectedValue);
                    ruleInfo.IsSequenceDepartmentZero = Utils.ToBool(DdlIsSequenceDepartmentZero.SelectedValue);
                    ruleInfo.IsSequenceYearZero = Utils.ToBool(DdlIsSequenceYearZero.SelectedValue);
                }

                foreach (var identifierRuleInfo in ruleInfoList)
                {
                    if (identifierRuleInfo.Id == ruleInfo.Id) continue;
                    if (identifierType != EIdentifierType.Attribute && identifierRuleInfo.IdentifierType == ruleInfo.IdentifierType)
                    {
                        LtlMessage.Text = Utils.GetMessageHtml("规则修改失败，本类型规则只能添加一次！", false);
                        return;
                    }
                    if (identifierRuleInfo.RuleName == TbRuleName.Text)
                    {
                        LtlMessage.Text = Utils.GetMessageHtml("规则修改失败，规则名称已存在！", false);
                        return;
                    }
                }

                IdentifierRuleDao.Update(ruleInfo);
            }
			else
			{
                var identifierType = EIdentifierTypeUtils.GetEnumType(DdlIdentifierType.SelectedValue);

                foreach (var thrRuleInfo in ruleInfoList)
                {
                    var ruleIdentifierType = EIdentifierTypeUtils.GetEnumType(thrRuleInfo.IdentifierType);
                    if (identifierType != EIdentifierType.Attribute && identifierType == ruleIdentifierType)
                    {
                        LtlMessage.Text = Utils.GetMessageHtml("规则添加失败，本类型规则只能添加一次！", false);
                        return;
                    }
                    if (thrRuleInfo.RuleName == TbRuleName.Text)
                    {
                        LtlMessage.Text = Utils.GetMessageHtml("规则添加失败，规则名称已存在！", false);
                        return;
                    }
                }

                var ruleInfo = new IdentifierRuleInfo
                {
                    SiteId = SiteId,
                    RuleName = TbRuleName.Text,
                    IdentifierType = EIdentifierTypeUtils.GetValue(identifierType),
                    MinLength = Utils.ToInt(TbMinLength.Text),
                    Suffix = TbSuffix.Text,
                    FormatString = TbFormatString.Text,
                    AttributeName = DdlAttributeName.SelectedValue,
                    Sequence = Utils.ToInt(TbSequence.Text)
                };

                if (identifierType == EIdentifierType.Sequence)
                {
                    ruleInfo.IsSequenceChannelZero = Utils.ToBool(DdlIsSequenceChannelZero.SelectedValue);
                    ruleInfo.IsSequenceDepartmentZero = Utils.ToBool(DdlIsSequenceDepartmentZero.SelectedValue);
                    ruleInfo.IsSequenceYearZero = Utils.ToBool(DdlIsSequenceYearZero.SelectedValue);
                }

                IdentifierRuleDao.Insert(ruleInfo);
            }

            Utils.CloseModalPage(Page);
        }
	}
}
