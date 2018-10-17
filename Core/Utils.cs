using SiteServer.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SS.GovPublic.Core
{
    public static class Utils
    {
        public const string HidePopWin = "window.parent.layer.closeAll();";

        public static string GetMessageHtml(string message, bool isSuccess)
        {
            return isSuccess
                ? $@"<div class=""alert alert-success"" role=""alert"">{message}</div>"
                : $@"<div class=""alert alert-danger"" role=""alert"">{message}</div>";
        }

        public static string GetSelectOptionText(string text, int parentsCount, bool isLastNode, bool[] isLastNodeArray)
        {
            var retval = string.Empty;
            if (isLastNode == false)
            {
                isLastNodeArray[parentsCount] = false;
            }
            else
            {
                isLastNodeArray[parentsCount] = true;
            }
            for (var i = 0; i < parentsCount; i++)
            {
                retval = string.Concat(retval, isLastNodeArray[i] ? "　" : "│");
            }
            retval = string.Concat(retval, isLastNode ? "└" : "├");
            retval = string.Concat(retval, text);

            return retval;
        }

        public static void AddListItems(ListControl listControl, string trueText, string falseText)
        {
            if (listControl != null)
            {
                var item = new ListItem(trueText, true.ToString());
                listControl.Items.Add(item);
                item = new ListItem(falseText, false.ToString());
                listControl.Items.Add(item);
            }
        }

        public static void AddListItems(ListControl listControl)
        {
            AddListItems(listControl, "是", "否");
        }

        public static void SelectSingleItem(ListControl listControl, string value)
        {
            if (listControl == null) return;

            listControl.ClearSelection();

            foreach (ListItem item in listControl.Items)
            {
                if (string.Equals(item.Value, value))
                {
                    item.Selected = true;
                    break;
                }
            }
        }

        public static void SelectSingleItemIgnoreCase(ListControl listControl, string value)
        {
            if (listControl == null) return;

            listControl.ClearSelection();
            foreach (ListItem item in listControl.Items)
            {
                if (EqualsIgnoreCase(item.Value, value))
                {
                    item.Selected = true;
                    break;
                }
            }
        }

        public static string ObjectCollectionToString(ICollection collection, string separatorStr)
        {
            var builder = new StringBuilder();
            if (collection != null)
            {
                foreach (var obj in collection)
                {
                    builder.Append(obj.ToString().Trim()).Append(separatorStr);
                }
                if (builder.Length != 0) builder.Remove(builder.Length - separatorStr.Length, separatorStr.Length);
            }
            return builder.ToString();
        }

        public static List<int> StringCollectionToIntList(string collection)
        {
            return StringCollectionToIntList(collection, ',');
        }

        public static List<int> StringCollectionToIntList(string collection, char split)
        {
            var list = new List<int>();
            if (!string.IsNullOrEmpty(collection))
            {
                var array = collection.Split(split);
                foreach (var s in array)
                {
                    list.Add(ToInt(s));
                }
            }
            return list;
        }

        public static string GetControlRenderHtml(Control control)
        {
            var builder = new StringBuilder();
            if (control != null)
            {
                var sw = new System.IO.StringWriter(builder);
                var htw = new HtmlTextWriter(sw);
                control.RenderControl(htw);
            }
            return builder.ToString();
        }

        public static string GetUrlWithoutQueryString(string rawUrl)
        {
            string urlWithoutQueryString;
            if (rawUrl != null && rawUrl.IndexOf("?", StringComparison.Ordinal) != -1)
            {
                var queryString = rawUrl.Substring(rawUrl.IndexOf("?", StringComparison.Ordinal));
                urlWithoutQueryString = rawUrl.Replace(queryString, "");
            }
            else
            {
                urlWithoutQueryString = rawUrl;
            }
            return urlWithoutQueryString;
        }

        public static string AddQueryString(string url, NameValueCollection queryString)
        {
            if (queryString == null || url == null || queryString.Count == 0)
                return url;

            var builder = new StringBuilder();
            foreach (string key in queryString.Keys)
            {
                builder.Append($"&{key}={HttpUtility.UrlEncode(queryString[key])}");
            }
            if (url.IndexOf("?", StringComparison.Ordinal) == -1)
            {
                if (builder.Length > 0) builder.Remove(0, 1);
                return string.Concat(url, "?", builder.ToString());
            }
            if (url.EndsWith("?"))
            {
                if (builder.Length > 0) builder.Remove(0, 1);
            }
            return string.Concat(url, builder.ToString());
        }

        public static bool EqualsIgnoreCase(string a, string b)
        {
            if (a == b) return true;
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return false;
            return string.Equals(a.Trim().ToLower(), b.Trim().ToLower());
        }

        public static int ToInt(string str)
        {
            int i;
            return int.TryParse(str, out i) ? i : 0;
        }

        public static bool ToBool(string str)
        {
            bool i;
            return bool.TryParse(str, out i) && i;
        }

        public static DateTime ToDateTime(string dateTimeStr)
        {
            DateTime dateTime;
            return DateTime.TryParse(dateTimeStr.Trim(), out dateTime) ? dateTime : DateTime.Now;
        }

        public static string ReplaceNewline(string inputString, string replacement)
        {
            if (string.IsNullOrEmpty(inputString)) return string.Empty;
            var retVal = new StringBuilder();
            inputString = inputString.Trim();
            foreach (var t in inputString)
            {
                switch (t)
                {
                    case '\n':
                        retVal.Append(replacement);
                        break;
                    case '\r':
                        break;
                    default:
                        retVal.Append(t);
                        break;
                }
            }
            return retVal.ToString();
        }

        public static string GetOpenLayerString(string title, string pageUrl, int width, int height)
        {
            string areaWidth = $"'{width}px'";
            string areaHeight = $"'{height}px'";
            var offsetLeft = "''";
            var offsetRight = "''";
            if (width == 0)
            {
                areaWidth = "($(window).width() - 50) +'px'";
                offsetRight = "'25px'";
            }
            if (height == 0)
            {
                areaHeight = "($(window).height() - 50) +'px'";
                offsetLeft = "'25px'";
            }
            return
                $@"$.layer({{type: 2, maxmin: true, shadeClose: true, title: '{title}', shade: [0.1,'#fff'], iframe: {{src: '{pageUrl}'}}, area: [{areaWidth}, {areaHeight}], offset: [{offsetLeft}, {offsetRight}]}});return false;";
        }

        public static void CloseModalPage(Page page)
        {
            page.Response.Clear();
            page.Response.Write($"<script>window.parent.location.reload(false);{HidePopWin}</script>");
            //page.Response.End();
        }

        public static int GetStartCount(char startChar, string content)
        {
            if (content == null)
            {
                return 0;
            }
            var count = 0;

            foreach (var theChar in content)
            {
                if (theChar == startChar)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count;
        }

        public static bool Contains(string text, string inner)
        {
            return text?.IndexOf(inner, StringComparison.Ordinal) >= 0;
        }

        public static string GetPageSqlString(string sqlString, string orderString, int itemsPerPage, int currentPageIndex, int pageCount, int recordsInLastPage)
        {
            var retval = string.Empty;

            var recsToRetrieve = itemsPerPage;
            if (currentPageIndex == pageCount - 1)
            {
                recsToRetrieve = recordsInLastPage;
            }

            orderString = orderString.ToUpper();
            var orderStringReverse = orderString.Replace(" DESC", " DESC2");
            orderStringReverse = orderStringReverse.Replace(" ASC", " DESC");
            orderStringReverse = orderStringReverse.Replace(" DESC2", " ASC");

            if (Context.DatabaseType == DatabaseType.MySql)
            {
                retval = $@"
SELECT * FROM (
    SELECT * FROM (
        SELECT * FROM ({sqlString}) AS t0 {orderString} LIMIT {itemsPerPage * (currentPageIndex + 1)}
    ) AS t1 {orderStringReverse} LIMIT {recsToRetrieve}
) AS t2 {orderString}";
            }
            else if (Context.DatabaseType == DatabaseType.SqlServer)
            {
                retval = $@"
SELECT * FROM (
    SELECT TOP {recsToRetrieve} * FROM (
        SELECT TOP {itemsPerPage * (currentPageIndex + 1)} * FROM ({sqlString}) AS t0 {orderString}
    ) AS t1 {orderStringReverse}
) AS t2 {orderString}";
            }
            else if (Context.DatabaseType == DatabaseType.PostgreSql)
            {
                retval = $@"
SELECT * FROM (
    SELECT * FROM (
        SELECT * FROM ({sqlString}) AS t0 {orderString} LIMIT {itemsPerPage * (currentPageIndex + 1)}
    ) AS t1 {orderStringReverse} LIMIT {recsToRetrieve}
) AS t2 {orderString}";
            }
            else if (Context.DatabaseType == DatabaseType.Oracle)
            {
                retval = $@"
SELECT * FROM (
    SELECT * FROM (
        SELECT * FROM ({sqlString}) {orderString} FETCH FIRST {itemsPerPage * (currentPageIndex + 1)} ROWS ONLY
    ) {orderStringReverse} FETCH FIRST {recsToRetrieve} ROWS ONLY
) {orderString}";
            }

            return retval;
        }
    }
}
