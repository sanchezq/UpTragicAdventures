using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject outsideHouseMesh;
    public GameObject poulies;
    public GameMode gameModeRef;
    public Transform m_target;
    [SerializeField]
    private float m_rotateSpeed;
    [SerializeField]
    private bool m_activeVertical = false;

    public KeyCode m_keyPress;
    [SerializeField]
    private float m_maxAngle = 55f;
    [SerializeField]
    private float m_minAngle = 15f;

    //Zoom
    private float m_zoom;
    private Camera m_cam;
    [SerializeField]
    private float m_smoothZoom = 0.3f;
    private float m_speedZoom = 2.5f;
    [SerializeField]
    private float m_maxZoom = 3.25f;
    [SerializeField]
    private float m_minZoom = 10f;
    [SerializeField]
    private float m_startMovPivot = 5f;
    [SerializeField]
    private float m_maxYPivot = 5f;
    [SerializeField]
    private float m_smoothMovDown = 1.5f;
    [SerializeField]
    private float m_smoothMovUp = 4f;

    private Vector3 m_startposPivot;

    private bool m_down = false;
    [SerializeField]
    private float m_startMovDown;

    public bool disableZoom = false;
    private bool zoomingOutScripted = false;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        m_cam = Camera.main;
        m_cam.orthographicSize = m_minZoom;
        m_zoom = m_cam.orthographicSize;
        m_startposPivot = transform.parent.position;
        transform.parent.position = new Vector3(transform.parent.position.x, m_maxYPivot-1f, transform.parent.position.z);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (gameModeRef.gameOver)
        {
            GameOver();
        }
        else
        {


            if (Input.GetKey(m_keyPress))
            {
                //Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                //m_rotateSpeedY = Mathf.Lerp(1,0,todo);

                float horizontal = Input.GetAxis("Mouse X") * m_rotateSpeed;
                float vertical = Input.GetAxis("Mouse Y") * m_rotateSpeed;
                //Quaternion rotation;
                if (m_activeVertical)
                {
                    m_target.Rotate(-vertical, horizontal, 0);
                    Vector3 angle = m_target.eulerAngles;
                    angle.x = angle.x > 180f ? angle.x - 360 : angle.x;
                    angle.x = Mathf.Clamp(angle.x, m_minAngle, m_maxAngle);
                    angle.z = 0f;
                    m_target.rotation = Quaternion.Euler(angle);
                }
                else
                {
                    m_target.Rotate(0, horizontal, 0);
                }
            }
            else
            {
                //Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            float scroll = 0;
            if (!disableZoom)
            {
                scroll = Input.GetAxis("Mouse ScrollWheel");
            } else if(zoomingOutScripted)
            {
                scroll = -0.05f;
            }
   
            //Debug.Log(scroll);
            m_zoom -= scroll * m_speedZoom;
            if (m_zoom <= m_maxZoom)
            {
                m_zoom = m_maxZoom;
            }
            if (m_zoom >= m_minZoom)
            {
                m_zoom = m_minZoom;
            }
            m_cam.orthographicSize = Mathf.Lerp(m_cam.orthographicSize, m_zoom, Time.deltaTime * m_smoothZoom);



            if (m_cam.orthographicSize >= m_startMovPivot && !m_down)
            {
                //transform.parent.position = new Vector3(transform.parent.position.x, Mathf.Lerp(transform.parent.position.y, m_maxYPivot, m_minZoom / m_cam.orthographicSize * Time.deltaTime), transform.parent.position.z);
                transform.parent.position = new Vector3(transform.parent.position.x, Mathf.Lerp(transform.parent.position.y, m_cam.orthographicSize / 2f, m_smoothMovUp * Time.deltaTime), transform.parent.position.z);
                if (transform.parent.position.y >= m_maxYPivot)
                {
                    transform.parent.position = new Vector3(transform.parent.position.x, m_maxYPivot, transform.parent.position.z);
                    m_down = true;
                }
                outsideHouseMesh.SetActive(true);
                poulies.SetActive(true);
                gameModeRef.PlayerZoomedOut();
            }
            if (m_down && m_cam.orthographicSize <= m_startMovDown)
            {
                transform.parent.position = new Vector3(transform.parent.position.x, Mathf.Lerp(transform.parent.position.y, m_startposPivot.y, m_smoothMovDown * Time.deltaTime), transform.parent.position.z);
                outsideHouseMesh.SetActive(false);
                poulies.SetActive(false);
                gameModeRef.PlayerZoomedIn();
                if (transform.parent.position.y <= m_startposPivot.y || scroll < 0f)
                {
                    m_down = false;
                }
            }
            if (scroll > 0f && m_cam.orthographicSize <= m_startMovDown)
            {
                m_down = true;
            }

            //transform.parent.position = new Vector3(transform.parent.position.x, Mathf.Lerp(m_startposPivot.y, m_maxYPivot, (m_cam.orthographicSize - m_maxZoom) / (m_minZoom - m_maxZoom)), transform.parent.position.z);
            //if (transform.parent.position.y >= m_maxYPivot)
            //{
            //    transform.parent.position = new Vector3(transform.parent.position.x, m_maxYPivot, transform.parent.position.z);
            //}
            //else if (transform.parent.position.y <= m_startposPivot.y)
            //{
            //    transform.parent.position = new Vector3(transform.parent.position.x, m_startposPivot.y, transform.parent.position.z);
            //}

        }
    }

   void GameOver()
    {
        m_zoom += 0.1f * m_speedZoom;
        if (m_zoom >= m_minZoom)
        {
            m_zoom = m_minZoom;
        }
        m_cam.orthographicSize = Mathf.Lerp(m_cam.orthographicSize, m_zoom, Time.deltaTime * m_smoothZoom);
        transform.parent.position = new Vector3(transform.parent.position.x, Mathf.Lerp(transform.parent.position.y, m_cam.orthographicSize / 2f, m_smoothMovUp * Time.deltaTime), transform.parent.position.z);
        if (transform.parent.position.y >= m_maxYPivot)
        {
            transform.parent.position = new Vector3(transform.parent.position.x, m_maxYPivot, transform.parent.position.z);
        }
        outsideHouseMesh.SetActive(true);
        poulies.SetActive(true);
        gameModeRef.PlayerZoomedOut();
    }

    public void ScriptedZoomOut()
    {
        zoomingOutScripted = true;
    }
}
