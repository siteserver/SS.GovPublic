<%@ Page Language="C#" Inherits="SS.GovPublic.Core.Pages.PageCategoryClass" %>
	<!DOCTYPE html>
	<html>

	<head>
		<meta charset="utf-8">
		<link href="../assets/plugin-utils/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
		<link href="../assets/plugin-utils/css/plugin-utils.css" rel="stylesheet" type="text/css" />
		<script src="../assets/js/jquery.min.js" type="text/javascript"></script>
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
										<b>分类法管理</b>
									</h4>
									<p class="text-muted font-13 m-b-30">
										点击分类名称可以进入分类项维护界面
									</p>
								</div>
							</div>

							<asp:Literal id="LtlMessage" runat="server" />

							<div class="form-horizontal">

								<asp:dataGrid id="DgContents" showHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-hover"
								  gridlines="none" runat="server">
									<Columns>
										<asp:TemplateColumn HeaderText="分类名称">
											<ItemTemplate>
												<asp:Literal ID="ltlClassName" runat="server"></asp:Literal>
											</ItemTemplate>
											<ItemStyle cssClass="center" />
										</asp:TemplateColumn>
										<asp:TemplateColumn HeaderText="分类代码">
											<ItemTemplate>
												<asp:Literal ID="ltlClassCode" runat="server"></asp:Literal>
											</ItemTemplate>
											<ItemStyle cssClass="center" />
										</asp:TemplateColumn>
										<asp:TemplateColumn HeaderText="上升">
											<ItemTemplate>
												<asp:HyperLink ID="hlUpLinkButton" runat="server">
													上升
												</asp:HyperLink>
											</ItemTemplate>
											<ItemStyle Width="60" cssClass="center" />
										</asp:TemplateColumn>
										<asp:TemplateColumn HeaderText="下降">
											<ItemTemplate>
												<asp:HyperLink ID="hlDownLinkButton" runat="server">
													下降
												</asp:HyperLink>
											</ItemTemplate>
											<ItemStyle Width="60" cssClass="center" />
										</asp:TemplateColumn>
										<asp:TemplateColumn HeaderText="启用">
											<ItemTemplate>
												<asp:Literal ID="ltlIsEnabled" runat="server"></asp:Literal>
											</ItemTemplate>
											<ItemStyle Width="60" cssClass="center" />
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
										<asp:Button class="btn btn-primary m-l-10" id="BtnAdd" text="新增分类法" runat="server" />
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