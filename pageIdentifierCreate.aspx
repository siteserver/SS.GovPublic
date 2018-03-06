<%@ Page Language="C#" Inherits="SS.GovPublic.Pages.PageIdentifierCreate" %>
  <!DOCTYPE html>
  <html>

  <head>
    <meta charset="utf-8">
    <link href="assets/plugin-utils/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/plugin-utils/css/plugin-utils.css" rel="stylesheet" type="text/css" />
  </head>

  <body>
    <div style="padding: 20px 0;">

      <div class="container">
        <form id="form" runat="server" class="form-horizontal">

          <div class="row">
            <div class="card-box">
              <div class="row">
                <div class="col-lg-10">
                  <h4 class="m-t-0 header-title">
                    <b>重新生成索引号</b>
                  </h4>
                  <p class="text-muted font-13 m-b-30">
                    系统将重新生成所选栏目下的指定类型信息的索引号。
                  </p>
                </div>
              </div>

              <asp:Literal id="LtlMessage" runat="server" />

              <div class="form-horizontal">

                <div class="form-group">
                  <label class="col-sm-3 control-label">生成索引号栏目</label>
                  <div class="col-sm-3">
                    <asp:DropDownList ID="DdlNodeId" class="form-control" runat="server"></asp:DropDownList>
                  </div>
                  <div class="col-sm-6">
                    <span class="help-block">选择需要重新生成索引号的栏目</span>
                  </div>
                </div>

                <div class="form-group">
                  <label class="col-sm-3 control-label">生成索引号信息类型</label>
                  <div class="col-sm-3">
                    <asp:DropDownList ID="DdlCreateType" class="form-control" runat="server"></asp:DropDownList>
                  </div>
                  <div class="col-sm-6">

                  </div>
                </div>

                <div class="form-group m-b-0">
                  <div class="col-sm-offset-3 col-sm-9">
                    <asp:Button class="btn btn-primary" id="Submit" text="确 定" onclick="Submit_OnClick" runat="server" />
                  </div>
                </div>

              </div>
            </div>
          </div>

        </form>
      </div>
    </div>
  </body>

  </html>