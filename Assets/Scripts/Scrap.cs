using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        velocity = new Vector3( Random.Range( -1.5f, -2.5f ), Random.Range(-.5f, .5f), 0);
        playerUnit = GameObject.FindObjectOfType<PlayerUnit>();
    }

    Vector3 velocity;
    float rotSpeed = 360;
    PlayerUnit playerUnit;

    // Update is called once per frame
    void Update()
    {
        if (playerUnit == null)
            return; 

        // Are we within pickup range?
        float dist = Vector3.Distance(this.transform.position, playerUnit.transform.position);
        if(dist <= playerUnit.PickupRange)
        {
            playerUnit.PickupScrap(this);
            SimplePool.Despawn(gameObject);

        }
    }

    private void FixedUpdate()
    {
        this.transform.Translate(velocity * Time.deltaTime, Space.World);
        this.transform.Rotate(0, 0, rotSpeed * Time.deltaTime);
    }

}
