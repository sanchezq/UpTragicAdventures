using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnedItem : MonoBehaviour
{
    public DetectClick slingshotScriptRef;
    Vector3 positionAtSlingshot;
    bool startedSlingshot = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.position = transform.parent.position + new Vector3(0, 0.5f * Time.deltaTime, 0);
        if (startedSlingshot)
        {
            transform.parent.position = positionAtSlingshot;
            if (slingshotScriptRef.getTargetObject() != transform.parent.gameObject)
            {
                Destroy(gameObject);
            }
        } else if (slingshotScriptRef.getTargetObject() == transform.parent.gameObject)
        {
            transform.parent.GetComponent<Rigidbody>().isKinematic = false;
            Mass.AddObject(transform.parent.gameObject);
            positionAtSlingshot = transform.parent.position;
            startedSlingshot = true;
        }
        
    }
}
