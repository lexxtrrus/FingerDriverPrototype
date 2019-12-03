using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour
{
    private int _points = 0;
    int start = 1;
    [SerializeField] RoadCreator _road;
    [SerializeField] Text _score;

    private void OnTriggerEnter(Collider other)
    {
        _points++;
        _score.text = "POINTS: " + _points;
        if(_points - 5 == start)
        {
            _road.CreateNewTrack();
            start = _points;
        }
    }
}
