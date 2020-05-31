using UnityEngine;

[ExecuteInEditMode]
public class BlurEffect : MonoBehaviour
{
	public Material BlurMaterial;
	[Range(0, 50)]
	public int iterations;
	[Range(0, 1)]
	public int downRes;

    public static BlurEffect Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<BlurEffect>();
            return instance;
        }
    }
    private static BlurEffect instance;

    void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		int width = src.width >> downRes;
		int height = src.height >> downRes;

		RenderTexture rt = RenderTexture.GetTemporary(width, height);
		Graphics.Blit(src, rt);

		for (int i = 0; i < iterations; i++)
		{
			RenderTexture rt2 = RenderTexture.GetTemporary(width, height);
			Graphics.Blit(rt, rt2, BlurMaterial);
			RenderTexture.ReleaseTemporary(rt);
			rt = rt2;
		}

		Graphics.Blit(rt, dst);
		RenderTexture.ReleaseTemporary(rt);
	}
}