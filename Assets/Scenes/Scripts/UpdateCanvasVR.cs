using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCanvasVR : MonoBehaviour
{
    private Camera m_MainCamera;
    private Vector3 initialVect;
    private float initialAngleY;

    // Start is called before the first frame update
    void Start()
    {

        m_MainCamera = Camera.main;

        RectTransform transform = GetComponent<RectTransform>();
        initialVect = m_MainCamera.transform.position - transform.position;
        initialAngleY = transform.rotation.eulerAngles.y;

    }

    // Update is called once per frame
    void Update()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector3 computedVect = m_MainCamera.transform.position - rectTransform.position;

        float angle = Vector3.SignedAngle(computedVect, initialVect, Vector3.up);
        rectTransform.rotation = Quaternion.Euler(rectTransform.rotation.x, initialAngleY - angle, rectTransform.rotation.z);
    }
}
