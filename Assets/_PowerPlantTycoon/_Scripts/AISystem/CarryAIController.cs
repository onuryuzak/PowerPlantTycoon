using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CarryAIController : MonoBehaviour
{
    public GameObject MagnetModelHolder;
    [SerializeField] private Transform _stackPoint;
    [SerializeField] private List<Transform> stackPoints;
    public MagnetStackController magnetStackController => _magnetStackController;
    private MagnetStackController _magnetStackController;
    private Animator _animator;
    public int StackIndexer = 0;
    public float YOffset = 0;

    //WayPoints

    private Transform targetWaypoint;
    private int targetWaypointIndex = 0;
    private float minDistance = 0.1f;
    private int lastWaypointIndex;
    public float movementSpeed = 5.0f;
    public float rotationSpeed = 2.0f;
    AIManager instance;

    bool waitAtLastPoint;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();

        instance = AIManager.instance;
        _magnetStackController = GetComponentInChildren<MagnetStackController>();
        GameManager.instance._carryAIController = this;
        lastWaypointIndex = instance.miningWaypoints.Count - 1;
        targetWaypoint = instance.miningWaypoints[targetWaypointIndex];
        if (GameManager.instance._miningAIController != null)
        {
            _animator.SetTrigger("CarryRun");
        }
        else
        {
            _animator.SetTrigger("CarryIdle");
        }
    }

    private void Update()
    {
        if (!waitAtLastPoint && GameManager.instance._miningAIController != null)
        {
            _animator.SetTrigger("CarryRun");
            float movementStep = movementSpeed * Time.deltaTime;
            float rotationStep = rotationSpeed * Time.deltaTime;

            Vector3 directionToTarget = targetWaypoint.position - transform.position;
            Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationStep);

            Debug.DrawRay(transform.position, transform.forward * 50f, Color.green, 0f); //Draws a ray forward in the direction the enemy is facing
            Debug.DrawRay(transform.position, directionToTarget, Color.red, 0f); //Draws a ray in the direction of the current target waypoint

            float distance = Vector3.Distance(transform.position, targetWaypoint.position);
            CheckDistanceToWaypoint(distance);

            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementStep);
        }

    }

    public void SetInitPosition(Vector3 initPos)
    {
        transform.position = initPos;
    }
    void CheckDistanceToWaypoint(float currentDistance)
    {
        if (currentDistance <= minDistance)
        {
            targetWaypointIndex++;
            UpdateTargetWaypoint();
        }
    }

    void UpdateTargetWaypoint()
    {

        if (targetWaypointIndex > lastWaypointIndex)
        {
            //_animator.SetTrigger("CarryIdle");
            waitAtLastPoint = true;
            targetWaypointIndex = 0;
            StartCoroutine(WaitForFirstPoint());
            _animator.SetTrigger("CarryIdle");

        }
        targetWaypoint.position = instance.miningWaypoints[targetWaypointIndex].position + new Vector3(Random.Range(0, 2), 0, Random.Range(0, 2));


    }

    IEnumerator WaitForFirstPoint()
    {
        yield return new WaitForSeconds(0.5f);
        waitAtLastPoint = false;
        _animator.SetTrigger("CarryRun");
    }


    public void collectMe(CollectableItem item, StackingType stackingType)
    {
        if (item.collectType == CollectType.Coal)
        {
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
    }
}
