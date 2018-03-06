<%@ Page Language="C#" Inherits="SS.GovPublic.Pages.PageSettings" %>
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
                    <b>信息公开设置</b>
                  </h4>
                  <p class="text-muted font-13 m-b-30">
                    如果未设置主题分类根栏目，系统将默认生成，主题分类根栏目只能选择首页或者一级栏目。
                  </p>
                </div>
              </div>

              <asp:Literal id="LtlMessage" runat="server" />

              <div class="form-horizontal">

                <div class="form-group">
                  <label class="col-sm-3 control-label">主题分类根栏目</label>
                  <div class="col-sm-3">
                    <asp:DropDownList ID="DdlGovPublicChannelId" class="form-control" runat="server"></asp:DropDownList>
                  </div>
                  <div class="col-sm-6">
                  </div>
                </div>

                <div class="form-group">
                  <label class="col-sm-3 control-label">选择机构分类后自动更改发布机构</label>
                  <div class="col-sm-3">
                    <asp:DropDownList ID="DdlIsPublisherRelatedDepartmentId" class="form-control" runat="server">
                      <asp:ListItem Text="是" Value="True" Selected="True" />
                      <asp:ListItem Text="否" Value="False" />
                    </asp:DropDownList>
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