using UnityEngine;

public static class CameraUtils
{
    public static Vector3 TransposePixelPositionInFrustrum(this Camera camera, float depth, Vector3 pixelPos)
    {
        if (camera.orthographic) return pixelPos;
        Vector3[] corners = new Vector3[4];
        camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), depth, Camera.MonoOrStereoscopicEye.Mono, corners);
        for (int i = 0; i < corners.Length; i++)
            corners[i] = camera.transform.TransformPoint(corners[i]);

        for (int i = 1; i < corners.Length; i++)
            Debug.DrawLine(corners[i - 1], corners[i], Color.green);


        Vector3 xPos = Vector3.Lerp(corners[0], corners[3], pixelPos.x / camera.pixelWidth);
        Vector3 yPos = Vector3.Lerp(corners[0], corners[1], pixelPos.y / camera.pixelHeight);
        Vector2 transposePosition = new Vector3(xPos.x, yPos.y, corners[0].z);
        DebugUtils.DrawCircle(transposePosition, 1, Color.red);

        return transposePosition;
    }


}




