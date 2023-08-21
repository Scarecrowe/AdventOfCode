namespace AdventOfCode.Test.Puzzles_2019
{
    using NUnit.Framework;

    [TestFixture]
    public class Day08 : PuzzleTest
    {
        private const string GoldActualResult =
@"

#  #  ##  #  # ####  ##  
# #  #  # #  #    # #  # 
##   #  # #  #   #  #  # 
# #  #### #  #  #   #### 
# #  #  # #  # #    #  # 
#  # #  #  ##  #### #  # 

";

        public Day08() => this.Setup(2019, 8, "2064", GoldActualResult);
    }
}