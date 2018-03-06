<%@ Page Language="C#" Inherits="SS.GovPublic.Pages.PageCategory" %>
  <!DOCTYPE html>
  <html>

  <head>
    <meta charset="utf-8">
    <link href="assets/plugin-utils/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/plugin-utils/css/plugin-utils.css" rel="stylesheet" type="text/css" />
    <script src="assets/js/jquery.min.js"></script>
    <script src="assets/layer/layer.min.js" type="text/javascript"></script>
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
                    <b>分类管理</b>
                  </h4>
                  <p class="text-muted font-13 m-b-30">
                    在此维护指定分类
                  </p>
                </div>
              </div>

              <asp:Literal id="LtlMessage" runat="server" />

              <table class="table table-hover">
                  <tr class="info thead">
                    <td>名称</td>
                    <td style="width:200px;">代码</td>
                    <td class="center" style="width:50px;">上升</td>
                    <td class="center" style="width:50px;">下降</td>
                    <td class="center" style="width:50px;">&nbsp;</td>
                    <td width="20" class="center">
                      <input onclick="_checkFormAll(this.checked)" type="checkbox" />
                    </td>
                  </tr>
                  <asp:Repeater ID="RptContents" runat="server">
                    <itemtemplate>
                      <asp:Literal id="ltlHtml" runat="server" />
                    </itemtemplate>
                  </asp:Repeater>
                </table>

                <hr />

                <div class="form-group">
                  <div class="col-xs-12">
                    <asp:Button class="btn btn-primary m-l-10" id="BtnAdd" Text="添 加" runat="server" />
                    <asp:Button class="btn" id="BtnDelete" Text="删 除" runat="server" />
                  </div>
                </div>

              <asp:Literal id="LtlScripts" runat="server" />

            </div>
          </div>

        </form>
      </div>

    </div>
  </body>

  </html>