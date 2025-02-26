using UnityEngine;

public class PlanetsTrigger : MonoBehaviour
{
    ShellWork shellwork;
    private void Start()
    {
        shellwork = transform.parent.GetComponent<ShellWork>();
    }
    private void OnTriggerEnter(Collider other)
    {
        shellwork.OnChildTriggerEnter(this.gameObject);
    }
}
