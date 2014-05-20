/* 
The MIT License

Copyright (c) 2011 John Svazic

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

Author: Patrick Hyatt 

*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GAHelloWorld
{
    [DebuggerDisplay("Gene={Gene}")]
    public class Chromosome : IComparable<Chromosome> {
		
		#region Properties

		private static Random rand = new Random(Environment.TickCount);
		public string Gene { get; private set; }
		public int Fitness { get; set; }

		private static char[] TARGET_GENE = null;
		/// <summary>
		/// stores the goal text
		/// </summary>
		public static void SetTargetGene(string targetGene) {
			TARGET_GENE = targetGene.ToCharArray();
		}

		#endregion Properties

		#region ctor

		public Chromosome(string gene)
        {
            this.Gene = gene;
            this.Fitness = CalculateFitness(gene);
        }
		
		#endregion ctor

		#region IComparable methods

		public int CompareTo(Chromosome other)
        {
            return this.Fitness.CompareTo(other.Fitness);
        }

		#endregion IComparable methods
		
		#region Methods

		/// <summary>
		/// determines how different the gene is from the target (what it should be). 0 is no difference. higher is more different
		/// </summary>
		public static int CalculateFitness(string gene)
        {
            int fitness = 0;
            for (int charIndex = 0; charIndex < TARGET_GENE.Length; charIndex++) {
                fitness += Math.Abs((int)gene[charIndex] - (int)TARGET_GENE[charIndex]); // total of absolute values of difference in letter values
            }

            return fitness;
        }

		/// <summary>
		/// builds new chromosome with random gene sequence
		/// </summary>
		public static Chromosome GenerateRandom()
        {
            StringBuilder geneBuilder = new StringBuilder();
            for (int count = 0; count < TARGET_GENE.Length; count++) {
                geneBuilder.Append((char)rand.Next(32, 122)); // not sure why adds 32 after instead of before
            }

            return new Chromosome(geneBuilder.ToString());
        }

		/// <summary>
		/// changes a random character in the gene to a random character
		/// </summary>
		public Chromosome Mutate()
        {
            char[] mutatedGene = this.Gene.ToCharArray();
            int randomIndex = rand.Next(0, this.Gene.Length); // get random location
            int mutateChange = rand.Next(32, 122); // get random letter

            mutatedGene[randomIndex] = (char)mutateChange;

            return new Chromosome(String.Join("", mutatedGene));
        }

		/// <summary>
		/// take the genes from this and a mate and split them in half and swap them
		/// </summary>
		public List<Chromosome> Mate(Chromosome mate) {
            int pivotIndex = rand.Next(0, this.Gene.Length - 1);

			// cut the genes in half and mix
            string firstSplit = this.Gene.Substring(0, pivotIndex) + mate.Gene.Substring(pivotIndex);
            string secondSplit = mate.Gene.Substring(0, pivotIndex) + this.Gene.Substring(pivotIndex);

            return new List<Chromosome>(2) { new Chromosome(firstSplit), new Chromosome(secondSplit) };
		}

		#endregion Methods
	}
}
