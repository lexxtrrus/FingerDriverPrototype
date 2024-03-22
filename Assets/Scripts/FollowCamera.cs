using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform m_carTransform;
    private float camZ;
    
    private void Start()
    {
        camZ = transform.position.z;
    }

    private void Update()
    {
        var pos = m_carTransform.position;
        pos.z = camZ;
        transform.position = pos;
    }
}
