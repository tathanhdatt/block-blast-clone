using UnityEngine;

namespace Dt.Extension
{
    public static class ColorExtension
    {
        public static Color SetAlpha(this Color color, float alpha)
        {
            Color c = color;
            c.a = alpha;
            return c;
        }
    }
}