using System;
using System.Collections.Generic;
using System.Text;
using SiteServer.Plugin;
using SS.GovPublic.Model;

namespace SS.GovPublic.Core
{
	public class GovPublicManager
	{
        private static CategoryClassInfo GetCategoryClassInfo(ECategoryClassType categoryType, int siteId)
        {
            var isSystem = categoryType == ECategoryClassType.Channel || categoryType == ECategoryClassType.Department;
            return new CategoryClassInfo(0, siteId, ECategoryClassTypeUtils.GetValue(categoryType),
                ECategoryClassTypeUtils.GetText(categoryType), isSystem, true, string.Empty, 0, string.Empty);
        }

	    public static string GetCategoryContentAttributeName(string classCode)
	    {
	        return $"category{classCode}Id";
	    }

        public static void Initialize(int siteId)
        {
            var configInfo = Main.Instance.GetConfigInfo(siteId);
            if (configInfo.GovPublicChannelId > 0)
            {
                var nodeInfo = Main.Instance.ChannelApi.GetChannelInfo(siteId, configInfo.GovPublicChannelId);
                if (nodeInfo == null || nodeInfo.ContentModelPluginId != Main.Instance.Id)
                {
                    configInfo.GovPublicChannelId = 0;
                }
            }
            if (configInfo.GovPublicChannelId == 0)
            {
                var nodeInfo = Main.Instance.ChannelApi.NewInstance(siteId);
                nodeInfo.ContentModelPluginId = Main.Instance.Id;
                nodeInfo.ChannelName = "信息公开";
                configInfo.GovPublicChannelId = Main.Instance.ChannelApi.Insert(siteId, nodeInfo);
                Main.Instance.ConfigApi.SetConfig(siteId, configInfo);
            }

            if (Main.CategoryClassDao.GetCount(siteId) == 0)
            {
                var categoryClassInfoList = new List<CategoryClassInfo>
                {
                    GetCategoryClassInfo(ECategoryClassType.Channel, siteId),
                    GetCategoryClassInfo(ECategoryClassType.Department, siteId),
                    GetCategoryClassInfo(ECategoryClassType.Form, siteId),
                    GetCategoryClassInfo(ECategoryClassType.Service, siteId)
                };

                foreach (var categoryClassInfo in categoryClassInfoList)
                {
                    Main.CategoryClassDao.Insert(categoryClassInfo);
                }
            }

            if (Main.IdentifierRuleDao.GetCount(siteId) == 0)
            {
                var ruleInfoList = new List<IdentifierRuleInfo>
                {
                    new IdentifierRuleInfo
                    {
                        SiteId = siteId,
                        RuleName = "机构分类代码",
                        IdentifierType = EIdentifierTypeUtils.GetValue(EIdentifierType.Department),
                        MinLength = 5,
                        Suffix = "-"
                    },
                    new IdentifierRuleInfo
                    {
                        SiteId = siteId,
                        RuleName = "主题分类代码",
                        IdentifierType = EIdentifierTypeUtils.GetValue(EIdentifierType.Channel),
                        MinLength = 5,
                        Suffix = "-"
                    },
                    new IdentifierRuleInfo
                    {
                        SiteId = siteId,
                        RuleName = "生效日期",
                        IdentifierType = EIdentifierTypeUtils.GetValue(EIdentifierType.Attribute),
                        Suffix = "-",
                        AttributeName = ContentAttribute.EffectDate,
                        FormatString = "yyyy"
                    },
                    new IdentifierRuleInfo
                    {
                        SiteId = siteId,
                        RuleName = "顺序号",
                        IdentifierType = EIdentifierTypeUtils.GetValue(EIdentifierType.Sequence),
                        MinLength = 5
                    }
                };

                foreach (var ruleInfo in ruleInfoList)
                {
                    Main.IdentifierRuleDao.Insert(ruleInfo);
                }
            }
        }

        public static string GetPreviewIdentifier(int siteId)
        {
            var builder = new StringBuilder();

            var ruleInfoList = Main.IdentifierRuleDao.GetRuleInfoList(siteId);
            foreach (var ruleInfo in ruleInfoList)
            {
                var identifierType = EIdentifierTypeUtils.GetEnumType(ruleInfo.IdentifierType);
                if (identifierType == EIdentifierType.Department)
                {
                    if (ruleInfo.MinLength > 0)
                    {
                        builder.Append("D123".PadLeft(ruleInfo.MinLength, '0')).Append(ruleInfo.Suffix);
                    }
                    else
                    {
                        builder.Append("D123").Append(ruleInfo.Suffix);
                    }
                }
                else if (identifierType == EIdentifierType.Channel)
                {
                    if (ruleInfo.MinLength > 0)
                    {
                        builder.Append("C123".PadLeft(ruleInfo.MinLength, '0')).Append(ruleInfo.Suffix);
                    }
                    else
                    {
                        builder.Append("C123").Append(ruleInfo.Suffix);
                    }
                }
                else if (identifierType == EIdentifierType.Attribute)
                {
                    if (ruleInfo.AttributeName == ContentAttribute.AbolitionDate || ruleInfo.AttributeName == ContentAttribute.EffectDate || ruleInfo.AttributeName == ContentAttribute.PublishDate || ruleInfo.AttributeName == nameof(IContentInfo.AddDate))
                    {
                        if (ruleInfo.MinLength > 0)
                        {
                            builder.Append(DateTime.Now.ToString(ruleInfo.FormatString).PadLeft(ruleInfo.MinLength, '0')).Append(ruleInfo.Suffix);
                        }
                        else
                        {
                            builder.Append(DateTime.Now.ToString(ruleInfo.FormatString)).Append(ruleInfo.Suffix);
                        }
                    }
                    else
                    {
                        if (ruleInfo.MinLength > 0)
                        {
                            builder.Append("A123".PadLeft(ruleInfo.MinLength, '0')).Append(ruleInfo.Suffix);
                        }
                        else
                        {
                            builder.Append("A123").Append(ruleInfo.Suffix);
                        }
                    }
                }
                else if (identifierType == EIdentifierType.Sequence)
                {
                    if (ruleInfo.MinLength > 0)
                    {
                        builder.Append("1".PadLeft(ruleInfo.MinLength, '0')).Append(ruleInfo.Suffix);
                    }
                    else
                    {
                        builder.Append("1").Append(ruleInfo.Suffix);
                    }
                }
            }

            return builder.ToString();
        }

