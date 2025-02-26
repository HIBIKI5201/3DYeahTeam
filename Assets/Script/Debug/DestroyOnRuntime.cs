using UnityEngine;

public class DestroyOnRuntime : MonoBehaviour
{
    private void Awake() => Destroy(gameObject);
}
