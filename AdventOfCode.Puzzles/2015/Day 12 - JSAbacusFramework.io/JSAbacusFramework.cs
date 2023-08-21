namespace AdventOfCode.Puzzles._2015.Day_12___JSAbacusFramework.io
{
    using System.Text;
    using AdventOfCode.Core.Extensions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class JSAbacusFramework
    {
        public static long SumAll(string json)
        {
            long result = 0;

            StringBuilder sb = new();

            for (int i = 0; i < json.Length; i++)
            {
                char character = json[i];

                if (character == '-' || char.IsNumber(character))
                {
                    sb.Append(character);
                    continue;
                }

                if (sb.Length > 0)
                {
                    result += sb.ToString().ToInt();
                    sb.Clear();
                }
            }

            return result;
        }

        public static long SumNonRed(string json)
        {
            dynamic jsonObject = JsonConvert.DeserializeObject(json) ?? new();

            return GetSum(jsonObject, "red");
        }

        private static void TraverseNode(JToken node, Action<JObject> action)
        {
            if (node.Type == JTokenType.Object)
            {
                action((JObject)node);

                foreach (JProperty child in node.Children<JProperty>())
                {
                    TraverseNode(child.Value, action);
                }
            }
            else if (node.Type == JTokenType.Array)
            {
                foreach (JToken child in node.Children())
                {
                    TraverseNode(child, action);
                }
            }
        }

        private static long GetSum(JObject @object, string? avoid = null)
        {
            bool shouldAvoid = @object.Properties()
                .Select(a => a.Value).OfType<JValue>()
                .Select(v => v.Value).Contains(avoid);

            return shouldAvoid ? 0 : @object.Properties().Sum((dynamic x) => (long)GetSum(x.Value, avoid));
        }

        private static long GetSum(JArray array, string avoid) => array.Sum((dynamic x) => (long)GetSum(x, avoid));

        private static long GetSum(JValue value, string avoid) => value.Type == JTokenType.Integer ? (long)(value.Value ?? 0) : 0;
    }
}
