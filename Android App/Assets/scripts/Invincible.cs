using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincible : MonoBehaviour
{
    [SerializeField] float turnSpeed = 90f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Obstacle>() != null)
        {
            Destroy(gameObject);
            return;
        }
        if(other.gameObject.name != "Player")
            return;
        Destroy(gameObject);
        GameManager.inst.actInvincible();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, turnSpeed*Time.deltaTime, 0);
    }
}
