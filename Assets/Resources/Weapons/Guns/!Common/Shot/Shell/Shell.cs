using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public int shellSpeed;
    public int speedDeviation;
    public int shellTorque;
    void Start()
    {
        //gameObject.transform.parent = GameManager.Shells.transform;
        var shellRigidBody = gameObject.GetComponent<Rigidbody2D>();
        Vector3 upRandom = new Vector3(gameObject.transform.up.x, gameObject.transform.up.y) + new Vector3(Random.Range(-0.6f, 0.6f), Random.Range(-0.6f, 0.6f));
        shellRigidBody.AddForce(upRandom.normalized * -1 * (shellSpeed + Random.Range(-speedDeviation, speedDeviation)));
        shellRigidBody.AddTorque(shellTorque * RandomExtra.Sign());
    }
}
