using UnityEngine;

public class ChargeManager : MonoBehaviour
{
    //チャージ機能を作る
    //時間制限があり、そのなかでボタンを押された回数を保存しておく
    private float _pushCounter;
    public float PushCounter { get => _pushCounter; }
    private float _timer;

    [SerializeField]
    private float _timLimit;

    private void Start()
    {
        _timer = Time.time;
    }

    private void Update()
    {
        if (Time.time < _timLimit + _timer)
        {
            ChargeAction();
        }
    }

    private void ChargeAction()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _pushCounter++;
            Debug.Log($"現在値は　{_pushCounter}");
        }
    }
}
