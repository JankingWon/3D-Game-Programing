using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour {

    public GameObject enemyPrefab;
    public int numEnemies;

    public override void OnStartServer()
    {
        for (int i = 0; i < numEnemies; i++)
        {
            
            float x = Random.Range(-8.0f, 8.0f);
            float z = Random.Range(0, 1);
            var pos = new Vector3(x, 0f, z);

            var rotation = Quaternion.Euler( 0f, z, 0f);

            var enemy = (GameObject)Instantiate(enemyPrefab, pos, rotation);
            NetworkServer.Spawn(enemy);
        }
    }
}