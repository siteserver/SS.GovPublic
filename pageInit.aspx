<%@ Page Language="C#" Inherits="SS.GovPublic.Pages.PageInit" %>
  <!DOCTYPE html>
  <html>

  <head>
    <meta charset="utf-8">
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/ionicons.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/siteserver.min.css" rel="stylesheet" type="text/css" />
    <script src="assets/js/jquery.min.js" type="text/javascript"></script>
    <script src="assets/layer/layer.min.js" type="text/javascript"></script>
    <script src="assets/sweetalert/sweetalert.min.js" type="text/javascript"></script>
    <script language="javascript">
      function selectChannel(channelName, channelId) {
        $('#channelName').html(channelName);
        $('#channelId').val(channelId);
      }
    </script>

  </head>

  <body>

    <form id="form" runat="server">

      <asp:Literal id="LtlMessage" runat="server" />

      <div class="container-fluid">

        <div class="row m-t-20">
          <div class="col-12">
            <div class="card-box">
              <h4 class="header-title">
                <b>信息公开插件初始化</b>
              </h4>
              <p class="alert alert-warning mt-3">
                信息公开插件需要对应的栏目存放信息公开内容，请添加信息公开栏目
              </p>

              <div class="form-group form-row">
                <label class="col-2 col-form-label text-right">父栏目</label>
                <div class="col-9">
                  <span id="channelName" class="m-l-10 m-r-10 text-danger">首页</span>
                  <input id="channelId" name="channelId" value="<%=SiteId%>" type="hidden">
                  <a href="javascript:;" class="btn btn-success" onclick="$.layer({type: 2, maxmin: true, shadeClose: true, title: '栏目选择', shade: [0.1,'#fff'], iframe: {src: '<%=UrlModalChannelSelect%>'}, area: ['460px', '450px'], offset: ['', '']});return false;">选择</a>
                </div>
                <div class="col-1"></div>
              </div>

              <div class="form-group form-row">
                <label class="col-2 col-form-label text-right">栏目索引</label>
                <div class="col-4 pt-2">
                  <asp:CheckBox class="checkbox checkbox-primary" Text="将栏目名称作为栏目索引" ID="CbIsNameToIndex" runat="server" />
                </div>
                <div class="col-6"></div>
              </div>

              <div class="form-group form-row">
                <label class="col-2 col-form-label text-right">说明</label>
                <div class="col-10 help-block pt-2">
                  栏目之间用换行分割，下级栏目在栏目前添加“－”字符，索引可以放到括号中，如：
                  <code>
                    <br> 栏目一(栏目索引)
                    <br> －下级栏目(下级索引)
                    <br> －－下下级栏目
                  </code>
                </div>
              </div>

              <div class="form-group form-row">
                <label class="col-1 col-form-label"></label>
                <div class="col-10">
                  <asp:TextBox class="form-control" Style="width: 98%; height: 240px" TextMode="MultiLine" ID="TbChannelNames" runat="server"
                  />
                </div>
                <div class="col-1"></div>
              </div>

              <hr />

              <div class="text-center">
                <asp:Button class="btn btn-primary" id="Submit" text="确 定" onclick="Submit_OnClick" runat="server" />
              </div>

            </div>
          </div>
        </div>

      </div>

      <asp:Literal id="LtlScript" runat="server" />

    </form>

  </body>

  </html>