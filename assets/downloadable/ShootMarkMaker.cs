//ORG: Ghostyii & MoonLight Game
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ShotMarkMaker : MonoBehaviour
{
    public Texture2D wallTexture;

    public Camera gunCam;
    public Shooter gun;
    //public ShootCaster shootcaster = null;
    public float existTime = 3f;

    private Texture2D newWallTexture;
    private MeshRenderer meshRenderer = null;

    private float wallHeight = 0f;
    private float wallWidth = 0f;

    private float bulletHeight = 0f;
    private float bulletWidth = 0f;

    private RaycastHit hit;
    private Queue<Vector2> uvQueues = new Queue<Vector2>();

    void Start()
    {
        if (!gun) { print("no gun ref"); return; }

        meshRenderer = GetComponent<MeshRenderer>();
        uvQueues = new Queue<Vector2>();
        if (!wallTexture) wallTexture = meshRenderer.material.mainTexture as Texture2D;

        newWallTexture = new Texture2D(wallTexture.width, wallTexture.height);
        newWallTexture.SetPixels(wallTexture.GetPixels());
        newWallTexture.Apply();

        meshRenderer.material.mainTexture = newWallTexture;

        wallHeight = wallTexture.height;
        wallWidth = wallTexture.width;

        bulletHeight = gun.bullueTexture.height;
        bulletWidth = gun.bullueTexture.width;

        gun.onShoot.AddListener(delegate { MakeShootMark(); });
    }

    public void MakeShootMark()
    {
        if (gun.GetHitObject().HasValue && gun.GetHitObject().Value.transform.name == this.name)
        {
            Vector2 uv = gun.GetHitObject().Value.textureCoord;

            uvQueues.Enqueue(uv);

            for (int i = 0; i < bulletWidth; i++)
            {
                for (int j = 0; j < bulletHeight; j++)
                {
                    float w = uv.x * wallWidth - bulletWidth / 2 + i;
                    float h = uv.y * wallHeight - bulletHeight / 2 + j;

                    Color wallColor = newWallTexture.GetPixel((int)w, (int)h);
                    Color bulletColor = gun.bullueTexture.GetPixel(i, j);

                    newWallTexture.SetPixel((int)w, (int)h, wallColor * bulletColor);
                }
            }
            newWallTexture.Apply();

            Invoke("ResetShootMark", existTime);
        }

    }

    private void ResetShootMark()
    {
        Vector2 uv = uvQueues.Dequeue();
        for (int i = 0; i < bulletWidth; i++)
        {
            for (int j = 0; j < bulletHeight; j++)
            {
                float w = uv.x * wallWidth - bulletWidth / 2 + i;
                float h = uv.y * wallHeight - bulletHeight / 2 + j;

                Color wallColor = wallTexture.GetPixel((int)w, (int)h);
                newWallTexture.SetPixel((int)w, (int)h, wallColor);
            }
        }

        newWallTexture.Apply();
    }
}