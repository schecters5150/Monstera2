using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserIndicator : MonoBehaviour
{
    public GameObject spawner;
    private SpriteRenderer sprite;
    private float maxSpawnTime;
    private float spawnTime;
    private Timer timer;
    // Start is called before the first frame update
    void Start()
    {
        maxSpawnTime = spawner.GetComponent<Spawner>().interval;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var grad = spawner.GetComponent<Spawner>().intervalTimer.GetTime() / maxSpawnTime;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1 - grad);
    }
}
