<%@ Page Language="C#" Inherits="SS.GovPublic.Pages.PageAnalysis" %>
  <%@ Register TagPrefix="ctrl" Namespace="SS.GovPublic.Controls" Assembly="SS.GovPublic" %>
    <!DOCTYPE html>
    <html>

    <head>
      <meta charset="utf-8">
      <link href="assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
      <link href="assets/css/ionicons.min.css" rel="stylesheet" type="text/css" />
      <link href="assets/css/siteserver.min.css" rel="stylesheet" type="text/css" />
    </head>

    <body>

      <form class="m-l-15 m-r-15 m-t-15" runat="server">

        <div class="card-box">
          <h4 class="m-t-0 m-b-20 header-title">
            数据统计分析
          </h4>

          <div class="form-inline" role="form">

            <div class="form-group">
              <label for="DdlTaxis" class="mr-sm-2">开始时间</label>
              <ctrl:DateTimeTextBox id="TbStartDate" class="form-control" Columns="30" runat="server" />
            </div>

            <div class="form-group m-l-10">
              <label for="DdlState" class="mr-sm-2">结束时间</label>
              <ctrl:DateTimeTextBox id="TbEndDate" class="form-control" Columns="30" runat="server" />
            </div>

            <div class="form-group m-l-10">
              <label for="TbDateFrom" class="mr-sm-2">信息公开分类</label>
              <asp:DropDownList ID="DdlChannelId" AutoPostBack="true" OnSelectedIndexChanged="Analysis_OnClick" class="form-control" runat="server"></asp:DropDownList>
            </div>

            <asp:Button OnClick="Analysis_OnClick" Text="统 计" class="btn btn-success m-l-10 btn-md" runat="server"></asp:Button>
          </div>

          <div class="table-responsive m-t-20" data-pattern="priority-columns">
            <table id="contents" class="table">
              <thead>
                <tr class="thead">
                  <th>统计对象</th>
                  <th>文档数</th>
                  <th>对比图</th>
                </tr>
              </thead>
              <tbody>
                <asp:Repeater ID="RptContents" runat="server">
                  <itemtemplate>
                    <asp:Literal id="ltlTrHtml" runat="server"></asp:Literal>
                    <td>
                      <asp:Literal id="ltlTarget" runat="server"></asp:Literal>
                    </td>
                    <td>
                      <asp:Literal id="ltlTotalCount" runat="server"></asp:Literal>
                    </td>
                    <td>
                      <asp:Literal id="ltlBar" runat="server"></asp:Literal>
                    </td>
                    </tr>
                  </itemtemplate>
                </asp:Repeater>

              </tbody>
            </table>
          </div>

        </div>

      </form>
    </body>

    </html>