using UnityEngine;
using UnityEngine.UIElements;

public class ResultUI : MonoBehaviour
{
    private UIDocument _document;
    private void Awake()
    {
        _document = GetComponent<UIDocument>();
    }
}
