namespace AdventOfCode.Test.Puzzles_2016
{
    using NUnit.Framework;

    [TestFixture]
    public class Day08 : PuzzleTest
    {
        private const string GoldActualAnswer =
@"

 ##  #### #    #### #     ##  #   #####  ##   ### 
#  # #    #    #    #    #  # #   ##    #  # #    
#    ###  #    ###  #    #  #  # # ###  #    #    
#    #    #    #    #    #  #   #  #    #     ##  
#  # #    #    #    #    #  #   #  #    #  #    # 
 ##  #    #### #### ####  ##    #  #     ##  ###  

";
        public Day08() => this.Setup(2016, 8, "106", GoldActualAnswer);
    }
}