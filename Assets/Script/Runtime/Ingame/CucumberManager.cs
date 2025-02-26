using UnityEngine;

public class CucumberManager : MonoBehaviour
{
    private GameObject _model;

    public GameObject CucumberModel { get => _model; }

    public void Init(GameObject obj)
    {
        _model = obj;

        transform.position = obj.transform.position;
        obj.transform.parent = transform;
    }
}
