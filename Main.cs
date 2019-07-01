using System.Collections.Generic;
using SiteServer.Plugin;
using SS.GovPublic.Core;
using SS.GovPublic.Core.Model;
using SS.GovPublic.Core.Pages;
using SS.GovPublic.Core.Provider;

namespace SS.GovPublic
{
    public class Main : PluginBase
    {
        public static ContentRepository ContentRepository { get; private set; }
        public static CategoryClassRepository CategoryClassRepository { get; private set; }
        public static CategoryRepository CategoryRepository { get; private set; }
        public static DepartmentRepository DepartmentRepository { get; private set; }
        public static IdentifierRuleRepository IdentifierRuleRepository { get; private set; }
        public static IdentifierSeqRepository IdentifierSeqRepository { get; private set; }

        public override void Startup(IService service)
        {
            ContentRepository = new ContentRepository();
            CategoryClassRepository = new CategoryClassRepository();
            CategoryRepository = new CategoryRepository();
            DepartmentRepository = new DepartmentRepository();
            IdentifierRuleRepository = new IdentifierRuleRepository();
            IdentifierSeqRepository = new IdentifierSeqRepository();

            service
                .AddContentModel(ContentRepository.TableName, ContentRepository.TableColumns, ContentRepository.InputStyles)
                .AddDatabaseTable(CategoryClassRepository.TableName, CategoryClassRepository.TableColumns)
                .AddDatabaseTable(CategoryRepository.TableName, CategoryRepository.TableColumns)
                .AddDatabaseTable(DepartmentRepository.TableName, DepartmentRepository.TableColumns)
                .AddDatabaseTable(IdentifierRuleRepository.TableName, IdentifierRuleRepository.TableColumns)
                .AddDatabaseTable(IdentifierSeqRepository.TableName, IdentifierSeqRepository.TableColumns)
                .AddSiteMenu(siteId => new Menu
                {
                    Text = "主动信息公开",
                    IconClass = "ion-ios-book",
                    Menus = new List<Menu>
                    {
                        new Menu
                        {
                            Text = "信息采集",
                            Href = PageMain.GetRedirectUrl(siteId, Context.UtilsApi.GetAdminUrl($"cms/pageContentAdd.aspx?siteId={siteId}"))
                        },
                        new Menu
                        {
                            Text = "信息管理",
                            Href = PageMain.GetRedirectUrl(siteId, Context.UtilsApi.GetAdminUrl($"cms/contents.cshtml?siteId={siteId}"))
                        },
                        new Menu
                        {
                            Text = "信息审核",
                            Href = PageMain.GetRedirectUrl(siteId, Context.UtilsApi.GetAdminUrl($"cms/pageContentSearch.aspx?isCheckOnly=true&siteId={siteId}"))
                        },
                        new Menu
                        {
                            Text = "分类法管理",
                            Href = PageInit.GetRedirectUrl(siteId, PageCategoryMain.GetRedirectUrl(siteId))
                        },
                        new Menu
                        {
                            Text = "索引号生成规则",
                            Href = PageInit.GetRedirectUrl(siteId, PageIdentifierRule.GetRedirectUrl(siteId))
                        },
                        new Menu
                        {
                            Text = "重新生成索引号",
                            Href = PageInit.GetRedirectUrl(siteId, PageIdentifierCreate.GetRedirectUrl(siteId))
                        },
                        new Menu
                        {
                            Text = "部门设置",
                            Href = "pages/departments.html"
                        },
                        new Menu
                        {
                            Text = "信息公开设置",
                            Href = PageInit.GetRedirectUrl(siteId, PageSettings.GetRedirectUrl(siteId))
                        },
                        new Menu
                        {
                            Text = "数据统计分析",
                            Href = PageInit.GetRedirectUrl(siteId, PageAnalysis.GetRedirectUrl(siteId))
                        }
                    }
                });

            service.ContentFormSubmit += Service_ContentFormSubmited; // 页面提交处理函数
            service.ContentFormLoad += Service_ContentFormLoad; // // 页面加载处理函数
        }

        private string Service_ContentFormLoad(object sender, ContentFormLoadEventArgs e)
        {
            if (!StringUtils.EqualsIgnoreCase(e.AttributeName,  ContentAttribute.Identifier)) return null;

            e.Form.TryGetValue(ContentAttribute.Identifier, out var identifier);

            return $@"
<div class=""form-group form-row"">
    <label class=""col-sm-1 col-form-label text-right"">信息分类</label>
    <div class=""col-sm-10"">
        {ContentRepository.GetCategoriesHtml(e.SiteId, e.ChannelId, e.Form)}
    </div>
    <div class=""col-sm-1"">
       
    </div>
</div>
<div class=""form-group form-row"">
    <label class=""col-sm-1 col-form-label text-right"">索引号</label>
    <div class=""col-sm-6"">
        <input id=""displayOnly{ContentAttribute.Identifier}"" name=""displayOnly{ContentAttribute.Identifier}"" type=""text"" class=""form-control"" disabled=""disabled"" value=""{identifier}"">
        <input id=""{ContentAttribute.Identifier}"" name=""{ContentAttribute.Identifier}"" type=""hidden"" value=""{identifier}"">
    </div>
    <div class=""col-sm-5"">
       <span class=""form-text text-muted"">索引号由系统自动生成</span>
    </div>
</div>
                    ";

        }

        private void Service_ContentFormSubmited(object sender, ContentFormSubmitEventArgs e)
        {
            ContentRepository.ContentFormSubmited(e.SiteId, e.ChannelId, e.ContentInfo, e.Form);
        }
    }
}