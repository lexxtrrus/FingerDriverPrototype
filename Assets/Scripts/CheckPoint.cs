using TMPro;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private int _points = 0;
    int start = 1;
    [SerializeField] RoadCreator _road;
    [SerializeField] TextMeshProUGUI _score;

    private void OnTriggerEnter(Collider other)
    {
        _points++;
        _score.text = "POINTS: " + _points;
        if (_points - 5 != start) return;
        _road.CreateNewTrack();
        start = _points;
    }
}
