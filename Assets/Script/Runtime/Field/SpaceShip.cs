using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    [SerializeField]
    private CucumberManager _cucumber;
    public CucumberManager Cucumber { get => _cucumber; }
}
