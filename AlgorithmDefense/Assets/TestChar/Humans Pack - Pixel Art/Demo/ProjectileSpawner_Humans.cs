using UnityEngine;
using System.Collections;

public class ProjectileSpawner_Humans : MonoBehaviour {

    [SerializeField] GameObject projectile;

    private Transform projectile_SpawnPoint;

    void Start()
    {
        projectile_SpawnPoint = transform.Find("Projectile_SpawnPoint");
    }

    // This function is called as an event in the animations
    public void spawnProjectile()
    {
        if (projectile_SpawnPoint != null)
        {
            // Turn spawn point in correct direction
            int shouldFlip = transform.GetComponent<SpriteRenderer>().flipX ? -1 : 1;
            if (shouldFlip != Mathf.Sign(projectile_SpawnPoint.localPosition.x))
                projectile_SpawnPoint.localPosition = new Vector3(-1 * projectile_SpawnPoint.localPosition.x, projectile_SpawnPoint.localPosition.y, transform.localPosition.z);

            // Spawn projectile
            GameObject newProjectile = Instantiate(projectile, projectile_SpawnPoint.position, projectile_SpawnPoint.rotation) as GameObject;
            // Turn projectile in correct direction
            newProjectile.transform.localScale = new Vector3(shouldFlip * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            // Set Tag e.g. "Blue", "Red", "Yellow".
            newProjectile.tag = transform.tag;
        }
    }
}
