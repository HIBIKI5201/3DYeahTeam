using SymphonyFrameWork.System;
using UnityEngine;

public class TransitNextPhase : MonoBehaviour
{
    IngameSystem _ingameSystem;
    private System.Collections.IEnumerator Start()
    {
        _ingameSystem = ServiceLocator.GetInstance<IngameSystem>();
        yield return new WaitForSeconds(1);
        //ServiceLocator.GetInstance<CutterMoveController>().OnCuttingFinish += TransitPhase;
        ServiceLocator.GetInstance<Phase1CutTest>().OnCuttingFinish += TransitPhase;
    }

    private void TransitPhase(GameObject centerCucumber)
    {
        try
        {
            _ingameSystem.SetCucumberInstance(centerCucumber);
            _ingameSystem.NextPhaseEvent();
        }
        catch
        {
            Debug.Log("きゅうり送信失敗");
        }
    }
}
