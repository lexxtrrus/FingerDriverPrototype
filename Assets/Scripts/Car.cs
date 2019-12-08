using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{

    [SerializeField] private RoadCreator m_track;
    [SerializeField] private InputRotation m_input;
    [SerializeField] private Transform m_trackingPoint;//точка по которой будет проверяться нахождение на трасе, вынесена в нос авто
    [SerializeField] private float m_carSpeed = 2f;
    [SerializeField] private float m_maxSteer = 90f;

    private void Start()
    {
        m_track.WhereIsTheCar(m_trackingPoint.position);
        //Debug.Log(m_track.currentTriangleNumber);
    }


    // Update is called once per frame
    private void Update()
    {
        if (m_track.IsPointInTrack(m_trackingPoint.position))
        {
            transform.Translate(transform.up * Time.deltaTime * m_carSpeed, Space.World);
            transform.Rotate(0f, 0f, m_maxSteer * m_input.SteerAxis * Time.deltaTime);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    
}
