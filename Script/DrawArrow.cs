using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawArrow : MonoBehaviour
{
    LineRenderer lineRef;
    public GameObject FXPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        lineRef = transform.GetChild(0).GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DrawArrowTick(Vector3 impulsion, GameObject target)
    {
        lineRef.SetPosition(0, target.transform.position);
        impulsion.y = transform.position.y;
        lineRef.SetPosition(1, transform.position + impulsion.normalized * (Mathf.Clamp(impulsion.magnitude / 500 + 0.5f, 0.5f, 2)));
        
        //Gizmos.DrawLine(transform.position, (transform.position + impulsion)*2, Color.green);
        //lineRef.SetPosition(1, transform.position + impulsion);
        //Debug.Log(transform.position + impulsion);
        //transform.rotation = Quaternion.LookRotation(impulsion.normalized, transform.up);
        //float s = Mathf.Clamp(impulsion.magnitude/10, 1, 2);
        //transform.localScale = new Vector3(s, s, s);
    }

    public void OnDestroy()
    {
        GameObject spawnedFX = Instantiate(FXPrefab, transform.position, transform.rotation);
        spawnedFX.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        Destroy(spawnedFX, 2.5f);

    }
}
