using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShellWork : MonoBehaviour
{
    GameObject[] planets;
    [SerializeField]List<GameObject> breakableObjects = new List<GameObject>();
    [SerializeField] float objDuration;
    float settedObjLength;
    public GameObject[] Planets{ get => planets; }
    Animator animator;

    public Action SerializePlanets;
    void Start()
    {
        animator = GetComponent<Animator>();

        planets = new GameObject[transform.childCount]; 
        for (int i = 0; i < transform.childCount; i++) 
        {
            planets[i] = transform.GetChild(i).gameObject;
        }
        for(int i = 0; i < breakableObjects.Count;i++)
        {
            settedObjLength += objDuration;

            foreach (GameObject check in planets)
            {
                if(Mathf.Abs(settedObjLength - check.transform.position.z) < objDuration) settedObjLength += objDuration;
            }

            Instantiate(breakableObjects[i], new Vector3(0, 0, settedObjLength), Quaternion.identity);
        }
    }
    void CrashPlanets(CrashedPlanet crashedPlanet)
    {
        if (crashedPlanet != 0)
        {
            SetCrashing((int)crashedPlanet);
        }
    }
    public Action<int> hitWithPlanet;
    void SetCrashing(int index)
    {
        animator.SetInteger("index", index);
        hitWithPlanet?.Invoke(index);
    }

    public void OnChildTriggerEnter(GameObject planet)
    {
        int i = Array.IndexOf(planets, planet);
        if (i < 0) return;
        SetCrashing(i + 1);
    }

}

public enum CrashedPlanet
{
    Nothing,
    Mercury,
    Mars,
    Venus,
    Uranus,
    Saturn,
    Jupiter,
    Sun,
}
