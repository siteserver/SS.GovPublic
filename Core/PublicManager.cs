﻿using System;
using System.Collections.Generic;
using System.Text;
using SiteServer.Plugin;
using SS.GovPublic.Core.Model;
using SS.GovPublic.Core.Provider;
using SS.GovPublic.Core.Utils;

namespace SS.GovPublic.Core
{
	public class PublicManager
	{
        public static List<IChannelInfo> GetPublicChannelInfoList(int siteId)
        {
            var channelInfoList = new List<IChannelInfo>();

            var channelIdList = Main.ChannelApi.GetChannelIdList(siteId);
            foreach (var channelId in channelIdList)
            {
                var channelInfo = Main.ChannelApi.GetChannelInfo(siteId, channelId);
                if (channelInfo.ContentModelPluginId == Main.PluginId)
                {
                    channelInfoList.Add(channelInfo);
                }
            }

            return channelInfoList;
        }

	    public static string GetCategoryContentAttributeName(string classCode)
	    {
	        return $"category{classCode}Id";
	    }

        public static string GetPreviewIdentifier(int siteId)
        {
            var builder = new StringBuilder();

            var ruleInfoList = Main.IdentifierRuleRepository.GetRuleInfoList(siteId);
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
            var ruleInfoList = Main.IdentifierRuleRepository.GetRuleInfoList(siteId);
            foreach (var ruleInfo in ruleInfoList)
            {
                var identifierType = EIdentifierTypeUtils.GetEnumType(ruleInfo.IdentifierType);
                if (identifierType == EIdentifierType.Department)
                {
                    if (contentInfo.Get<int>(nameof(ContentAttribute.DepartmentId)) != departmentId)
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
                    if (GovPublicUtils.EqualsIgnoreCase(ruleInfo.AttributeName, ContentAttribute.EffectDate) && TranslateUtils.ToDateTime(contentInfo.Get<string>(ruleInfo.AttributeName)) != effectDate)
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
            var nodeInfo = Main.ChannelApi.GetChannelInfo(siteId, channelId);

            var ruleInfoList = Main.IdentifierRuleRepository.GetRuleInfoList(siteId);
            foreach (var ruleInfo in ruleInfoList)
            {
                var identifierType = EIdentifierTypeUtils.GetEnumType(ruleInfo.IdentifierType);
                if (identifierType == EIdentifierType.Department)
                {
                    var departmentCode = DepartmentManager.GetDepartmentCode(siteId, departmentId);
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
                        var dateTime = TranslateUtils.ToDateTime(contentInfo.Get<string>(ruleInfo.AttributeName));
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
                        var attributeValue = contentInfo.Get<string>(ruleInfo.AttributeName);
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
                    if (ruleInfo.IsSequenceYearZero && contentInfo.AddDate != null)
                    {
                        targetAddYear = contentInfo.AddDate.Value.Year;
                    }

                    var sequence = Main.IdentifierSeqRepository.GetSequence(targetSiteId, targetChannelId, targetDepartmentId, targetAddYear, ruleInfo.Sequence);

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
