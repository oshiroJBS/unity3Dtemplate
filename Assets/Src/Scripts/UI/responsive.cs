using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using YsoCorp;

public class responsive : YCBehaviour
{
    [Range(1,10)] public float size = 2;
    private Vector3 newSize;
    private Vector3 originalSize;
    // Start is called before the first frame update
    void Start()
    {
        newSize = this.transform.localScale * size;
        originalSize = this.transform.localScale;

    }

    // Update is called once per frame
    void Update()
    {

        if (Screen.width > Screen.height)
        {
            this.transform.localScale = newSize;
            
        }
        else
        {
            this.transform.localScale = originalSize;
        }
    }
}
