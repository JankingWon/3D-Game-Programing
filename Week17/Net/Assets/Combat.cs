using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Combat : NetworkBehaviour
{
    public bool enemy_or_player;
    public const int maxHealth = 100;
    [SyncVar(hook = "OnchangeHealth")]
    public int health = maxHealth;
    public RectTransform healthbar;
    //public RectTransform healthbarText;
    private NetworkStartPosition[] spawnPoints;

    private void Start()
    {
        if (isLocalPlayer)
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
    }

    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        health -= amount;
        Debug.Log("health value = " + health.ToString());
        if (health <= 0)
        {
            Debug.Log("Dead!");
            if (enemy_or_player)
                DestroyObject(gameObject);
            else
            {
                health = maxHealth;
                RpcRespawn();
            }
        }
        
        //GameObject.Find("Canvas").GetComponent<Text>().text = health.ToString();
    }
    void OnchangeHealth(int health)
    {
       Debug.Log("change");
       healthbar.sizeDelta = new Vector2(health > 0 ? health : 0, healthbar.sizeDelta.y);
    //    gameObject.GetComponent<UnityEngine.UI.Text>().text = health.ToString();
    }
    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            // move back to zero location
            Vector3 spawnPoint = Vector3.zero;
            if(spawnPoint != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }
            transform.position = spawnPoint;
        }
    }
}