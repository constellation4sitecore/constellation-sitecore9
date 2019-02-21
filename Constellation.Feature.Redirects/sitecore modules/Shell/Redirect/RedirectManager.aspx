<%@ Page Language="C#" Inherits="Constellation.Feature.Redirects.UI.RedirectManager" %>

<%@ Register Assembly="Sitecore.Kernel" Namespace="Sitecore.Web.UI.HtmlControls" TagPrefix="sc" %>
<%@ Register Assembly="Sitecore.Kernel" Namespace="Sitecore.Web.UI.WebControls" TagPrefix="sc" %>
<%@ Register Assembly="Sitecore.Kernel" Namespace="Sitecore.Web.UI.WebControls.Ribbons" TagPrefix="sc" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ca" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head id="Head1" runat="server">
	<title>Sitecore</title>
	<sc:Stylesheet ID="Stylesheet1" Src="Content Manager.css" DeviceDependant="true" runat="server" />
	<sc:Stylesheet ID="Stylesheet2" Src="Ribbon.css" DeviceDependant="true" runat="server" />
	<sc:Stylesheet ID="Stylesheet3" Src="Grid.css" DeviceDependant="true" runat="server" />
	<sc:Script ID="Script1" Src="/sitecore/shell/Controls/InternetExplorer.js" runat="server" />
	<sc:Script ID="Script2" Src="/sitecore/shell/Controls/Sitecore.js" runat="server" />
	<sc:Script ID="Script3" Src="/sitecore/shell/Controls/SitecoreObjects.js" runat="server" />
	<sc:Script ID="Script4" Src="/sitecore/shell/Applications/Content Manager/Content Editor.js" runat="server" />
	<script type="text/javascript" language="javascript">

		function OnResize() {
			var doc = $(document.documentElement);
			var ribbon = $("RibbonContainer");
			var grid = $("GridContainer");

			grid.style.height = doc.getHeight() - ribbon.getHeight() + 'px';
			grid.style.width = doc.getWidth() + 'px';

			Redirects.render();

			/* re-render again after some "magic amount of time" - without this second re-render grid doesn't pick correct width sometimes */
			setTimeout("Redirects.render()", 300);
		}

		function refresh() {
			Redirects.scHandler.refresh();
		}

		window.onresize = OnResize;
  
	</script>
