// By : Myles Cameron Johnston 03-05-2024
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class SpatialAffect : MonoBehaviour
{
    public GameObject WorldCenter;
    private Color32[] gridColors;
    private Color32[] gridColorsHR;
    private Color32[] gridColorsQR;
    public Texture2D gridColorsTexture2D;
    public Texture2D gridColorsHRTexture2D;
    public Texture2D gridColorsQRTexture2D;
    public RawImage RawImage;
    private Vector3 worldPosition = new Vector3(0, 0, 0);
    private Vector3 screenPosition = new Vector3 (0, 0, 0);
    public int gridSize = 256;
    public int frameCount = 0;
    public byte[] imageBytes;

    // Start is called before the first frame update
    void Start()
    {
        gridColors = new Color32[Screen.width * Screen.height];
        gridColorsHR = new Color32[(Screen.width / 2) * (Screen.height / 2)];
        gridColorsQR = new Color32[(Screen.width / 4) * (Screen.height / 4)];
        gridColorsTexture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        gridColorsHRTexture2D = new Texture2D(Screen.width / 2, Screen.height / 2, TextureFormat.ARGB32, false);
        gridColorsQRTexture2D = new Texture2D(Screen.width / 4, Screen.height / 4, TextureFormat.ARGB32, false);
        RawImage.texture = gridColorsTexture2D;
        if (!Directory.Exists(Application.dataPath + "/../Images/"))
        {
            Directory.CreateDirectory(Application.dataPath + "/../Images/");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (frameCount++ < 2048)
        {
            gridColors = new Color32[Screen.width * Screen.height];
            gridColorsHR = new Color32[(Screen.width / 2) * (Screen.height / 2)];
            gridColorsQR = new Color32[(Screen.width / 4) * (Screen.height / 4)];
            for (int z = -gridSize / 2; z < gridSize / 2; z++)
            {
                for (int y = -gridSize / 2; y < gridSize / 2; y++)
                {
                    for (int x = -gridSize / 2; x < gridSize / 2; x++)
                    {
                        worldPosition.x = x;
                        worldPosition.y = y;
                        worldPosition.z = z;
                        screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
                        Vector3 directionToTarget = worldPosition - Camera.main.transform.position;
                        float angle = Vector3.Angle(Camera.main.transform.forward, directionToTarget);

                        // Determine if target is to the left or right
                        if (Vector3.Dot(Camera.main.transform.right, directionToTarget) < 0)
                        {
                            // Target is to the left (counterclockwise)
                            angle = 360 - angle;
                        }

                        if (screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height && angle > -45 && angle < 45 && screenPosition.x + (screenPosition.y * Screen.width) < gridColors.Length && gridColors[(int)(screenPosition.x + (screenPosition.y * (float)Screen.width))].a < 255 - (byte)screenPosition.z)
                        {
                            gridColors[(int)(screenPosition.x + (screenPosition.y * Screen.width))].a = (byte)Mathf.Clamp((255 - (screenPosition.z)), 0, 255);
                            gridColors[(int)(screenPosition.x + (screenPosition.y * Screen.width))].r = (byte)((x + (gridSize / 2)) * (255f / ((float)gridSize)));
                            gridColors[(int)(screenPosition.x + (screenPosition.y * Screen.width))].g = (byte)((y + (gridSize / 2)) * (255f / ((float)gridSize)));
                            gridColors[(int)(screenPosition.x + (screenPosition.y * Screen.width))].b = (byte)((z + (gridSize / 2)) * (255f / ((float)gridSize)));
                        }
                        if (screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height && angle > -45 && angle < 45 && (int)((screenPosition.x / 2) + ((screenPosition.y / 2) * (Screen.width / 2))) < gridColorsHR.Length && gridColorsHR[(int)((screenPosition.x / 2f) + ((screenPosition.y / 2f)  * ((float)Screen.width / 2f)))].a < 255 - (byte)screenPosition.z)
                        {
                            gridColorsHR[(int)((screenPosition.x / 2) + ((screenPosition.y / 2) * (Screen.width / 2)))].a = (byte)Mathf.Clamp((255 - (screenPosition.z)), 0, 255);
                            gridColorsHR[(int)((screenPosition.x / 2) + ((screenPosition.y / 2) * (Screen.width / 2)))].r = (byte)((x + (gridSize / 2)) * (255f / ((float)gridSize)));
                            gridColorsHR[(int)((screenPosition.x / 2) + ((screenPosition.y / 2) * (Screen.width / 2)))].g = (byte)((y + (gridSize / 2)) * (255f / ((float)gridSize)));
                            gridColorsHR[(int)((screenPosition.x / 2) + ((screenPosition.y / 2) * (Screen.width / 2)))].b = (byte)((z + (gridSize / 2)) * (255f / ((float)gridSize)));
                        }
                        if (screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height && angle > -45 && angle < 45 && (int)((screenPosition.x / 4) + ((screenPosition.y / 4) * (Screen.width / 4))) < gridColorsQR.Length && gridColorsQR[(int)((screenPosition.x / 4f) + ((screenPosition.y / 4f)  * ((float)Screen.width / 4f)))].a < 255 - (byte)screenPosition.z)
                        {
                            gridColorsQR[(int)((screenPosition.x / 4) + ((screenPosition.y / 4) * (Screen.width / 4)))].a = (byte)Mathf.Clamp((255 - (screenPosition.z)), 0, 255);
                            gridColorsQR[(int)((screenPosition.x / 4) + ((screenPosition.y / 4) * (Screen.width / 4)))].r = (byte)((x + (gridSize / 2)) * (255f / ((float)gridSize)));
                            gridColorsQR[(int)((screenPosition.x / 4) + ((screenPosition.y / 4) * (Screen.width / 4)))].g = (byte)((y + (gridSize / 2)) * (255f / ((float)gridSize)));
                            gridColorsQR[(int)((screenPosition.x / 4) + ((screenPosition.y / 4) * (Screen.width / 4)))].b = (byte)((z + (gridSize / 2)) * (255f / ((float)gridSize)));
                        }
                    }
                }
            }
            gridColors[gridColors.Length - 1].a = (byte)255;
            gridColors[gridColors.Length - 1].r = (byte)(((Vector3.Angle(Camera.main.transform.right, WorldCenter.transform.right) + 180f) / 360f) * 255f);
            gridColors[gridColors.Length - 1].g = (byte)(((Vector3.Angle(Camera.main.transform.up, WorldCenter.transform.up) + 180f) / 360f) * 255f);
            gridColors[gridColors.Length - 1].b = (byte)(((Vector3.Angle(Camera.main.transform.forward, WorldCenter.transform.forward) + 180f) / 360f) * 255f);

            gridColorsHR[gridColorsHR.Length - 1].a = (byte)255;
            gridColorsHR[gridColorsHR.Length - 1].r = (byte)(((Vector3.Angle(Camera.main.transform.right, WorldCenter.transform.right) + 180f) / 360f) * 255f);
            gridColorsHR[gridColorsHR.Length - 1].g = (byte)(((Vector3.Angle(Camera.main.transform.up, WorldCenter.transform.up) + 180f) / 360f) * 255f);
            gridColorsHR[gridColorsHR.Length - 1].b = (byte)(((Vector3.Angle(Camera.main.transform.forward, WorldCenter.transform.forward) + 180f) / 360f) * 255f);

            gridColorsQR[gridColorsQR.Length - 1].a = (byte)255;
            gridColorsQR[gridColorsQR.Length - 1].r = (byte)(((Vector3.Angle(Camera.main.transform.right, WorldCenter.transform.right) + 180f) / 360f) * 255f);
            gridColorsQR[gridColorsQR.Length - 1].g = (byte)(((Vector3.Angle(Camera.main.transform.up, WorldCenter.transform.up) + 180f) / 360f) * 255f);
            gridColorsQR[gridColorsQR.Length - 1].b = (byte)(((Vector3.Angle(Camera.main.transform.forward, WorldCenter.transform.forward) + 180f) / 360f) * 255f);

            gridColorsTexture2D.SetPixels32(gridColors);
            gridColorsTexture2D.Apply();
            imageBytes = gridColorsTexture2D.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/../Images/" + frameCount.ToString("0000") + "_512.png", imageBytes);

            gridColorsHRTexture2D.SetPixels32(gridColorsHR);
            gridColorsHRTexture2D.Apply();
            imageBytes = gridColorsHRTexture2D.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/../Images/" + frameCount.ToString("0000") + "_256.png", imageBytes);
            
            gridColorsQRTexture2D.SetPixels32(gridColorsQR);
            gridColorsQRTexture2D.Apply();
            imageBytes = gridColorsQRTexture2D.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/../Images/" + frameCount.ToString("0000") + "_128.png", imageBytes);
            
            Debug.Log("Frame " + frameCount);
            frameCount++;
        }
    }
}
