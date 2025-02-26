using UnityEngine;
using SymphonyFrameWork.System;
using System.Linq;

public class ChoseObjectScript : MonoBehaviour
{
    private GameObject selectedObject;
    private Material selectedMaterial;
    private bool isOkNextPhase;
    IngameSystem _ingameSystem;
    [SerializeField] private Material isSelectMaterial;
    private void Awake()
    {
        _ingameSystem = ServiceLocator.GetInstance<IngameSystem>();
        selectedObject = GameObject.Find("center");
        BoxCollider boxCollider = selectedObject.GetComponent<BoxCollider>();
        Destroy(boxCollider);
        selectedObject = null;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            SelectObject();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (isOkNextPhase)
            {
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

            Debug.Log("選択されたオブジェクト: " + selectedObject.name);
            Debug.Log("選択されたオブジェクトのサイズ: " + ChoseGameObjectSize);
        }
    }
}
