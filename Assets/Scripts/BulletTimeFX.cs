using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTimeFX : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Material BulletTimeMaterial;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, BulletTimeMaterial);
    }


}
