using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
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
        GameManager.inst.actShield();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
