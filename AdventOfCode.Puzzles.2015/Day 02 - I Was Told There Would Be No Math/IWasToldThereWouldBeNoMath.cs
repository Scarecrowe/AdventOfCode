namespace AdventOfCode.Puzzles._2015.Day_02___I_Was_Told_There_Would_Be_No_Math
{
    public class IWasToldThereWouldBeNoMath
    {
        public IWasToldThereWouldBeNoMath(string[] input) => this.Dimensions = input;

        private string[] Dimensions { get; }

        public int WrappingPaper() => this.Dimensions.Sum(x => new Present(x).WrappingPaper());

        public int Ribbon() => this.Dimensions.Sum(x => new Present(x).Ribbon());
    }
}
