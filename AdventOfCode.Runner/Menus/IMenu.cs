namespace AdventOfCode.Runner.Menus
{
    using System.Threading.Tasks;

    public interface IMenu
    {
        Task<IMenu> Execute();
    }
}
