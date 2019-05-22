<%@ Page Language="C#" Inherits="SS.GovPublic.Core.Pages.ModalIdentifierRuleAdd" %>
  <!DOCTYPE html>
  <html style="background:#fff">

  <head>
    <meta charset="utf-8">
    <link href="../assets/plugin-utils/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../assets/plugin-utils/css/plugin-utils.css" rel="stylesheet" type="text/css" />
    <script src="../assets/js/jquery.min.js" type="text/javascript"></script>
    <script src="../assets/layer/layer.min.js" type="text/javascript"></script>
  </head>

  <body style="padding: 0;background:#fff">
    <div style="padding: 20px 0;">

      <div class="container">
        <form id="form" runat="server" class="form-horizontal">

          <asp:Literal id="LtlMessage" runat="server" />

          <div class="form-horizontal">

            <div class="form-group">
              <label class="control-label col-xs-12">
                规则名称
                <asp:RequiredFieldValidator ControlToValidate="TbRuleName" errorMessage=" *" foreColor="red" display="Dynamic" runat="server"
                />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="TbRuleName" ValidationExpression="[^',]+" errorMessage=" *"
                  foreColor="red" display="Dynamic" />
              </label>
              <div class="col-xs-12">
                <asp:TextBox cssClass="form-control" id="TbRuleName" runat="server" />
              </div>
            </div>

            <div class="form-group">
              <label class="control-label col-xs-12">
                规则类型
              </label>
              <div class="col-xs-12">
                <asp:DropDownList ID="DdlIdentifierType" class="form-control" OnSelectedIndexChanged="DdlIdentifierType_SelectedIndexChanged"
                  AutoPostBack="true" runat="server"></asp:DropDownList>
              </div>
            </div>

            <asp:PlaceHolder ID="PhAttributeName" runat="server">
              <div class="form-group">
                <label class="control-label col-xs-12">
                  字段
                </label>
                <div class="col-xs-12">
                  <asp:DropDownList ID="DdlAttributeName" class="form-control" runat="server"></asp:DropDownList>
                </div>
              </div>
            </asp:PlaceHolder>

            <asp:PlaceHolder ID="PhMinLength" runat="server">
              <div class="form-group">
                <label class="control-label col-xs-12">
                  最小位数
                  <asp:RequiredFieldValidator ControlToValidate="TbMinLength" errorMessage=" *" foreColor="red" display="Dynamic" runat="server"
                  />
                  <asp:RegularExpressionValidator ControlToValidate="TbMinLength" ValidationExpression="\d+" Display="Dynamic" ErrorMessage="必须为大于等于零的整数"
                    foreColor="red" runat="server" />
                  <asp:CompareValidator ControlToValidate="TbMinLength" Operator="GreaterThanEqual" ValueToCompare="0" Display="Dynamic" ErrorMessage="必须为大于等于零的整数"
                    foreColor="red" runat="server" />
                </label>
                <div class="col-xs-12">
                  <asp:TextBox id="TbMinLength" Text="0" class="form-control" runat="server" />
                </div>
                <div class="col-xs-12 help-block">
                  0表示不设置最小位数，设置后不足位数将通过补零方式填充
                </div>
              </div>
            </asp:PlaceHolder>

            <asp:PlaceHolder ID="PhFormatString" runat="server">
              <div class="form-group">
                <label class="control-label col-xs-12">
                  日期格式
                  <asp:RequiredFieldValidator ControlToValidate="TbFormatString" errorMessage=" *" foreColor="red" display="Dynamic" runat="server"
                  />
                </label>
                <div class="col-xs-12">
                  <asp:TextBox id="TbFormatString" Text="yyyy" class="form-control" runat="server" />
                </div>
              </div>
            </asp:PlaceHolder>

            <asp:PlaceHolder ID="PhSequence" runat="server">
              <div class="form-group">
                <label class="control-label col-xs-12">
                  顺序号起始数字
                  <asp:RequiredFieldValidator ControlToValidate="TbSequence" errorMessage=" *" foreColor="red" display="Dynamic" runat="server"
                  />
                  <asp:RegularExpressionValidator ControlToValidate="TbSequence" ValidationExpression="\d+" Display="Dynamic" ErrorMessage="必须为大于等于零的整数"
                    foreColor="red" runat="server" />
                  <asp:CompareValidator ControlToValidate="TbSequence" Operator="GreaterThanEqual" ValueToCompare="0" Display="Dynamic" ErrorMessage="必须为大于等于零的整数"
                    foreColor="red" runat="server" />
                </label>
                <div class="col-xs-12">
                  <asp:TextBox id="TbSequence" Text="0" class="form-control" runat="server" />
                </div>
                <div class="col-xs-12 help-block">
                  每发布一篇信息，顺序号将在原有基础上自动增1
                </div>
              </div>

              <div class="form-group">
                <label class="control-label col-xs-12">
                  不同栏目顺序号归零
                </label>
                <div class="col-xs-12">
                  <asp:DropDownList id="DdlIsSequenceChannelZero" class="form-control" runat="server"></asp:DropDownList>
                </div>
                <div class="col-xs-12 help-block">
                  在不同的栏目中添加内容，顺序号需要归零后重新开始
                </div>
              </div>

              <div class="form-group">
                <label class="control-label col-xs-12">
                  不同部门顺序号归零
                </label>
                <div class="col-xs-12">
                  <asp:DropDownList id="DdlIsSequenceDepartmentZero" class="form-control" runat="server"></asp:DropDownList>
                </div>
                <div class="col-xs-12 help-block">
                  当添加内容的所属部门不相同时，顺序号需要按照部门归零后重新开始
                </div>
              </div>

              <div class="form-group">
                <label class="control-label col-xs-12">
                  不同年份顺序号归零
                </label>
                <div class="col-xs-12">
                  <asp:DropDownList id="DdlIsSequenceYearZero" class="form-control" runat="server"></asp:DropDownList>
                </div>
                <div class="col-xs-12 help-block">
                  当添加内容的添加时间为不同年份时，顺序号需要按照年份归零后重新开始
                </div>
              </div>
            </asp:PlaceHolder>

            <div class="form-group">
              <label class="control-label col-xs-12">
                分隔符
              </label>
              <div class="col-xs-12">
                <asp:TextBox id="TbSuffix" Text="-" class="form-control" runat="server" />
              </div>
            </div>

            <hr />

            <div class="form-group m-b-0">
              <div class="col-xs-12 text-right">
                <asp:Button id="BtnSubmit" class="btn btn-primary m-l-10" text="确 定" runat="server" onClick="Submit_OnClick" />
                <button type="button" class="btn btn-default m-l-10" onclick="window.parent.layer.closeAll()">取 消</button>
              </div>
            </div>

          </div>

        </form>
      </div>

    </div>
  </body>

  </html>