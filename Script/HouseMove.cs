using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseMove : MonoBehaviour
{
    private GameMode gameModeRef;
    [SerializeField]
    private float m_speedDown;

    private Vector3 m_initPos;

    // Start is called before the first frame update
    void Start()
    {
        m_initPos = transform.position;
        gameModeRef = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMode>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameModeRef.gameOver)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - m_speedDown * Time.deltaTime, transform.position.z);
        }
        else
        {
            transform.position = m_initPos;
        }
    }
}
