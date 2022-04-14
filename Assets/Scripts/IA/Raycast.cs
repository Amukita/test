using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public Transform player;
   
    // Start is called before the first frame update
    void Start()
    {
        
        
    }
    void Update()
    {
        TestRaycast();
    }

    public void TestRaycast()
    {
        RaycastHit rayHit = new RaycastHit();
        Ray ray = new Ray(transform.position, player.position - transform.position);

        if (Physics.Raycast(ray, out rayHit, 1000))
        {
            if (rayHit.transform == player)
            {
                Debug.Log("El rayo golpeo al jugador");
            }
            else
            {
                Debug.Log("El rayo golpeo algo");
            }
        }
        else
        {
            Debug.Log("El rayo no golpeo nada");
        }

    }
}

   
    
