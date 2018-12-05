using System.Linq;
using System.Web;

namespace WhereIs.Slack
{
    public static class PayloadMapper
    {
        public static SlackRequest Map(string payload)
        {
            var pairs = HttpUtility.ParseQueryString("?" + payload);
            var instance = new SlackRequest();
            var targetProperties = typeof(SlackRequest).GetProperties();

            foreach (var prop in targetProperties)
            {
                string normalisedProp = prop.Name;

                for (var index = 1; index < normalisedProp.Length; index++)
                {
                    var letter = normalisedProp[index];
                    if (char.IsUpper(letter))
                    {
                        normalisedProp = normalisedProp.Insert(index, "_");
                        index++;
                    }
                }

                normalisedProp = normalisedProp.ToLower();

                if (pairs.AllKeys.Contains(normalisedProp))
                {
                    prop.SetValue(instance, pairs[normalisedProp]);
                }
            }

            return instance;
        }
    }
}