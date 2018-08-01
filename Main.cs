using System.Collections.Generic;
using SiteServer.Plugin;
using SS.GovPublic.Model;
using SS.GovPublic.Pages;
using SS.GovPublic.Provider;

namespace SS.GovPublic
{
    public class Main : PluginBase
    {
        public static Dao Dao { get; private set; }
        public static CategoryClassDao CategoryClassDao { get; private set; }
        public static CategoryDao CategoryDao { get; private set; }
        public static ContentDao ContentDao { get; private set; }
        public static DepartmentDao DepartmentDao { get; private set; }
        public static IdentifierRuleDao IdentifierRuleDao { get; private set; }
        public static IdentifierSeqDao IdentifierSeqDao { get; private set; }

        public static Main Instance { get; private set; }

        public override void Startup(IService service)
        {
            Instance = this;

            Dao = new Dao();
            CategoryClassDao = new CategoryClassDao();
            CategoryDao = new CategoryDao();
            ContentDao = new ContentDao();
            DepartmentDao = new DepartmentDao();
            IdentifierRuleDao = new IdentifierRuleDao();
            IdentifierSeqDao = new IdentifierSeqDao();

            service
                .AddContentModel(ContentDao.TableName, ContentDao.Columns)
                .AddDatabaseTable(CategoryClassDao.TableName, CategoryClassDao.Columns)
                .AddDatabaseTable(CategoryDao.TableName, CategoryDao.Columns)
                .AddDatabaseTable(IdentifierRuleDao.TableName, IdentifierRuleDao.Columns)
                .AddDatabaseTable(IdentifierSeqDao.TableName, IdentifierSeqDao.Columns)
                .AddSiteMenu(siteId => new Menu
                {
                    Text = "主动信息公开",
                    IconClass = "ion-ios-book",
                    Menus = new List<Menu>
                    {
                        new Menu
                        {
                            Text = "信息采集",
                            Href = PageMain.GetRedirectUrl(siteId, $"@/cms/pageContentAdd.aspx?siteId={siteId}")
                        },
                        new Menu
                        {
                            Text = "信息管理",
                            Href = PageMain.GetRedirectUrl(siteId, $"@/cms/pageContentSearch.aspx?siteId={siteId}")
                        },
                        new Menu
                        {
                            Text = "信息审核",
                            Href = PageMain.GetRedirectUrl(siteId, $"@/cms/pageContentSearch.aspx?isCheckOnly=true&siteId={siteId}")
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
            if(e.AttributeName == ContentAttribute.Identifier)
            {
                var identifier = e.Form.GetString(nameof(ContentAttribute.Identifier));
                return $@"
<div class=""form-group form-row"">
    <label class=""col-sm-1 col-form-label text-right"">信息分类</label>
    <div class=""col-sm-10"">
        {ContentDao.GetCategoriesHtml(e.SiteId, e.ChannelId, e.Form)}
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

            return null;
        }

        private void Service_ContentFormSubmited(object sender, ContentFormSubmitEventArgs e)
        {
            ContentDao.ContentFormSubmited(e.SiteId, e.ChannelId, e.ContentInfo, e.Form);
        }
    }
}