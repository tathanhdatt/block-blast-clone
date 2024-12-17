namespace Dt.Extension
{
    public static class Vector3Extension
    {
        public static UnityEngine.Vector3 Add(this UnityEngine.Vector3 vector3, 
            float x = 0, float y = 0, float z = 0)
        {
            return new UnityEngine.Vector3(vector3.x + x, vector3.y + y, vector3.z + z);
        }

        public static UnityEngine.Vector3 Assign(this UnityEngine.Vector3 vector3,
            float? x = null, float? y = null, float? z = null)
        {
            return new UnityEngine.Vector3(x ?? vector3.x, y ?? vector3.y, z ?? vector3.z);
        }
    }
}