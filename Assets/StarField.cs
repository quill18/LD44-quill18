using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarField : MonoBehaviour
{
    void Start()
    {
        Bounds bounds = WorldManager.Instance.ScreenBounds;

        minX = bounds.min.x * 1.1f;
        maxX = bounds.max.x * 1.1f;
        float minY = bounds.min.y * 1.1f;
        float maxY = bounds.max.y * 1.1f;

        stars = new Transform[numStars];
        speeds = new float[numStars];

        for (int i = 0; i < numStars; i++)
        {
            GameObject go = new GameObject();
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = Sprites[Random.Range(0, Sprites.Length)];
            sr.color = Random.ColorHSV(0f, 1f, 0.00f, 1f, 0.4f, 0.6f);
            sr.sortingLayerName = "Stars";
            go.transform.position = new Vector3(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY),
                0
                );
            go.transform.SetParent(this.transform);
            stars[i] = go.transform;
            speeds[i] = Random.Range(1f, 3f);
        }
    }

    public Sprite[] Sprites;
    int numStars = 250;
    Transform[] stars;
    float[] speeds;

    float minX;
    float maxX;


    private void Update()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            float x = stars[i].position.x - speeds[i] * Time.deltaTime;

            if (x < minX)
                x = maxX;

            stars[i].position = new Vector3(x, stars[i].position.y, 0);
        }
    }
}
