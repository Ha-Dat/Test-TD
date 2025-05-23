using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class RedTurret : MonoBehaviour
{
    [Header("Attribute")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float bps = 1f; // Bullets Per Second

    private List<Transform> targets = new List<Transform>();
    private float timeUntilFire;

    private void OnDrawGizmosSelected(){
        Handles.color = Color.white;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }

    void Update(){
        CleanUpTargets();

        if (targets.Count < 2){
            FindTargets();
        }

        if (targets.Count > 0){
            RotateTowardTarget(targets[0]);

            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / bps){
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private void Shoot(){
        foreach (Transform t in targets){
            if (t == null) continue;
            GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
            RedTurretBullet bulletScript = bulletObj.GetComponent<RedTurretBullet>();
            bulletScript.SetTarget(t);
        }
    }

    private void FindTargets(){
        targets.Clear();
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);
        for (int i = 0; i < hits.Length && targets.Count < 2; i++){
            targets.Add(hits[i].transform);
        }
    }

    private void RotateTowardTarget(Transform target){
        if (target == null) return;
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void CleanUpTargets(){
        targets.RemoveAll(t => t == null || Vector2.Distance(transform.position, t.position) > targetingRange);
    }
}
