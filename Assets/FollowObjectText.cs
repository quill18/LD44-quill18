using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObjectText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        target = this.transform.parent;
        offset = this.transform.localPosition;
        this.transform.SetParent(null); // Become Superman
        this.transform.rotation = Quaternion.identity;
    }

    Transform target;
    Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }
        this.transform.position = target.position + offset;
    }
}
