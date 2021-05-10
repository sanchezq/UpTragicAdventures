using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonDeathTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.childCount > 0)
        {
            if (other.transform.GetChild(0).GetComponent<respawnedItem>() != null)
            {
                Destroy(other.transform.GetChild(0).gameObject);
                other.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
}
