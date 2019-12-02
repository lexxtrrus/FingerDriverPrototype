using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private int _points = 0;
    [SerializeField] RoadCreator _road;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(++_points + " " +other.transform.position);
        if(_points % 6 == 0)
        {
            _road.CreateNewTrack();
        }
    }
}
