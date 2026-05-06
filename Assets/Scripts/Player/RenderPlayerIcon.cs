using UnityEngine;

public class RenderPlayerIcon : MonoBehaviour
{
    [SerializeField] private Camera renderCam;
    [SerializeField] private int width = 512;
    [SerializeField] private int height = 512;

    [ContextMenu("Exportar PNG")]
    void Export()
    {
        RenderTexture rt = new RenderTexture(width, height, 32);
        renderCam.targetTexture = rt;
        renderCam.backgroundColor = Color.clear;
        renderCam.clearFlags = CameraClearFlags.SolidColor;
        renderCam.Render();

        Texture2D img = new Texture2D(width, height, TextureFormat.RGBA32, false);
        RenderTexture.active = rt;
        img.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        img.Apply();

        byte[] bytes = img.EncodeToPNG();
        System.IO.File.WriteAllBytes(
            Application.dataPath + "/PlayerIcon.png", bytes);

        Debug.Log("PNG exportao en: " + Application.dataPath);
        renderCam.targetTexture = null;
        RenderTexture.active = null;
    }
}