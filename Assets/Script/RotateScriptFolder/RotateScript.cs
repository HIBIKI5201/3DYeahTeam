using UnityEngine;
using ChargeShot.Runtime.Ingame;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 50f;  // 回転速度
    public float perfectRotation = 90;
    private bool rotatingClockwise = true;  // 時計回りか反時計回りかを管理
    private float currentRotation = 0f;  // 現在の回転角度
    private GameObject savedRightSide;
    public GameObject _targetObject; // 切断したいオブジェクト
    public GameObject _cuttingPlane; // 切断平面
    public Material _capMaterial; // 切断面に適用するマテリアル
    void Update()
    {
        // 回転方向に応じて回転
        if (rotatingClockwise)
        {
            transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
            currentRotation += rotationSpeed * Time.deltaTime;
        }
        else
        {
            transform.Rotate(-rotationSpeed * Time.deltaTime, 0, 0);
            currentRotation -= rotationSpeed * Time.deltaTime;
        }

        // 180度回転したら回転方向を反転
        if (Mathf.Abs(currentRotation) >= 180f)
        {
            rotatingClockwise = !rotatingClockwise;  // 回転方向を反転
            currentRotation = 0f;  // 回転角度をリセット
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (savedRightSide == null)
            {
                // 最初のカット
                Cut(_targetObject);
            }
            else
            {
                // 右側のオブジェクトをさらにカット
                Cut(_targetObject);
                Cut(savedRightSide);
            }
        }
    }
    public void Cut(GameObject target)
    {
        // オブジェクトを切断
        var pieces = MeshCutService.Cut(target, _cuttingPlane.transform.position, _cuttingPlane.transform.up, _capMaterial);

        // 切断されたオブジェクトが存在する場合のみ処理を行う
        if (pieces != null)
        {
            var leftSide = pieces[0];
            var rightSide = pieces[1];
            rightSide.transform.position += rightSide.transform.right * -0.1f;
            if (savedRightSide == null)
            {
                savedRightSide = rightSide; // 右側のオブジェクトを保存
            }
        }
        else
        {
            Debug.LogError("オブジェクトの切断に失敗しました。");
        }
    }
    public void Distance(float currentYRotation)
    {
        // Mathf.DeltaAngle を使って適切な回転差分を取得
        float distanceRotation = Mathf.Abs(Mathf.DeltaAngle(currentYRotation, perfectRotation));
        Debug.Log(distanceRotation);
        // 目標角度との差が10度未満なら「perfectTiming」
        if (Mathf.Abs(distanceRotation) < 10)
        {
            Debug.Log("perfectTiming");
        }
        else
        {
            Debug.Log("badTiming");
        }
    }
}
