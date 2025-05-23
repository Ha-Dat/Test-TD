using UnityEngine;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [Header("Attribute")]
    [SerializeField] private Transform turretRotationPonit;
    [SerializeField] private LayerMask enemyMask;

    [Header("ATtribute")]
    [SerializeField] private float targetingRange = 3f;

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.white;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }

    private Transform target;

    private void findTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)
        transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private void RotateTowardTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x -
        transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPonit.rotation = targetRotation;
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            findTarget();
            return;
        }

        RotateTowardTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
    }
}
