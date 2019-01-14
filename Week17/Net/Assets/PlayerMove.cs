using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour
{
    public float rotSpeed = 150.0f;
    public float movSpeed = 3.0f;
    public RectTransform healthbar;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    private float x, z;

    void Update()
    {
        if (!isLocalPlayer)
            return;

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * rotSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime* movSpeed;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
        transform.Translate(x, 0, z);
        //transform.Rotate(0, 0, z);
    }
    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }
    [Command]
    void CmdFire()
    {
        // create the bullet object from the bullet prefab
        var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // make the bullet move away in front of the player
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        NetworkServer.Spawn(bullet);
        // make bullet disappear after 2 seconds
        Destroy(bullet, 2.0f);
    }
}