using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    
    public delegate void OnLevelLoadedDelegate();
    public static event OnLevelLoadedDelegate LevelLoaded;

    public delegate void OnLevelStartedDelegate();
    public static event OnLevelStartedDelegate LevelStarted;

    public delegate void OnMiningAreaEnterDelegate(Transform miningAreaPos);
    public static event OnMiningAreaEnterDelegate MiningAreaEnter;

    public delegate void OnMiningAreaExitDelegate();
    public static event OnMiningAreaExitDelegate MiningAreaExit;

    public delegate void OnElectricSocketAreaExitDelegate();
    public static event OnElectricSocketAreaExitDelegate ElectricSocketAreaExit;



    public static void OnLevelLoaded()
    {
        LevelLoaded?.Invoke();
    }
    public static void OnLevelStarted()
    {
        LevelStarted?.Invoke();
    }

    public static void OnMiningAreaEnter(Transform miningAreaPos)
    {
        MiningAreaEnter?.Invoke(miningAreaPos);
    }
    public static void OnMiningAreaExit()
    {
        MiningAreaExit?.Invoke();
    }
    public static void OnElectricSocketAreaExit()
    {
        ElectricSocketAreaExit?.Invoke();
    }


}
