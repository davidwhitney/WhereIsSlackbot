using System.Linq;
using System.Reflection;
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
                var expectedKeyName = GenerateKeyName(prop);
                if (!pairs.AllKeys.Contains(expectedKeyName))
                {
                    continue;
                }

                prop.SetValue(instance, pairs[expectedKeyName]);
            }

            return instance;
        }

        private static string GenerateKeyName(MemberInfo prop)
        {
            var normalisedProp = prop.Name;

            for (var index = 1; index < normalisedProp.Length; index++)
            {
                var letter = normalisedProp[index];
                if (!char.IsUpper(letter))
                {
                    continue;
                }

                normalisedProp = normalisedProp.Insert(index, "_");
                index++;
            }

            return normalisedProp.ToLower();
        }
    }
}