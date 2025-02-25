
using SymphonyFrameWork.System;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class TrajectionMovement : MonoBehaviour
{
    [SerializeField]List<float> checkSpeed = new List<float>();
    List<float> caluculateSpeed = new List<float>();
    IngameSystem ingameSystem;
    ShellWork shellwork;

    private void Start()
    {
        ingameSystem = ServiceLocator.GetInstance<IngameSystem>();
        shellwork = GetComponent<ShellWork>();
        for (int i = 0; i < shellwork.Planets.Length; i++)
        {
            caluculateSpeed.Add( shellwork.Planets[i].transform.position.z / 10 );
        }
    }
    void CheckPow(float Phase1Data, float Phase2Data, float Phase3Data)
    {
        var resultPow= Phase1Data + Phase2Data + Phase3Data;
        for (int i = checkSpeed.Count-1; i  >= 0  ; i--)
        {
            if (checkSpeed[i] >= resultPow)
            {
                TranjectCucumber(i);
                return;
            }
                
        }
    }

    void TranjectCucumber(int index)
    {
        resultSpeed = caluculateSpeed[index];
        
        tranjected = true;
    }
    float resultSpeed;
    bool tranjected;
    private void Update()
    {
        if(tranjected)
        ingameSystem.Cucumber.transform.Translate(transform.forward * Time.deltaTime * resultSpeed);
    }

}
