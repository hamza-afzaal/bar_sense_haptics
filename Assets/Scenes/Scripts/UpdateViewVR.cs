using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateViewVR : MonoBehaviour
{

    private Camera m_MainCamera;

    // Start is called before the first frame update
    void Start()
    {

        m_MainCamera = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        RectTransform transform = GetComponent<RectTransform>();
        print(transform.position.x);
    }
}
