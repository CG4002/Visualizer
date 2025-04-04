using UnityEngine;
using System.Collections;

public class ProjectileManager : MonoBehaviour
{
    private GameState gameState;
    public ProjectileLauncher projectileLauncher;
    public PlayerBombHandler bombHandler;

    void Start()
    {
        gameState = GameState.Instance;
        gameState.gameActionOccurred.AddListener(HandleProjectile);
        Debug.Log("ProjectileManager: ProjectileManager Ready");
        if (projectileLauncher == null)
        {
            Debug.LogError("ProjectileManager: ProjectileLauncher is not assigned. Please assign it in the Inspector.");
        }

        if (projectileLauncher == null)
        {
            Debug.LogError("ProjectileManager: ProjectileLauncher is not assigned. Please assign it in the Inspector.");
        }

        if (bombHandler == null)
        {
            bombHandler = GetComponent<PlayerBombHandler>();
            if (bombHandler == null)
            {
                Debug.LogError("ProjectileManager: PlayerBombHandler is not assigned or found. Please assign it in the Inspector.");
            }
        }
    }

    void OnDestroy()
    {
        if(gameState != null)
        {
            gameState.gameActionOccurred.RemoveListener(HandleProjectile);
        }
    }

    public void HandleProjectile(string actionType)
    {
        // Doesn't matter if enemy is active or not, just animate
        if (actionType == "golf")
        {
            if (projectileLauncher != null)
            {
                Debug.Log("ProjectileManager: Preparing to fire Golf");
                projectileLauncher.FireProjectile("Golf");
                Debug.Log("ProjectileManager: Fired Golf Projectile");
            }
        }
        else if (actionType == "badminton")
        {
            if (projectileLauncher != null)
            {
                projectileLauncher.FireProjectile("Badminton");
                Debug.Log("ProjectileManager: Fired Badminton Projectile");
            }
        }
        else if (actionType == "bomb")
        {
            if (projectileLauncher != null && gameState.PlayerBombCount > 0 && bombHandler != null)
            {
                projectileLauncher.FireProjectile("Bomb");
                Debug.Log("ProjectileManager: Fired Bomb Projectile");
                if (gameState.EnemyActive)
                {
                    StartCoroutine(DelayBombEffect());
                    Debug.Log("ProjectileManager: Spawned snow cloud on enemy");
                }
            }
            else
            {
                Debug.Log("ProjectileManager: Cannot fire bomb - no bombs available, no active enemy, or missing bomb handler");
            }
        }
        else
        {
            Debug.LogWarning($"ProjectileManager: Unhandled actionType '{actionType}' or no enemy active.");
        }
    }

    private IEnumerator DelayBombEffect()
    {
        // Estimate time for projectile to reach target - adjust this value based on testing
        float estimatedTimeToReachTarget = 2f;

        Debug.Log($"ProjectileManager: Waiting {estimatedTimeToReachTarget} seconds for projectile to reach target");
        yield return new WaitForSeconds(estimatedTimeToReachTarget);

        // Now spawn the bomb effect on the enemy
        bombHandler.SpawnBombOnEnemy();
        Debug.Log("ProjectileManager: Spawned snow cloud on enemy");
    }
}
