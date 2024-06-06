using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Faca : MonoBehaviour
{
    //object position
    public Vector3 objectPosition;
    public GameObject cutObject;

    public GameObject corteUi;
    public GameObject panelaUi;

    SpawnObject spawn;

    private void Start()
    {
        spawn = GameObject.Find("SpawnerManager").GetComponent<SpawnObject>();
    }

    private void Update()
    {
        objectPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Faca"))
        {
            Instantiate(cutObject, objectPosition, Quaternion.identity);
            // destroy the object
            Destroy(gameObject);

            if (gameObject.CompareTag("Cebola"))
            {
                if (!spawn.cebolacorte)
                {
                    corteUi.SetActive(false);
                    panelaUi.SetActive(true);
                    spawn.cebolacorte = true;
                }
            }

            if (gameObject.CompareTag("Alho"))
            {
                if (spawn.alhopronto)
                {
                    if (!spawn.alhocorte)
                    {
                        corteUi.SetActive(false);
                        panelaUi.SetActive(true);
                        spawn.alhocorte = true;

                    }
                }
            }        
        }
    }
}
