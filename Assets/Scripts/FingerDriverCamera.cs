using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerDriverCamera : MonoBehaviour
{
    [SerializeField] private Transform m_carTransform;
    private float camZ;
    // Start is called before the first frame update
    private void Start()
    {
        camZ = transform.position.z;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 pos = m_carTransform.position;
        pos.z = camZ;
        transform.position = pos;
    }
}
