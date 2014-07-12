<%@ Control Language="C#" AutoEventWireup="true" 
	CodeBehind="GAPage.ascx.cs" 
	Inherits="GA.SC.UI.SampleSite.Sublayouts.GAPage" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>

<div class="GAPage">
	<h1><sc:Text Field="Title" runat="server"></sc:Text></h1>
	<sc:Text Field="Content" runat="server"></sc:Text>
</div>