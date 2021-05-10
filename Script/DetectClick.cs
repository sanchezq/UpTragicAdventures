using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectClick : MonoBehaviour
{
    [SerializeField] private LayerMask movableObject;
    [SerializeField] private LayerMask dragZone;
    [SerializeField] private LayerMask insideWall;

    public float impulseForce = 1f;
    public GameObject arrowPrefab;

    private bool objectDragging = false;
    private GameObject m_object = null;
    private Rigidbody m_body = null;

    private DrawArrow currentArrow;
    private AudioSource pullSound;
    private AudioSource releaseSound;
    public AudioClip releaseSound1;
    public AudioClip releaseSound2;

    private void Awake()
    {
        pullSound = transform.GetChild(0).GetComponent<AudioSource>();
        releaseSound = transform.GetChild(1).GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            objectDragging = true;
        }
        else
        {
            objectDragging = false;
        }

        RaycastHit hit, hit2;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (objectDragging && Physics.Raycast(ray, out hit, Mathf.Infinity, movableObject) && m_object == null)
        {
            if (Physics.Raycast(ray, out hit2, Mathf.Infinity, insideWall))
            {
                if (hit2.distance < hit.distance)
                {
                    return;
                }
            }
            m_object = hit.collider.gameObject;
            m_body = m_object.GetComponent<Rigidbody>();

            // Draw Arrow & play pull sounds
            currentArrow = Instantiate(arrowPrefab, hit.collider.gameObject.transform.position, transform.rotation, transform.parent).GetComponent<DrawArrow>();
            pullSound.transform.position = currentArrow.transform.position;
            pullSound.Play();
        }

        if (objectDragging && currentArrow != null)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, dragZone))
            {
                Vector3 pos = hit.point - ray.direction * m_object.transform.position.y - m_object.transform.position;
                pos = pos * 100f * impulseForce + transform.up * Vector3.Distance(m_object.transform.position, pos) * 150;
                //pos = new Vector3(pos.x, Vector3.Distance(m_object.transform.position, pos), pos.z) * 100f * impulseForce;

                currentArrow.DrawArrowTick(-pos, m_object);
            }
        }

        if (!objectDragging && Physics.Raycast(ray, out hit, Mathf.Infinity, dragZone) && m_object != null)
        {
            Vector3 pos = hit.point - ray.direction * m_object.transform.position.y - m_object.transform.position;
            float mag = pos.magnitude;
            pos.Normalize();
            pos = pos * Mathf.Clamp((mag * 5) * 100f * impulseForce, 100, 3000);
            //pos = pos + transform.up * Vector3.Distance(m_object.transform.position, pos);
            //pos = new Vector3(pos.x, Vector3.Distance(m_object.transform.position, pos), pos.z) * 100f * impulseForce;
            m_body.AddForce(-pos);

            if (m_object.GetComponent<ThisIsAPiano>() != null)
            {
                m_object.GetComponent<ThisIsAPiano>().PlayPianoSound();
            }
            m_object = null;

            //Remove drawn arrow & play release sounds
            releaseSound.transform.position = currentArrow.transform.position;
            if (Random.Range(0f,1f) < 0.5f)
            {
                releaseSound.clip = releaseSound1;
            } else
            {
                releaseSound.clip = releaseSound2;
            }
            releaseSound.pitch = Random.Range(0.9f, 1.1f);
            releaseSound.Play();
            Destroy(currentArrow.gameObject);
        }
    }

    public GameObject getTargetObject()
    {
        return m_object;
    }
}
