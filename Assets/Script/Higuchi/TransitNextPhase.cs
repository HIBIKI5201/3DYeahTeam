using SymphonyFrameWork.System;
using UnityEngine;
using System.Linq;

public class TransitNextPhase : MonoBehaviour
{
    IngameSystem _ingameSystem;
    private System.Collections.IEnumerator Start()
    {
        _ingameSystem = ServiceLocator.GetInstance<IngameSystem>();
        yield return new WaitForSeconds(1);
        ServiceLocator.GetInstance<CutterMoveController>().OnCuttingFinish += TransitPhase;
    }

    private void TransitPhase(GameObject centerCucumber)
    {
        try
        {
            _ingameSystem.SetCucumberInstance(centerCucumber);
        }
        catch
        {
            Debug.Log("きゅうり送信失敗");
        }
    }
}
