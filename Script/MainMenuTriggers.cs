using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTriggers : MonoBehaviour
{
    private Camera camera;
    private Light light;

    public bool isOn = false;
    // Start is called before the first frame update
    void Start()
    {
        camera = gameObject.GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    if(isOn)
                    {
                        Debug.Log("Light");
                        light = hit.collider.gameObject.GetComponent<Light>();
                        light.intensity = Mathf.Lerp(10.5f, 0.01f, Time.time / 15);
                        isOn = false;
                    }
                    else
                    {
                        light = hit.collider.gameObject.GetComponent<Light>();
                        light.intensity = Mathf.Lerp(0.01f, 10.5f, Time.time / 15);
                        isOn = true;
                    }
                    
                }
                else
                {
                    Debug.Log("NotaLight");
                }
            }
        }
    }

            
}
