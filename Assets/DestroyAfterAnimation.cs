using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    public AnimationClip clip;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