</head>
<body style="background: transparent; height: 100%">
	<form id="RedirectManagerForm" runat="server">
	<sc:AjaxScriptManager ID="Ajaxscriptmanager1" runat="server"></sc:AjaxScriptManager>
	<sc:ContinuationManager ID="Continuationmanager1" runat="server"></sc:ContinuationManager>
	<table border="0" cellspacing="0" cellpadding="0" width="100%" height="100%">
		<tbody>
			<tr>
				<td id="RibbonContainer">
					<sc:Ribbon runat="server" ID="Ribbon"></sc:Ribbon>
				</td>
			</tr>
			<tr>
				<td id="GridCell" height="100%" valign="top">
					<sc:Border runat="server" ID="GridContainer">
						<ca:grid id="Redirects" autofocussearchbox="false" runningmode="Callback" cssclass="Grid"
							showheader="true" headercssclass="GridHeader" fillcontainer="true" footercssclass="GridFooter"
							groupbycssclass="GroupByCell" groupbytextcssclass="GroupByText" groupbysortascendingimageurl="group_asc.gif"
							groupbysortdescendingimageurl="group_desc.gif" groupbysortimagewidth="10" groupbysortimageheight="10"
							groupingnotificationtextcssclass="GridHeaderText" groupingpagesize="5" manualpaging="true"
							pagesize="15" pagerstyle="Slider" pagertextcssclass="GridFooterText" pagerbuttonwidth="41"
							pagerbuttonheight="22" pagerimagesfolderurl="/sitecore/shell/Themes/standard/componentart/grid/pager/"
							showsearchbox="true" searchtextcssclass="GridHeaderText" searchboxcssclass="SearchBox"
							sliderheight="20" sliderwidth="150" slidergripwidth="9" sliderpopupoffsetx="20"
							sliderpopupclienttemplateid="SliderTemplate" treelineimagesfolderurl="/sitecore/shell/Themes/standard/componentart/grid/lines/"
							treelineimagewidth="22" treelineimageheight="19" preexpandongroup="false" imagesbaseurl="/sitecore/shell/Themes/standard/componentart/grid/"
							indentcellwidth="22" loadingpanelclienttemplateid="LoadingFeedbackTemplate" loadingpanelposition="MiddleCenter"
							width="100%" height="100%" runat="server" xmlns:ca="http://www.sitecore.net/xhtml">
							<levels>
								<ca:gridlevel datakeyfield="ItemId" showtableheading="false" showselectorcells="false" rowcssclass="Row" columnreorderindicatorimageurl="reorder.gif" datacellcssclass="DataCell" headingcellcssclass="HeadingCell" headingcellhovercssclass="HeadingCellHover" headingcellactivecssclass="HeadingCellActive" headingrowcssclass="HeadingRow" headingtextcssclass="HeadingCellText" selectedrowcssclass="SelectedRow" groupheadingcssclass="GroupHeading" sortascendingimageurl="asc.gif" sortdescendingimageurl="desc.gif" sortimagewidth="13" sortimageheight="13">
								<columns>
									<ca:gridcolumn datafield="ItemId" visible="false" issearchable="false" />
								    <ca:gridcolumn datafield="SiteName" allowsorting="true" issearchable="false" allowgrouping="true" sorteddatacellcssclass="SortedDataCell" headingtext="Site Name" />
									<ca:gridcolumn datafield="OldUrl" allowsorting="true" issearchable="true" allowgrouping="false" sorteddatacellcssclass="SortedDataCell" headingtext="Old URL" />
									<ca:gridcolumn datafield="NewUrl" allowsorting="true" issearchable="true" allowgrouping="false" sorteddatacellcssclass="SortedDataCell" headingtext="New URL" />
									<ca:gridcolumn datafield="IsPermanent" allowsorting="true" issearchable="false" allowgrouping="true" sorteddatacellcssclass="SortedDataCell" headingtext="Is Permanent?" />
								</columns>
								</ca:gridlevel>
							</levels>
							<clienttemplates>
								<ca:clienttemplate id="LocalNameTemplate">
									<img src="w=16&amp;h=16&amp;as=1## DataItem.GetMember('Profile.PortraitFullPath').Value ##" width="16" height="16" border="0" alt="" align="absmiddle" />
									## DataItem.GetMember('LocalName').Value ##
								</ca:clienttemplate>
								<ca:clienttemplate id="LoadingFeedbackTemplate">
									<table cellspacing="0" cellpadding="0" border="0">
										<tr>
											<td style="font-size:10px;"><sc:Text ID="Literal1" text="Loading..." runat="server"></sc:Text>;</td>
											<td><img src="/sitecore/shell/Themes/standard/componentart/grid/spinner.gif?w=16&amp;h=16&amp;as=1" width="16" height="16" border="0" /></td>
										</tr>
									</table>
								</ca:clienttemplate>
			  
								<ca:clienttemplate id="SliderTemplate">
									<table class="SliderPopup" cellspacing="0" cellpadding="0" border="0">
										<tr>
										  <td><div style="padding:4px;font:8pt tahoma;white-space:nowrap;overflow:hidden">## DataItem.GetMember('Name').Value ##</div></td>
										</tr>
										<tr>
										  <td style="height:14px;background-color:#666666;padding:1px 8px 1px 8px;color:white">
										  ## DataItem.PageIndex + 1 ## / ## Redirects.PageCount ##
										  </td>
										</tr>
									</table>
								</ca:clienttemplate>
							</clienttemplates>
						</ca:grid>
					</sc:Border>
				</td>
			</tr>
		</tbody>
	</table>
	</form>
</body>
</html>
