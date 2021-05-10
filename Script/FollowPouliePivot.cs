using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPouliePivot : MonoBehaviour
{
    public GameObject pivotRef;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = pivotRef.transform.position;
    }
}
