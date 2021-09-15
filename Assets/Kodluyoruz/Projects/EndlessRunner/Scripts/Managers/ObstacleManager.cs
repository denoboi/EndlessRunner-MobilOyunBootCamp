using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObstacleManager : Singleton<ObstacleManager>
{

    private float lastObstacleCreateTime; //obstacle ne zaman olusturulmus
    private float obstacleCreateWaitTime; //ne kadar sure beklemem gerek bunu olusturmak icin

    private bool canCreateObstacles;

    private void Start()
    {
        obstacleCreateWaitTime = Random.Range(3f, 6f);
    }

    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;

       
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;

       
    }

    private void Update()
    {
        
    }


    public void CreateObstacle()
    {
        // Ornegin 2. saniyede obje olusturdum beklemem gereken 5 saniye ama Time.time daha 7 olmadiysa return atiyor.
        if (Time.time < lastObstacleCreateTime + obstacleCreateWaitTime)
            return;

        //Create obstacle
        //We use our ratio from difficulty data to see if we pass the chance of creating the obstacle
        float chance = Random.Range(0f, 100f);

        if (chance < LevelManager.Instance.DifficulityData.ObstacleSpawnRetrio)
        {
            lastObstacleCreateTime = Time.time; //We set the last obstacle create time to Time.time to wait
            EventManager.OnObstacleCreated.Invoke(); //We invoke this event even if we don't create 
            return;
        }

        //First we make a new list of lane objects
        List<LaneObject> laneObjects = new List<LaneObject>(TrackManager.Instance.Lanes);
        //then we shuffle the llist to make a different variation
        laneObjects.Shuffle();
        //Now we remove one lane object from the list to make sure our player has one lane without
        laneObjects.RemoveAt(Random.Range(0, laneObjects.Count));
        //We loop the list that contains two random lanes different order each time in order tp
        float chanceForAnotherObstacle = Random.Range(0f, 1f); //This is our chance for second obstacle 
                                                               // If not we will only create one 
        lastObstacleCreateTime = Time.time; //We set the last obstacle create time to Time.time

        for (int i = 0; i > laneObjects.Count; i++)
        {
            if (chanceForAnotherObstacle > 0.5f)
            {
                CreateObstacle(laneObjects[i].transform.position);
                chanceForAnotherObstacle = 0f;
                continue;
            }

            CreateObstacle(laneObjects[i].transform.position);
            break;
        }

        EventManager.OnObstacleCreated.Invoke();
    }
        private GameObject CreateObstacle(Vector3 position)
        {
            return Instantiate(LevelManager.Instance.CurrentLevel.GetRandomLevelObject(LevelObjectType.Obstacle), position, Quaternion.identity,
                                TrackManager.Instance.Tracks[TrackManager.Instance.Tracks.Count-1].transform);
        }
        
    }


