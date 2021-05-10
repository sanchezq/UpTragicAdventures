using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRemains : MonoBehaviour
{
    bool goDown = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("TimeBeforeTranslate");
    }


    private void Update()
    {
        translateRemains();
    }

    IEnumerator TimeBeforeTranslate()
    {
        yield return new WaitForSeconds(5f);

        for (int i=0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<MeshCollider>() != null)
            {
                transform.GetChild(i).GetComponent<MeshCollider>().enabled = false;
                transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = true;
            }
        }

        goDown = true;

        StartCoroutine("TimeBeforeDestroy");
    }

    IEnumerator TimeBeforeDestroy()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
    }

    void translateRemains()
    {
        if (goDown)
        {
            transform.position += new Vector3(0, -1, 0)*Time.deltaTime;
        }
            
    }
    

}
