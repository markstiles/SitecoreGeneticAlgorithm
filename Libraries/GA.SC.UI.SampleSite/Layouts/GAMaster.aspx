<%@ Page language="c#" AutoEventWireup="true" 
	Inherits="GA.SC.UI.SampleSite.Layouts.GAMaster" 
	CodeBehind="GAMaster.aspx.cs" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>
<%@ Import Namespace="GA.Nucleus.Population" %>
<%@ Import Namespace="GA.Nucleus.Gene" %>
<%@ Import Namespace="GA.SC.EV" %>
<%@ Import Namespace="GA.SC" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en" xml:lang="en" xmlns="http://www.w3.org/1999/xhtml">                  
<head>
	<title></title>
	<link href='http://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css'>
	<style>
		* { font-family: 'Open Sans', sans-serif; font-size:12px; color:#444; }
		body { margin:0px; padding:0px; }
		h1 { font-size:17px; }
		a { color: #3366FF; }
		a:hover { text-decoration:underline; }
		.GAContent { text-align:center; border:1px solid #ccc; padding:5px; }
	        .GAContent a { font-weight:bold; font-size:44px; color:#fff; text-decoration:none; display:block; height:100%; }
		.main { width:1000px; margin:0px auto; }
			.header { }
				.logo { margin:10px 0px; display: inline-block; vertical-align:middle; }
					.logo a { font-size:28px; font-weight:bold; text-decoration: none; color:#000;  }
						.logo a:hover { text-decoration: none; color: #3366FF; }
				.headcontent { display: inline-block; vertical-align:middle; left: 228px; position: relative; width: 600px } 
					.headcontent .GAContent { height:40px; }
	                    .headcontent .GAContent a { font-size:16px; } 
				.nav { border-color: #ccc; border-style: solid; border-width: 1px 0; padding: 5px 0; }
					.nav ul { margin:0px; padding:0px; }
					.nav li { display:inline-block; }
					.nav a { display:block; margin:0 4px; padding:3px 5px; border-radius:4px; text-decoration:none; font-size:14px; }
					.nav a:hover { color:#fff; background-color:#3366FF; }
			.content { }
			.left, .center, .right { display:inline-block; vertical-align:top; min-height:500px; padding:10px; }	
			.left { width:220px; }
			.center { width:430px; border-left:1px solid #ccc; border-right:1px solid #ccc; }
			.right { width:280px; }
				.left .GAContent,
				.right .GAContent { height:120px; margin-bottom:25px; }
				.center .GAContent { height:120px; }
		.infoBox { position:absolute; top:0px; left:50px; }
			.infoBox .tab { position:absolute; bottom:-38px; top:-6px; cursor:pointer; height:19px; background:#fff; box-shadow:4px 3px 4px -2px #777; font-weight:bold; padding:16px 10px 10px; width:70px; border-radius:0px 0px 10px 0px; border:1px solid #777; border-top:0px; }
			.infoBox .monitor { display: none; padding:0px 20px 10px; border:1px solid #ccc; border-radius:0 0 10px 10px; margin:0px 0 0 6px; border-top:0px; background:#eee; box-shadow:4px 3px 4px -2px #777; }
		.col1, .col2 { display: inline-block; vertical-align:top; }
		.col1 { width: 200px; padding-top:50px; }
		.col2 { width: 650px; }
		.PopOptions { margin-bottom:10px; } 
			.PopOptions label { display:inline-block; width:125px; text-align:right; margin-right:5px;}
			.PopOptions span { font-weight: bold; }
		.DNAList { font-size: 12px; margin:5px auto; height:646px; overflow:auto; border:1px solid #bbb; }
			.count { display:inline-block; width:8%; }
			.dna { display:inline-block; width:74%; text-align:center; }
			.fitness { display:inline-block; width:13%; text-align:right; }
			.gender { display:inline-block; width: 3%; text-align:center; }
		.EV { display:inline-block; width:165px; }
			.EV .tags { height:183px; overflow:auto; border:1px solid #ccc; margin-bottom:5px; }
			.EV .entry { text-align:left; }
			.EV .key { display:inline-block; width:75px; text-align:right; text-transform:uppercase; }
			.EV .value { display:inline-block; font-weight:bold; }
		.genotype { }
			.genotype label { display: block; }
			.genotype > label { margin-bottom:10px; }
			.chromo { width:165px; border:1px solid #aaa; padding:3px; }
				.chromo label.title { background: none repeat scroll 0 0 #fff; border: 1px solid #ccc; padding: 2px 4px; }
				.chromo label { margin-bottom:5px; }
				.genes { height:100px; overflow:auto; border:1px solid #ccc; margin-bottom:5px; }
					.gene { }
		.odd,
		.even { padding:4px 5px; #ccc; border-right:1px solid #ccc; }
		.odd { }
		.even { background:#fff; }
	</style>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script> 
    <script>
		var contextSite = <asp:Literal ID="ltlContextSite" runat="server"/>;
        var $j = jQuery.noConflict();
        $j(document).ready(function () {
            $j(".GAContent a, .nav a").click(function (e) {
                e.preventDefault();
                var tagName = $j(this).attr("tag");
                var href = $j(this).attr("href");
                var value = $j(this).attr("value");
                $j.ajax({
                    type: "POST",
                    url: "/sitecore modules/Web/GA/WebService/EventTracking.asmx/TrackEvent",
                    data: "{ 'TagClick':'" + tagName + "', 'Site':'" + contextSite + "', 'Value':'" + value + "'}",
                    contentType: "application/json",
                    dataType: "json",
                    success: function (data, status) {
                        window.location.href = href;
                    },
                    error: function (e) { }
                });
            });
            $j(".infoBox .tab").click(function(e){
            	var mon = $j(".infoBox .monitor");
            	if(mon.is( ":hidden" )){
            		mon.slideDown();
            	} else {
            		mon.slideUp();
            	}
            });
        });
    </script>
</head>
<body>
	<form method="post" runat="server" id="mainform">
		<div class="main">
			<div class="header">
				<div class="logo"><asp:HyperLink ID="lnkLogo" Text="GA Testsite" runat="server"></asp:HyperLink></div>
				<div class="headcontent">
					<sc:Placeholder Key="headcontent" runat="server"></sc:Placeholder>
				</div>
				<div class="nav">
					<asp:Repeater ID="rptNav" runat="server">
						<HeaderTemplate><ul></HeaderTemplate>
						<ItemTemplate>
							<li><a value="1" tag="<%# ((KeyValuePair<string, string>)Container.DataItem).Value %>" href="<%# ((KeyValuePair<string, string>)Container.DataItem).Key %>"><%# ((KeyValuePair<string, string>)Container.DataItem).Value %></a></li>
						</ItemTemplate>
						<FooterTemplate></ul></FooterTemplate>
					</asp:Repeater>
				</div>
			</div>
			<div class="content">
				<div class="left">
					<sc:Placeholder Key="leftcontent1" runat="server"></sc:Placeholder>
					<sc:Placeholder Key="leftcontent2" runat="server"></sc:Placeholder>
					<sc:Placeholder Key="leftcontent3" runat="server"></sc:Placeholder>
				</div>
				<div class="center">
					<sc:Placeholder Key="main" runat="server"></sc:Placeholder>
					<sc:Placeholder Key="centercontent" runat="server"></sc:Placeholder>
				</div>
				<div class="right">
					<sc:Placeholder Key="rightcontent1" runat="server"></sc:Placeholder>
					<sc:Placeholder Key="rightcontent2" runat="server"></sc:Placeholder>
					<sc:Placeholder Key="rightcontent3" runat="server"></sc:Placeholder>
				</div>
			</div>
		</div>
		<div class="infoBox">
			<div class="monitor">
				<div class="col1">
					<div class="btns">
						<asp:Button ID="btnClearClicks" runat="server" Text="Clear Clicks" OnClick="btnClearClicks_Click" />
						<asp:Button ID="btnRestart" runat="server" Text="Restart" OnClick="btnRestart_Click" />
					</div>
					<h3>Population Numbers</h3>
					<div class="PopOptions">	
						<div class="formRow">
							<label title="The probability you'll mate.">Crossover Ratio (?) : </label>
							<span><asp:Literal ID="ltlCrossover" runat="server"></asp:Literal></span>
						</div>	
						<div class="formRow">
							<label title="The percentage of the population that doesn't change each generation.">Elitism Ratio (?) : </label>
							<span><asp:Literal ID="ltlElitism" runat="server"></asp:Literal></span>
						</div>
						<div class="formRow">
							<label title="The percentage of the highest fitness value that's acceptable in another karyotype as a candidate for selection.">Fitness Ratio (?) : </label>
							<span><asp:Literal ID="ltlFitness" runat="server"></asp:Literal></span>
						</div>
						<div class="formRow">
							<label title="If the list is ascending or descending">Fitness Sorting (?) : </label>
							<span><asp:Literal ID="ltlFitSort" runat="server"></asp:Literal></span>
						</div>
						<div class="formRow">
							<label title="The fitness value required for the algorithm to begin selecting based on the fittest karyotypes instead of randomly selecting a karyotype.">Fitness Threshold (?) : </label>
							<span><asp:Literal ID="ltlThreshold" runat="server"></asp:Literal></span>
						</div>
						<div class="formRow">
							<label title="Probability that a karyotype will mutate.">Mutation Ratio (?) : </label>
							<span><asp:Literal ID="ltlMutation" runat="server"></asp:Literal></span>
						</div>
						<div class="formRow">
							<label title="Number of times to try to randomly find a better parent from the one randomly selected.">Tournament Size (?) : </label>
							<span><asp:Literal ID="ltlTourney" runat="server"></asp:Literal></span>
						</div>
						<div class="formRow">
							<label title="Number of karyotypes to create in your population.">Population Size (?) : </label>
							<span><asp:Literal ID="ltlPopSize" runat="server"></asp:Literal></span>
						</div>
					</div>
					<h3>Genotype</h3>
					<div class="genotype">
						<label>Chromosomes:</label>
						<asp:Repeater ID="rptChromos" runat="server" OnItemDataBound="rptChromos_ItemDataBound">
							<ItemTemplate>
								<div class="chromo">
									<label class="title"><%# ((KeyValuePair<string, Genotype>)Container.DataItem).Key %> - Length : <%# ((KeyValuePair<string, Genotype>)Container.DataItem).Value.GeneLimit %></label>
									<label><%# ((KeyValuePair<string, Genotype>)Container.DataItem).Value.Count %> Genes</label>
									<div class="genes">
										<asp:Repeater ID="rptGenes" runat="server">
											<ItemTemplate>
												<div class="gene <%# OddEven(Container.ItemIndex) %>">
													<%# ((IGene)Container.DataItem).GeneID %>
												</div>
											</ItemTemplate>
										</asp:Repeater>
									</div>
								</div>
							</ItemTemplate>
						</asp:Repeater>
					</div>
					<h3>Click Values</h3>
					<div class="EV">
						<div class="tags">
							<asp:Repeater ID="rptEV" runat="server">
								<ItemTemplate>
									<div class="entry <%# OddEven(Container.ItemIndex) %>">
										<div class="key">
											<%# ((KeyValuePair<string, List<IEngagementValue>>)Container.DataItem).Key %> : 
										</div>
										<div class="value">
											<%# ((KeyValuePair<string, List<IEngagementValue>>)Container.DataItem).Value.Sum(a => a.Value) %>
										</div>
									</div>
								</ItemTemplate>
							</asp:Repeater>
						</div>
					</div>
				</div>
				<div class="col2">
					<h3>Population Makeup</h3>
					<div class="Controls">
						<div>
							<label>Unique Karyotypes:</label> 
							<span>
								<asp:Literal ID="ltlUKaryos" runat="server"></asp:Literal>
							</span>
						</div>
						<div class="DNAList">
							<asp:Repeater ID="rptDNAList" runat="server">
								<ItemTemplate>
									<div class="<%# OddEven(Container.ItemIndex) %>">
										<div class="count"><%# Container.ItemIndex + 1 %>:</div> 
										<div class="dna"><%# ((IKaryotype)Container.DataItem).Phenotype.DNASequence() %></div>
										<div class="fitness"><%# ((IKaryotype)Container.DataItem).Fitness %></div>
										<div class="gender"><%# (((IKaryotype)Container.DataItem).Gender) ? "M" : "F" %></div>
									</div>
								</ItemTemplate>
							</asp:Repeater>
						</div>
					</div>
				</div>
			</div>
			<div class="tab">
				GA Monitor
			</div>
		</div>
	</form>
</body>
</html>
