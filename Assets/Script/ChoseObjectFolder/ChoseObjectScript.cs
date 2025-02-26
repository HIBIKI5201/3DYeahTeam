using UnityEngine;

public class ChoseObjectScript : MonoBehaviour
{
    private GameObject selectedObject;
    private Material selectedMaterial;
    [SerializeField] private Material isSelectMaterial;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            SelectObject();
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
            selectedObject = hitObject;
            var ChoseGameObjectSize = hitObject.gameObject.GetComponent<MeshFilter>().mesh.bounds.size;
            selectedMaterial = selectedObject.GetComponent<Renderer>().material;
            selectedObject.GetComponent<Renderer>().material = isSelectMaterial; 

            Debug.Log("選択されたオブジェクト: " + selectedObject.name);
            Debug.Log("選択されたオブジェクトのサイズ: " + ChoseGameObjectSize);
        }
    }
}
