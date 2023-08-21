namespace AdventOfCode.Test.Puzzles_2022
{
    using NUnit.Framework;

    [TestFixture]
    public class Day10 : PuzzleTest
    {
        private const string GoldActualAnswer =
@"

###   ##  ###  #  # #### #  # ####   ## 
#  # #  # #  # # #  #    # #  #       # 
#  # #  # #  # ##   ###  ##   ###     # 
###  #### ###  # #  #    # #  #       # 
#    #  # #    # #  #    # #  #    #  # 
#    #  # #    #  # #    #  # ####  ##  

";
        public Day10() => this.Setup(2022, 10, $"14060", GoldActualAnswer);
    }
}