using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSurfaceRoughness : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject siblingObject = transform.GetChild(i).gameObject;
            if (siblingObject.name == "Plane")
                continue;

            BarValue barValues = siblingObject.GetComponent<BarValue>();
            HapticSurface surface = siblingObject.GetComponent<HapticSurface>();
            surface.hlStiffness = 1;
            if (barValues.NORM_MEAN_HUMIDITY < 0.25f)
                surface.hlStaticFriction = Convert.ToSingle(0.0f);
            else if (barValues.NORM_MEAN_HUMIDITY < 0.5f)
                surface.hlStaticFriction = Convert.ToSingle(0.33f);
            else if (barValues.NORM_MEAN_HUMIDITY < 0.75f)
                surface.hlStaticFriction = Convert.ToSingle(0.66f);
            else
                surface.hlStaticFriction = Convert.ToSingle(0.99f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
