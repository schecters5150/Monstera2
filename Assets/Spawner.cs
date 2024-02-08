using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spawnedObject;
    public GameObject spawningObject;
    private EnemyStatusModel enemyStatusModel;
    public float interval;
    public float phase;

    public Timer phaseTimer;
    public Timer intervalTimer;
    void Start()
    {
        intervalTimer = new Timer();
        phaseTimer = new Timer();
        enemyStatusModel = spawningObject.GetComponent<EnemyStatusModel>();
        if(phase == 0) intervalTimer.Trigger(interval);
        else phaseTimer.Trigger(phase * interval);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!enemyStatusModel.isInLOS) return;
         
        if (intervalTimer.IsUp() && phaseTimer.IsUp()){
            intervalTimer.Trigger(interval);
            var newObject = Instantiate(spawnedObject, transform.position, transform.rotation) as GameObject;
            newObject.transform.rotation = spawningObject.transform.Find("Physics and Hit detection").transform.rotation;
        }

        intervalTimer.CalculateTime();
        phaseTimer.CalculateTime();
    }
}
