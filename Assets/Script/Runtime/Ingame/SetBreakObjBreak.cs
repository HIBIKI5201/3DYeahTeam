﻿using SymphonyFrameWork.System;
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
        getReady = true;
        
    }
    bool getReady;
    bool onlyOnce;
    private void Update()
    {
        if (getReady && ingameSystem.Cucumber.transform.position.z >= transform.position.z && !onlyOnce)
        {
            onlyOnce = true; animator.SetTrigger("break");
            trajectionMovment.SetCrashedName(transform.name);
            trajectionMovment.PlayExplosionSound(9);
        }
    }
}
