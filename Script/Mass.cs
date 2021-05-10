using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mass : MonoBehaviour
{
    [SerializeField] private GameObject balloon1;
    [SerializeField] private GameObject balloon2;
    [SerializeField] private GameObject balloon3;
    [SerializeField] private GameObject balloon4;
    [SerializeField] private GameObject wall1;
    [SerializeField] private GameObject wall2;
    [SerializeField] private GameObject wall3;
    [SerializeField] private GameObject wall4;
    [SerializeField] private GameObject House;

    public float startRot = .3f;
    public float speedRot = 1f;
    public float rotLim = 15f;
    public float timeToReact = 10f;
    public int nbBalloon = 4;
    public GameMode gameModeRef;

    private Vector2 limitX, limitZ;

    private static List<GameObject> listObjects = new List<GameObject>();

    public float massZoneA, massZoneB, massZoneC, massZoneD, 
                  ballZoneA, ballZoneB, ballZoneC, ballZoneD,
                  rotZoneA = 0f, rotZoneB = 0f, rotZoneC = 0f, rotZoneD = 0f,
                  rotLimZoneA, rotLimZoneB, rotLimZoneC, rotLimZoneD,
                  timeZoneA, timeZoneB, timeZoneC, timeZoneD,
                  radiusMax;

    [Header ("Balloon Settings")]
    private Color initColor;

    public float alphaBalloon, gR, gG,
                 gB, yR, yG, yB, oR, oG,
                 oB, rR, rG, rB;

    //irwin le troll
    // Start is called before the first frame update
    void Start()
    {
        initColor = balloon1.GetComponent<SkinnedMeshRenderer>().material.color;
        limitX = new Vector2(wall1.transform.position.x, wall1.transform.position.x);
        limitZ = new Vector2(wall1.transform.position.z, wall1.transform.position.z);

        List<GameObject> walls = new List<GameObject>();
        walls.Add(wall2);
        walls.Add(wall3);
        walls.Add(wall4);

        GenerateLimits(walls);

        GenerateRadius();

        listObjects.AddRange(GameObject.FindGameObjectsWithTag("ObjectWithMass"));

        ballZoneA = nbBalloon; ballZoneB = nbBalloon; ballZoneC = nbBalloon; ballZoneD = nbBalloon;
        rotLimZoneA = rotLim; rotLimZoneB = rotLim; rotLimZoneC = rotLim; rotLimZoneD = rotLim;
        timeZoneA = timeToReact; timeZoneB = timeToReact; timeZoneC = timeToReact; timeZoneD = timeToReact;
    }

    public static void RemoveObject(GameObject target)
    {
        listObjects.Remove(target);
        //Destroy(target);
    }

    public static void AddObject(GameObject target)
    {
        listObjects.Add(target);
    }

    void GenerateRadius()
    {
        radiusMax = Vector2.Distance(transform.position, new Vector2(limitX.x, limitZ.x));

        if (radiusMax < Vector2.Distance(transform.position, new Vector2(limitX.x, limitZ.y)))
        {
            radiusMax = Vector2.Distance(transform.position, new Vector2(limitX.x, limitZ.y));
        }
        else if (radiusMax < Vector2.Distance(transform.position, new Vector2(limitX.y, limitZ.x)))
        {
            radiusMax = Vector2.Distance(transform.position, new Vector2(limitX.y, limitZ.x));
        }
        else if (radiusMax < Vector2.Distance(transform.position, new Vector2(limitX.y, limitZ.y)))
        {
            radiusMax = Vector2.Distance(transform.position, new Vector2(limitX.y, limitZ.y));
        }
    }

    void GenerateLimits(List<GameObject> walls)
    {
        foreach (GameObject wall in walls)
        {
            if (wall.transform.position.x > limitX.y)
            {
                limitX.y = wall.transform.position.x;
            }
            else if (wall.transform.position.x < limitX.x)
            {
                limitX.x = wall.transform.position.x;
            }

            if (wall.transform.position.z > limitZ.y)
            {
                limitZ.y = wall.transform.position.z;
            }
            else if (wall.transform.position.z < limitZ.x)
            {
                limitZ.x = wall.transform.position.z;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        massZoneA = 0f; massZoneB = 0f; massZoneC = 0f; massZoneD = 0f;

        foreach (GameObject obj in listObjects)
        {
            if (obj != null)
            {
                if (obj.transform.localPosition.y > transform.localPosition.y && Vector3.Distance(obj.transform.localPosition, transform.localPosition) < radiusMax)
                {
                    if (obj.transform.localPosition.x > transform.localPosition.x)
                    {
                        if (obj.transform.localPosition.z > transform.localPosition.y)
                        {
                            massZoneA += obj.GetComponent<Rigidbody>().mass * Vector3.Distance(transform.localPosition, obj.transform.localPosition) / radiusMax;
                        }
                        else
                        {
                            massZoneB += obj.GetComponent<Rigidbody>().mass * Vector3.Distance(transform.localPosition, obj.transform.localPosition) / radiusMax;
                        }
                    }
                    else
                    {
                        if (obj.transform.localPosition.z > transform.localPosition.y)
                        {
                            massZoneC += obj.GetComponent<Rigidbody>().mass * Vector3.Distance(transform.localPosition, obj.transform.localPosition) / radiusMax;
                        }
                        else
                        {
                            massZoneD += obj.GetComponent<Rigidbody>().mass * Vector3.Distance(transform.localPosition, obj.transform.localPosition) / radiusMax;
                        }
                    }
                }
            }
        }

        float totalMass = massZoneA + massZoneB + massZoneC + massZoneD;
        massZoneA /= totalMass; massZoneB /= totalMass; massZoneC /= totalMass; massZoneD /= totalMass;

        ZoneRotate(ref massZoneA, ref rotZoneA, ref rotLimZoneA, ref rotZoneD, ref timeZoneA, ref ballZoneA, -Vector3.forward - Vector3.left, 1);
        ZoneRotate(ref massZoneB, ref rotZoneB, ref rotLimZoneB, ref rotZoneC, ref timeZoneB, ref ballZoneB, -Vector3.forward + Vector3.left, 2);
        ZoneRotate(ref massZoneC, ref rotZoneC, ref rotLimZoneC, ref rotZoneB, ref timeZoneC, ref ballZoneC, Vector3.forward - Vector3.left, 3);
        ZoneRotate(ref massZoneD, ref rotZoneD, ref rotLimZoneD, ref rotZoneA, ref timeZoneD, ref ballZoneD, Vector3.forward + Vector3.left, 4);
    }

    void ZoneRotate(ref float p_massZone, ref float p_rotZone, ref float p_rotLimZone, ref float p_rotOppZone, ref float p_timeZone, ref float p_ballZone, Vector3 p_direction, int p_zone)
    {
        if (p_massZone > startRot && p_massZone != Mathf.Infinity)
        {
            if (p_rotZone < p_rotLimZone)
            {
                balloon1.transform.parent.GetComponent<Animator>().SetFloat("AnimSpeedMult", 1);
                balloon2.transform.parent.GetComponent<Animator>().SetFloat("AnimSpeedMult", 1);
                balloon3.transform.parent.GetComponent<Animator>().SetFloat("AnimSpeedMult", 1);
                balloon4.transform.parent.GetComponent<Animator>().SetFloat("AnimSpeedMult", 1);

                //ChangeColorBalloon(p_ballZone, p_zone);
                House.transform.Rotate((p_direction) * Time.deltaTime * 0.1f * speedRot); 
                p_rotZone += Time.deltaTime;
                p_rotOppZone -= Time.deltaTime;
                p_timeZone = timeToReact;
            }
            else if (p_timeZone > 0f)
            {
                //TODO ANIMATION BALLOON TIMER
               
               AnimationBalloon(p_zone, p_timeZone);
                p_timeZone -= Time.deltaTime;
            }
            else
            {
                p_ballZone--;
                ChangeColorBalloon(p_ballZone, p_zone);
                p_timeZone = timeToReact;
                p_rotLimZone += rotLim;

                if (p_ballZone == 0)
                {
                    gameModeRef.GameOver();
                }
            }
        }
    }

    void AnimationBalloon(int p_zone, float p_timeZone)
    {
        float alpha = ((int)p_timeZone) % 2; 
        if (p_zone == 1)
        {

            //balloon1.GetComponent<SkinnedMeshRenderer>().material.color = new Color(10, 0, 0, alpha);
            //balloon1.transform.parent.GetComponent<Animator>().SetFloat("AnimSpeedMult", 3);
            
        }
        else if (p_zone == 2)
        {
            //balloon2.GetComponent<SkinnedMeshRenderer>().material.color = new Color(10, 0, 0, alpha);
            //balloon2.transform.parent.GetComponent<Animator>().SetFloat("AnimSpeedMult", 3);
        }
        else if (p_zone == 3)
        {
            //balloon3.GetComponent<SkinnedMeshRenderer>().material.color = new Color(10, 0, 0, alpha);
            //balloon3.transform.parent.GetComponent<Animator>().SetFloat("AnimSpeedMult", 3);
        }
        else
        {
            //balloon4.GetComponent<SkinnedMeshRenderer>().material.color = new Color(10, 0, 0, alpha);
            //balloon4.transform.parent.GetComponent<Animator>().SetFloat("AnimSpeedMult", 3);
        }
    }

    void ChangeColorBalloon(float p_life, int p_zone)
    {
        Color color = new Color(initColor.r, initColor.g, initColor.b, alphaBalloon);
        Color emission = new Color(0f, 0f, 0f);
        switch(p_life)
        {
            case 4:
                Debug.Log("Green");
                emission = new Color(gR, gG, gB);
                break;
            case 3:
                Debug.Log("Yellow");
                color = new Color(yR, yG, yB); ;
                break;
            case 2:
                Debug.Log("Orange");
                color = new Color(oR, oG, oB);
                break;
            case 1:
                Debug.Log("RED");
                color = new Color(rR, rG, rB);
                break;

        }
        if (p_zone == 1)
        {
            balloon1.transform.parent.GetComponent<Animator>().SetFloat("AnimSpeedMult", 1);
            balloon1.GetComponent<SkinnedMeshRenderer>().material.color = color;
            balloon1.GetComponent<SkinnedMeshRenderer>().material.SetColor("_EmissionColor", emission);
            balloon1.transform.parent.GetComponent<AudioSource>().Play();
            balloon1.transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();
            //balloon1.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,value);

            if (p_life == 0)
            {
                balloon1.transform.parent.GetComponent<Animator>().SetBool("isExplode", true);
            }
        }
        else if (p_zone == 2)
        {
            balloon2.transform.parent.GetComponent<Animator>().SetFloat("AnimSpeedMult", 1);
            balloon2.GetComponent<SkinnedMeshRenderer>().material.color = color;
            balloon2.GetComponent<SkinnedMeshRenderer>().material.SetColor("_EmissionColor", emission);
            balloon2.transform.parent.GetComponent<AudioSource>().Play();
            balloon2.transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();

            if (p_life == 0)
            {
                balloon2.transform.parent.GetComponent<Animator>().SetBool("isExplode", true);
            }
        }
        else if (p_zone == 3)
        {
            balloon3.transform.parent.GetComponent<Animator>().SetFloat("AnimSpeedMult", 1);
            balloon3.GetComponent<SkinnedMeshRenderer>().material.color = color;
            balloon3.GetComponent<SkinnedMeshRenderer>().material.SetColor("_EmissionColor", emission);
            balloon3.transform.parent.GetComponent<AudioSource>().Play();
            balloon3.transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();

            if (p_life == 0)
            {
                balloon3.transform.parent.GetComponent<Animator>().SetBool("isExplode", true);
            }
        }
        else 
        {
            balloon4.transform.parent.GetComponent<Animator>().SetFloat("AnimSpeedMult", 1);
            balloon4.GetComponent<SkinnedMeshRenderer>().material.color = color;
            balloon4.GetComponent<SkinnedMeshRenderer>().material.SetColor("_EmissionColor", emission);
            balloon4.transform.parent.GetComponent<AudioSource>().Play();
            balloon4.transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();

            if (p_life == 0)
            {
                balloon4.transform.parent.GetComponent<Animator>().SetBool("isExplode", true);
            }
        }
    }
}
