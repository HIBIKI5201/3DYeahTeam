using System;
using UnityEngine;

public class ShellWork : MonoBehaviour
{
    GameObject[] planets;
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
