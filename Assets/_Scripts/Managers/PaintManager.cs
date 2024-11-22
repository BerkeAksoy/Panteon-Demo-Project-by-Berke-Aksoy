using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaintManager : MonoBehaviour
{
    private static PaintManager _instance;

    public static PaintManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Paint manager is null.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance == this) {
                return; 
            }
            Destroy(gameObject);
        }

        totalPixels = canvasTexture.width * canvasTexture.height;
        ResetPaintWall();
    }

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI paintPercentText;
    [SerializeField] private Slider brushSizeSlider;
    [SerializeField] GameObject restartPanel;

    [Header("Brush Settings")]
    public Color currentColor = Color.red;
    private float brushSizeModifier = 0.1f; // Brush size slider also starts from 0.1f

    [Header("Painting")]
    public Camera mainCamera;
    public Texture2D canvasTexture;
    private int paintedPixels = 0;
    private int totalPixels;
    private int defaultBrushSize = 200; // In pixels
    private Color initialColor = Color.white;

    void Start()
    {
        brushSizeSlider.onValueChanged.AddListener(value => brushSizeModifier = value); // Update brush size when slider changes
    }

    void Update()
    {
        CheckPaintable();
    }

    private void CheckPaintable()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Began)
            {
                Ray ray = mainCamera.ScreenPointToRay(touch.position);
                HandlePaintRaycast(ray);
            }
        }
        else if (Input.GetMouseButton(0)) // For mouse input
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            HandlePaintRaycast(ray);
        }
    }

    private void HandlePaintRaycast(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Paintable"))
            {
                Paint(hit.textureCoord);
            }
        }
    }

    private void Paint(Vector2 uv) // uv: A Vector2 representing the UV coordinates(ranging from 0 to 1) of the surface where the painting should occur
    {
        // Converts the UV coordinates into pixel coordinates on the texture
        int x = (int)(uv.x * canvasTexture.width);
        int y = (int)(uv.y * canvasTexture.height);

        int brushRadius = Mathf.CeilToInt(brushSizeModifier * defaultBrushSize);
        int brushArea = brushRadius * brushRadius;

        // This loop is designed for circler brush
        for (int i = -brushRadius; i <= brushRadius; i++)
        {
            for (int j = -brushRadius; j <= brushRadius; j++)
            {
                if (i * i + j * j <= brushArea)
                {
                    // Adjusts the pixel coordinates to stay within the valid range of the texture dimensions to prevent errors
                    int px = Mathf.Clamp(x + i, 0, canvasTexture.width - 1);
                    int py = Mathf.Clamp(y + j, 0, canvasTexture.height - 1);
                    if (canvasTexture.GetPixel(px, py) != currentColor)
                    {
                        if (currentColor == initialColor) // Acts like eraser
                            paintedPixels--;
                        else if (canvasTexture.GetPixel(px, py) == initialColor)
                            paintedPixels++;

                        canvasTexture.SetPixel(px, py, currentColor);
                    }
                }
            }
        }

        canvasTexture.Apply();
        UpdatePaintPercentage();
    }

    private void ResetPaintWall() // Resets the paintable wall to defined initialColor
    {
        for(int i=0; i < canvasTexture.width; i++)
        {
            for(int j = 0; j < canvasTexture.height; j++)
            {
                canvasTexture.SetPixel(i, j, initialColor);
            }
        }

        paintedPixels = 0;
        canvasTexture.Apply();
        UpdatePaintPercentage();
    }

    private void UpdatePaintPercentage()
    {
        int percentage = Mathf.CeilToInt((float)paintedPixels / totalPixels * 100f);
        paintPercentText.text = percentage.ToString() + "%";

        if(percentage == 100)
        {
            restartPanel.SetActive(true);
        }
    }

    public void CloseRestartPanel()
    {
        restartPanel.SetActive(false);
    }

    public void SetBrushColor(ColorButton colorButton)
    {
        currentColor = colorButton.color;
        Debug.Log("Color set to " + colorButton.color);
    }
}
