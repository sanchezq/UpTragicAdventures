using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsDeathTrigger : MonoBehaviour
{
    public GameObject balloonSpawnerRef;
    public DetectClick clicScriptRef;
    public GameObject objectRespawnPos;
    GameObject spawnedBalloon;
    GameObject currentFallenObject;
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
        if (other.tag == "ObjectWithMass" && other.GetComponent<Debris>() == null)
        {
            if (currentFallenObject != other.gameObject)
            {
                currentFallenObject = other.gameObject;
                Mass.RemoveObject(other.gameObject);
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                spawnedBalloon = Instantiate(balloonSpawnerRef, other.transform.position + new Vector3(0, 1.2f, 0), other.transform.rotation, other.transform);
                spawnedBalloon.GetComponent<respawnedItem>().slingshotScriptRef = clicScriptRef;
                currentFallenObject.transform.position = objectRespawnPos.transform.position;
                currentFallenObject = null;
            }
            
        } else
        {
            Destroy(other.gameObject);
        }
    }
}
