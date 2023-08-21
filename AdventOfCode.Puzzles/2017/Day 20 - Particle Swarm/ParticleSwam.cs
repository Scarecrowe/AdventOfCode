namespace AdventOfCode.Puzzles._2017.Day_20___Particle_Swarm
{
    public class ParticleSwam
    {
        public ParticleSwam(string[] input) => this.Particles = Parse(input);

        public List<Particle> Particles { get; }

        public int Run(bool removeCollisions = false)
        {
            Dictionary<int, int> closest = new();

            for (int i = 1; i <= 500; i++)
            {
                this.Particles.ForEach((particle) => particle.Move());

                if (removeCollisions)
                {
                    List<Particle> remove = new();

                    for (int j = 0; j < this.Particles.Count; j++)
                    {
                        for (int k = 0; k < this.Particles.Count; k++)
                        {
                            if (j != k)
                            {
                                if (this.Particles[j].Position == this.Particles[k].Position && !remove.Contains(this.Particles[j]))
                                {
                                    remove.Add(this.Particles[j]);
                                }
                            }
                        }
                    }

                    remove.ForEach(x => this.Particles.Remove(x));
                }

                int number = this.Particles.Aggregate((a, b) => a.Distance() < b.Distance() ? a : b).Number;

                if (!closest.ContainsKey(number))
                {
                    closest.Add(number, 0);
                }

                closest[number]++;
            }

            return removeCollisions
                ? this.Particles.Count
                : closest.Aggregate((a, b) => a.Value > b.Value ? a : b).Key;
        }

        private static List<Particle> Parse(string[] input) => input.Select((line, i) => new Particle(line, i)).ToList();
    }
}
