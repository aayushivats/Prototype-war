using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Enemy Prefab
    public GameObject enemyUnitPrefab;
    public GameObject groundPlane;

    // Start is called before the first frame update
    void Start()
    {
        //Generate Enemies on ground
        Bounds planeBounds = groundPlane.GetComponent<Collider>().bounds;
        Vector3 pos;
        pos.x = 0.5f * Random.Range(planeBounds.min.x, planeBounds.max.x); 
        pos.y = groundPlane.transform.position.y;
        pos.z = 0.5f * Random.Range(planeBounds.min.z, planeBounds.max.z);

        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                GameObject go = Instantiate(enemyUnitPrefab, pos + Vector3.forward * i + Vector3.right * j, Quaternion.identity);
                go.name = "EnemyUnit " + (i*3 + j);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
