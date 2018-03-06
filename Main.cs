using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using SiteServer.Plugin;
using SS.GovPublic.Core;
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
        public static IdentifierRuleDao IdentifierRuleDao { get; private set; }
        public static IdentifierSeqDao IdentifierSeqDao { get; private set; }

        public ConfigInfo GetConfigInfo(int siteId)
        {
            return ConfigApi.GetConfig<ConfigInfo>(siteId) ?? new ConfigInfo();
        }

        public static Main Instance { get; private set; }

        public override void Startup(IService service)
        {
            Instance = this;

            Dao = new Dao();
            CategoryClassDao = new CategoryClassDao();
            CategoryDao = new CategoryDao();
            ContentDao = new ContentDao();
            IdentifierRuleDao = new IdentifierRuleDao();
            IdentifierSeqDao = new IdentifierSeqDao();

            service
                .AddContentModel(ContentDao.TableName, ContentDao.Columns)
                .AddDatabaseTable(CategoryClassDao.TableName, CategoryClassDao.Columns)
                .AddDatabaseTable(CategoryDao.TableName, CategoryDao.Columns)
                .AddDatabaseTable(IdentifierRuleDao.TableName, IdentifierRuleDao.Columns)
                .AddDatabaseTable(IdentifierSeqDao.TableName, IdentifierSeqDao.Columns)
                .AddSiteMenu(Service_AddSiteMenu);
             
            service.ContentFormSubmit += Service_ContentFormSubmited; // 页面提交处理函数
            service.ContentFormLoad += Service_ContentFormLoad; // // 页面加载处理函数

        }

        private string Service_ContentFormLoad(object sender, ContentFormLoadEventArgs e)
        { 
            if(e.AttributeName == ContentAttribute.Identifier)
            {
                var identifier = e.Form.GetString(nameof(ContentAttribute.Identifier));
                return $@"
                    <div class=""form-group"">
                        <label class=""col-sm-1 control-label"">信息分类</label>
                        <div class=""col-sm-6"">
                            {ContentDao.GetCategoriesHtml(e.SiteId, e.ChannelId, e.Form)}
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
            else
            {
                return string.Empty;
            }
        }

        private void Service_ContentFormSubmited(object sender, ContentFormSubmitEventArgs e)
        {
            ContentDao.ContentFormSubmited(e.SiteId, e.ChannelId, e.ContentInfo, e.Form);
        }

        private Menu Service_AddSiteMenu(int siteId)
        {
            GovPublicManager.Initialize(siteId);
            return new Menu
            {
                Text = "主动信息公开",
                IconClass = "ion-ios-book",
                Menus = new List<Menu>
                {
                    new Menu
                    {
                        Text = "信息采集",
                        Href =
                            FilesApi.GetAdminDirectoryUrl(
                                $"cms/pageContentAdd.aspx?siteId={siteId}&channelId={GetConfigInfo(siteId).GovPublicChannelId}")
                    },
                    new Menu
                    {
                        Text = "信息管理",
                        Href =
                            FilesApi.GetAdminDirectoryUrl(
                                $"cms/pageContentSearch.aspx?siteId={siteId}&channelId={GetConfigInfo(siteId).GovPublicChannelId}")
                    },
                    new Menu
                    {
                        Text = "信息审核",
                        Href =
                            FilesApi.GetAdminDirectoryUrl(
                                $"cms/pageContentCheck.aspx?siteId={siteId}&channelId={GetConfigInfo(siteId).GovPublicChannelId}")
                    },
                    new Menu
                    {
                        Text = "分类法管理",
                        Href = $"{nameof(PageCategoryMain)}.aspx"
                    },
                    new Menu
                    {
                        Text = "索引号生成规则",
                        Href = $"{nameof(PageIdentifierRule)}.aspx"
                    },
                    new Menu
                    {
                        Text = "重新生成索引号",
                        Href = $"{nameof(PageIdentifierCreate)}.aspx"
                    },
                    new Menu
                    {
                        Text = "信息公开设置",
                        Href = $"{nameof(PageSettings)}.aspx"
                    }
                }
            };
        }

    }
}