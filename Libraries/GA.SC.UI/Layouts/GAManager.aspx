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
		* { font-family:Tahoma; font-size:10px; color:#333; }
		.clear { clear:both; }
		body { background:url("/sitecore/shell/themes/standard/gradients/gray1.gif") repeat-y scroll 50% 0 #4E4E4E; margin:0px; padding:0px; }
		h1, h2, h3, h4 { padding: 5px; margin:0px; color:#fff; font-style:normal; font-family:Tahoma;}
		h1 { font-size:18px; font-family:Tahoma; font-weight:normal; background:#fff; color:#072D6B; border-left:1px solid #333; border-right:1px solid #333; border-bottom:1px solid #eee; }
		h2 { background: none repeat scroll 0 0 #D9D9D9; border-bottom: 1px solid #ccc; display:block; margin:0px;
			border-top: 1px solid #fff; color: #555555; font: bold 8pt tahoma; padding: 1px 2px; vertical-align:middle; }
			h2 a { font-weight:normal; display:inline-block; margin-left:20px; }
			h2 img { vertical-align: middle; margin:0 6px 0 3px; }
		h3 { color:#333; padding:5px 0; }
		.odd,
		.even { padding:4px 0; }
		.odd { }
		.even { background:#fff; }
		.HelloWorld { display:none; width: 200px; display:inline-block; }
		.Algo {  }
			.CurKaryo { font-size:12px; font-weight: bold; padding:5px 0px; }
			.DNAList { font-size: 12px; margin:5px auto; height:300px; overflow:auto; border:1px solid #bbb; }
				.count { display:inline-block; width:10%; }
				.dna { display:inline-block; width:74%; text-align:center; }
				.fitness { display:inline-block; width:15%; text-align:right;}
			.box { text-align:center; border:1px solid #ccc; display:inline-block; margin:10px 5px; height:37px; font-size:17px; width:75px; background-color:#ccc; color:#fff; padding-top:13px; }
			.btn { display:inline-block; }
			.Display { }
			.Buttons input[type='submit'] { height:20px; }
		.Section { background:#F0F1F2; border-right:1px solid #333; border-left:1px solid #333; border-bottom:1px solid #bbb; position:relative;}
		.Controls { padding:10px; font:8pt tahoma; }
		.PopNav { display:inline-block; width:169px; vertical-align:top; text-align:right; border-right:2px solid #ccc; padding-right:15px; margin-right:15px; }
			.PopOptions { margin-bottom:10px; } 
				.PopOptions input[type='text'] { width:30px; text-align:center; }
				.PopOptions label { display:inline-block; width:95px; text-align:right; }
			.PopStatus { margin-bottom:10px; }
				.PopStatus label { font-weight:bold; display: inline-block; font-weight: bold; text-align: right; width: 105px; }
				.PopStatus span { }
			.Btns input { border:1px solid #999; background:#f8f8f8; cursor:pointer; }
				.Btns input:hover { background: #FFCC99; color: #002E4C; }
		.Results { width:50%; display:inline-block; vertical-align:top; border-right:2px solid #ccc; padding-right:15px; margin-right:15px; }
		.EV { display:inline-block; width:125px; }
			.EV .tags { height:108px; overflow:auto; border:1px solid #ccc; margin-bottom:10px; }
			.EV .entry { text-align:left; }
			.EV .key { display:inline-block; width:75px; text-align:right; text-transform:uppercase; }
			.EV .value { display:inline-block; font-weight:bold; }
	</style>
	<script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script type="text/javascript">
    	$(document).ready(function () {
    		$('h2').dblclick(function () {
    			$(this).next(".Controls").toggle();
    		});
    	});
	</script>
</head>
<body>
	<form method="post" runat="server" id="mainform">
		<div class="Algo">
			<h1>Genetic Algorithm Manager</h1>
			<div class="Section">
				<h2><asp:Image ID="imgPopEngVal" runat="server" /> Population Controls</h2>
				<div class="Controls">
					<div class="PopNav">
						<h3>Population Numbers</h3>
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
								<label title="the fitness value required for the algorithm to begin selecting based on the fittest karyotypes instead of randomly selecting a karyotype.">Fitness Threshold (?)</label>
								<asp:TextBox ID="txtThreshold" runat="server"></asp:TextBox>
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
					<div class="Results">
						<h3>Display Results</h3>
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
					<div class="EV">
						<h3>Tag Values</h3>
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
				</div>
			</div>
			<div class="Section">
				<h2><asp:Image ID="imgPopInfo" runat="server" />Population Makeup</h2>
				<div class="Controls">
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
		</div>
	</form>
</body>
</html>