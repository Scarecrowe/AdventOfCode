namespace AdventOfCode.Test.Puzzles_2018
{
    using NUnit.Framework;

    [TestFixture]
    public class Day10 : PuzzleTest
    {
        private const string ExpectedGoldResult =
@"######  #    #    ##    ######  #####   ######  #    #  ##### 
 #       #    #   #  #        #  #    #       #  #    #  #    #
 #       #    #  #    #       #  #    #       #  #    #  #    #
 #       #    #  #    #      #   #    #      #   #    #  #    #
 #####   ######  #    #     #    #####      #    ######  ##### 
 #       #    #  ######    #     #         #     #    #  #     
 #       #    #  #    #   #      #        #      #    #  #     
 #       #    #  #    #  #       #       #       #    #  #     
 #       #    #  #    #  #       #       #       #    #  #     
 ######  #    #  #    #  ######  #       ######  #    #  #     ";

        public Day10() => this.Setup(2018, 10, $"\r\n {ExpectedGoldResult}", "10136");
    }
}