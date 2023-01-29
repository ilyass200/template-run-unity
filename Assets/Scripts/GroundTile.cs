using UnityEngine;
using System.Collections.Generic;


public class GroundTile : MonoBehaviour {

    GroundSpawner groundSpawner;
    public Transform spawnPosition;

    [Header("Spawns")]
    [SerializeField] List<Transform> spawnObstacleSinglePositions;
    [SerializeField] List<Transform> spawnObstacleDoublePositions;
    [SerializeField] List<Transform> spawnObstacleTriplePositions;

    [SerializeField] List<Transform> spawnObstacleSingleUpPositions;
    [SerializeField] List<Transform> spawnObstacleDoubleUpPositions;
    [SerializeField] List<Transform> spawnObstacleTripleUpPositions;

    [SerializeField] List<Transform> spawnCoinsPositions;
    
    [Header("Coins")]
    [SerializeField] GameObject goldPrefab;
    [SerializeField] GameObject emeraldPrefab;
    [SerializeField] GameObject rubisPrefab;
    [SerializeField] GameObject diamantPrefab;
    [SerializeField] GameObject darkDiamantPrefab;
    
    [Header("Obstacles")]
    [SerializeField] GameObject obstacleSinglePrefab;
    [SerializeField] GameObject obstacleDoublePrefab;
    [SerializeField] GameObject obstacleTriplePrefab;

    private void Start () {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
	}

    private void OnTriggerExit (Collider other)
    {
        groundSpawner.SpawnTile(true);
        Destroy(gameObject, 2);
    }

    public void SpawnObstacle ()
    {
        int obstaclesToSpawn = 1;
        if(GlobalUI.instance.levelMode == 1) obstaclesToSpawn = 1; 
        else if(GlobalUI.instance.levelMode == 2) obstaclesToSpawn = 2;
        else if(GlobalUI.instance.levelMode == 3) obstaclesToSpawn = 3;
        else if(GlobalUI.instance.levelMode == 4) obstaclesToSpawn = 4;
        else obstaclesToSpawn = 5;

        for (int i = 0; i < obstaclesToSpawn; i++) {
            float randomPercent = getRandomPercent();
            if(randomPercent > 0.8f){ // 20%
                Transform spawnPoint;
                if(getRandomPercent() > 0.5){
                    int randPosIndex = Random.Range(0, spawnObstacleTriplePositions.Count);
                    spawnPoint = spawnObstacleTriplePositions[randPosIndex];
                    spawnObstacleTriplePositions.Remove(spawnObstacleTriplePositions[randPosIndex]);
                } else{
                    int randPosIndex = Random.Range(0, spawnObstacleTripleUpPositions.Count);
                    spawnPoint = spawnObstacleTripleUpPositions[randPosIndex];
                    spawnObstacleTripleUpPositions.Remove(spawnObstacleTripleUpPositions[randPosIndex]);
                }
                Instantiate(obstacleTriplePrefab, spawnPoint.position, Quaternion.identity, transform);
            } if(randomPercent > 0.6f){ // 40%
                Transform spawnPoint;
                if(getRandomPercent() > 0.5){
                    int randPosIndex = Random.Range(0, spawnObstacleDoublePositions.Count);
                    spawnPoint = spawnObstacleDoublePositions[randPosIndex];
                    spawnObstacleDoublePositions.Remove(spawnObstacleDoublePositions[randPosIndex]);
                } else{
                    int randPosIndex = Random.Range(0, spawnObstacleDoubleUpPositions.Count);
                    spawnPoint = spawnObstacleDoubleUpPositions[randPosIndex];
                    spawnObstacleDoubleUpPositions.Remove(spawnObstacleDoubleUpPositions[randPosIndex]);
                }
                Instantiate(obstacleDoublePrefab, spawnPoint.position, Quaternion.identity, transform);
            } else {
                Transform spawnPoint;
                if(getRandomPercent() > 0.5){
                    int randPosIndex = Random.Range(0, spawnObstacleSinglePositions.Count);
                    spawnPoint = spawnObstacleSinglePositions[randPosIndex];
                    spawnObstacleSinglePositions.Remove(spawnObstacleSinglePositions[randPosIndex]);
                } else{
                    int randPosIndex = Random.Range(0, spawnObstacleSingleUpPositions.Count);
                    spawnPoint = spawnObstacleSingleUpPositions[randPosIndex];
                    spawnObstacleSingleUpPositions.Remove(spawnObstacleSingleUpPositions[randPosIndex]);
                }
                Instantiate(obstacleSinglePrefab, spawnPoint.position, Quaternion.identity, transform);
            }
        }

    }


    float getRandomPercent(){ return Random.value; }


    public void SpawnCoins ()
    {
        int coinsToSpawn = 1;
        if(GlobalUI.instance.levelMode == 1) coinsToSpawn = Random.Range(1, 3);
        else if(GlobalUI.instance.levelMode == 2) coinsToSpawn = Random.Range(2, 5);
        else if(GlobalUI.instance.levelMode == 3) coinsToSpawn = Random.Range(3, 7);
        else if(GlobalUI.instance.levelMode == 4) coinsToSpawn = Random.Range(4, 9);
        else coinsToSpawn = Random.Range(6, 11);
        
        for (int i = 0; i < coinsToSpawn; i++) {
                float randomPercent = getRandomPercent();
                GameObject coinPrefab = null;

                if(randomPercent > 0.995f) {
                    float blackRandomPercent = getRandomPercent();
                    if(blackRandomPercent > 0.5f) coinPrefab = darkDiamantPrefab;
                    else coinPrefab = diamantPrefab;
                }
                else if(randomPercent > 0.95f) coinPrefab = diamantPrefab; // 5%
                else if(randomPercent > 0.76f) coinPrefab = emeraldPrefab; // 14%
                else if(randomPercent > 0.2f) coinPrefab = goldPrefab; // 20%

            if(coinPrefab == null) return;
            GameObject coinTmp = Instantiate(coinPrefab, transform);
            coinTmp.transform.Rotate(new Vector3(0, 90, 0));
            int randPosIndex = Random.Range(0, spawnCoinsPositions.Count);
            coinTmp.transform.position = spawnCoinsPositions[randPosIndex].position;
            spawnCoinsPositions.Remove(spawnCoinsPositions[randPosIndex]);
        }
    }

    Vector3 GetRandomPointInCollider (Collider collider)
    {
        Vector3 point = new Vector3(
            Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            Random.Range(collider.bounds.min.y, collider.bounds.max.y),
            Random.Range(collider.bounds.min.z, collider.bounds.max.z)
            );
        if (point != collider.ClosestPoint(point)) {
            point = GetRandomPointInCollider(collider);
        }

        point.y = 1;
        return point;
    }
}