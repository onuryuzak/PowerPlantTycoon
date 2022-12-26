using System.Collections;
using System.Collections.Generic;
using Artera;
using UnityEngine;

public class GameManager : BaseSingleton<GameManager>
{
    [SerializeField] Character _player;
    public CarryAIController _carryAIController;
    public MiningAIController _miningAIController;
    [SerializeField] LevelManager _levelManager;
    [SerializeField] LevelGenerator _levelGenerator;

    #region BASE
    void Start()
    {
        //initialize();
    }
    #endregion

    #region METHODS
    void initialize()
    {
        _levelGenerator.initialize();
        _levelManager.initialize();

        LevelPrefabSO levelSO = _levelManager.currentLevel;
        loadLevel(levelSO);
    }

    void loadLevel(LevelPrefabSO levelSO)
    {
        Level level = _levelGenerator.loadLevel(levelSO);

        EventManager.OnLevelLoaded();
    }

    public void nextLevel()
    {
        // TODO:
        LevelPrefabSO nextLevel = _levelManager.nextLevel();
        loadLevel(nextLevel);
    }

    public void retryLevel()
    {
        // TODO:
        LevelPrefabSO currentLevel = _levelManager.currentLevel;
        loadLevel(currentLevel);
    }
    #endregion

    #region INTERFACE
    #endregion

    #region ACTIONS
    #endregion

    #region COROUTINE
    #endregion

    #region HELPER
    public Character player => _player;
    #endregion
}
