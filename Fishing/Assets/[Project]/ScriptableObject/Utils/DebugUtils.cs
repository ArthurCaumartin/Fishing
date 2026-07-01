using UnityEngine;

public static class DebugUtils
{
    public static void DrawCircle(Vector3 position, float size, Color color)
    {
        int resolution = 50;
        Vector3 lastPos = Vector3.zero;

        for (int i = 0; i < resolution + 1; i++)
        {
            float rad = (Mathf.PI * 2) / resolution;
            rad *= i;
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);

            Vector3 drawPos = (new Vector3(x, 0, y) * size) + position;

            if (i == 0)
            {
                lastPos = drawPos;
                continue;
            }
            Debug.DrawLine(drawPos, lastPos, color);
            lastPos = drawPos;
            // Gizmos.DrawRay(drawPos - Vector3.up, Vector3.up * 2);
        }
    }
}




