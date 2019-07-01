using System;
using System.Text;
using SiteServer.Plugin;
using SS.GovPublic.Core.Model;

namespace SS.GovPublic.Core
{
    public enum ECategoryLoadingType
    {
        Tree,
        List,
        Select
    }

    public class ECategoryLoadingTypeUtils
    {
        public static string GetValue(ECategoryLoadingType type)
        {
            if (type == ECategoryLoadingType.Tree)
            {
                return "Tree";
            }
            else if (type == ECategoryLoadingType.Select)
            {
                return "Select";
            }
            else if (type == ECategoryLoadingType.List)
            {
                return "List";
            }
            else
            {
                throw new Exception();
            }
        }

        public static ECategoryLoadingType GetEnumType(string typeStr)
        {
            var retval = ECategoryLoadingType.List;

            if (Equals(ECategoryLoadingType.Tree, typeStr))
            {
                retval = ECategoryLoadingType.Tree;
            }
            else if (Equals(ECategoryLoadingType.Select, typeStr))
            {
                retval = ECategoryLoadingType.Select;
            }

            return retval;
        }

        public static bool Equals(ECategoryLoadingType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, ECategoryLoadingType type)
        {
            return Equals(type, typeStr);
        }
    }

    public class CategoryTreeItem
    {
        private readonly string _iconFolderUrl;
        private readonly string _iconEmptyUrl;
        private readonly string _iconMinusUrl;
        private readonly string _iconPlusUrl;

        private bool _enabled = true;
        private CategoryInfo _categoryInfo;

        public static CategoryTreeItem CreateInstance(CategoryInfo categoryInfo, bool enabled)
        {
            var item = new CategoryTreeItem
            {
                _enabled = enabled,
                _categoryInfo = categoryInfo
            };

            return item;
        }

        private CategoryTreeItem()
        {
            var treeDirectoryUrl = Context.UtilsApi.GetAdminUrl("assets/icons/tree/");
            _iconFolderUrl = treeDirectoryUrl + "folder.gif";
            _iconEmptyUrl = treeDirectoryUrl + "empty.gif";
            _iconMinusUrl = treeDirectoryUrl + "minus.png";
            _iconPlusUrl = treeDirectoryUrl + "plus.png";
        }

        public string GetItemHtml(ECategoryLoadingType loadingType)
        {
            var htmlBuilder = new StringBuilder();
            var parentsCount = _categoryInfo.ParentsCount;

            if (loadingType == ECategoryLoadingType.Tree || loadingType == ECategoryLoadingType.Select)
            {
                parentsCount = parentsCount + 1;
            }

            for (var i = 0; i < parentsCount; i++)
            {
                htmlBuilder.Append($@"<img align=""absmiddle"" src=""{_iconEmptyUrl}"" />");
            }

            if (_categoryInfo.ChildrenCount > 0)
            {
                htmlBuilder.Append(
                    $@"<img align=""absmiddle"" style=""cursor:pointer"" onClick=""displayChildren(this);"" isAjax=""true"" isOpen=""false"" id=""{_categoryInfo
                        .Id}"" src=""{_iconPlusUrl}"" />");
            }
            else
            {
                htmlBuilder.Append($@"<img align=""absmiddle"" src=""{_iconEmptyUrl}"" />");
            }

            if (!string.IsNullOrEmpty(_iconFolderUrl))
            {
                htmlBuilder.Append($@"<img align=""absmiddle"" src=""{_iconFolderUrl}"" />");
            }

            htmlBuilder.Append("&nbsp;");

            if (_enabled)
            {
                //if (loadingType == ECategoryLoadingType.Tree)
                //{
                //    var linkUrl = PageGovPublicContent.GetRedirectUrl(_categoryInfo.SiteId, _categoryInfo.ClassCode, _categoryInfo.Id);

                //    htmlBuilder.Append(
                //        $"<a href='{linkUrl}' isLink='true' onclick='fontWeightLink(this)' target='content'>{_categoryInfo.CategoryName}</a>");

                //}
                //else if (loadingType == ECategoryLoadingType.Select)
                //{
                //    htmlBuilder.Append($@"<a href=""{ModalGovPublicCategorySelect.GetRedirectUrl(_categoryInfo.SiteId, _categoryInfo.ClassCode, _categoryInfo.Id)}"" href=""javascript:;"">{_categoryInfo.CategoryName}</a>");
                //}
                //else
                //{
                    htmlBuilder.Append(_categoryInfo.CategoryName);
                //}
            }
            else
            {
                htmlBuilder.Append(_categoryInfo.CategoryName);
            }

            if (_categoryInfo.ContentNum >= 0)
            {
                htmlBuilder.Append("&nbsp;");
                htmlBuilder.Append(
                    $@"<span style=""font-size:8pt;font-family:arial"" class=""gray"">({_categoryInfo.ContentNum})</span>");
            }

            htmlBuilder.Replace("displayChildren", $"displayChildren_{_categoryInfo.ClassCode}");

            return htmlBuilder.ToString();
        }

