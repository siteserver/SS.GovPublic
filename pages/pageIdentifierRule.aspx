<%@ Page Language="C#" Inherits="SS.GovPublic.Core.Pages.PageIdentifierRule" %>
	<!DOCTYPE html>
	<html>

	<head>
		<meta charset="utf-8">
		<link href="../assets/plugin-utils/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
		<link href="../assets/plugin-utils/css/plugin-utils.css" rel="stylesheet" type="text/css" />
		<script src="../assets/js/jquery.min.js"></script>
		<script src="../assets/layer/layer.min.js" type="text/javascript"></script>
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
										<b>索引号生成规则管理</b>
									</h4>
									<p class="text-muted font-13 m-b-30">
										索引号预览：
										<asp:Literal ID="LtlPreview" runat="server"></asp:Literal>
										如果修改了索引号生成规则，之前添加的信息将不受影响。
									</p>
								</div>
							</div>

							<asp:Literal id="LtlMessage" runat="server" />

							<asp:dataGrid id="DgContents" showHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-hover"
							gridlines="none" runat="server">
								<Columns>
									<asp:TemplateColumn HeaderText="规则排序">
										<ItemTemplate>
											<asp:Literal ID="ltlIndex" runat="server"></asp:Literal>
										</ItemTemplate>
										<ItemStyle cssClass="center" Width="100" />
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="规则名称">
										<ItemTemplate>
											<asp:Literal ID="ltlRuleName" runat="server"></asp:Literal>
										</ItemTemplate>
										<ItemStyle cssClass="center" />
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="规则类型">
										<ItemTemplate>
											<asp:Literal ID="ltlIdentifierType" runat="server"></asp:Literal>
										</ItemTemplate>
										<ItemStyle cssClass="center" />
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="最小位数">
										<ItemTemplate>
											<asp:Literal ID="ltlMinLength" runat="server"></asp:Literal>
										</ItemTemplate>
										<ItemStyle Width="100" cssClass="center" />
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="分隔符">
										<ItemTemplate>
											<asp:Literal ID="ltlSuffix" runat="server"></asp:Literal>
										</ItemTemplate>
										<ItemStyle Width="100" cssClass="center" />
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="上升">
										<ItemTemplate>
											<asp:HyperLink ID="hlUpLinkButton" runat="server">上升</asp:HyperLink>
										</ItemTemplate>
										<ItemStyle Width="60" cssClass="center" />
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="下降">
										<ItemTemplate>
											<asp:HyperLink ID="hlDownLinkButton" runat="server">下降</asp:HyperLink>
										</ItemTemplate>
										<ItemStyle Width="60" cssClass="center" />
									</asp:TemplateColumn>
									<asp:TemplateColumn>
										<ItemTemplate>
											<asp:Literal ID="ltlSettingUrl" runat="server"></asp:Literal>
										</ItemTemplate>
										<ItemStyle Width="120" cssClass="center" />
									</asp:TemplateColumn>
									<asp:TemplateColumn>
										<ItemTemplate>
											<asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
										</ItemTemplate>
										<ItemStyle Width="60" cssClass="center" />
									</asp:TemplateColumn>
									<asp:TemplateColumn>
										<ItemTemplate>
											<asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal>
										</ItemTemplate>
										<ItemStyle Width="60" cssClass="center" />
									</asp:TemplateColumn>
								</Columns>
							</asp:dataGrid>

							<hr />

							<div class="form-group">
								<div class="col-xs-12">
									<asp:Button class="btn btn-primary m-l-10" id="BtnAdd" Text="新增索引规则" runat="server" />
								</div>
							</div>

						</div>
					</div>

				</form>
			</div>

		</div>
	</body>

	</html>