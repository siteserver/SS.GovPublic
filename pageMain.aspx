<%@ Page Language="C#" Inherits="SS.GovPublic.Pages.PageMain" %>
  <!DOCTYPE html>
  <html>

  <head>
    <meta charset="utf-8">
  </head>
  <frameset id="frame" framespacing="0" border="false" cols="180,*" frameborder="0" scrolling="yes">
    <frame name="tree" scrolling="auto" marginwidth="0" marginheight="0" src="<%=AdminUrl%>/cms/pageContentTree.aspx?siteId=<%=SiteId%>&contentModelPluginId=<%=ContentModelPluginId%>&linkUrl=<%=LinkUrl%>">
    <frame name="content" scrolling="auto" marginwidth="0" marginheight="0" src="pageBlank.html">
  </frameset>
  <noframes>

    <body>
      <p>This page uses frames, but your browser doesn't support them.</p>
    </body>
  </noframes>

  </html>