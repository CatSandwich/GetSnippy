using UnityEngine;

public static class Extensions
{
    public static Vector2Int Round(this Vector2 v) => new(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
}