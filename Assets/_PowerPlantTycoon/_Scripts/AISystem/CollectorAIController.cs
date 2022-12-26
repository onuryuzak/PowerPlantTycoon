using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorAIController : MonoBehaviour
{
    private Animator _animator;

    //Waypoints
    private Transform targetWaypoint;
    private int targetWaypointIndex = 0;
    private float minDistance = 0.1f;
    private int lastWaypointIndex;
    public float movementSpeed = 5.0f;
    public float rotationSpeed = 2.0f;
    AIManager instance;
    FactoryController factoryController;
    bool waitForMoney;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();

        factoryController = FindObjectOfType<FactoryController>();
        instance = AIManager.instance;
        // lastWaypointIndex = instance.collectorWaypoints.Count - 1;
        // targetWaypoint = instance.collectorWaypoints[targetWaypointIndex];
    }

    private void Update()
    {
        if (factoryController.WireConnectedSomeBuilding && factoryController.factoryEnergy > 0)
        {
            waitForMoney = true;
        }
        else
        {
            waitForMoney = false;
        }
        if (waitForMoney)
        {
            _animator.SetTrigger("Run");
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

    public void collectMe(CollectableItem item, StackingType stackingType)
    {
        if (item.collectType == CollectType.Money)
        {
            item.onCollected(Vector3.up, transform, 1, () =>
            {
                InventoryManager.instance.addMoney(((MoneyItem)item).moneyValue);
                Destroy(item.gameObject);
            });
        }
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

            targetWaypointIndex = 0;

        }
        // targetWaypoint.position = instance.collectorWaypoints[targetWaypointIndex].position + new Vector3(Random.Range(0, 1), 0, Random.Range(0, 1));


    }
    public void SetInitPosition(Vector3 initPos)
    {
        transform.position = initPos;
    }
}
