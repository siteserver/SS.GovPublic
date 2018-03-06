using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.Plugin;
using SS.GovPublic.Core;
using SS.GovPublic.Model;

namespace SS.GovPublic.Provider
{
    public class ContentDao
    {
        public const string TableName = "ss_govpublic_content";

        public static List<TableColumn> Columns => new List<TableColumn>
        {
            new TableColumn
            {
                AttributeName = ContentAttribute.Identifier,
                DataType = DataType.VarChar,
                DataLength = 200,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Customize,
                    DisplayName = "索引号"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.DocumentNo,
                DataType = DataType.VarChar,
                DataLength = 200,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Text,
                    DisplayName = "文号",
                    IsRequired = true
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.DepartmentId,
                DataType = DataType.Integer,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Hidden,
                    DisplayName = "部门",
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.Publisher,
                DataType = DataType.VarChar,
                DataLength = 200,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Text,
                    DisplayName = "发布机构"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.Keywords,
                DataType = DataType.VarChar,
                DataLength = 200,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Text,
                    DisplayName = "关键词"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.PublishDate,
                DataType = DataType.DateTime,
                InputStyle = new InputStyle
                {
                    InputType = InputType.DateTime,
                    DisplayName = "发文日期",
                    IsRequired = true,
                    DefaultValue = "{Current}"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.EffectDate,
                DataType = DataType.DateTime,
                InputStyle = new InputStyle
                {
                    InputType = InputType.DateTime,
                    DisplayName = "生效日期",
                    IsRequired = true,
                    DefaultValue = "{Current}"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.IsAbolition,
                DataType = DataType.VarChar,
                DataLength = 10,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Radio,
                    DisplayName = "是否废止",
                    IsRequired = true,
                    ListItems = new List<ListItem>
                    {
                        new ListItem
                        {
                            Text= "是",
                            Value = true.ToString(),
                            Selected = false
                        },
                        new ListItem
                        {
                            Text= "否",
                            Value = false.ToString(),
                            Selected = true
                        },
                    }
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.AbolitionDate,
                DataType = DataType.DateTime,
                InputStyle = new InputStyle
                {
                    InputType = InputType.DateTime,
                    DisplayName = "废止日期",
                    IsRequired = true,
                    DefaultValue = "{Current}"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.Description,
                DataType = DataType.VarChar,
                DataLength = 2000,
                InputStyle = new InputStyle
                {
                    InputType = InputType.TextArea,
                    DisplayName = "内容概述"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.ImageUrl,
                DataType = DataType.VarChar,
                DataLength = 200,
                InputStyle = new InputStyle
                {
                    InputType = InputType.Image,
                    DisplayName = "图片"
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.FileUrl,
                DataType = DataType.VarChar,
                DataLength = 200,
                InputStyle = new InputStyle
                {
                    InputType = InputType.File,
                    DisplayName = "附件",
                }
            },
            new TableColumn
            {
                AttributeName = ContentAttribute.Content,
                DataType = DataType.Text,
                InputStyle = new InputStyle
                {
                    InputType = InputType.TextEditor,
                    DisplayName = "内容"
                }
            }
        };

        

        public static string GetIdentifierHtml(int siteId, int channelId, IAttributes attributes)
        {
            var identifier = attributes.GetString(nameof(ContentAttribute.Identifier));

            return $@"
<div class=""form-group"">
    <label class=""col-sm-1 control-label"">信息分类</label>
    <div class=""col-sm-6"">
        {GetCategoriesHtml(siteId, channelId, attributes)}
    </div>
    <div class=""col-sm-5"">
    </div>
</div>
<div class=""form-group"">
    <label class=""col-sm-1 control-label"">索引号</label>
    <div class=""col-sm-6"">
        <input id=""displayOnly{ContentAttribute.Identifier}"" name=""displayOnly{ContentAttribute.Identifier}"" type=""text"" class=""form-control"" disabled=""disabled"" value=""{identifier}"">
        <input id=""{ContentAttribute.Identifier}"" name=""{ContentAttribute.Identifier}"" type=""hidden"" value=""{identifier}"">
    </div>
    <div class=""col-sm-5"">
        <span class=""help-block"">索引号由系统自动生成</span>
    </div>
</div>
";
        }

        public static string GetCategoriesHtml(int siteId, int channelId, IAttributes attributes)
        {
            var pairList = new List<KeyValuePair<string, DropDownList>>();

            var ddlChannelId = new DropDownList
            {
                ID = "categoryChannelId",
                CssClass = "form-control"
            };
            var channelIdList = Main.Instance.ChannelApi.GetChannelIdList(siteId);
            var nodeCount = channelIdList.Count;
            var isLastNodeArray = new bool[nodeCount];
            foreach (var theChannelId in channelIdList)
            {
                var nodeInfo = Main.Instance.ChannelApi.GetChannelInfo(siteId, theChannelId);
                var listItem = new ListItem(Utils.GetSelectOptionText(nodeInfo.ChannelName, nodeInfo.ParentsCount, nodeInfo.IsLastNode, isLastNodeArray),
                    nodeInfo.Id.ToString());
                if (nodeInfo.ContentModelPluginId != Main.Instance.Id)
                {
                    listItem.Value = "0";
                    listItem.Attributes.Add("disabled", "disabled");
                    listItem.Attributes.Add("style", "background-color:#f0f0f0;color:#9e9e9e");
                }
                ddlChannelId.Items.Add(listItem);
            }
            Utils.SelectSingleItem(ddlChannelId, channelId.ToString());
            pairList.Add(new KeyValuePair<string, DropDownList>("主题", ddlChannelId));

            var ddlDepartmentId = new DropDownList
            {
                ID = "categoryDepartmentId",
                CssClass = "form-control"
            };
            var departmentInfoList = Main.Dao.GetDepartmentInfoList();
            isLastNodeArray = new bool[departmentInfoList.Count];
            foreach (var departmentInfo in departmentInfoList)
            {
                var listItem = new ListItem(Utils.GetSelectOptionText(departmentInfo.DepartmentName, departmentInfo.ParentsCount, departmentInfo.IsLastNode, isLastNodeArray),
                    departmentInfo.Id.ToString());
                ddlDepartmentId.Items.Add(listItem);
            }
            Utils.SelectSingleItem(ddlDepartmentId, attributes.GetString(nameof(ContentAttribute.DepartmentId)));
            pairList.Add(new KeyValuePair<string, DropDownList>("机构", ddlDepartmentId));

            var classInfoList = Main.CategoryClassDao.GetCategoryClassInfoList(siteId);
            foreach (var classInfo in classInfoList)
            {
                if (classInfo.IsSystem || !classInfo.IsEnabled) continue;

                var attributeName = GovPublicManager.GetCategoryContentAttributeName(classInfo.ClassCode);
                var attributeValue = attributes.GetString(attributeName);

                var ddlCategoryId = new DropDownList
                {
                    ID = attributeName,
                    CssClass = "form-control"
                };
                var categoryIdList = Main.CategoryDao.GetCategoryIdList(siteId, classInfo.ClassCode);
                isLastNodeArray = new bool[categoryIdList.Count];
                foreach (var categoryId in categoryIdList)
                {
                    var categoryInfo = Main.CategoryDao.GetCategoryInfo(categoryId);
                    var listItem = new ListItem(Utils.GetSelectOptionText(categoryInfo.CategoryName, categoryInfo.ParentsCount, categoryInfo.IsLastNode, isLastNodeArray), categoryInfo.Id.ToString());
                    ddlCategoryId.Items.Add(listItem);
                }
                Utils.SelectSingleItem(ddlCategoryId, attributeValue);
                pairList.Add(new KeyValuePair<string, DropDownList>(classInfo.ClassName, ddlCategoryId));
            }

            var builder = new StringBuilder();
            builder.Append(@"<div class=""row"">");
            var count = 0;
            foreach (var keyValuePair in pairList)
            {
                if (count > 1)
                {
                    builder.Append(@"</div><div class=""row m-t-10"">");
                    count = 0;
                }
                builder.Append($@"
<div class=""col-xs-2 control-label"">{keyValuePair.Key}分类</div>
<div class=""col-xs-4"">
    {Utils.GetControlRenderHtml(keyValuePair.Value)}
</div>
");
                count++;
            }
            builder.Append("</div>");

            builder.Append(@"<script>
$(document).ready(function () {
    if ($('#Publisher').val().length == 0) $('#Publisher').val($('#categoryDepartmentId').find('option:selected').text().replace(/[　│└├]/g,''));
    $('#categoryDepartmentId').change(function(){
        $('#Publisher').val($(this).children('option:selected').text().replace(/[　│└├]/g,''));
    });
    $('input[type=""radio""][name=""IsAbolition""]').change(function(){
        var isAbolition = $('input[type=""radio""][name=""IsAbolition""]:checked').val();
        isAbolition == 'True' ? $('#AbolitionDate').parent().parent().show() : $('#AbolitionDate').parent().parent().hide();
    });
    var isAbolition = $('input[type=""radio""][name=""IsAbolition""]:checked').val();
    isAbolition == 'True' ? $('#AbolitionDate').parent().parent().show() : $('#AbolitionDate').parent().parent().hide();
});
</script>");

            return builder.ToString();
        }

        public static void ContentFormSubmited(int siteId, int channelId, IContentInfo contentInfo, IAttributes form)
        {
            var categoryChannelId = form.GetInt("categoryChannelId");
            var categoryDepartmentId = form.GetInt("categoryDepartmentId");
            if (categoryChannelId == 0)
            {
                throw new Exception("请选择正确的主题分类");
            }
            contentInfo.ChannelId = categoryChannelId;
            if (categoryDepartmentId == 0)
            {
                throw new Exception("请选择正确的机构分类");
            }
            contentInfo.Set(nameof(ContentAttribute.DepartmentId), categoryDepartmentId.ToString());

            var classInfoList = Main.CategoryClassDao.GetCategoryClassInfoList(siteId);
            foreach (var classInfo in classInfoList)
            {
                if (classInfo.IsSystem || !classInfo.IsEnabled) continue;

                var attributeName = GovPublicManager.GetCategoryContentAttributeName(classInfo.ClassCode);
                var attributeValue = form.GetInt(attributeName);

                contentInfo.Set(attributeName, attributeValue.ToString());

                //if (attributeValue > 0)
                //{
                //    Main.CategoryDao.UpdateContentNum(PublishmentSystemInfo, categoryClassInfo.ContentAttributeName, categoryId);
                //}
            }

            if (contentInfo.Id == 0)
            {
                var identifier = GovPublicManager.GetIdentifier(siteId, categoryChannelId,
                    categoryDepartmentId, contentInfo);
                contentInfo.Set(nameof(ContentAttribute.Identifier), identifier);
            }
            else
            {
                var effectDate = contentInfo.GetDateTime(nameof(ContentAttribute.EffectDate), DateTime.Now);
                if (string.IsNullOrEmpty(contentInfo.GetString(ContentAttribute.Identifier)) || GovPublicManager.IsIdentifierChanged(siteId, categoryChannelId, categoryDepartmentId, effectDate, contentInfo))
                {
                    var identifier = GovPublicManager.GetIdentifier(siteId, categoryChannelId,
                    categoryDepartmentId, contentInfo);
                    contentInfo.Set(nameof(ContentAttribute.Identifier), identifier);
                }
            }
        }

        private readonly string _connectionString;
        private readonly IDataApi _helper;

        public ContentDao()
        {
            _connectionString = Main.Instance.ConnectionString;
            _helper = Main.Instance.DataApi;
        }

        public void CreateIdentifier(int siteId, int parentChannelId, bool isAll)
        {
            var channelIdList = Main.Instance.ChannelApi.GetChannelIdList(siteId, parentChannelId);
            channelIdList.Add(parentChannelId);
            foreach (var channelId in channelIdList)
            {
                var contentIdList = Main.Instance.ContentApi.GetContentIdList(siteId, channelId);
                foreach (var contentId in contentIdList)
                {
                    var contentInfo = Main.Instance.ContentApi.GetContentInfo(siteId, channelId, contentId);
                    if (isAll || string.IsNullOrEmpty(contentInfo.GetString(ContentAttribute.Identifier)))
                    {
                        var identifier = GovPublicManager.GetIdentifier(siteId, contentInfo.ChannelId, contentInfo.GetInt(ContentAttribute.DepartmentId), contentInfo);
                        contentInfo.Set(ContentAttribute.Identifier, identifier);
                        Main.Instance.ContentApi.Update(siteId, channelId, contentInfo);
                    }
                }
            }
        }
    }
}
