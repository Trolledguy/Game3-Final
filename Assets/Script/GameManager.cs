using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PathfindingGrid pathfindingGrid;

    public Enemy enemyPrefab;
    

    private void Awake()
    {
        SetUp();

        StartCoroutine(SpawnEnemyRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(5f); // Spawn an enemy every 5 seconds
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab != null)
        {
            Vector3 spawnPosition = pathfindingGrid.GetWorldPosition(
                Random.Range(0, pathfindingGrid.gridWidth),
                Random.Range(0, pathfindingGrid.gridHeight)
            );
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void SetUp()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (pathfindingGrid == null)
        {
            pathfindingGrid = FindObjectOfType<PathfindingGrid>();
        }
    }

    public void EndGame(bool hasWon)
    {
        if(hasWon)
        {
            UIManager.instance.ShowWinScreen();
        }
        else
        {
            UIManager.instance.TogglePauseMenu(true);
        }
    }

}