        public static string GetScript(int siteId, string classCode, ECategoryLoadingType loadingType)
        {
            var script = @"
<script language=""JavaScript"">
function getTreeLevel(e) {
	var length = 0;
	if (!isNull(e)){
		if (e.tagName == 'TR') {
			length = parseInt(e.getAttribute('treeItemLevel'));
		}
	}
	return length;
}

function getTrElement(element){
	if (isNull(element)) return;
	for (element = element.parentNode;;){
		if (element != null && element.tagName == 'TR'){
			break;
		}else{
			element = element.parentNode;
		} 
	}
	return element;
}

function getImgClickableElementByTr(element){
	if (isNull(element) || element.tagName != 'TR') return;
	var img = null;
	if (!isNull(element.childNodes)){
		var imgCol = element.getElementsByTagName('IMG');
		if (!isNull(imgCol)){
			for (x=0;x<imgCol.length;x++){
				if (!isNull(imgCol.item(x).getAttribute('isOpen'))){
					img = imgCol.item(x);
					break;
				}
			}
		}
	}
	return img;
}

var weightedLink = null;

function fontWeightLink(element){
    if (weightedLink != null)
    {
        weightedLink.style.fontWeight = 'normal';
    }
    element.style.fontWeight = 'bold';
    weightedLink = element;
}

var completedClassID = null;
function displayChildren(img){
	if (isNull(img)) return;

	var tr = getTrElement(img);

    var isToOpen = img.getAttribute('isOpen') == 'false';
    var isByAjax = img.getAttribute('isAjax') == 'true';
    var classID = img.getAttribute('id');

	if (!isNull(img) && img.getAttribute('isOpen') != null){
		if (img.getAttribute('isOpen') == 'false'){
			img.setAttribute('isOpen', 'true');
            img.setAttribute('src', '{iconMinusUrl}');
		}else{
            img.setAttribute('isOpen', 'false');
            img.setAttribute('src', '{iconPlusUrl}');
		}
	}

    if (isToOpen && isByAjax)
    {
        var div = document.createElement('div');
        div.innerHTML = ""<img align='absmiddle' border='0' src='{iconLoadingUrl}' /> 加载中，请稍候..."";
        img.parentNode.appendChild(div);
        $(div).addClass('loading');
        loadingChannels(tr, img, div, classID);
    }
    else
    {
        var level = getTreeLevel(tr);
    	
	    var collection = new Array();
	    var index = 0;

	    for ( var e = tr.nextSibling; !isNull(e) ; e = e.nextSibling) {
		    if (!isNull(e) && !isNull(e.tagName) && e.tagName == 'TR'){
		        var currentLevel = getTreeLevel(e);
		        if (currentLevel <= level) break;
		        if(e.style.display == '') {
			        e.style.display = 'none';
		        }else{
			        if (currentLevel != level + 1) continue;
			        e.style.display = '';
			        var imgClickable = getImgClickableElementByTr(e);
			        if (!isNull(imgClickable)){
				        if (!isNull(imgClickable.getAttribute('isOpen')) && imgClickable.getAttribute('isOpen') =='true'){
					        imgClickable.setAttribute('isOpen', 'false');
                            imgClickable.setAttribute('src', '{iconPlusUrl}');
					        collection[index] = imgClickable;
					        index++;
				        }
			        }
		        }
            }
	    }
    	
	    if (index > 0){
		    for (i=0;i<=index;i++){
			    displayChildren(collection[i]);
		    }
	    }
    }
}
";
            
            var url = GetLoadingGovPublicCategoriesUrl(siteId);
            var pars = GetLoadingGovPublicCategoriesParameters(siteId, classCode, loadingType);

            script += $@"
function loadingChannels(tr, img, div, classID){{
    var url = '{url}';
    var pars = '{pars}&parentID=' + classID;

    jQuery.post(url, pars, function(data, textStatus)
    {{
        $($.parseHTML(data)).insertAfter($(tr));
        img.setAttribute('isAjax', 'false');
        img.parentNode.removeChild(div);
    }});
    completedClassID = classID;
}}

function loadingChannelsOnLoad(paths){{
    if (paths && paths.length > 0){{
        var siteIds = paths.split(',');
        var classID = siteIds[0];
        var img = $('#' + classID);
        if (img.attr('isOpen') == 'false'){{
            displayChildren(img[0]);
//            if (completedClassID && completedClassID == classID){{
//                if (paths.indexOf(',') != -1){{
//                    setTimeout(""loadingChannelsOnLoad("" + paths + "")"", 3000);
//                }}
//            }} 
        }}
    }}
}}
</script>
";

            var item = new CategoryTreeItem();
            script = script.Replace("{iconEmptyUrl}", item._iconEmptyUrl);
            script = script.Replace("{iconFolderUrl}", item._iconFolderUrl);
            script = script.Replace("{iconMinusUrl}", item._iconMinusUrl);
            script = script.Replace("{iconPlusUrl}", item._iconPlusUrl);

            script = script.Replace("{iconLoadingUrl}", Context.UtilsApi.GetAdminUrl("assets/icons/loading.gif"));

            script = script.Replace("loadingChannels", $"loadingChannels_{classCode}");
            script = script.Replace("displayChildren", $"displayChildren_{classCode}");

            return script;
        }

        public static string GetScriptOnLoad(string path)
        {
            return $@"
<script language=""JavaScript"">
$(document).ready(function(){{
    loadingChannelsOnLoad('{path}');
}});
</script>
";
        }

        public const string TypeGetLoadingGovPublicCategories = "GetLoadingGovPublicCategories";

        public static string GetLoadingGovPublicCategoriesUrl(int siteId)
        {
            return Utils.GetPluginUrl($"pages/AjaxService.aspx?siteId={siteId}&type={TypeGetLoadingGovPublicCategories}");
        }

        public static string GetLoadingGovPublicCategoriesParameters(int siteId, string classCode, ECategoryLoadingType loadingType)
        {
            return $"siteId={siteId}&classCode={classCode}&loadingType={loadingType}";
        }
    }
}
