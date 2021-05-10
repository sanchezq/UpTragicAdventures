using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    GameObject brokenWallSpawned;
    public GameObject brokenWallPrefab;
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
        if (collision.gameObject.tag == "ObjectWithMass")
        {
            if (collision.gameObject.GetComponent<Rigidbody>().mass >= 7)
            {
                totalHits += collision.gameObject.GetComponent<Rigidbody>().mass;
                if (totalHits > 15)
                {
                    brokenWallSpawned = Instantiate(brokenWallPrefab, transform.position, transform.rotation);
                    Mass.AddObject(brokenWallSpawned.transform.GetChild(0).gameObject);
                    Mass.AddObject(brokenWallSpawned.transform.GetChild(1).gameObject);
                    Mass.AddObject(brokenWallSpawned.transform.GetChild(2).gameObject);
                    Mass.AddObject(brokenWallSpawned.transform.GetChild(3).gameObject);
                    Camera.main.GetComponent<StressReceiver>().InduceStress(1);
                    Destroy(gameObject);
                }
            }
        }
    }

}
