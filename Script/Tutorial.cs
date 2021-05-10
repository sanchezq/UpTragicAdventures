using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public GameObject clicScriptRef;
    public GameMode gameModeRef;
    Animator animator;

    int transition = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        LaunchTuTo();
    }

    // Update is called once per frame
    void Update()
    {
        Transition();
    }


    void LaunchTuTo()
    {

        animator.SetTrigger("TutoStart");

    }

    
    void Transition()
    {
        if (transition==0 && !gameModeRef.GetPlayerZoomOutState())
        {
            animator.SetInteger("transition", 1);
            transition++;

        }

        if (transition == 1 && Input.GetMouseButtonDown(1))
        {
            animator.SetInteger("transition", 2);
            transition++;

        }

        if (transition ==2 && clicScriptRef.GetComponent<DetectClick>().getTargetObject() != null)
        {
            animator.enabled = false;
            gameModeRef.StartActualGame();
            Destroy(gameObject);
        }

    }


}
