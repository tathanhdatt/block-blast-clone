using UnityEngine;

namespace Dt.Extension
{
    public static class Vector2Extension
    {
        public static Vector2 Add(this Vector2 vector2, float x = 0, float y = 0)
        {
            return new Vector2(vector2.x + x, vector2.y + y);
        }

        public static Vector2 Assign(this Vector2 vector2, float? x = null, float? y = null)
        {
            return new Vector2(x ?? vector2.x, y ?? vector2.y);
        }

        public static Vector2 Multiply(this Vector2 vector2, float? x = null, float? y = null)
        {
            if (x != null)
            {
                vector2.x *= x.Value;
            }

            if (y != null)
            {
                vector2.y *= y.Value;
            }

            return vector2;
        }
    }
}