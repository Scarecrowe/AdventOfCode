namespace AdventOfCode.Animation.Terraria
{
    using AdventOfCode.Core;

    public interface ITerrariaRenderer
    {
        Vector<long> ClayMin { get; }

        Vector<long> ClayMax { get; }

        VectorArray<long, EntityType> Map { get; }

        Vector<long> Animate();

        long Settle(bool countWater = true);
    }
}
