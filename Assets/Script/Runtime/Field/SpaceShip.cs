using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    [SerializeField]
    private CucumberManager _cucumber;
    public CucumberManager Cucumber { get => _cucumber; }

    [SerializeField]
    private GameObject _knife;
    public GameObject Knife { get => _knife; }
}
