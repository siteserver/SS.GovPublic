using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using SS.GovPublic.Core;

namespace SS.GovPublic.Pages
{
    public class AjaxService : Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            var type = Request["type"];
            var retString = string.Empty;

            if (type == CategoryTreeItem.TypeGetLoadingGovPublicCategories)
            {
                var classCode = Request["classCode"];
                var siteId = Utils.ToInt(Request["siteId"]);
                var parentId = Utils.ToInt(Request["parentID"]);
                var loadingType = Request["loadingType"];
                retString = GetLoadingGovPublicCategories(siteId, classCode, parentId, loadingType);
            }
            //else if (type == NodeTreeItem.TypeGetLoadingChannels)
            //{
            //    var siteId = Utils.ToInt(Request["siteId"]);
            //    var parentId = Utils.ToInt(Request["parentID"]);
            //    var loadingType = Request["loadingType"];
            //    var additional = Request["additional"];
            //    retString = GetLoadingChannels(siteId, parentId, loadingType, additional, body);
            //}

            Page.Response.Write(retString);
            Page.Response.End();
        }

        //public static string GetGetLoadingChannelsUrl()
        //{
        //    return PageUtils.GetAjaxUrl(nameof(AjaxOtherService), new NameValueCollection
        //    {
        //        {"type", TypeGetLoadingChannels }
        //    });
        //}

        //public static string GetGetLoadingChannelsParameters(int siteId, ELoadingType loadingType, NameValueCollection additional)
        //{
        //    return TranslateUtils.NameValueCollectionToString(new NameValueCollection
        //    {
        //        {"siteId", siteId.ToString() },
        //        {"loadingType", ELoadingTypeUtils.GetValue(loadingType)},
        //        {"additional", TranslateUtils.EncryptStringBySecretKey(TranslateUtils.NameValueCollectionToString(additional))}
        //    });
        //}

        public string GetLoadingGovPublicCategories(int siteId, string classCode, int parentId, string loadingType)
        {
            var list = new List<string>();

            var eLoadingType = ECategoryLoadingTypeUtils.GetEnumType(loadingType);

            var categoryIdList = Main.CategoryDao.GetCategoryIdListByParentId(siteId, classCode, parentId);

            foreach (var categoryId in categoryIdList)
            {
                var categoryInfo = Main.CategoryDao.GetCategoryInfo(categoryId);
                list.Add(PageCategory.GetCategoryRowHtml(siteId, categoryInfo, true, eLoadingType));
            }

            //arraylist.Reverse();

            var builder = new StringBuilder();
            foreach (var html in list)
            {
                builder.Append(html);
            }
            return builder.ToString();
        }

        //public string GetLoadingChannels(int siteId, int parentId, string loadingType, string additional)
        //{
        //    var arraylist = new ArrayList();

        //    var eLoadingType = ELoadingTypeUtils.GetEnumType(loadingType);

        //    var channelIdList = DataProvider.NodeDao.GetChannelIdListByParentId(siteId, parentId);

        //    var publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(siteId);

        //    var nameValueCollection = TranslateUtils.ToNameValueCollection(TranslateUtils.DecryptStringBySecretKey(additional));

        //    foreach (int channelId in channelIdList)
        //    {
        //        var enabled = AdminUtility.IsOwningChannelId(body.AdministratorName, channelId);
        //        if (!enabled)
        //        {
        //            if (!AdminUtility.IsHasChildOwningChannelId(body.AdministratorName, channelId))
        //            {
        //                continue;
        //            }
        //        }
        //        var nodeInfo = Main.ChannelApi.GetChannelInfo(siteId, channelId);

        //        arraylist.Add(ChannelLoading.GetChannelRowHtml(publishmentSystemInfo, nodeInfo, enabled, eLoadingType, nameValueCollection, Main.Context.ConfigApi.PhysicalApplicationPath.AdministratorName));
        //    }

        //    //arraylist.Reverse();

        //    var builder = new StringBuilder();
        //    foreach (string html in arraylist)
        //    {
        //        builder.Append(html);
        //    }
        //    return builder.ToString();
        //}
    }
}
