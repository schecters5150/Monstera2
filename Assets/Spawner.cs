using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spawnedObject;
    private EnemyStatusModel enemyStatusModel;
    public float interval;

    private Timer intervalTimer;
    void Start()
    {
        intervalTimer = new Timer();
        enemyStatusModel = GetComponent<EnemyStatusModel>();
        intervalTimer.Trigger(interval);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!enemyStatusModel.isInLOS) return;
        
        intervalTimer.CalculateTime();
        if (intervalTimer.IsUp()){
            intervalTimer.Trigger(interval);
            Instantiate(spawnedObject, transform.position, transform.rotation);
        }
    }
}
