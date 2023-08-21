namespace AdventOfCode.Puzzles._2016.Day_09___Explosives_in_Cyberspace
{
    using System.Text;
    using AdventOfCode.Core.Extensions;

    public class ExplosivesInCyberspace
    {
        public static long Decompress(string input, int version = 1)
        {
            StringBuilder sb = new();
            StringBuilder instruction = new();

            DecompressMode mode = DecompressMode.Normal;
            int subsequent = 0;
            int repeat = 0;
            long count = 0;

            for (int i = 0; i < input.Length; i++)
            {
                char character = input[i];

                switch (mode)
                {
                    case DecompressMode.Normal:
                        if (character == '(')
                        {
                            mode = DecompressMode.Instruction;
                            continue;
                        }

                        sb.Append(character);

                        break;
                    case DecompressMode.Instruction:
                        switch (character)
                        {
                            case ')':
                                mode = DecompressMode.Decompressing;
                                repeat = instruction.ToString().ToInt();
                                instruction.Clear();
                                break;
                            case 'x':
                                subsequent = instruction.ToString().ToInt();
                                instruction.Clear();
                                break;
                            default:
                                instruction.Append(character);
                                break;
                        }

                        break;
                    case DecompressMode.Decompressing:
                        string next = input.Substring(i, subsequent);

                        if (version != 1 && next.Contains('('))
                        {
                            count += Decompress(next, version) * repeat;
                        }
                        else
                        {
                            count += next.Length * repeat;
                        }

                        i += subsequent - 1;
                        mode = DecompressMode.Normal;
                        break;
                }
            }

            count += sb.Length;

            return count;
        }
    }
}
