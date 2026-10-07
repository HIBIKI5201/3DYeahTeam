using SymphonyFrameWork.System;
using System.Collections.Generic;
using UnityEngine;
public class ChoseObjectScript : MonoBehaviour
{
    [SerializeField]
    private Material _isSelectMaterial;
    [SerializeField]
    private Material _isSelectInnerMaterial;
    [SerializeField]
    private RotateCucumber _rotateobj;

    private GameObject _selectedObject;
    private Material _selectedMaterial;
    private Material _selectedInnerMaterial;
    private IngameSystem _ingameSystem;

    private bool _isOkNextPhase;
    private bool _isChose;

    private List<GameObject> _choseObjects = new List<GameObject>();

    private async void Start()
    {
        await Awaitable.NextFrameAsync();

        _ingameSystem = ServiceLocator.GetInstance<IngameSystem>();
        _rotateobj = ServiceLocator.GetInstance<RotateCucumber>();
        _rotateobj.OnCutEnd += CutEnd;
    }

    private void CutEnd(GameObject[] gameObjects)
    {
        foreach (var pieces in gameObjects)
        {
            _choseObjects.Add(pieces);
        }

        _isChose = true;
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

            if (_isOkNextPhase && _isChose)
            {
                // "select" 以外を削除
                _choseObjects.RemoveAll(g =>
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
                Material[] origin = { _selectedMaterial, _selectedInnerMaterial };
                _selectedObject.GetComponent<Renderer>().sharedMaterials = origin;
            }

            _isOkNextPhase = true;
            _selectedObject = hitObject;

            var ChoseGameObjectSize = hitObject.gameObject.GetComponent<MeshFilter>().mesh.bounds.size;
            _selectedMaterial = _selectedObject.GetComponent<Renderer>().sharedMaterials[0];
            _selectedInnerMaterial = _selectedObject.GetComponent<Renderer>().sharedMaterials[1];
            Material[] isSelect = { _isSelectMaterial, _isSelectInnerMaterial };
            _selectedObject.GetComponent<Renderer>().sharedMaterials = isSelect;

            _choseObjects.Add(_selectedObject);

            Debug.Log("選択されたオブジェクト: " + _selectedObject.name);
            Debug.Log("選択されたオブジェクトのサイズ: " + ChoseGameObjectSize);
            var rotateObj = ServiceLocator.GetInstance<RotateCucumber>();

            //今度ここの修正をする
            //rotateObj.Distance(rotateObj.AngleDifference);
        }
    }
}
