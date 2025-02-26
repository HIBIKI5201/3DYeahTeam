using UnityEngine;
using SymphonyFrameWork.System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;

public class ChoseObjectScript : MonoBehaviour
{
    private GameObject selectedObject;
    private Material selectedMaterial;
    private bool isOkNextPhase;
    private bool isDone;
    IngameSystem _ingameSystem;
    [SerializeField] private Material isSelectMaterial;
    [SerializeField] RotateObject rotateobj;
    List<GameObject> a = new List<GameObject>();
    private void Awake()
    {
        _ingameSystem = ServiceLocator.GetInstance<IngameSystem>();
        selectedObject = GameObject.Find("center");
        BoxCollider boxCollider = selectedObject.GetComponent<BoxCollider>();
        Destroy(boxCollider);
        selectedObject = null;
    }
    private void Start()
    {
        //var a = ServiceLocator.GetInstance<RotateObject>();
        rotateobj._cutEnd += CutEnd;
    }
    private void CutEnd(List<GameObject> gameObjects)
    {
        foreach (var g in gameObjects)
        {
            a.Add(g);
        }
       
        isDone = true;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectObject();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            selectedObject.name = "select";
            if (isOkNextPhase && isDone)
            {
                // "select" 以外を削除
                a.RemoveAll(g =>
                {
                    if (g.name != "select")
                    {
                        Destroy(g);
                        return true;
                    }
                    return false;
                });

                // `selectedObject` を `ingameSystem` に送信
                _ingameSystem.SetCucumberInstance(selectedObject);
                _ingameSystem.NextPhaseEvent();
            }
        }
    }

    void SelectObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
       
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (selectedObject != null)
            {
                selectedObject.GetComponent<Renderer>().material = selectedMaterial;
            }
            isOkNextPhase = true;
            selectedObject = hitObject;
            var ChoseGameObjectSize = hitObject.gameObject.GetComponent<MeshFilter>().mesh.bounds.size;
            selectedMaterial = selectedObject.GetComponent<Renderer>().material;
            selectedObject.GetComponent<Renderer>().material = isSelectMaterial;
            a.Add(selectedObject);
            Debug.Log("選択されたオブジェクト: " + selectedObject.name);
            Debug.Log("選択されたオブジェクトのサイズ: " + ChoseGameObjectSize);
        }
    }
}
