using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PulseTextureRenderer : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    [SerializeField] private TextMeshProUGUI heartrateText;

    [SerializeField] private int textureWidth = 256;
    [SerializeField] private int textureHeight = 64;

    [SerializeField] private float amplitude = 20f;
    [SerializeField] private float frequency = 1f;
    [SerializeField] private float speed = 2f;

    [SerializeField] private Color pulseColor = Color.green;
    [SerializeField] private Color backgroundColor = Color.black;

    private Texture2D texture;
    private float timeOffset;

    private void Start()
    {
        texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point;
        ClearTexture();
        rawImage.texture = texture;
    }

    private void Update()
    {
        timeOffset += Time.deltaTime * speed;
        DrawPulseWave();
        texture.Apply();
        heartrateText.text = $"{frequency * 60} bpm";
    }

    private void ClearTexture()
    {
        Color[] fillColor = new Color[textureWidth * textureHeight];
        for (int i = 0; i < fillColor.Length; i++)
            fillColor[i] = backgroundColor;

        texture.SetPixels(fillColor);
        texture.Apply();
    }

    private void DrawPulseWave()
    {
        ClearTexture();

        for (int x = 0; x < textureWidth; x++)
        {
            float t = (x / (float)textureWidth + timeOffset) * frequency;
            float yNorm = PulseShape(t); // 0..1
            int y = Mathf.Clamp(Mathf.RoundToInt(yNorm * amplitude), 0, textureHeight - 1);

            // –исуем вертикальную линию, чтобы пик был заметен
            for (int py = 0; py <= y; py++)
            {
                texture.SetPixel(x, py, pulseColor);
            }
        }
    }

    // ‘орма волны Ч резкий пик и спад
    private float PulseShape(float t)
    {
        t = t - Mathf.Floor(t); // 0..1

        if (t < 0.1f) // резкий подъЄм
            return Mathf.Lerp(0f, 1f, t / 0.1f);
        else if (t < 0.3f) // спад
            return Mathf.Lerp(1f, 0f, (t - 0.1f) / 0.2f);
        else // плоска€ лини€
            return 0f;
    }
}
