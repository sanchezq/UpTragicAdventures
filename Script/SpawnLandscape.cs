using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLandscape : MonoBehaviour
{
    int currentLandscape;
    bool advancingLandscape = false;
    bool spawning = true;
    Vector3 currentLandscapeInitPos;
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (advancingLandscape)
        {
            transform.GetChild(currentLandscape).position += new Vector3(0, 0, -Time.deltaTime * 5f);
            //Debug.Log(transform.GetChild(currentLandscape).position.z);
            if (transform.GetChild(currentLandscape).localPosition.z < -250)
            {
                advancingLandscape = false;
                transform.GetChild(currentLandscape).position = currentLandscapeInitPos;
                Spawn();
            }
        }
    }

    public void Spawn()
    {
        if (spawning)
        {
            currentLandscape = Random.Range(0, transform.childCount);
            currentLandscapeInitPos = transform.GetChild(currentLandscape).position;
            advancingLandscape = true;
        }
    }

    public void StopSpawningLandscape()
    {
        spawning = false;
    }
}
