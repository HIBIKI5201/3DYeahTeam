using System;
using UnityEngine;

public class ShellWork : MonoBehaviour
{
    GameObject[] planets;
    public GameObject[] Planets{ get => planets; }
    Animator animator;
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
    void SetCrashing(int index)
    {
        animator.SetInteger("crashed", index);
    }
    private void OnTriggerEnter(Collider other)
    {
        int i = Array.IndexOf(planets, other.gameObject);
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
