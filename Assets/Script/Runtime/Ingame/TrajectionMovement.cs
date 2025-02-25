
using SymphonyFrameWork.System;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class TrajectionMovement : MonoBehaviour
{
    [SerializeField , Tooltip("小さい順に評価値を入れて")]float[] checkSpeed = new float[7] {0f,10f,20f,30f,40f,50f,60f};
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
        for (int i = checkSpeed.Length-1; i  >= 0  ; i--)
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