        public static bool IsIdentifierChanged(int siteId, int channelId, int departmentId, DateTime effectDate, IContentInfo contentInfo)
        {
            var isIdentifierChanged = false;
            var ruleInfoList = Main.IdentifierRuleDao.GetRuleInfoList(siteId);
            foreach (var ruleInfo in ruleInfoList)
            {
                var identifierType = EIdentifierTypeUtils.GetEnumType(ruleInfo.IdentifierType);
                if (identifierType == EIdentifierType.Department)
                {
                    if (contentInfo.GetInt(nameof(ContentAttribute.DepartmentId)) != departmentId)
                    {
                        isIdentifierChanged = true;
                    }
                }
                else if (identifierType == EIdentifierType.Channel)
                {
                    if (contentInfo.Id != channelId)
                    {
                        isIdentifierChanged = true;
                    }
                }
                else if (identifierType == EIdentifierType.Attribute)
                {
                    if (Utils.EqualsIgnoreCase(ruleInfo.AttributeName, ContentAttribute.EffectDate) && Utils.ToDateTime(contentInfo.GetString(ruleInfo.AttributeName)) != effectDate)
                    {
                        isIdentifierChanged = true;
                    }
                }
            }
            return isIdentifierChanged;
        }

        public static string GetIdentifier(int siteId, int channelId, int departmentId, IContentInfo contentInfo)
        {
            var builder = new StringBuilder();
            var nodeInfo = Main.Instance.ChannelApi.GetChannelInfo(siteId, channelId);

            var ruleInfoList = Main.IdentifierRuleDao.GetRuleInfoList(siteId);
            foreach (var ruleInfo in ruleInfoList)
            {
                var identifierType = EIdentifierTypeUtils.GetEnumType(ruleInfo.IdentifierType);
                if (identifierType == EIdentifierType.Department)
                {
                    var departmentCode = Main.Dao.GetDepartmentCode(departmentId);
                    if (ruleInfo.MinLength > 0)
                    {
                        builder.Append(departmentCode.PadLeft(ruleInfo.MinLength, '0')).Append(ruleInfo.Suffix);
                    }
                    else
                    {
                        builder.Append(departmentCode).Append(ruleInfo.Suffix);
                    }
                }
                else if (identifierType == EIdentifierType.Channel)
                {
                    var channelCode = nodeInfo.IndexName;
                    if (ruleInfo.MinLength > 0)
                    {
                        builder.Append(channelCode.PadLeft(ruleInfo.MinLength, '0')).Append(ruleInfo.Suffix);
                    }
                    else
                    {
                        builder.Append(channelCode).Append(ruleInfo.Suffix);
                    }
                }
                else if (identifierType == EIdentifierType.Attribute)
                {
                    if (ruleInfo.AttributeName == ContentAttribute.AbolitionDate || ruleInfo.AttributeName == ContentAttribute.EffectDate || ruleInfo.AttributeName == ContentAttribute.PublishDate || ruleInfo.AttributeName == nameof(IContentInfo.AddDate))
                    {
                        var dateTime = Utils.ToDateTime(contentInfo.GetString(ruleInfo.AttributeName));
                        if (ruleInfo.MinLength > 0)
                        {
                            builder.Append(dateTime.ToString(ruleInfo.FormatString).PadLeft(ruleInfo.MinLength, '0')).Append(ruleInfo.Suffix);
                        }
                        else
                        {
                            builder.Append(dateTime.ToString(ruleInfo.FormatString)).Append(ruleInfo.Suffix);
                        }
                    }
                    else
                    {
                        var attributeValue = contentInfo.GetString(ruleInfo.AttributeName);
                        if (ruleInfo.MinLength > 0)
                        {
                            builder.Append(attributeValue.PadLeft(ruleInfo.MinLength, '0')).Append(ruleInfo.Suffix);
                        }
                        else
                        {
                            builder.Append(attributeValue).Append(ruleInfo.Suffix);
                        }
                    }
                }
                else if (identifierType == EIdentifierType.Sequence)
                {
                    var targetSiteId = siteId;
                    var targetChannelId = 0;
                    if (ruleInfo.IsSequenceChannelZero)
                    {
                        targetChannelId = nodeInfo.Id;
                    }
                    var targetDepartmentId = 0;
                    if (ruleInfo.IsSequenceDepartmentZero)
                    {
                        targetDepartmentId = departmentId;
                    }
                    var targetAddYear = 0;
                    if (ruleInfo.IsSequenceYearZero)
                    {
                        targetAddYear = contentInfo.AddDate.Year;
                    }

                    var sequence = Main.IdentifierSeqDao.GetSequence(targetSiteId, targetChannelId, targetDepartmentId, targetAddYear, ruleInfo.Sequence);

                    if (ruleInfo.MinLength > 0)
                    {
                        builder.Append(sequence.ToString().PadLeft(ruleInfo.MinLength, '0')).Append(ruleInfo.Suffix);
                    }
                    else
                    {
                        builder.Append(sequence.ToString()).Append(ruleInfo.Suffix);
                    }
                }
            }

            return builder.ToString();
        }
    }
}
