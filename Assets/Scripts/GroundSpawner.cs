using UnityEngine;

public class GroundSpawner : MonoBehaviour {

    [SerializeField] GameObject groundTile;
    Vector3 nextSpawnPoint;

    public void SpawnTile (bool spawnItems)
    {
        GameObject tempTile = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
        GroundTile groundTileTmp = tempTile.GetComponent<GroundTile>();
        nextSpawnPoint = groundTileTmp.spawnPosition.position;

        if (spawnItems) {
            groundTileTmp.SpawnObstacle();
            groundTileTmp.SpawnCoins();
        }
    }

    private void Start () {
        for (int i = 0; i < 5; i++) {
            if (i < 3) {
                SpawnTile(false);
            } else {
                SpawnTile(true);
            }
        }
    }
}