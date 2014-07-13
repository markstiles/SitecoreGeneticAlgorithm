<%@ Page language="c#" AutoEventWireup="true" 
	Inherits="GA.SC.UI.Layouts.GAManager"
	CodeBehind="GAManager.aspx.cs" %>
<%@ Import Namespace="GA.Nucleus.Population" %>
<%@ Import Namespace="GA.SC.EV" %>
<%@ Import Namespace="GA.SC" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en" xml:lang="en" xmlns="http://www.w3.org/1999/xhtml">                  
<head>
	<title></title>
	<link href='http://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css'>
	<style>
		* { font-family:Tahoma; font-size:10px; }
		h1 { font-size:20px; margin:0 0 10px; }
		h2 { margin:0px 0px 10px;  font-size: 16px; font-weight: normal; text-align: left; border:1px solid #bbb; background:#ddd; padding:1px 7px; }
		.odd,
		.even { padding:4px 0; }
		.odd { }
		.even { background:#fff; }
		.HelloWorld { display:none; width: 200px; display:inline-block; }
		.Algo { width: 820px; background: none repeat scroll 0 0 #fff; border: 1px solid #ddd; padding: 10px; margin:0px auto 30px; }
			.left { width: 400px; display: inline-block; vertical-align:top; }
				.CurKaryo { font-size:12px; font-weight: bold; padding:5px 0px; background:#ddd; border:1px solid #bbb; border-bottom:0px;}
				.DNAList { font-size: 12px; height:300px; overflow:auto; border:1px solid #bbb; }
					.count { display:inline-block; width:35px; text-align:right; }
					.dna { display:inline-block; width:260px; text-align:center; }
					.fitness { display:inline-block; width:48px; text-align:center;}
			.right { width: 400px; display: inline-block; vertical-align:top; }
				.box { text-align:center; border:1px solid #ccc; display:inline-block; margin:10px 5px; height:37px; font-size:17px; width:75px; background-color:#ccc; color:#fff; padding-top:13px; }
				.btn { display:inline-block; }
				.Display { height:150px; }
				.Buttons input[type='submit'] { height:20px; }
		.Section { border:1px solid #ccc; padding:10px 15px; margin-bottom:3px; background:#eee; }
		.PopOptions { text-align: left; width:135px; display:inline-block; } 
		.PopOptions input[type='text'] { width:30px; text-align:center; }
		.PopOptions label { display:inline-block; width:95px; text-align:right; }
		.PopNav { display:inline-block; width:230px; vertical-align:top; }
		.PopStatus { text-align:left; margin-bottom:15px; }
			.PopStatus label { font-weight:bold; display: inline-block; font-weight: bold; text-align: right; width: 105px; }
			.PopStatus span { }
		.Btns input { border:1px solid #999; background:#f8f8f8; cursor:pointer; }
			.Btns input:hover { background: #FFCC99; color: #002E4C; }
		.EV { border:1px solid #ccc; background:#eee; padding:10px; }
			.EV .tags { height:108px; overflow:auto; border:1px solid #ccc; margin-bottom:10px; }
			.EV .entry { text-align:left; }
			.EV .key { display:inline-block; width:75px; text-align:right; text-transform:uppercase; }
			.EV .value { display:inline-block; font-weight:bold; }
	</style>
</head>
<body>
	<form method="post" runat="server" id="mainform">
		<div class="Algo">
			<h1>Algorithm Manager</h1>
			<div class="left">
				<div class="Section">
					<h2>Population Values</h2>
					<div class="PopOptions">
						<div class="formRow">
							<label title="the probability you'll mate (and possibly mutate) instead of just mutating.">Crossover Ratio (?)</label>
							<asp:TextBox ID="txtCrossover" runat="server"></asp:TextBox>
						</div>	
						<div class="formRow">
							<label title="the percentage of the population that doesn't change each generation.">Elitism Ratio (?)</label>
							<asp:TextBox ID="txtElitism" runat="server"></asp:TextBox>
						</div>
						<div class="formRow">
							<label title="the percentage of the highest fitness value that's acceptable in another karyotype as a candidate for selection.">Fitness Ratio (?)</label>
							<asp:TextBox ID="txtFitness" runat="server"></asp:TextBox>
						</div>
						<div class="formRow">
							<label title="probability that a karyotype will mutate.">Mutation Ratio (?)</label>
							<asp:TextBox ID="txtMutation" runat="server"></asp:TextBox>
						</div>
						<div class="formRow">
							<label title="number of times to try to randomly find a better parent from the one randomly selected.">Tournament Size (?)</label>
							<asp:TextBox ID="txtTourney" runat="server"></asp:TextBox>
						</div>
						<div class="formRow">
							<label title="number of karyotypes to create in your population.">Population Size (?)</label>
							<asp:TextBox ID="txtPopSize" runat="server"></asp:TextBox>
						</div>
					</div>
					<div class="PopNav">
						<div class="PopStatus">
							<label>Karyotype Count is:</label> 
							<span>
								<asp:Literal ID="ltlKaryos" runat="server"></asp:Literal>
							</span>
							<br/>
							<label>Unique Karyotypes:</label> 
							<span>
								<asp:Literal ID="ltlUKaryos" runat="server"></asp:Literal>
							</span>
							<br/>
						</div>
						<div class="Btns">
							<asp:Button ID="btnNextGen" runat="server" Text="Next Gen" OnClick="btnNextGen_Click" />
							<asp:Button ID="btnRestart" runat="server" Text="Restart" OnClick="btnRestart_Click" />
						</div>
					</div>
				</div>
				<div class="Section">
					<h2>Population Information</h2>
					<div class="CurKaryo">
						<asp:Literal ID="ltlKaryotype" runat="server" />
					</div>
					<div class="DNAList">
						<asp:Repeater ID="rptDNAList" runat="server">
							<ItemTemplate>
								<div class="<%# OddEven(Container.ItemIndex) %>">
									<div class="count"><%# Container.ItemIndex + 1 %>:</div> 
									<div class="dna"><%# ((IKaryotype)Container.DataItem).ExpressedHaploid.DNASequence() %></div>
									<div class="fitness"><%# ((IKaryotype)Container.DataItem).Fitness %></div>
								</div>
							</ItemTemplate>
						</asp:Repeater>
					</div>
				</div>
			</div>
			<div class="right">
				<div class="Section EV">
					<h2>Engagement Values by Tag</h2>
					<div class="tags">
						<asp:Repeater ID="rptEV" runat="server">
							<ItemTemplate>
								<div class="entry <%# OddEven(Container.ItemIndex) %>">
									<div class="key">
										<%# ((KeyValuePair<string, List<IEngagementValue>>)Container.DataItem).Key %> : 
									</div>
									<div class="value">
										<%# ((KeyValuePair<string, List<IEngagementValue>>)Container.DataItem).Value.Sum(a => ConfigUtil.Context.ValueModifier.CurrentValue(a)) %>
									</div>
								</div>
							</ItemTemplate>
						</asp:Repeater>
					</div>
					<div class="Btns">
						<asp:Button ID="btnClearEvents" runat="server" Text="Clear Events" OnClick="btnClearEvents_Click" />
					</div>
				</div>
				<div class="Section">
					<h2>Results</h2>
					<div class="Display">
						<asp:Panel ID="pnlOne" CssClass="box" runat="server">
							<asp:Literal ID="ltlOne" runat="server"></asp:Literal>
						</asp:Panel>
						<asp:Panel ID="pnlTwo" CssClass="box" runat="server">
							<asp:Literal ID="ltlTwo" runat="server"></asp:Literal>
						</asp:Panel>
						<asp:Panel ID="pnlThree" CssClass="box" runat="server">
							<asp:Literal ID="ltlThree" runat="server"></asp:Literal>
						</asp:Panel>
						<asp:Panel ID="pnlFour" CssClass="box" runat="server">
							<asp:Literal ID="ltlFour" runat="server"></asp:Literal>
						</asp:Panel>
						<asp:Panel ID="pnlFive" CssClass="box" runat="server">
							<asp:Literal ID="ltlFive" runat="server"></asp:Literal>
						</asp:Panel>
						<asp:Panel ID="pnlSix" CssClass="box" runat="server">
							<asp:Literal ID="ltlSix" runat="server"></asp:Literal>
						</asp:Panel>
						<asp:Panel ID="pnlSeven" CssClass="box" runat="server">
							<asp:Literal ID="ltlSeven" runat="server"></asp:Literal>
						</asp:Panel>
						<asp:Panel ID="pnlEight" CssClass="box" runat="server">
							<asp:Literal ID="ltlEight" runat="server"></asp:Literal>
						</asp:Panel>
					</div>
					<div class="Btns">
						<div class="btn">
							<asp:Button ID="btnA" runat="server" Text="Red" OnClick="btn_Click" />
						</div>
						<div class="btn">
							<asp:Button ID="btnB" runat="server" Text="Blue" OnClick="btn_Click" />
						</div>
						<div class="btn">
							<asp:Button ID="btnC" runat="server" Text="Black" OnClick="btn_Click" />
						</div>
						<div class="btn">
							<asp:Button ID="btnD" runat="server" Text="Green" OnClick="btn_Click" />
						</div>
						<div class="btn">
							<asp:Button ID="btnE" runat="server" Text="Purple" OnClick="btn_Click" />
						</div>
						<div class="btn">
							<asp:Button ID="btnF" runat="server" Text="Orange" OnClick="btn_Click" />
						</div>
						<div class="btn">
							<asp:Button ID="btnG" runat="server" Text="Magenta" OnClick="btn_Click" />
						</div>
					</div>
				</div>
			</div>
		</div>
	</form>
</body>
</html>