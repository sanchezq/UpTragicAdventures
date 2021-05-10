using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableWall : MonoBehaviour
{
    bool activated = false;
    public bool setMovable = false;
    float totalHits = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!activated && collision.gameObject.tag == "ObjectWithMass")
        {
            if (collision.gameObject.GetComponent<Rigidbody>().mass >= 7)
            {
                totalHits += collision.gameObject.GetComponent<Rigidbody>().mass;
                if (totalHits > 15)
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                    activated = true;
                    GetComponent<AudioSource>().Play();
                    Camera.main.GetComponent<StressReceiver>().InduceStress(0.45f);

                    if (setMovable)
                    {
                        gameObject.tag = "ObjectWithMass";
                        gameObject.layer = 8;
                    }
                }
            }
        }
    }
}
