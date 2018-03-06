<%@ Page Language="C#" Inherits="SS.GovPublic.Pages.ModalCategoryClassAdd" %>
	<!DOCTYPE html>
	<html style="background:#fff">

	<head>
		<meta charset="utf-8">
		<link href="assets/plugin-utils/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
		<link href="assets/plugin-utils/css/plugin-utils.css" rel="stylesheet" type="text/css" />
		<script src="assets/js/jquery.min.js" type="text/javascript"></script>
		<script src="assets/layer/layer.min.js" type="text/javascript"></script>
	</head>

	<body style="padding: 0;background:#fff">
		<div style="padding: 20px 0;">

			<div class="container">
				<form id="form" runat="server" class="form-horizontal">

					<asp:Literal id="LtlMessage" runat="server" />

					<div class="form-horizontal">

						<div class="form-group">
							<label class="control-label col-xs-12">
								分类法名称
								<asp:RequiredFieldValidator ControlToValidate="TbClassName" errorMessage=" *" foreColor="red" display="Dynamic" runat="server"
								/>
								<asp:RegularExpressionValidator runat="server" ControlToValidate="TbClassName" ValidationExpression="[^',]+" errorMessage=" *"
								  foreColor="red" display="Dynamic" />
							</label>
							<div class="col-xs-12">
								<asp:TextBox class="form-control" id="TbClassName" runat="server" />
							</div>
						</div>

						<div class="form-group">
							<label class="control-label col-xs-12">
								分类代码
								<asp:RequiredFieldValidator ControlToValidate="TbClassCode" errorMessage=" *" foreColor="red" display="Dynamic" runat="server"
								/>
								<asp:RegularExpressionValidator runat="server" ControlToValidate="TbClassCode" ValidationExpression="[^',]+" errorMessage=" *"
								  foreColor="red" display="Dynamic" />
							</label>
							<div class="col-xs-12">
								<asp:TextBox cssClass="form-control" id="TbClassCode" runat="server" />
							</div>
						</div>

						<div class="form-group">
							<label class="control-label col-xs-12">
								是否启用分类
							</label>
							<div class="col-xs-12">
								<asp:DropDownList ID="DdlIsEnabled" cssClass="form-control" runat="server">
									<asp:ListItem Text="启用" Value="True" Selected="true"></asp:ListItem>
									<asp:ListItem Text="不启用" Value="False"></asp:ListItem>
								</asp:DropDownList>
							</div>
						</div>

						<div class="form-group">
							<label class="control-label col-xs-12">
								说明
							</label>
							<div class="col-xs-12">
								<asp:TextBox class="form-control" style="height:50px" TextMode="MultiLine" MaxLength="50" id="TbDescription" runat="server"
								/>
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