namespace AdventOfCode.Test.Puzzles_2019
{
    using NUnit.Framework;

    [TestFixture]
    public class Day11 : PuzzleTest
    {
        private const string GoldActualAnswer =
@"

  ##  ###  #### #  # ####  ##  ####  ##    
 #  # #  # #    # #     # #  # #    #  #   
 #  # ###  ###  ##     #  #    ###  #      
 #### #  # #    # #   #   # ## #    # ##   
 #  # #  # #    # #  #    #  # #    #  #   
 #  # ###  #### #  # ####  ### #     ###   

";
        public Day11() => this.Setup(2019, 11, "1907", GoldActualAnswer);
    }
}