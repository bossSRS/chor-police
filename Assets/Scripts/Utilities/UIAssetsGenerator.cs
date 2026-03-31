// UIAssetsGenerator.cs
// Author: Sadikur Rahman
// Description: Generates UI textures and sprites programmatically for the game UI.

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class UIAssetsGenerator : MonoBehaviour
{
    public enum UIAssetType
    {
        ButtonNormal,
        ButtonHover,
        ButtonPressed,
        PanelBackground,
        CardBackground,
        PoliceCard,
        ChorCard,
        DakatCard,
        BabuCard,
        WinBackground,
        LoseBackground,
        NeutralBackground
    }

    [Header("Generated Assets")]
    public bool generateOnAwake = false;
    public string outputFolder = "Assets/Resources/UI";

    [Header("Colors")]
    public Color buttonNormalColor = new Color(0.2f, 0.5f, 0.8f);
    public Color buttonHoverColor = new Color(0.3f, 0.6f, 0.9f);
    public Color buttonPressedColor = new Color(0.1f, 0.4f, 0.7f);
    public Color panelBackgroundColor = new Color(0.1f, 0.1f, 0.15f, 0.9f);
    public Color cardBackgroundColor = new Color(0.15f, 0.15f, 0.2f, 0.95f);
    public Color policeColor = new Color(0.2f, 0.4f, 1f);
    public Color chorColor = new Color(1f, 0.8f, 0f);
    public Color dakatColor = new Color(0.8f, 0.2f, 0.2f);
    public Color babuColor = new Color(0.3f, 0.8f, 0.3f);
    public Color winColor = new Color(0.2f, 0.8f, 0.2f);
    public Color loseColor = new Color(0.8f, 0.2f, 0.2f);

    private void Awake()
    {
        if (generateOnAwake && Application.isEditor)
        {
            GenerateAllAssets();
        }
    }

    [ContextMenu("Generate All UI Assets")]
    public void GenerateAllAssets()
    {
        CreateOutputFolder();

        GenerateTexture("ButtonNormal", buttonNormalColor, 4, 4);
        GenerateTexture("ButtonHover", buttonHoverColor, 4, 4);
        GenerateTexture("ButtonPressed", buttonPressedColor, 4, 4);
        GenerateTexture("PanelBackground", panelBackgroundColor, 32, 32, true);
        GenerateTexture("CardBackground", cardBackgroundColor, 32, 32, true);
        GenerateTexture("PoliceCard", policeColor, 128, 128, true);
        GenerateTexture("ChorCard", chorColor, 128, 128, true);
        GenerateTexture("DakatCard", dakatColor, 128, 128, true);
        GenerateTexture("BabuCard", babuColor, 128, 128, true);
        GenerateTexture("WinBackground", winColor, 64, 64, true);
        GenerateTexture("LoseBackground", loseColor, 64, 64, true);
        GenerateTexture("NeutralBackground", new Color(0.5f, 0.5f, 0.5f), 64, 64, true);

        Debug.Log("UI Assets generated successfully!");
    }

    private void CreateOutputFolder()
    {
        if (!System.IO.Directory.Exists(outputFolder))
        {
            System.IO.Directory.CreateDirectory(outputFolder);
        }
    }

    private void GenerateTexture(string name, Color color, int width, int height, bool roundedCorners = false)
    {
        Texture2D texture = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];

        for (int i = 0; i < pixels.Length; i++)
        {
            int x = i % width;
            int y = i / width;

            if (roundedCorners)
            {
                float centerX = width / 2f;
                float centerY = height / 2f;
                float distance = Mathf.Sqrt(Mathf.Pow(x - centerX, 2) + Mathf.Pow(y - centerY, 2));
                float maxDistance = width / 2f;

                if (distance < maxDistance - 2)
                {
                    pixels[i] = color;
                }
                else if (distance < maxDistance)
                {
                    pixels[i] = new Color(color.r, color.g, color.b, 0.3f);
                }
                else
                {
                    pixels[i] = new Color(0, 0, 0, 0);
                }
            }
            else
            {
                pixels[i] = color;
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;

        byte[] bytes = texture.EncodeToPNG();
        string path = $"{outputFolder}/{name}.png";

        System.IO.File.WriteAllBytes(path, bytes);
#if UNITY_EDITOR
        AssetDatabase.ImportAsset(path);
        AssetDatabase.Refresh();
#endif

        Debug.Log($"Generated: {name}");
    }

    public static Texture2D CreateRuntimeTexture(Color color, int width = 32, int height = 32)
    {
        Texture2D texture = new Texture2D(width, height);
        Color[] pixels = texture.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = color;
        }

        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    public static Sprite CreateRuntimeSprite(Color color, int width = 32, int height = 32)
    {
        Texture2D texture = CreateRuntimeTexture(color, width, height);
        return Sprite.Create(texture, new Rect(0, 0, width, height), Vector2.one * 0.5f);
    }
}
