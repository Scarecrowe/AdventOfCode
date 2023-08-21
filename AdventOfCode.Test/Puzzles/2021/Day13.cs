namespace AdventOfCode.Test.Puzzles_2021
{
    using NUnit.Framework;

    [TestFixture]
    public class Day13 : PuzzleTest
    {
        private const string GoldActualAnswer =
@"

###  #### ####   ## #  # ###  #### #### 
#  #    # #       # #  # #  # #       # 
#  #   #  ###     # #### #  # ###    #  
###   #   #       # #  # ###  #     #   
#    #    #    #  # #  # # #  #    #    
#    #### #     ##  #  # #  # #    #### 

";

        public Day13() => this.Setup(2021, 13, "610", GoldActualAnswer);
    }
}