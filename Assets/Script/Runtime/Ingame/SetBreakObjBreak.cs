using SymphonyFrameWork.System;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class SetBreakObjBreak : MonoBehaviour
{
    IngameSystem ingameSystem;
    TrajectionMovement trajectionMovment;
    Animator animator;
    async void Start()
    {
        await Awaitable.NextFrameAsync();
        ingameSystem = ServiceLocator.GetInstance<IngameSystem>();
        trajectionMovment =  ServiceLocator.GetInstance<TrajectionMovement>();
        animator =transform.GetComponent<Animator>();
        
    }

    bool onlyOnce;
    private void Update()
    {
        if(ingameSystem.Cucumber.transform.position.z >= transform.position.z && !onlyOnce) { onlyOnce = true; animator.SetTrigger("break"); trajectionMovment.SetCrashedName(transform.name); }
    }
}
