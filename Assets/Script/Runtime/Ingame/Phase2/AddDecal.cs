using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AddDecal : MonoBehaviour
{
    [SerializeField]
    private Material _material;
    [SerializeField]
    private Vector3 _decalSize;

    /// <summary>
    /// デカールをはるメソッド
    /// </summary>
    /// <param name="targetObj">追従するオブジェクト</param>
    /// <param name="decalPos">貼り付けたい場所</param>
    /// <param name="decalRotate">角度</param>
    public void OnDecal(Transform targetObj, Vector3 decalPos, Quaternion decalRotate)
    {
        if (_material == null || targetObj == null)
        {
            Debug.LogWarning("デカールのマテリアルまたは親オブジェクトが設定されていません");
            return;
        }

        //オブジェクトを生成、コンポーネントを追加
        GameObject decalObj = new GameObject("CuttingMarker");
        DecalProjector decal = decalObj.AddComponent<DecalProjector>();

        //各種設定をいじる
        decal.material = _material;
        decal.size = _decalSize;
        decal.renderingLayerMask = 128;

        //親オブジェクトに設定
        decalObj.transform.SetParent(targetObj);
        //座標を合わせる
        decalObj.transform.position = decalPos;
        decalObj.transform.rotation = decalRotate;
    }
}
