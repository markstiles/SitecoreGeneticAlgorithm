<%@ Control Language="C#" AutoEventWireup="true" 
	CodeBehind="GAMonitor.ascx.cs" 
	Inherits="GA.SC.UI.SampleSite.Sublayouts.GAMonitor" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>
<%@ Import Namespace="GA.Nucleus.Population" %>
<%@ Import Namespace="GA.Nucleus.Gene" %>
<%@ Import Namespace="GA.SC.UI.SampleSite.EV" %>

<style>
	h3 { background-color:#474747; color:#fff; height:40px; line-height:40px; padding:0 15px; margin:0px; border-top:5px solid #dc291e; }
        h3 label { color:#fff; font-weight:normal; }
    .infoBox { position:absolute; top:0px; left:50px; color:#000; }
		.infoBox .tab { position:absolute; bottom:-38px; top:-6px; cursor:pointer; height:19px; background:#fff; box-shadow:4px 3px 4px -2px #777; font-weight:bold; padding:16px 10px 10px; width:70px; border-radius:0px 0px 10px 0px; border:1px solid #777; border-top:0px; }
		.infoBox .monitor { display: none; padding:0px 20px 10px; border:1px solid #ccc; border-radius:0 0 10px 10px; margin:0px 0 0 6px; border-top:0px; background:#eee; box-shadow:4px 3px 4px -2px #777; }
	.col1, .col2 { display: inline-block; vertical-align:top; }
	.col1 { width: 200px; padding-top:50px; margin-right:20px; }
	.col2 { width: 650px; padding-top:10px; }
    .btns { margin-bottom:17px; }
	.PopOptions { margin-bottom:10px; } 
		.PopOptions label { display:inline-block; width:125px; text-align:right; margin-right:5px; color:#000;}
		.PopOptions span { font-weight: bold; color:#000; }
	.DNAList { font-size: 12px; margin:0 auto 5px auto; height:646px; overflow:auto; border:1px solid #bbb; }
		.count { display:inline-block; width:8%; color:#000; }
		.dna { display:inline-block; width:74%; text-align:center; color:#000; }
		.fitness { display:inline-block; width:13%; text-align:right; color:#000; }
		.gender { display:inline-block; width: 3%; text-align:center; color:#000; }
	.EV { }
		.EV .tags { max-height:183px; overflow:auto; border:1px solid #ccc; margin-bottom:5px; }
		.EV .entry { text-align:left; }
		.EV .key { display:inline-block; width:75px; text-align:right; text-transform:uppercase; color:#000; }
		.EV .value { display:inline-block; font-weight:bold; color:#000; }
	.genotype { }
        .genotype > h4 { margin-top: 7px; }
        h4 { color:#000; background-color:#fff; margin:0px; padding:3px 5px; border-right:1px solid #474747; border-left:1px solid #474747; border-bottom:1px solid #ccc; border-top:3px solid #dc291e; }
        h4 label { display: block; font-weight:normal; color:#474747; }
		.chromo {  border:1px solid #474747; margin-bottom:7px; border-top:0px; }
			.genes { height:100px; overflow:auto; }
				.gene { color:#000; }
	.odd,
	.even { padding:4px 5px; border-right:1px solid #ccc; }
	.odd { }
	.even { background:#fff; }
</style>
<script type="text/javascript">
    $j(document).ready(function () {
        if(window.location.hash.indexOf("#monitor") != -1)
        	$j(".infoBox .monitor").show();

        $j(".infoBox .tab").click(function(e){
            var mon = $j(".infoBox .monitor");
            if(mon.is( ":hidden" )){
            	mon.slideDown();
            	window.location.hash = "#monitor";
            } else {
            	mon.slideUp();
            	window.location.hash = "";
            }
        });
    });
</script>

<div class="infoBox">
	<div class="monitor">
		<div class="col1">
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
			<h3>Genotype <label>(Chromosome List)</label></h3>
			<div class="genotype">
				<asp:Repeater ID="rptChromos" runat="server" OnItemDataBound="rptChromos_ItemDataBound">
					<ItemTemplate>
						<h4>
                            <%# ((KeyValuePair<string, GenePool>)Container.DataItem).Key %> 
                            <label>
                                Gene Length : <%# ((KeyValuePair<string, GenePool>)Container.DataItem).Value.GeneLimit %>
                                <br />
							    Gene Pool: <%# ((KeyValuePair<string, GenePool>)Container.DataItem).Value.Count %>
							</label>
						</h4>
                        <div class="chromo">
							<div class="genes">
								<asp:Repeater ID="rptGenes" runat="server">
									<ItemTemplate>
										<div class="gene <%# OddEven(Container.ItemIndex) %>">
											<%# ((IGene)Container.DataItem).GeneName %>
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
									<%# GetItemName(((KeyValuePair<string, List<IValue>>)Container.DataItem).Key) %> : 
								</div>
								<div class="value">
									<%# ((KeyValuePair<string, List<IValue>>)Container.DataItem).Value.Sum(a => a.Value) %>
								</div>
							</div>
						</ItemTemplate>
					</asp:Repeater>
				</div>
			</div>
		</div>
		<div class="col2">
            <div class="btns">
				<asp:Button ID="btnClearClicks" runat="server" Text="Clear Clicks" OnClick="btnClearClicks_Click" />
				<asp:Button ID="btnRestart" runat="server" Text="Restart Population" OnClick="btnRestart_Click" />
			</div>
			<h3>Population Makeup - <label>Unique Karyotypes: <asp:Literal ID="ltlUKaryos" runat="server"></asp:Literal></label></h3>
			<div class="Controls">
				<div class="DNAList">
					<asp:Repeater ID="rptDNAList" runat="server">
						<ItemTemplate>
							<div class="<%# OddEven(Container.ItemIndex) %>">
								<div class="count"><%# Container.ItemIndex + 1 %>:</div> 
								<div class="dna">
                                    <%# ((IKaryotype)Container.DataItem).Phenotype.ChromosomeSequence() %>
								</div>
								<div class="fitness"><%# ((IKaryotype)Container.DataItem).Fitness() %></div>
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