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
using System.Linq;

namespace GAHelloWorld
{
    public class Population {
		
		#region Properties

		private const int TOURNAMENT_SIZE = 3;
        private static Random _rand = new Random(Environment.TickCount);
		public float Eliteism { get; set; }
		public float Mutation { get; set; }
		public float Crossover { get; set; }
		public List<Chromosome> Chromosomes { get; set; }
		
		#endregion Properties

		#region ctor

		public Population(int size, float crossoverRatio, float eliteismRatio, float mutationRatio)
        {
            this.Crossover = crossoverRatio;
            this.Eliteism = eliteismRatio;
            this.Mutation = mutationRatio;

            InitializePopulation(size);
        }

		#endregion ctor

		#region Methods

		/// <summary>
		/// builds 'size' number of chromosomes and sorts
		/// </summary>
		private void InitializePopulation(int size)
        {
			this.Chromosomes = new List<Chromosome>(size);
            for (int count = 0; count < size; count++)
            {
                this.Chromosomes.Add(Chromosome.GenerateRandom());
            }

            this.Chromosomes.Sort();
        }

		/// <summary>
		/// updates the elite portion by mating the most fit and mutate randomly
		/// </summary>
        public void Evolve() {
            List<Chromosome> evolvedSet = new List<Chromosome>(this.Chromosomes);

			//get a position in the number of chromosomes based on the percent of elitism
            int unchangedIndex = (int)Math.Round(this.Chromosomes.Count * this.Eliteism);

			//start looping through those elites
            for (int changedIndex = unchangedIndex; changedIndex < this.Chromosomes.Count - 1; changedIndex++) {
				//roll the dice. 
				if (_rand.NextDouble() <= this.Crossover) { // if a random double is less than the crossover value (high probability) then mate
                    List<Chromosome> parents = this.SelectParents();
                    List<Chromosome> children = parents.First().Mate(parents.Last());
					
					evolvedSet[changedIndex] = children.First(); //replace an elite

                    if (_rand.NextDouble() <= this.Mutation) // if random is less than mutation rate (low probability) then mutate
                        evolvedSet[changedIndex] = evolvedSet[changedIndex].Mutate();

                    if (changedIndex < evolvedSet.Count - 1) { // if not the last
                        changedIndex++;

                        evolvedSet[changedIndex] = children.Last(); // set the next too 
						if (_rand.NextDouble() <= this.Mutation) // possibly mutate it since the next round may not mate or mutate
                            evolvedSet[changedIndex] = evolvedSet[changedIndex].Mutate();
                    }
                } else if (_rand.NextDouble() <= this.Mutation) { // or if the random double is less than the mutation rate (low probability) then mutate
                    evolvedSet[changedIndex] = evolvedSet[changedIndex].Mutate();
                }
                changedIndex++;
            }
            evolvedSet.Sort();
            this.Chromosomes = evolvedSet;
        }

		/// <summary>
		/// Selects two chromosomes randomly and tries to improve odds by comparing it's fitness to other chromosomes also randomly selected
		/// </summary>
		private List<Chromosome> SelectParents() {
            List<Chromosome> parents = new List<Chromosome>(2);

			//finds two randomly selected parents
            for (int parentIndex = 0; parentIndex < 2; parentIndex++) {
                parents.Add(this.Chromosomes[_rand.Next(this.Chromosomes.Count - 1)]);

				//it tries TOURNAMENT_SIZE times to randomly find a better parent
                for (int tournyIndex = 0; tournyIndex < TOURNAMENT_SIZE; tournyIndex++) {
                    int randomIndex = _rand.Next(this.Chromosomes.Count - 1);
                    if (this.Chromosomes[randomIndex].Fitness < parents[parentIndex].Fitness) { // closer to 0 is more fit
                        parents[parentIndex] = this.Chromosomes[randomIndex];
                    }
                }
            }

            return parents;
		}

		#endregion Methods
	}
}
