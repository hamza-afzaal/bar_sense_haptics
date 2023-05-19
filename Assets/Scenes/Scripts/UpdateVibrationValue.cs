using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateVibrationValue : MonoBehaviour
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
            HapticEffect effect = siblingObject.GetComponent<HapticEffect>();
            effect.effectType = HapticEffect.EFFECT_TYPE.VIBRATE;
            effect.Magnitude = 0.5f;
            effect.Frequency = 200.0f;
            effect.Direction = new Vector3(0.7f, 0.7f, 0.7f);
            double gain_value = ((barValues.NORM_TOTAL_PRECIPITATION - 0.5) / 0.5) * (1 - 0.25) + 0.25;

            if (gain_value <= 0.25f)
                effect.Gain = 0.0f;
            else if (gain_value <= 0.5f)
                effect.Gain = 0.33f;
            else if (gain_value <= 0.75f)
                effect.Gain = 0.66f;
            else
                effect.Gain = 0.99f;

            //if (barValues.NORM_TOTAL_PRECIPITATION > 0.5f)
            //{
            //    effect.Gain = 
            //}
            //else
            //{
            //    effect.Gain = 0.0f;
            //}
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
