using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace SS.GovPublic.Core
{
    public class Utils
    {
        public static string GetMessageHtml(string message, bool isSuccess)
        {
            return isSuccess
                ? $@"<div class=""alert alert-success"" role=""alert"">{message}</div>"
                : $@"<div class=""alert alert-danger"" role=""alert"">{message}</div>";
        }

        public static string GetPageContentUrl(int siteId, int channelId)
        {
            return Main.Instance.FilesApi.GetAdminDirectoryUrl($"cms/pagecontent.aspx?SiteId={siteId}&ChannelId={channelId}");
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

        public static string GetSelectedListControlValueCollection(ListControl listControl)
        {
            var list = new List<string>();
            if (listControl != null)
            {
                foreach (ListItem item in listControl.Items)
                {
                    if (item.Selected)
                    {
                        list.Add(item.Value);
                    }
                }
            }
            return string.Join(",", list);
        }

        public static string ToStringWithQuote(List<string> collection)
        {
            var builder = new StringBuilder();
            if (collection != null)
            {
                foreach (var obj in collection)
                {
                    builder.Append("'").Append(obj).Append("'").Append(",");
                }
                if (builder.Length != 0) builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }

        public static List<string> StringCollectionToStringList(string collection)
        {
            return StringCollectionToStringList(collection, ',');
        }

        public static List<string> StringCollectionToStringList(string collection, char split)
        {
            var list = new List<string>();
            if (!string.IsNullOrEmpty(collection))
            {
                var array = collection.Split(split);
                foreach (var s in array)
                {
                    list.Add(s);
                }
            }
            return list;
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

        public static string ReplaceNewlineToBr(string inputString)
        {
            if (string.IsNullOrEmpty(inputString)) return string.Empty;
            var retVal = new StringBuilder();
            inputString = inputString.Trim();
            foreach (var t in inputString)
            {
                switch (t)
                {
                    case '\n':
                        retVal.Append("<br />");
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

        public static string HtmlDecode(string inputString)
        {
            return HttpUtility.HtmlDecode(inputString);
        }

        public static string HtmlEncode(string inputString)
        {
            return HttpUtility.HtmlEncode(inputString);
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

        public static object Eval(object dataItem, string name)
        {
            object o = null;
            try
            {
                o = DataBinder.Eval(dataItem, name);
            }
            catch
            {
                // ignored
            }
            if (o == DBNull.Value)
            {
                o = null;
            }
            return o;
        }

        public static int EvalInt(object dataItem, string name)
        {
            var o = Eval(dataItem, name);
            return o == null ? 0 : Convert.ToInt32(o);
        }

        public static decimal EvalDecimal(object dataItem, string name)
        {
            var o = Eval(dataItem, name);
            return o == null ? 0 : Convert.ToDecimal(o);
        }

        public static string EvalString(object dataItem, string name)
        {
            var o = Eval(dataItem, name);
            return o?.ToString() ?? string.Empty;
        }

        public static DateTime EvalDateTime(object dataItem, string name)
        {
            var o = Eval(dataItem, name);
            if (o == null)
            {
                return DateTime.MinValue;
            }
            return (DateTime)o;
        }

        public static bool EvalBool(object dataItem, string name)
        {
            var o = Eval(dataItem, name);
            return o != null && Convert.ToBoolean(o.ToString());
        }

        public static List<string> GetHtmlFormElements(string content)
        {
            var list = new List<string>();

            const RegexOptions options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase;

            var regex = "<input\\s*[^>]*?/>|<input\\s*[^>]*?>[^>]*?</input>";
            var reg = new Regex(regex, options);
            var mc = reg.Matches(content);
            for (var i = 0; i < mc.Count; i++)
            {
                var element = mc[i].Value;
                list.Add(element);
            }

            regex = "<textarea\\s*[^>]*?/>|<textarea\\s*[^>]*?>[^>]*?</textarea>";
            reg = new Regex(regex, options);
            mc = reg.Matches(content);
            for (var i = 0; i < mc.Count; i++)
            {
                var element = mc[i].Value;
                list.Add(element);
            }

            regex = "<select\\b[\\s\\S]*?</select>";
            reg = new Regex(regex, options);
            mc = reg.Matches(content);
            for (var i = 0; i < mc.Count; i++)
            {
                var element = mc[i].Value;
                list.Add(element);
            }

            return list;
        }

        private const string XmlDeclaration = "<?xml version='1.0'?>";

        private const string XmlNamespaceStart = "<root>";

        private const string XmlNamespaceEnd = "</root>";

        public static XmlDocument GetXmlDocument(string element, bool isXml)
        {
            var xmlDocument = new XmlDocument
            {
                PreserveWhitespace = true
            };
            try
            {
                if (isXml)
                {
                    xmlDocument.LoadXml(XmlDeclaration + XmlNamespaceStart + element + XmlNamespaceEnd);
                }
                else
                {
                    xmlDocument.LoadXml(XmlDeclaration + XmlNamespaceStart + Main.Instance.ParseApi.HtmlToXml(element) + XmlNamespaceEnd);
                }
            }
            catch
            {
                // ignored
            }
            //catch(Exception e)
            //{
            //    TraceUtils.Warn(e.ToString());
            //    throw e;
            //}
            return xmlDocument;
        }

        public static void ParseHtmlElement(string htmlElement, out string tagName, out string innerXml, out NameValueCollection attributes)
        {
            tagName = string.Empty;
            innerXml = string.Empty;
            attributes = new NameValueCollection();

            var document = GetXmlDocument(htmlElement, false);
            XmlNode elementNode = document.DocumentElement;
            if (elementNode == null) return;

            elementNode = elementNode.FirstChild;
            tagName = elementNode.Name;
            innerXml = elementNode.InnerXml;
            if (elementNode.Attributes == null) return;

            var elementIe = elementNode.Attributes.GetEnumerator();
            while (elementIe.MoveNext())
            {
                var attr = (XmlAttribute)elementIe.Current;
                if (attr != null)
                {
                    var attributeName = attr.Name;
                    attributes.Add(attributeName, attr.Value);
                }
            }
        }

        public static string GetHtmlElementById(string html, string id)
        {
            const RegexOptions options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase;

            var regex = $"<input\\s*[^>]*?id\\s*=\\s*(\"{id}\"|\'{id}\'|{id}).*?>";
            var reg = new Regex(regex, options);
            var match = reg.Match(html);
            if (match.Success)
            {
                return match.Value;
            }

            regex = $"<\\w+\\s*[^>]*?id\\s*=\\s*(\"{id}\"|\'{id}\'|{id})[^>]*/\\s*>";
            reg = new Regex(regex, options);
            match = reg.Match(html);
            if (match.Success)
            {
                return match.Value;
            }

            regex = $"<(\\w+?)\\s*[^>]*?id\\s*=\\s*(\"{id}\"|\'{id}\'|{id}).*?>[^>]*</\\1[^>]*>";
            reg = new Regex(regex, options);
            match = reg.Match(html);
            if (match.Success)
            {
                return match.Value;
            }

            return string.Empty;
        }

        public static string GetHtmlElementByRole(string html, string role)
        {
            const RegexOptions options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase;

            var regex = $"<input\\s*[^>]*?role\\s*=\\s*(\"{role}\"|\'{role}\'|{role}).*?>";
            var reg = new Regex(regex, options);
            var match = reg.Match(html);
            if (match.Success)
            {
                return match.Value;
            }

            regex = $"<\\w+\\s*[^>]*?role\\s*=\\s*(\"{role}\"|\'{role}\'|{role})[^>]*/\\s*>";
            reg = new Regex(regex, options);
            match = reg.Match(html);
            if (match.Success)
            {
                return match.Value;
            }

            regex = $"<(\\w+?)\\s*[^>]*?role\\s*=\\s*(\"{role}\"|\'{role}\'|{role}).*?>[^>]*</\\1[^>]*>";
            reg = new Regex(regex, options);
            match = reg.Match(html);
            if (match.Success)
            {
                return match.Value;
            }

            return string.Empty;
        }

        public static void RewriteSubmitButton(StringBuilder builder, string clickString)
        {
            var submitElement = GetHtmlElementByRole(builder.ToString(), "submit");
            if (string.IsNullOrEmpty(submitElement))
            {
                submitElement = GetHtmlElementById(builder.ToString(), "submit");
            }
            if (!string.IsNullOrEmpty(submitElement))
            {
                var document = GetXmlDocument(submitElement, false);
                XmlNode elementNode = document.DocumentElement;
                if (elementNode != null)
                {
                    elementNode = elementNode.FirstChild;
                    if (elementNode.Attributes != null)
                    {
                        var elementIe = elementNode.Attributes.GetEnumerator();
                        var attributes = new StringDictionary();
                        while (elementIe.MoveNext())
                        {
                            var attr = (XmlAttribute)elementIe.Current;
                            if (attr != null)
                            {
                                var attributeName = attr.Name.ToLower();
                                if (attributeName == "href")
                                {
                                    attributes.Add(attr.Name, "javascript:;");
                                }
                                else if (attributeName != "onclick")
                                {
                                    attributes.Add(attr.Name, attr.Value);
                                }
                            }
                        }
                        attributes.Add("onclick", clickString);
                        attributes.Remove("id");
                        attributes.Remove("name");

                        //attributes.Add("id", "submit_" + styleID);

                        if (EqualsIgnoreCase(elementNode.Name, "a"))
                        {
                            attributes.Remove("href");
                            attributes.Add("href", "javascript:;");
                        }

                        if (!string.IsNullOrEmpty(elementNode.InnerXml))
                        {
                            builder.Replace(submitElement,
                                $@"<{elementNode.Name} {ToAttributesString(attributes)}>{elementNode.InnerXml}</{elementNode
                                    .Name}>");
                        }
                        else
                        {
                            builder.Replace(submitElement,
                                $@"<{elementNode.Name} {ToAttributesString(attributes)}/>");
                        }
                    }
                }
            }
        }

        public static string ToAttributesString(NameValueCollection attributes)
        {
            var builder = new StringBuilder();
            if (attributes != null && attributes.Count > 0)
            {
                foreach (string key in attributes.Keys)
                {
                    var value = attributes[key];
                    if (!string.IsNullOrEmpty(value))
                    {
                        value = value.Replace("\"", "'");
                    }
                    builder.Append($@"{key}=""{value}"" ");
                }
                builder.Length--;
            }
            return builder.ToString();
        }

        public static string ToAttributesString(StringDictionary attributes)
        {
            var builder = new StringBuilder();
            if (attributes != null && attributes.Count > 0)
            {
                foreach (string key in attributes.Keys)
                {
                    var value = attributes[key];
                    if (!string.IsNullOrEmpty(value))
                    {
                        value = value.Replace("\"", "'");
                    }
                    builder.Append($@"{key}=""{value}"" ");
                }
                builder.Length--;
            }
            return builder.ToString();
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

        public static string SwalError(string title, string text)
        {
            var script = $@"swal({{
  title: '{title}',
  text: '{ReplaceNewline(text, string.Empty)}',
  icon: 'error',
  button: '关 闭',
}});";

            return script;
        }

        public static string SwalSuccess(string title, string text)
        {
            return SwalSuccess(title, text, "关 闭", null);
        }

        public static string SwalSuccess(string title, string text, string button, string scripts)
        {
            if (!string.IsNullOrEmpty(scripts))
            {
                scripts = $@".then(function (value) {{
  {scripts}
}})";
            }
            var script = $@"swal({{
  title: '{title}',
  text: '{ReplaceNewline(text, string.Empty)}',
  icon: 'success',
  button: '{button}',
}}){scripts};";
            return script;
        }

        public static string SwalWarning(string title, string text, string btnCancel, string btnSubmit, string scripts)
        {
            var script = $@"swal({{
  title: '{title}',
  text: '{ReplaceNewline(text, string.Empty)}',
  icon: 'warning',
  buttons: {{
    cancel: '{btnCancel}',
    catch: '{btnSubmit}'
  }}
}})
.then(function(willDelete){{
  if (willDelete) {{
    {scripts}
  }}
}});";
            return script;
        }

        public static string GetOpenLayerString(string title, string pageUrl)
        {
            return GetOpenLayerString(title, pageUrl, 0, 0);
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

        public static string GetOpenLayerStringWithTextBoxValue(string title, string pageUrl, string textBoxId)
        {
            return GetOpenLayerStringWithTextBoxValue(title, pageUrl, textBoxId, 0, 0);
        }

        public static string GetOpenLayerStringWithTextBoxValue(string title, string pageUrl, string textBoxId, int width, int height)
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
                $@"$.layer({{type: 2, maxmin: true, shadeClose: true, title: '{title}', shade: [0.1,'#fff'], iframe: {{src: '{pageUrl}' + '&{textBoxId}=' + $('#{textBoxId}').val()}}, area: [{areaWidth}, {areaHeight}], offset: [{offsetLeft}, {offsetRight}]}});return false;";
        }

        public static string GetOpenLayerStringWithCheckBoxValue(string title, string pageUrl, string checkBoxId, string alertText)
        {
            return GetOpenLayerStringWithCheckBoxValue(title, pageUrl, checkBoxId, alertText, 0, 0);
        }

        public static string GetOpenLayerStringWithCheckBoxValue(string title, string pageUrl, string checkBoxId, string alertText, int width, int height)
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

            if (string.IsNullOrEmpty(alertText))
            {
                return
                    $@"$.layer({{type: 2, maxmin: true, shadeClose: true, title: '{title}', shade: [0.1,'#fff'], iframe: {{src: '{pageUrl}' + '&{checkBoxId}=' + _getCheckBoxCollectionValue(document.getElementsByName('{checkBoxId}'))}}, area: [{areaWidth}, {areaHeight}], offset: [{offsetLeft}, {offsetRight}]}});return false;";
            }
            return
                $@"if (!_alertCheckBoxCollection(document.getElementsByName('{checkBoxId}'), '{alertText}')){{$.layer({{type: 2, maxmin: true, shadeClose: true, title: '{title}', shade: [0.1,'#fff'], iframe: {{src: '{pageUrl}' + '&{checkBoxId}=' + _getCheckBoxCollectionValue(document.getElementsByName('{checkBoxId}'))}}, area: [{areaWidth}, {areaHeight}], offset: [{offsetLeft}, {offsetRight}]}});}};return false;";
        }

        public static string GetOpenLayerStringWithTwoCheckBoxValue(string title, string pageUrl, string checkBoxId1, string checkBoxId2, string alertText, int width, int height)
        {
            var offset = string.Empty;
            if (width == 0)
            {
                offset = "offset: ['0px','0px'],";
            }
            if (height == 0)
            {
                offset = "offset: ['0px','0px'],";
            }

            return
                $@"var collectionValue1 = _getCheckBoxCollectionValue(document.getElementsByName('{checkBoxId1}'));var collectionValue2 = _getCheckBoxCollectionValue(document.getElementsByName('{checkBoxId2}'));if (collectionValue1.length == 0 && collectionValue2.length == 0){{alert('{alertText}');}}else{{$.layer({{type: 2, maxmin: true, shadeClose: true, title: '{title}', shade: [0.1,'#fff'], iframe: {{src: '{pageUrl}' + '&{checkBoxId1}=' + _getCheckBoxCollectionValue(document.getElementsByName('{checkBoxId1}')) + '&{checkBoxId2}=' + _getCheckBoxCollectionValue(document.getElementsByName('{checkBoxId2}'))}}, area: [{width}, {height}], {offset}}});}};return false;";
        }

        public static void SetCancelAttribute(IAttributeAccessor accessor)
        {
            accessor.SetAttribute("onclick", HidePopWin);
        }

        public static void CloseModalPage(Page page)
        {
            page.Response.Clear();
            page.Response.Write($"<script>window.parent.location.reload(false);{HidePopWin}</script>");
            //page.Response.End();
        }

        public static void CloseModalPage(Page page, string scripts)
        {
            page.Response.Clear();
            page.Response.Write($"<script>{scripts}</script>");
            page.Response.Write($"<script>window.parent.location.reload(false);{HidePopWin}</script>");
            //page.Response.End();
        }

        public static void CloseModalPageAndRedirect(Page page, string redirectUrl)
        {
            page.Response.Clear();
            page.Response.Write($"<script>window.parent.location.href = '{redirectUrl}';{HidePopWin}</script>");
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public static void CloseModalPageAndRedirect(Page page, string redirectUrl, string scripts)
        {
            page.Response.Clear();
            page.Response.Write($"<script>{scripts}</script>");
            page.Response.Write($"<script>window.parent.location.href = '{redirectUrl}';{HidePopWin}</script>");
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public static void CloseModalPageWithoutRefresh(Page page)
        {
            page.Response.Clear();
            page.Response.Write($"<script>{HidePopWin}</script>");
            //page.Response.End();
        }

        public static void CloseModalPageWithoutRefresh(Page page, string scripts)
        {
            page.Response.Clear();
            page.Response.Write($"<script>{scripts}</script>");
            page.Response.Write($"<script>{HidePopWin}</script>");
            //page.Response.End();
        }

        public const string HidePopWin = "window.parent.layer.closeAll();";
    }
}
