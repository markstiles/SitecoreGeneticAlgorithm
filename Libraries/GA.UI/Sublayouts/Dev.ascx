<%@ Control Language="C#" AutoEventWireup="true" 
	CodeBehind="Dev.ascx.cs" 
	Inherits="GA.UI.Sublayouts.Dev" %>
<%@ Import Namespace="GA.Lib.Chromosome" %>
<%@ Import Namespace="GA.SC" %>

<style>
	* { font-family:Tahoma; font-size:10px; }
	.HelloWorld { display:none; width: 200px; display:inline-block; }
	.Algo { width: 820px;  }
		.left { width: 400px; display: inline-block; vertical-align:top; }
			.CurChrome { font-size:20px; font-weight: bold; background:#ddd; border:1px solid #bbb; border-bottom:0px;}
			.ChromeList { font-size: 12px; height:300px; overflow:auto; border:1px solid #bbb; }
				.ChromeList .odd,
				.ChromeList .even { padding:4px 0; }
				.ChromeList .odd { }
				.ChromeList .even { background:#fff; }
                .count { display:inline-block; width:35px; text-align:right; }
                .dna { display:inline-block; width:260px; text-align:center; }
                .fitness { display:inline-block; width:48px; text-align:center;}
		.right { width: 400px; display: inline-block; vertical-align:top; }
			.one, .two, .three, .four { display:inline-block; margin:10px; height:50px; width:50px; background-color:#ccc; }
			.A, .B, .C, .D { width:30px; display:inline-block; }
	.Section { border:1px solid #ccc; padding:10px 15px; margin-bottom:10px; }
	.PopOptions { text-align: left; width:135px; display:inline-block; } 
	.PopOptions input[type='text'] { width:30px; text-align:center; }
	.PopOptions label { display:inline-block; width:85px; text-align:right; }
	.PopNav { display:inline-block; width:230px; vertical-align:top; }
	.ChromStatus { text-align:left; margin-bottom:15px; }
		.ChromStatus label { font-weight:bold; display: inline-block; font-weight: bold; text-align: right; width: 87px; }
		.ChromStatus span { }
	.PopNav .Btns input { border:1px solid #999; background:#f8f8f8; cursor:pointer; }
		.PopNav .Btns input:hover { background: #FFCC99; color: #002E4C; }
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
		<div class="Section">
			<div class="PopOptions">
				<div class="formRow">
					<label>Crossover Ratio</label>
					<asp:TextBox ID="txtCrossover" runat="server"></asp:TextBox>
				</div>	
				<div class="formRow">
					<label>Elitism Ratio</label>
					<asp:TextBox ID="txtElitism" runat="server"></asp:TextBox>
				</div>
				<div class="formRow">
					<label>Fitness Ratio</label>
					<asp:TextBox ID="txtFitness" runat="server"></asp:TextBox>
				</div>
				<div class="formRow">
					<label>Mutation Ratio</label>
					<asp:TextBox ID="txtMutation" runat="server"></asp:TextBox>
				</div>
				<div class="formRow">
					<label>Population Scalar</label>
					<asp:TextBox ID="txtScalar" runat="server"></asp:TextBox>
				</div>
				<div class="formRow">
					<label>Tournament Size</label>
					<asp:TextBox ID="txtTourney" runat="server"></asp:TextBox>
				</div>
			</div>
			<div class="PopNav">
				<div class="ChromStatus">
					<label>Gene Count is:</label> 
					<span>
						<asp:Literal ID="ltlChromes" runat="server"></asp:Literal>
					</span>
					<br/>
					<label>Unique Genes:</label> 
					<span>
						<asp:Literal ID="ltlUChromes" runat="server"></asp:Literal>
					</span>
					<br/>
				</div>
				<div class="Btns">
					<asp:Button ID="btnNextGen" runat="server" Text="Next Gen" OnClick="btnNextGen_Click" />
					<asp:Button ID="btnClearEvents" runat="server" Text="Clear Events" OnClick="btnClearEvents_Click" />
					<asp:Button ID="btnRestart" runat="server" Text="Restart" OnClick="btnRestart_Click" />
				</div>
			</div>
		</div>
		<div class="Section">
			<div class="CurChrome">
				<asp:Literal ID="ltlChrome" runat="server" />
			</div>
			<div class="ChromeList">
				<asp:Repeater ID="rptChromeList" runat="server">
					<ItemTemplate>
						<div class="<%# OddEven(Container.ItemIndex) %>">
							<div class="count"><%# Container.ItemIndex + 1 %>:</div> 
							<div class="dna"><%# ((IChromosome)Container.DataItem).GeneSequence() %></div>
                            <div class="fitness"><%# ((IChromosome)Container.DataItem).Fitness %></div>
						</div>
					</ItemTemplate>
				</asp:Repeater>
				<asp:Literal ID="ltlChromeList" runat="server" />
			</div>
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
				<asp:Button ID="btnA" runat="server" CssClass="One" OnClick="btn_Click" />
			</div>
			<div class="B">
				<asp:Button ID="btnB" runat="server" CssClass="Two" OnClick="btn_Click" />
			</div>
			<div class="C">
				<asp:Button ID="btnC" runat="server" CssClass="Three" OnClick="btn_Click" />
			</div>
			<div class="D">
				<asp:Button ID="btnD" runat="server" CssClass="Four" OnClick="btn_Click" />
			</div>
		</div>
		<div class="EV">
			<asp:Repeater ID="rptEV" runat="server">
				<ItemTemplate>
					<%# ((KeyValuePair<string, List<EngagementValue>>)Container.DataItem).Key %>-<%# ((KeyValuePair<string, List<EngagementValue>>)Container.DataItem).Value.Sum(a => a.CurrentValue()) %><br/>
				</ItemTemplate>
			</asp:Repeater>
		</div>
	</div>
</div>