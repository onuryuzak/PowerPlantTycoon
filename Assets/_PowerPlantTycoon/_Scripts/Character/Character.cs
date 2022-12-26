using Artera;
using Artera.AI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;
using UnityEngine.UI;

public interface IController
{
    void stop();
}

public interface IActivatable
{
    void activate();
    void deactivate();
    bool isActive { get; }
}
[RequireComponent(typeof(IController))]
public class Character : MonoBehaviour
{
    // Components
    [HideInInspector] public FSM _fsm;
    private Animator _animator;

    [SerializeField] private DynamicJoystick _joystick;
    [SerializeField] private PlayerPropertiesSO _playerSO;
    [SerializeField] private Transform _stackPoint;
    [SerializeField] private List<Transform> stackPoints;
    public Image radialCountdown;
    public PlayerPowerUpSO PlayerPowerUpSO;
    public float playerSpeed => PlayerPowerUpSO.PlayerSpeed;
    public int coalMiningCount => PlayerPowerUpSO.CoalMiningCount;
    public float rotationSpeed => _playerSO.rotationSpeed;


    public GameObject HoseHead;
    public Transform WireCarryTransform;
    public GameObject MagnetModelHolder;
    public GameObject PickAxeModelHolder;
    public DynamicJoystick joystick => _joystick;
    public CharacterController characterController;
    public bool anyMovement => _joystick.Direction != Vector2.zero;
    public bool inMiningArea;
    public bool WireOnHand;
    public MagnetStackController magnetStackController => _magnetStackController;
    private ElectricWireItem _currentElectricWireItem;

    private MagnetStackController _magnetStackController;
    private CharacterAnimationEvent characterAnimationEvent;
    public int StackIndexer = 0;
    public float YOffset = 0;


    public CollectType currentCollectType { get; set; }

    #region BASE
    private void Awake()
    {
        SetRadialCountDownActive(false);
        HoseHead.SetActive(false);
        PickAxeModelHolder.SetActive(false);
        _animator = GetComponent<Animator>();
        characterAnimationEvent = GetComponentInChildren<CharacterAnimationEvent>();
        _magnetStackController = GetComponentInChildren<MagnetStackController>();
        _fsm = GetComponent<FSM>();
        initializeFSM();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InventoryManager.instance.addMoney(100);
        }
    }
    #endregion

    #region METHODS

    public void SetRadialCountDownActive(bool state)
    {
        radialCountdown.enabled = state;
    }
    
    public void collectMe(CollectableItem item, StackingType stackingType)
    {
        if (_fsm.activeState == _fsm.bringState((int)CharacterState.Mining)) return;
        if (item.collectType == CollectType.Money)
        {
            item.onCollected(Vector3.up, transform, 1, () =>
            {
                InventoryManager.instance.addMoney(((MoneyItem)item).moneyValue);
                Destroy(item.gameObject);
            });
        }
        if (item.collectType == CollectType.Coal)
        {
            currentCollectType = item.collectType;
            Transform collectParent = _stackPoint;
            if (StackIndexer <= stackPoints.Count)
            {
                Vector3 targetPos = new Vector3(stackPoints[StackIndexer].localPosition.x, stackPoints[StackIndexer].localPosition.y + YOffset, stackPoints[StackIndexer].localPosition.z);
                Quaternion targetRot = Quaternion.Euler(new Vector3(-90, 90, UnityEngine.Random.Range(0, 360)));
                item.transform.rotation = targetRot;
                item.onCollected(targetPos, collectParent, 1, () =>
                {
                    item.stackingType = stackingType;
                    _magnetStackController.addItemToList(item.GetComponent<CoalItem>());
                });
                StackIndexer++;
                if (StackIndexer == stackPoints.Count)
                {
                    StackIndexer = 0;
                    YOffset += 0.2f;
                }
            }
        }
        if (item.collectType == CollectType.Wire)
        {
            Transform collectParent = WireCarryTransform;
            item.onCollected(Vector3.zero, collectParent, 1, () =>
            {

            });
        }
    }
    public void spendMoneyEffect(Vector3 target, int countAtSameTime = 1, Action<Transform> onMoneyArrivedListener = null)
    {
        MoneyItem item = ObjectCreator.createMoneyCollecatble();
        item.transform.position = transform.position + Vector3.up * 2;
        item.GetComponent<BoxCollider>().enabled = false;
        item.enabled = false;
        item.transform.DOJump(target, 2, 1, .5f).OnComplete(() =>
        {
            onMoneyArrivedListener?.Invoke(item.transform);
        });
    }
    public void releaseMe(Transform plugTransform)
    {
        _currentElectricWireItem.onRelease(plugTransform);
    }
    public void SetCurrentWire(ElectricWireItem currentWire)
    {
        _currentElectricWireItem = currentWire;
    }

    void initializeFSM()
    {
        if (_fsm.isInitialized)
        {
            _fsm.switchState((int)CharacterState.Idle);
        }
        else
        {
            _fsm.onInitilized = () =>
            {
                _fsm.switchState((int)CharacterState.Idle);
                _fsm.onInitilized = null;
            };
        }
    }
    //public bool checkNextStepIsGround(Vector3 dir)
    //{
    //    Vector3 startPosition = transform.position + Vector3.up + dir * 2;
    //    Ray ray = new Ray(startPosition, -transform.up);
    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit, 10))
    //    {
    //        if (hit.transform.CompareTag("Water"))
    //        {
    //            return false;
    //        }
    //    }

    //    return true;
    //}

    void onMiningArea(Transform miningAreaPos)
    {
        if (!WireOnHand)
        {
            characterAnimationEvent.GetPileOfCoalPosition(miningAreaPos);
            _fsm.switchState((int)CharacterState.Mining);
            inMiningArea = true;
        }


    }
    private void OnEnable()
    {
        EventManager.MiningAreaEnter += onMiningArea;
    }
    private void OnDisable()
    {
        EventManager.MiningAreaEnter -= onMiningArea;
    }
}
#endregion
