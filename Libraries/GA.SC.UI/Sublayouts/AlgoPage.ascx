<%@ Control Language="C#" AutoEventWireup="true" 
	CodeBehind="AlgoPage.ascx.cs" 
	Inherits="GA.SC.UI.Sublayouts.AlgoPage" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>

<div class="AlgoPage">
	<h1><sc:Text Field="Title" runat="server"></sc:Text></h1>
	<sc:Text Field="Content" runat="server"></sc:Text>
</div>