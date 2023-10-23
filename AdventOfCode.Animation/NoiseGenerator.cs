namespace AdventOfCode.Animation
{
    using System.Drawing;
    using AdventOfCode.Core.Extensions;

    public class NoiseGenerator
    {
        public static float[][] SmoothNoise(float[][] baseNoise, int octave)
        {
            int width = baseNoise.Length;
            int height = baseNoise[0].Length;

            float[][] result = new float[width][].EmptyArray<float>(width, height);

            int samplePeriod = 1 << octave;
            float sampleFrequency = 1.0f / samplePeriod;

            for (int x = 0; x < width; x++)
            {
                int sampleA = (x / samplePeriod) * samplePeriod;
                int sampleB = (sampleA + samplePeriod) % width;
                float horizontalBlend = (x - sampleA) * sampleFrequency;

                for (int y = 0; y < height; y++)
                {
                    int sampleC = (y / samplePeriod) * samplePeriod;
                    int sampleD = (sampleC + samplePeriod) % height;
                    float verticalBlend = (y - sampleC) * sampleFrequency;

                    float top = Interpolate(baseNoise[sampleA][sampleC], baseNoise[sampleB][sampleC], horizontalBlend);
                    float bottom = Interpolate(baseNoise[sampleA][sampleD], baseNoise[sampleB][sampleD], horizontalBlend);
                    result[x][y] = Interpolate(top, bottom, verticalBlend);
                }
            }

            return result;
        }

        public static float[] SmoothLine(float[] line, int severity = 1)
        {
            for (int i = 1; i < line.Length; i++)
            {
                int start = i - severity > 0 ? i - severity : 0;
                int end = i + severity < line.Length ? i + severity : line.Length;
                float sum = 0;

                for (int j = start; j < end; j++)
                {
                    sum += line[j];
                }

                float avg = sum / (end - start);
                line[i] = avg;
            }

            return line;
        }

        public static float[][] Fill(float[][] noise, int startX, int startY, int width, int height, float value)
        {
            for(int x = startX; x < width; x++)
            {
                for (int y = startY; y < startY + height; y++)
                {
                    noise[x][y] = value;
                }
            }

            return noise;
        }

        public static float[][] SmoothEdges(float[][] noise, int width, int height, int curveHeight)
        {
            Fill(noise, 0, 0, width, curveHeight, 0);
            Fill(noise, 0, height - curveHeight, width, curveHeight, 1);

            var curves = FindCurves(noise, curveHeight + 2, curveHeight, 0.5f, 1.0f);

            foreach(var (start, end) in curves)
            {
                float[] line = Curve(start, end, 0, curveHeight);
                int length = end - start;

                for (int x = 0; x < length; x++)
                {
                    for (int y = (int)line[x]; y < curveHeight; y++)
                    {
                        noise[x + start][y] = 1.0f;
                    }
                }
            }

            curves = FindCurves(noise, height - (curveHeight + 2), curveHeight, 0.0f, 0.5f);

            foreach (var (start, end) in curves)
            {
                float[] line = Curve(start, end, curveHeight, curveHeight);
                int length = end - start;

                for (int x = 0; x < length; x++)
                {
                    for (int y = 0; y < (int)line[x]; y++)
                    {
                        noise[x + start][y + height - curveHeight] = 0.0f;
                    }
                }
            }

            return noise;
        }

        public static float[][] PerlinNoise(float[][] baseNoise, int octaveCount = 8, float persistance = 0.5f)
        {
            int width = baseNoise.Length;
            int height = baseNoise[0].Length;

            float[][][] smooth = new float[octaveCount][][];

            for (int i = 0; i < octaveCount; i++)
            {
                smooth[i] = SmoothNoise(baseNoise, i);
            }

            float[][] result = new float[width][].EmptyArray<float>(width, height);
            float amplitude = 1.0f;
            float totalAmplitude = 0.0f;

            for (int octave = octaveCount - 1; octave >= 0; octave--)
            {
                amplitude *= persistance;
                totalAmplitude += amplitude;

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        result[i][j] += smooth[octave][i][j] * amplitude;
                    }
                }
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    result[i][j] /= totalAmplitude;
                }
            }

            return result;
        }

        public static float[][] AdjustLevels(float[][] image, float low, float high)
        {
            int width = image.Length;
            int height = image[0].Length;

            float[][] newImage = new float[width][].EmptyArray<float>(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    float col = image[i][j];

                    if (col <= low)
                    {
                        newImage[i][j] = 0;
                    }
                    else if (col >= high)
                    {
                        newImage[i][j] = 1;
                    }
                    else
                    {
                        newImage[i][j] = (col - low) / (high - low);
                    }
                }
            }

            return newImage;
        }

        public static Color GetColor(Color gradientStart, Color gradientEnd, float t)
        {
            float u = 1 - t;

            Color result = Color.FromArgb(
                (int)((gradientStart.A * u) + (gradientEnd.A * t)),
                (int)((gradientStart.R * u) + (gradientEnd.R * t)),
                (int)((gradientStart.G * u) + (gradientEnd.G * t)),
                (int)((gradientStart.B * u) + (gradientEnd.B * t)));

            return result;
        }

        public static Color[][] MapGradient(Color gradientStart, Color gradientEnd, float[][] perlinNoise)
        {
            int width = perlinNoise.Length;
            int height = perlinNoise[0].Length;

            Color[][] result = new Color[width][].EmptyArray<Color>(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    result[i][j] = GetColor(gradientStart, gradientEnd, perlinNoise[i][j]);
                }
            }

            return result;
        }

        public static float Interpolate(float x0, float x1, float alpha)
        {
            return (x0 * (1 - alpha)) + (alpha * x1);
        }

        public static float[][] WhiteNoise(int width, int height, int seed)
        {
            Random random = new(seed);

            float[][] result = new float[width][].EmptyArray(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    result[x][y] = (float)random.NextDouble() % 1;
                }
            }

            return result;
        }

        private static float[] Curve(int start, int end, int y, int curveHeight)
        {
            int length = end - start;
            float[] line = new float[length];
            int increment = (length / 2) / curveHeight;

            int i = RandomGenerator.Next(0, increment);

            for (int x = 0; x < length / 2; x++)
            {
                if (x % increment == 0)
                {
                    i++;
                }

                if (i > curveHeight)
                {
                    i = curveHeight;
                }

                line[x] = y == 0 ? curveHeight - i : i;
            }

            for (int x = length / 2; x < length; x++)
            {
                if (x % increment == 0)
                {
                    i--;
                }

                if (i < 0)
                {
                    i = 0;
                }

                line[x] = y == 0 ? curveHeight - i : i;
            }

            return SmoothLine(line, 4);
        }

        private static List<(int Start, int End)> FindCurves(float[][] noise, int y, int curveHeight, float minValue, float maxValue)
        {
            List<(int X, int Y)> result = new();
            int? start = null;

            for (int x = 0; x < 3328; x++)
            {
                if (noise[x][y] >= minValue && noise[x][y] <= maxValue)
                {
                    if (start == null)
                    {
                        start = x;
                    }

                    continue;
                }
                else if (start != null)
                {
                    if (((x - start.Value) / 2) / curveHeight <= 0)
                    {
                        start = null;
                        continue;
                    }

                    result.Add((start.Value, x));
                    start = null;
                }
            }

            return result;
        }
    }
}
