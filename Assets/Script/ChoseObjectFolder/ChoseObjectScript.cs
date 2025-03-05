using SymphonyFrameWork.System;
using System.Collections.Generic;
using UnityEngine;
public class ChoseObjectScript : MonoBehaviour
{
    private GameObject _selectedObject;
    private Material _selectedMaterial;
    private bool _isOkNextPhase;
    private bool _isDone;
    IngameSystem _ingameSystem;
    [SerializeField] private Material _isSelectMaterial;
    [SerializeField] RotateCucumber _rotateobj;
    List<GameObject> a = new List<GameObject>();
    private async void Awake()
    {
        await Awaitable.NextFrameAsync();

        _ingameSystem = ServiceLocator.GetInstance<IngameSystem>();
    }
    private async void Start()
    {
        await Awaitable.NextFrameAsync();

        //_rotateobj = ServiceLocator.GetInstance<RotateObject>();
        _rotateobj = ServiceLocator.GetInstance<RotateCucumber>();
        _rotateobj.OnCutEnd += CutEnd;
    }
    private void CutEnd(List<GameObject> gameObjects)
    {
        foreach (var g in gameObjects)
        {
            a.Add(g);
        }

        _isDone = true;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectObject();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_selectedObject)
            {
                _selectedObject.name = "select";
            }

            if (_isOkNextPhase && _isDone)
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
                _ingameSystem.SetCucumberInstance(_selectedObject);
                _ = SceneLoader.UnloadScene(SceneListEnum.SpaceShip.ToString());
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

            if (_selectedObject != null)
            {
                _selectedObject.GetComponent<Renderer>().material = _selectedMaterial;
            }
            _isOkNextPhase = true;
            _selectedObject = hitObject;
            var ChoseGameObjectSize = hitObject.gameObject.GetComponent<MeshFilter>().mesh.bounds.size;
            _selectedMaterial = _selectedObject.GetComponent<Renderer>().material;
            _selectedObject.GetComponent<Renderer>().material = _isSelectMaterial;
            a.Add(_selectedObject);
            Debug.Log("選択されたオブジェクト: " + _selectedObject.name);
            Debug.Log("選択されたオブジェクトのサイズ: " + ChoseGameObjectSize);
            var rotateObj =  ServiceLocator.GetInstance<RotateObject>();

            rotateObj.Distance(rotateObj.AngleDifference);
        }
    }
}
