<%@ Control Language="C#" AutoEventWireup="true" 
	CodeBehind="Dev.ascx.cs" 
	Inherits="GA.UI.Sublayouts.Dev" %>

<style>
	.HelloWorld { display:none; width: 200px; display:inline-block; }
	.Algo { width: 820px;  }
		.left { width: 400px; display: inline-block; vertical-align:top; }
			.CurChrome { font-weight: bold; margin-bottom: 10px; }
			.ChromeList { font-size: 12px; }
		.right { width: 400px; display: inline-block; vertical-align:top; }
			.one, .two, .three, .four { display:inline-block; margin:10px; height:50px; width:50px; background-color:#ccc; }
			.A, .B, .C, .D { width:30px; display:inline-block; }

</style>

<div class="HelloWorld">
	<div class="Gene">
		<asp:Literal ID="ltlGene" runat="server"></asp:Literal>
	</div>
	<div class="Out">
		<asp:Literal ID="ltlOut" runat="server"></asp:Literal>
	</div>
</div>
<div class="Algo">
	<div class="left">
		<div class="CurChrome">
			<asp:Literal ID="ltlChrome" runat="server" />
		</div>
		<div class="ChromeList">
			<asp:Literal ID="ltlChromeList" runat="server" />
		</div>
	</div>
	<div class="right">
		<div class="Display">
			<div class="one">
				<asp:Literal ID="ltlOne" runat="server"></asp:Literal>
			</div>
			<div class="two">
				<asp:Literal ID="ltlTwo" runat="server"></asp:Literal>
			</div>
			<div class="three">
				<asp:Literal ID="ltlThree" runat="server"></asp:Literal>
			</div>
			<div class="four">
				<asp:Literal ID="ltlFour" runat="server"></asp:Literal>
			</div>
		</div>
		<div class="Buttons">
			<div class="A">
				<asp:Button ID="btnA" runat="server" Text="1" CssClass="One" OnClick="btn_Click" />
			</div>
			<div class="B">
				<asp:Button ID="btnB" runat="server" Text="2" CssClass="Two" OnClick="btn_Click" />
			</div>
			<div class="C">
				<asp:Button ID="btnC" runat="server" Text="3" CssClass="Three" OnClick="btn_Click" />
			</div>
			<div class="D">
				<asp:Button ID="btnD" runat="server" Text="4" CssClass="Four" OnClick="btn_Click" />
			</div>
			<div class="NextGen">
				<asp:Button ID="btnNextGen" runat="server" Text="Next Gen" OnClick="btnNextGen_Click" />
			</div>
			<div class="ClearEvents">
				<asp:Button ID="btnClearEvents" runat="server" Text="Clear Events" OnClick="btnClearEvents_Click" />
			</div>
			<div class="Restart">
				<asp:Button ID="btnRestart" runat="server" Text="Restart" OnClick="btnRestart_Click" />
			</div>
		</div>
		<div class="EV">
			<asp:Literal ID="ltlEV" runat="server" />
		</div>
	</div>
</div>