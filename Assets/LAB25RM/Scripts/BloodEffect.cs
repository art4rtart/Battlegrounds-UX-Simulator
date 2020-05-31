using UnityEngine;

[ExecuteInEditMode]
public class BloodEffect : MonoBehaviour
{
    public Material BlurMaterial;
    [Range(0, 100)]
    public int iterations;
    [Range(0, 4)]
    public int downRes;

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        int width = src.width >> downRes;
        int height = src.height >> downRes;

        RenderTexture rt = RenderTexture.GetTemporary(width, height);
        Graphics.Blit(src, rt, BlurMaterial);

        for (int i = 0; i < iterations; i++)
        {
            RenderTexture rt2 = RenderTexture.GetTemporary(width, height);
            Graphics.Blit(rt, rt2, BlurMaterial);
            RenderTexture.ReleaseTemporary(rt);
            rt = rt2;
        }

        Graphics.Blit(rt, dst, BlurMaterial);
        RenderTexture.ReleaseTemporary(rt);
    }
}