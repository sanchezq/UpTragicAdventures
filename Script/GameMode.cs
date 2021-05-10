using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameMode : MonoBehaviour
{
    [Header("Parameters")]
    public bool ActivateMenus = true;
    public float distanceSpeed = 5;
    public float startDistance = 100;

    [Header("References")]
    public AudioSource AmbientSound_Wind;
    public AudioSource AmbientSound_House;
    public AudioSource MainMenu_Music;
    public GameObject canvasRef;
    public GameObject gameOverMenuRef;
    public Image WhiteScreenPanel;
    public TextMeshProUGUI distanceUI;
    public GameObject startMenuUI;
    public Mass massScriptRef;
    public CameraMove camScriptRef;
    public GameObject UICredits;
    public GameObject endPlatform;
    public GameObject housePivotRef;
    public GameObject landscapeSpawnRef;
    public GameObject UIpause;
    public Slider SliderDistance;
    public GameObject tutorialRef;

    public bool gameOver = false;
    bool distanceUIActive = false;
    float currentDistance = 100;
    bool zoomedOut = true;
    bool raiseEnd;
    bool gamePaused = false;
    // Start is called before the first frame update
    void Start()
    {
        canvasRef.SetActive(true);

        if (ActivateMenus)
        {
            startMenuUI.SetActive(true);
            WhiteScreenPanel.gameObject.SetActive(false);
            massScriptRef.enabled = false;
            camScriptRef.disableZoom = true;
        } else
        {
            StartActualGame();
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (distanceUIActive)
        {
            currentDistance += Time.deltaTime * 10;
            distanceUI.text = Mathf.Round(currentDistance).ToString() + " km";
            SliderDistance.value = currentDistance;
            if (currentDistance >= 4000)
            {
                distanceUIActive = false;
                GameWon();
            }
        }

        if (raiseEnd)
        {
            endPlatform.transform.position += new Vector3(0, Time.deltaTime, 0);
            housePivotRef.transform.rotation = Quaternion.Lerp(housePivotRef.transform.rotation, Quaternion.Euler(new Vector3(0, 45, 0)), Time.deltaTime*0.25f);
            if (endPlatform.transform.position.y > -18)
            {
                raiseEnd = false;

                Invoke("GameWonp2", 3.5f);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                UIpause.SetActive(false);
                Time.timeScale = 1;
                gamePaused = false;
            } else
            {
                UIpause.SetActive(true);
                Time.timeScale = 0;
                gamePaused = true;
            }
        }

        //DEBUG
        if (Input.GetKeyDown(KeyCode.P) && distanceUIActive)
        {
            GameOver();
        }

        if (zoomedOut)
        {
            AmbientSound_House.volume = Mathf.Lerp(AmbientSound_House.volume, 0.3f, Time.deltaTime);
            AmbientSound_House.pitch = Mathf.Lerp(AmbientSound_House.pitch, 1f, Time.deltaTime*50);
            AmbientSound_Wind.volume = Mathf.Lerp(AmbientSound_Wind.volume, 0.05f, Time.deltaTime);
            AmbientSound_Wind.pitch = Mathf.Lerp(AmbientSound_Wind.pitch, 1f, Time.deltaTime*50);
        } else
        {
            AmbientSound_House.volume = Mathf.Lerp(AmbientSound_House.volume, 0.15f, Time.deltaTime);
            AmbientSound_House.pitch = Mathf.Lerp(AmbientSound_House.pitch, 0.65f, Time.deltaTime*50);
            AmbientSound_Wind.volume = Mathf.Lerp(AmbientSound_Wind.volume, 0.01f, Time.deltaTime);
            AmbientSound_Wind.pitch = Mathf.Lerp(AmbientSound_Wind.pitch, 0.5f, Time.deltaTime*50);
        }


    }

    public void StartActualGame()
    {
        gameOver = false;
        
        //WhiteScreenPanel.gameObject.SetActive(true);
        massScriptRef.enabled = true;
        
        gameOverMenuRef.SetActive(false);

        //White Fade In
        //WhiteScreenPanel.color = Color.gray;
        //WhiteScreenPanel.color = new Color(WhiteScreenPanel.color.r, WhiteScreenPanel.color.g, WhiteScreenPanel.color.b, 125);
        //StartCoroutine(FadeInPanel(WhiteScreenPanel, new Color(0, 0, 0, 0)));

        // Wind Sounds
        AmbientSound_Wind.Play();
        AmbientSound_Wind.volume = 0;
        StartCoroutine(FadeInSound(AmbientSound_Wind, 0.05f));


        //Distance UI
        currentDistance = startDistance;
        Invoke("DisplayDistanceUI", 4);
    }

    public void StartGame()
    {
        //if (PlayerPrefs.GetInt(""))
        tutorialRef.SetActive(true);
        startMenuUI.SetActive(false);
        Invoke("EnableTutoZoom", 1.5f);
        StartCoroutine(FadeOutSound(MainMenu_Music, 0.0f));
    }

    public void EnableTutoZoom()
    {
        camScriptRef.disableZoom = false;
    }

    IEnumerator FadeInSound(AudioSource sound, float endVolume)
    {
        for (float ft = 0f; ft < 10;)
        {
            sound.volume = Mathf.Lerp(sound.volume, endVolume, ft*.001f);
            ft += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator FadeOutSound(AudioSource sound, float endVolume)
    {
        for (float ft = 0f; ft < 10;)
        {
            sound.Stop();

            sound.volume = Mathf.Lerp(sound.volume, endVolume, ft * .001f);
            ft += Time.deltaTime;
            yield return null;
        }
    }


    IEnumerator FadeInPanel(Image panel, Color finalColor)
    {
        for (float ft = 0f; ft < 13;)
        {
            panel.color = Color.Lerp(panel.color, finalColor, ft * 0.001f);
            ft += Time.deltaTime;
            yield return null;
        }
    }

    void DisplayDistanceUI()
    {
        distanceUI.gameObject.SetActive(true);
        distanceUIActive = true;
    }

    public void GameOver()
    {
        gameOver = true;
        //Hide distance UI
        distanceUIActive = false;
        distanceUI.gameObject.SetActive(false);

        gameOverMenuRef.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Mathf.Round(currentDistance).ToString() + " km";
        gameOverMenuRef.SetActive(true);
    }

    public void GameWon()
    {
        //Lock les inputs
        //Avancer au dela de la camera
        raiseEnd = true;
        massScriptRef.enabled = false;
        camScriptRef.disableZoom = true;
        camScriptRef.ScriptedZoomOut();
        landscapeSpawnRef.GetComponent<SpawnLandscape>().StopSpawningLandscape();
    }
    
    public void GameWonp2()
    {
        UICredits.transform.GetChild(0).gameObject.SetActive(true);
        UICredits.gameObject.GetComponent<Animator>().SetTrigger("CreditsAnim");
        Invoke("QuitGame", 33);
    }

    public void PlayerZoomedOut()
    {
        if (!zoomedOut)
        {
            zoomedOut = true;
        }
    }

    public void PlayerZoomedIn()
    {
        if (zoomedOut)
        {
            zoomedOut = false;
        }
    }

    public void QuitGame()
    {
        Debug.Log("GameQuit");
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public bool GetPlayerZoomOutState()
    {
        return zoomedOut;
    }
}
