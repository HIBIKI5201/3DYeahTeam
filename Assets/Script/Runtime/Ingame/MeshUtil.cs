using UnityEngine;

public static class MeshUtil
{
    public static void MeshColliderRefresh(GameObject gameObject)
    {
        MeshCollider collider = null;

        if (!gameObject.TryGetComponent(out collider))
        {
            collider = gameObject.AddComponent<MeshCollider>();
        }

        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        collider.sharedMesh = mesh;
    }
}
