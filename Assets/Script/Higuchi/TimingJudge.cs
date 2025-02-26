//using UnityEngine;

//public class TimingJudge : MonoBehaviour
//{
//    [SerializeField, Header("ベストタイミング1")] float _bestTiming1;
//    [SerializeField, Header("ベストタイミング2")] float _bestTiming2;

//    CutterMoveController _cutter = new();
//    private void Start()
//    {
//        _cutter._OnCuttingData += Judge;
//    }
//    private void Judge(float pos)
//    {
//        Debug.Log($"差 {Mathf.Abs(_bestTiming1 - pos)}");
//    }
//}
