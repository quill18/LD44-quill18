using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportOval : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        alpha = 0;
        angle = Random.Range(0f, 360f);
        Color c = Random.ColorHSV();
        c.a = alpha;
        sr = this.GetComponent<SpriteRenderer>();
        sr.color = c;

        speed = Random.Range(270f, 500f);
        if (Random.Range(0, 2) == 0)
            speed *= -1f;
    }

    float angle;
    float speed;
    float alpha;
    SpriteRenderer sr;

    // Update is called once per frame
    void Update()
    {
        angle += speed * Time.deltaTime;

        this.transform.rotation = Quaternion.Euler(0, 0, angle);

        alpha += Time.deltaTime/4f;
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;

    }
}
