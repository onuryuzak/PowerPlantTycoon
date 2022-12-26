using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class Tags
{
    static public string PLAYER = "Player";
    static public string COAL_AREA = "CoalArea";
    static public string RUNNER_EXIT = "CoalAreaExit";
    static public string COLLECTABES = "Collectables";
}
sealed public class Animations
{
    static public string Idle = "idle";
    static public string Mining = "mining";
    static public string Run = "run";
    static public string CarryRun = "carryRun";
    static public string CarryIdle = "carryIdle";


}

public enum CollectType
{
    Coal,
    Money,
    Wire
}
public enum StackingType
{
    Magnet
}
public enum PlaceType
{
    Home,
    PlugSocket
}

public enum ProductType
{
    Coal
}
public enum ProductIdType
{
    Coal_1
}

public enum PowerUpIdType
{
    Speed = 0,
    CoalPiece
}
public enum AIType
{
    Mining,
    Carry,
    Collector
}
public enum BuildingType
{
    Building_1 = 0,
    Building_2,
    Building_3,
    Building_4,
    Building_5,
    Building_6,
    Building_7,
}
public enum ProductState
{
    Raw, Middle, Final
}
