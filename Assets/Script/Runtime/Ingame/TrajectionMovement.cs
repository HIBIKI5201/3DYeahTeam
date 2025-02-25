
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
    GameObject effect;
    ParticleSystem jetEffect;

    private void Start()
    {
        Camera camera = Camera.main;
        ingameSystem = ServiceLocator.GetInstance<IngameSystem>();
        shellwork = GetComponent<ShellWork>();
        shellwork.hitWithPlanet += SetHitStop;
        camera.transform.parent = ingameSystem.Cucumber.transform;
        effect.transform.parent = ingameSystem.Cucumber.transform;
        jetEffect = effect.GetComponent<ParticleSystem>();

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

    void SetHitStop(int index)
    {
        hitStopTimeRemaining = hitStopDuration;
        hittingPlanetIndex = index;
    }
    void TranjectCucumber(int index)
    {
        resultSpeed = caluculateSpeed[index];
        finalPlanteIndex = index;
        tranjected = true;
        jetEffect.Play();
        slidingTimer = coastTime;
    }

    int hittingPlanetIndex;
    int finalPlanteIndex;
    float hitStopTimeRemaining = 0;
    [SerializeField] float hitStopDuration = 0.5f;
    [SerializeField] float coastTime = 3f;
    float slidingTimer;
    float resultSpeed;
    bool tranjected;
    private void Update()
    {
        if (hitStopTimeRemaining > 0f)
        {
            hitStopTimeRemaining -= Time.unscaledDeltaTime;
            return;
        }
        if(hittingPlanetIndex == finalPlanteIndex)
        {
            slidingTimer -= Time.unscaledDeltaTime;
            if (coastTime > 0f) ingameSystem.Cucumber.transform.Translate(transform.forward * Time.deltaTime * coastTime);
            if (jetEffect.isPlaying) jetEffect.Stop(); 
            return;
        }
        if (tranjected)
        ingameSystem.Cucumber.transform.Translate(transform.forward * Time.deltaTime * resultSpeed);
    }

}
