
using SymphonyFrameWork.System;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TrajectionMovement : MonoBehaviour
{
    [SerializeField , Tooltip("小さい順に評価値を入れて")]float[] checkSpeed = new float[7] {0f,5f,10f,15f,20f,25f,30f};
    List<float> caluculateSpeed = new List<float>();
    IngameSystem ingameSystem;
    ShellWork shellwork;
    [SerializeField] GameObject effect;
    ParticleSystem jetEffect;
    [SerializeField]Transform cameraTarget;

    Volume postPro;
    Bloom bloom;
    [SerializeField] float bloomLowIntensity = 2.1f;
    [SerializeField] float bloomHighIntensity = 150f;

    async void Start()
    {
        await Awaitable.NextFrameAsync();

        shellwork = transform.GetComponent<ShellWork>();
        shellwork.hitWithPlanet += SetHitStop;

        ingameSystem = ServiceLocator.GetInstance<IngameSystem>();

        postPro = ServiceLocator.GetInstance<MainSystem>().Volume;
        postPro.profile.TryGet<Bloom>(out bloom);
        bloom.intensity.Override(bloomLowIntensity);

        float cucumberFixedScale = 0.05f / SerchCucumberLength(ingameSystem.Cucumber.CucumberModel.GetComponent<MeshFilter>());
        if (float.IsInfinity(cucumberFixedScale)) cucumberFixedScale =0.0001f;
        cameraTarget.parent = ingameSystem.Cucumber.transform;
        effect.transform.parent = ingameSystem.Cucumber.gameObject.transform;
        ingameSystem.Cucumber.transform.GetChild(0).rotation *= new Quaternion(0,Mathf.Sin(Mathf.PI/4),0,Mathf.Cos(Mathf.PI/4));
        ingameSystem.Cucumber.transform.GetChild(0).localScale = new Vector3(cucumberFixedScale, cucumberFixedScale, cucumberFixedScale);
        jetEffect = effect.transform.GetComponent<ParticleSystem>();
        Rigidbody rb = ingameSystem.Cucumber.transform.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        ingameSystem.Cucumber.transform.AddComponent<BoxCollider>();

        for (int i = 0; i < transform.childCount; i++)
        {
            caluculateSpeed.Add(transform.GetChild(i).position.z / 10);
        }
    }

    float SerchCucumberLength(MeshFilter meshFilter)
    {
        float longestEdgeLength = 0f;
        if (meshFilter != null)
        {
            // メッシュの頂点と三角形インデックスを取得
            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;

            

            // 三角形の辺を計算
            for (int i = 0; i < triangles.Length; i += 3)
            {
                // 三角形の3つの頂点インデックスを取得
                int index0 = triangles[i];
                int index1 = triangles[i + 1];
                int index2 = triangles[i + 2];

                // 三角形の辺の長さを計算
                float edge01 = Vector3.Distance(vertices[index0], vertices[index1]);
                float edge12 = Vector3.Distance(vertices[index1], vertices[index2]);
                float edge20 = Vector3.Distance(vertices[index2], vertices[index0]);

                // 一番長い辺を見つける
                float longestEdgeInTriangle = Mathf.Max(edge01, edge12, edge20);
                longestEdgeLength = Mathf.Max(longestEdgeLength, longestEdgeInTriangle);
            }
            
        }
        return longestEdgeLength;
    }

    void CheckPow(float Phase1Data, float Phase2Data, float Phase3Data)
    {
        var resultPow= Phase1Data + Phase2Data + Phase3Data;
        for (int i = checkSpeed.Length-1; i  >= 0  ; i--)
        {
            //Debug.Log(checkSpeed[i]);
            if (checkSpeed[i] <= resultPow)
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
        resultSpeed = caluculateSpeed[index] + caluculateSpeed[index] / 2;
        deceleration = resultSpeed / 5;
        finalPlanteIndex = index;
        tranjected = true;
        jetEffect.Play();
        slidingTimer = coastTime;
        cameraTarget.localPosition = new Vector3(0, cameraTarget.localPosition.y, 9.3f);
    }

    int hittingPlanetIndex = -100;
    int finalPlanteIndex;
    float hitStopTimeRemaining = 0;
    [SerializeField] float hitStopDuration = 0.05f;
    [SerializeField] float coastTime = 3f;
    float slidingTimer;
    float resultSpeed;
    float deceleration;
    bool tranjected;
    public Action AllFinished;

    float theIntensity;
    private void Update()
    {
        // test用コード
        if (Input.GetKeyDown(KeyCode.Space)) { CheckPow(10, 10, 10);  }

        if (hittingPlanetIndex == transform.childCount - 1 && bloomHighIntensity > theIntensity) { theIntensity += 100 * Time.deltaTime; bloom.intensity.Override(theIntensity); bloom.tint.Override( new Color(theIntensity * 100* Time.deltaTime +10 , 10, 10)); }

        if (hittingPlanetIndex > 0 && hittingPlanetIndex <= transform.childCount-2)
        {
            cameraTarget.localPosition = new Vector3(cameraTarget.localPosition.x, cameraTarget.localPosition.y, cameraTarget.localPosition.z - 1f * hittingPlanetIndex * Time.deltaTime);
        }
        if (hitStopTimeRemaining > 0f)
        {
            hitStopTimeRemaining -= Time.unscaledDeltaTime;
            return;
        }
        if(hittingPlanetIndex-1 == finalPlanteIndex && hittingPlanetIndex > 0)
        {
            //Debug.Log("Sliding");
            slidingTimer -= Time.unscaledDeltaTime;
            if (coastTime > 0f) { ingameSystem.Cucumber.transform.Translate(transform.forward * Time.deltaTime * resultSpeed  );  if(resultSpeed >0) resultSpeed -= deceleration * Time.deltaTime; }
            if (jetEffect.isPlaying) jetEffect.Stop();
            if(slidingTimer <= 2f )
            {
                AllFinished?.Invoke(); 
                
                bloom.intensity.Override(bloomLowIntensity); bloom.tint.Override(new Color(0, 0, 0));
            }
            tranjected = false;
            return;
        }
        if (tranjected)
        {
            ingameSystem.Cucumber.transform.Translate(transform.forward * Time.deltaTime * resultSpeed);
        }
        
    }

}
