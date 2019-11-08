using System.Collections.Generic;
using System.Linq;
using SixLabors.ImageSharp.PixelFormats;

namespace WhereIs.CapacityMonitoring
{
    public class Hotness : Dictionary<int, Rgba32>
    {
        public Hotness()
        {
            Add(17, Rgba32.LightGreen);
            Add(34, Rgba32.Green);
            Add(50, Rgba32.Yellow);
            Add(67, Rgba32.Orange);
            Add(100, Rgba32.Red);
        }

        public Rgba32 Rank(int percentageUsed) =>
            percentageUsed >= 100
                ? Rgba32.Red
                : this.FirstOrDefault(x => percentageUsed <= x.Key).Value;
    }
}