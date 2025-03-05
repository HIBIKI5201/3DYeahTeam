using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ChargeShot.Runtime.Ingame;
using SymphonyFrameWork.System;
using SymphonyFrameWork.Utility;

/// <summary>
/// カッターの管理クラス
/// </summary>
public class Phase1CutTest : MonoBehaviour
{
    [Header("カッターの移動設定")]
    [SerializeField] private float _speed = 2.0f;

    //移動限界たち
    [SerializeField] private float _maxX = 1.0f;
    [SerializeField] private float _minX = -1.0f;
    private float _center;

    [Header("切断設定")]
    [SerializeField] private Material _capMaterial;

    private float[] _bestTimings = new float[2];//ベストな切る位置

    [Header("ナイフの初期位置")]
    [SerializeField] private Vector3 _knifeOffset = new Vector3(0, 0, 0);//ナイフの初期位置

    //切断後のオブジェクトたち
    private GameObject _leftPiece;
    private GameObject _centerPiece;
    private GameObject _rightPiece;

    private List<float> _pointList = new List<float>();//切断誤差を記録、最終的にスコアになる

    private IngameSystem _ingameSystem;
    private AudioManager _audioManager;

    private GameObject _cucumber;
    private int _cutCount = 0;
    private float _firstCutPos;
    private float _nullLeft;
    private float _nullRight;
    private float _nullPos;
    private bool _movingRight = true;

    public event Action<GameObject> OnCuttingFinish;

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    private async void Initialize()
    {
        _audioManager = ServiceLocator.GetInstance<AudioManager>();

        //シーンがロードされるのを待つ
        await SymphonyTask.WaitUntil(() => SceneLoader.GetExistScene(SceneListEnum.SpaceShip.ToString(), out _));

        //システムの取得
        _ingameSystem = ServiceLocator.GetInstance<IngameSystem>();
        _cucumber = _ingameSystem.Cucumber.CucumberModel;

        _center = (_minX + _maxX) / 2;
        _nullLeft = _minX;
        _nullRight = _maxX;

        //ナイフの位置設定
        var ship = ServiceLocator.GetInstance<SpaceShip>();
        if (ship != null)
        {
            var knife = ship.Knife;
            knife.transform.SetParent(transform);
            knife.transform.localPosition = _knifeOffset;//ナイフの初期位置を設定
            knife.transform.localRotation = Quaternion.Euler(3, -90, 5);//ナイフの角度の制御
        }
    }

    private void Update()
    {
        MoveCutter();
    }

    /// <summary>
    /// カッターを左右に移動させる
    /// </summary>
    private void MoveCutter()
    {
        if (_cutCount >= 2) return;

        Vector3 pos = transform.position;
        pos.x += _movingRight ? _speed * Time.deltaTime : -_speed * Time.deltaTime;
        transform.position = pos;

        if (pos.x >= _maxX)
        {
            _movingRight = false;
        }

        if (pos.x <= _minX)
        {
            _movingRight = true;
        }
    }

    /// <summary>
    /// きゅうりを切断する
    /// </summary>
    public void Phase1Cut()
    {
        GameObject[] pieces = null;
        GameObject target = _cucumber;

        if (_cutCount >= 2) return;

        if (_cutCount != 0 &&//切断回数が初回の場合
            transform.position.x >= _nullLeft &&
            transform.position.x <= _nullRight) return;
        if (_cutCount == 1)
        {
            if (transform.position.x > _firstCutPos)
            {
                target = _rightPiece;
            }
            else
            {
                target = _leftPiece;
            }
        }


        pieces = MeshCutService.Cut(target, transform.position, transform.right, _capMaterial);
        if (pieces == null) return;
        _audioManager.PlaySoundEffect(3);

        _cutCount++;
        GameObject leftPiece = pieces[0];
        GameObject rightPiece = pieces[1];

        MeshUtil.MeshColliderRefresh(leftPiece);

        if (_cutCount == 1)//一回目の場合
        {
            _rightPiece = rightPiece;
            _leftPiece = leftPiece;
            _firstCutPos = transform.position.x;

            if (_center < _firstCutPos)//真ん中よりも右側なら右ピースを右にずらす
            {
                _nullPos = _firstCutPos + rightPiece.transform.position.x + rightPiece.transform.right.x * 300f;
                rightPiece.transform.position += rightPiece.transform.right * 300f;
                _maxX += leftPiece.transform.right.x * 300f;
                _nullLeft = _firstCutPos;
                _nullRight = _nullPos;

            }
            else//左側なら左ピースを左にずらす
            {
                _nullPos = _firstCutPos - leftPiece.transform.position.x + leftPiece.transform.right.x * -300f;
                leftPiece.transform.position += leftPiece.transform.right * -300f;
                _maxX += leftPiece.transform.right.x * -300f;//包丁の移動範囲を更新
                _nullLeft = _nullPos;
                _nullRight = _firstCutPos;
            }
        }
        else//二回目の場合
        {
            if (transform.position.x > _firstCutPos)
            {
                _centerPiece = leftPiece;
                _rightPiece = rightPiece;
                rightPiece.transform.position += rightPiece.transform.right * 300f;
            }
            else
            {
                _centerPiece = rightPiece;
                _leftPiece = leftPiece;
                leftPiece.transform.position += leftPiece.transform.right * -300f;
            }
        }

        _pointList.Add(Mathf.Abs(_bestTimings[_cutCount - 1] - transform.position.x));

        if (_cutCount == 2)
        {
            StartCoroutine(FinishCutting());
        }
    }

    /// <summary>
    /// 切断結果を計算し、ゲームに反映
    /// </summary>
    private IEnumerator FinishCutting()
    {
        float totalPoint = _pointList.Sum();
        float accuracy = Mathf.Abs((totalPoint / 100) - 100);
        _ingameSystem.CucumberData.Phase1Data = accuracy;

        yield return new WaitForSeconds(3f);

        Destroy(_leftPiece);
        Destroy(_rightPiece);

        OnCuttingFinish?.Invoke(_centerPiece);
        Destroy(gameObject);
    }
}
