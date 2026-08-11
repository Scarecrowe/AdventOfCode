namespace AdventOfCode.Puzzles._2018.Day_15___Beverage_Bandits
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.ComponentModel;
    using System.Text;

    public class BeverageBandits
    {
        public BeverageBandits(string[] input)
        {
            this.Walls = new();
            this.Bandits = new();
            this.Map = new();
            this.Input = input;
        }

        public BeverageBandits(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public string[] Input { get; }

        public IFrameRenderer? Renderer { get; }

        public VectorDictionary<int, char> Map { get; private set; }

        public List<Vector<int>> Walls { get; private set; }

        public List<Bandit> Bandits { get; private set; }

        private int CombatRound { get; set; }

        private int TurnCount { get; set; }

        private string LastAction { get; set; } = "INITIALISING";

        public string Battle()
        {
            this.ParseInput();

            int round = 0;

            while (this.Bandits.Any(c => c.Type == BanditType.Elf) && this.Bandits.Any(c => c.Type == BanditType.Goblin))
            {
                round += this.Round();
            }

            return $"{round * this.Bandits.Sum(x => x.HealthPoints)}";
        }

        public string BattleWithTechnology()
        {
            int attackPoints = 4;

            while (true)
            {
                this.ParseInput(attackPoints);

                int round = 0;

                while (true)
                {
                    int result = this.Round(true);

                    if (result == -1)
                    {
                        break;
                    }

                    round += result;

                    if (!this.Bandits.Any(x => x.Type == BanditType.Goblin))
                    {
                        return $"{round * this.Bandits.Sum(x => x.HealthPoints)}";
                    }
                }

                attackPoints++;
            }
        }

        public BeverageBandits RenderSilver(int renderEveryTurn = 1)
        {
            this.RenderBattle(elfAttackPoints: 3, exitOnElfDeath: false, renderEveryTurn, title: "BEVERAGE BANDITS");
            return this;
        }

        public BeverageBandits RenderGold(int renderEveryTurn = 1)
        {
            int attackPoints = 4;

            while (true)
            {
                this.ParseInput(attackPoints);
                this.CombatRound = 0;
                this.TurnCount = 0;
                this.LastAction = $"ELF ATTACK POWER {attackPoints:00}";

                int round = 0;
                this.RenderFrame($"BEVERAGE BANDITS // TECH {attackPoints:00}");

                while (true)
                {
                    int result = this.Round(true, renderEveryTurn, $"BEVERAGE BANDITS // TECH {attackPoints:00}");

                    if (result == -1)
                    {
                        break;
                    }

                    round += result;

                    if (!this.Bandits.Any(x => x.Type == BanditType.Goblin))
                    {
                        this.LastAction = $"ELVES WIN // TECH {attackPoints:00} // OUTCOME {round * this.Bandits.Sum(x => x.HealthPoints)}";

                        for (int i = 0; i < 24; i++)
                        {
                            this.RenderFrame($"BEVERAGE BANDITS // TECH {attackPoints:00}");
                        }

                        return this;
                    }
                }

                attackPoints++;
            }
        }

        private void RenderBattle(int elfAttackPoints, bool exitOnElfDeath, int renderEveryTurn, string title)
        {
            this.ParseInput(elfAttackPoints);
            this.CombatRound = 0;
            this.TurnCount = 0;
            this.LastAction = "INITIAL POSITIONS";
            this.RenderFrame(title);

            int round = 0;

            while (this.Bandits.Any(c => c.Type == BanditType.Elf) && this.Bandits.Any(c => c.Type == BanditType.Goblin))
            {
                int result = this.Round(exitOnElfDeath, renderEveryTurn, title);

                if (result == -1)
                {
                    break;
                }

                round += result;
                this.CombatRound = round;
            }

            this.LastAction = $"COMBAT ENDS // OUTCOME {round * this.Bandits.Sum(x => x.HealthPoints)}";

            for (int i = 0; i < 24; i++)
            {
                this.RenderFrame(title);
            }
        }

        private static bool IsAdjacent(Bandit attacker, List<VectorCell<int, char>> adjacent)
            => adjacent.Any(c => c.Value == (attacker.Type == BanditType.Elf ? 'G' : 'E'));

        private static Bandit GetDefender(List<Bandit> defenders, List<VectorCell<int, char>> adjacent)
            => adjacent.Where(c => c.Value == defenders[0].Letter)
                .Select(c => defenders.First(d => (d.Point.Y, d.Point.X) == (c.Point.Y, c.Point.X)))
                .OrderBy(c => c.HealthPoints)
                .ThenBy(c => (c.Point.Y, c.Point.X))
                .First();

        private void ParseInput(int elfAttackPoints = 3)
        {
            this.Bandits = new();
            this.Walls = new();
            this.Map = new(this.Input, (c, x, y) =>
            {
                if (c == 'E' || c == 'G')
                {
                    this.Bandits.Add(new(c, new(x, y), 200, c == 'E' ? elfAttackPoints : 3));
                }

                if (c == '#')
                {
                    this.Walls.Add(new(x, y));
                }

                return c;
            });
            this.Bandits = this.Bandits.OrderBy(c => (c.Point.Y, c.Point.X)).ToList();
        }

        private List<Bandit> GetDefenders(Bandit bandit)
            => this.Bandits.Where(x => x.Type == bandit.DefenderType()).ToList();

        private List<VectorCell<int, char>> GetAdjacent(Bandit bandit)
            => this.Map.AdjacentCardinal(bandit.Point).Where(c => c.Value != '#').ToList();

        private Vector<int>? GetMove(Bandit attacker, List<Bandit> defenders, List<VectorCell<int, char>> attackerAdjacent)
        {
            List<Vector<int>> blocked = new(this.Walls);
            blocked.AddRange(this.Bandits.Select(c => c.Point));

            List<(Bandit defender, long distance)> distances = new();

            foreach (Bandit defender in defenders)
            {
                blocked.Remove(defender.Point);
                distances.Add((defender, this.Map.BreadthFirstSearch(attacker.Point, defender.Point, blocked).Distance));
                blocked.Add(defender.Point);
            }

            distances = distances.Where(c => c.distance > 0).ToList();

            if (!distances.Any())
            {
                return null;
            }

            long min = distances.Where(c => c.distance > 0).Min(d => d.distance);

            distances = distances.Where(c => c.distance > 0)
                .Where(c => c.distance == min)
                .OrderBy(c => c.distance)
                .ToList();

            List<(BreadthFirstSearchResult<int> search, Bandit defender)> moves = new();

            foreach ((Bandit defender, long distance) distance in distances)
            {
                List<VectorCell<int, char>> defenderAdjacent = this.GetAdjacent(distance.defender).OrderBy(c => (c.Point.Y, c.Point.X)).ToList();

                min = long.MaxValue;

                foreach (VectorCell<int, char> adjacentB in defenderAdjacent)
                {
                    if (adjacentB.Value != '.')
                    {
                        continue;
                    }

                    foreach (VectorCell<int, char> adjacentA in attackerAdjacent)
                    {
                        if (adjacentA.Value != '.')
                        {
                            continue;
                        }

                        BreadthFirstSearchResult<int> search = this.Map.BreadthFirstSearch(adjacentA.Point, adjacentB.Point, blocked);

                        if (search.Distance == 0 || search.Distance > min)
                        {
                            continue;
                        }

                        min = Math.Min(search.Distance, min);

                        moves.Add((search, distance.defender));
                    }
                }
            }

            if (moves.Count == 0)
            {
                return null;
            }

            min = moves.Min(d => d.search.Distance);

            return moves.Where(c => c.search.Distance == min)
                       .OrderBy(c => (c.search.Path[c.search.Path.Count - 1].Y, c.search.Path[c.search.Path.Count - 1].X))
                       .First().search.Path[0];
        }

        private bool Attack(Bandit attacker, List<Bandit> defenders, List<VectorCell<int, char>> adjacent, ref int i)
        {
            Bandit defender = GetDefender(defenders, adjacent);
            attacker.Attack(defender);

            this.LastAction = $"{attacker.Letter} ATTACKS {defender.Letter} FOR {attacker.AttackPoints:00} // HP {Math.Max(defender.HealthPoints, 0):000}";

            if (defender.HealthPoints <= 0)
            {
                int index = this.Bandits.IndexOf(defender);
                this.Bandits.RemoveAt(index);

                this.Map[defender.Point] = '.';
                this.LastAction = $"{attacker.Letter} KILLS {defender.Letter}";

                if (index < i)
                {
                    i--;
                }

                return true;
            }

            return false;
        }

        private int Round(bool exitOnElfDeath = false)
            => this.Round(exitOnElfDeath, 0, string.Empty);

        private int Round(bool exitOnElfDeath, int renderEveryTurn, string title)
        {
            this.Bandits = this.Bandits.OrderBy(c => (c.Point.Y, c.Point.X)).ToList();

            for (int i = 0; i < this.Bandits.Count; i++)
            {
                Bandit attacker = this.Bandits[i];

                List<Bandit> defenders = this.GetDefenders(attacker);

                if (!defenders.Any())
                {
                    return 0;
                }

                List<VectorCell<int, char>> adjacent = this.GetAdjacent(attacker).OrderBy(c => (c.Point.Y, c.Point.X)).ToList();

                if (!IsAdjacent(attacker, adjacent))
                {
                    Vector<int>? move = this.GetMove(attacker, defenders, adjacent);

                    if (move == null)
                    {
                        this.LastAction = $"{attacker.Letter} WAITS";
                        this.RenderTurn(renderEveryTurn, title);
                        continue;
                    }

                    Vector<int> from = new(attacker.Point.X, attacker.Point.Y);
                    this.Map[attacker.Point] = '.';
                    attacker.Move(new(move.X, move.Y));
                    this.Map[new(move.X, move.Y)] = attacker.Letter;

                    this.LastAction = $"{attacker.Letter} MOVES {from.X:00},{from.Y:00} TO {move.X:00},{move.Y:00}";

                    adjacent = this.GetAdjacent(attacker);
                }

                if (IsAdjacent(attacker, adjacent))
                {
                    bool dead = this.Attack(attacker, defenders, adjacent, ref i);

                    if (exitOnElfDeath && dead && attacker.Type == BanditType.Goblin)
                    {
                        this.RenderTurn(renderEveryTurn, title);
                        return -1;
                    }
                }

                this.RenderTurn(renderEveryTurn, title);
            }

            this.CombatRound++;
            return 1;
        }

        private void RenderTurn(int renderEveryTurn, string title)
        {
            if (this.Renderer == null || renderEveryTurn <= 0)
            {
                return;
            }

            this.TurnCount++;

            if (this.TurnCount % renderEveryTurn == 0)
            {
                this.RenderFrame(title);
            }
        }

        private void RenderFrame(string title)
        {
            if (this.Renderer == null)
            {
                return;
            }

            this.Renderer.RenderFrame(new Frame(this.BuildFrame(title)));
        }

        private string[] BuildFrame(string title)
        {
            List<string> result = [];

            int elves = this.Bandits.Count(c => c.Type == BanditType.Elf);
            int goblins = this.Bandits.Count(c => c.Type == BanditType.Goblin);
            int hp = this.Bandits.Sum(c => c.HealthPoints);

            result.Add(title);
            result.Add($"ROUND {this.CombatRound:000} // TURN {this.TurnCount:0000} // E {elves:00} // G {goblins:00} // HP {hp:0000}");
            result.Add(this.LastAction);
            result.Add(string.Empty);

            for (int y = 0; y < this.Map.Height; y++)
            {
                StringBuilder map = new();

                for (int x = 0; x < this.Map.Width; x++)
                {
                    map.Append(this.Map[new(x, y)]);
                }

                string hitPoints = string.Join(", ", this.Bandits
                    .Where(c => c.Point.Y == y)
                    .OrderBy(c => c.Point.X)
                    .Select(c => $"{c.Letter}({c.HealthPoints:000})"));

                result.Add(hitPoints.Length == 0
                    ? map.ToString()
                    : $"{map}   {hitPoints}");
            }

            return [.. result];
        }

        private void Print(int round)
        {
            PuzzleConsole.WriteLine(round == 0
                ? "Initially:"
                : $"After {round} round{(round > 1 ? "s" : string.Empty)}:");

            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < this.Map.Width; x++)
                {
                    PuzzleConsole.Write($"{this.Map[new(x, y)]}");
                }

                PuzzleConsole.WriteLine("   " + string.Join(", ", this.Bandits
                        .Where(c => c.Point.Y == y)
                        .OrderBy(c => c.Point.X)
                        .Select(c => $"{(c.Type == BanditType.Elf ? 'E' : 'G')}({c.HealthPoints})")));
            }

            PuzzleConsole.WriteLine();
        }
    }
}
