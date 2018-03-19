<%@ Page Language="C#" Inherits="SS.GovPublic.Pages.PageBase" %>
	<%@ Register TagPrefix="ctrl" Namespace="SiteServer.BackgroundPages.Controls" Assembly="SiteServer.BackgroundPages" %>
		<!DOCTYPE html>
		<html style="background-color: #fff;">

		<head>
			<meta charset="utf-8">
			<link href="assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
			<link href="assets/css/ionicons.min.css" rel="stylesheet" type="text/css" />
			<link href="assets/css/siteserver.min.css" rel="stylesheet" type="text/css" />
			<script src="assets/js/jquery.min.js" type="text/javascript"></script>
			<style>
				.scroll {
					overflow-x: scroll !important
				}

				.list-group {
					margin-bottom: 0;
				}

				.table>tbody>tr>td,
				.table>tbody>tr>th,
				.table>tfoot>tr>td,
				.table>tfoot>tr>th,
				.table>thead>tr>td,
				.table>thead>tr>th {
					border-top: none;
				}

				.table a,
				.table span,
				.table div {
					font-size: 12px !important;
				}
			</style>
			<script type="text/javascript">
				$(document).ready(function () {
					$('body').height($(window).height());
					$('body').addClass('scroll');
				});
			</script>
		</head>

		<body style="margin: 0; padding: 0; background-color: #fff;">
			<form class="m-0" runat="server">
				<div class="list-group mail-list">
					<div onclick="location.reload(true);" class="list-group-item b-0" style="background-color: #eee;cursor: pointer;">
						栏目列表
					</div>
				</div>
				<table class="table table-sm table-hover">
					<tbody>
						<ctrl:ChannelTree runat="server"></ctrl:ChannelTree>
					</tbody>
				</table>
			</form>
		</body>

		</html>