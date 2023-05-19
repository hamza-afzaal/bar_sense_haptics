using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


public class CameraRotateVR : MonoBehaviour
{
    public GameObject[] hapticDevices = { null, null };
    public GameObject turnTable = null;
    public float threshold = 0.1f;
    public float speed = 100f;
    public bool disableSpinner = false;

    public GameObject leftIcon = null;
    public GameObject rightIcon = null;

    public Color standardColor = Color.grey;
    public Color usedColor = Color.red;

    public GameObject cam = null;

    public float pThresholdX = 225.0f;


    void Start()
    {
        if (turnTable == null)
        {
            Debug.LogError("The 'TurnTable' object must point to the parent of the Camera and the Haptic.");
            return;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (turnTable == null) return;


        // Keypress for confused users
        if (Input.GetKeyDown("c"))
        {
            disableSpinner = !disableSpinner;
        }

        // Reset color of icons
        if (leftIcon != null)
            leftIcon.GetComponent<RawImage>().color = standardColor;

        if (rightIcon != null)
            rightIcon.GetComponent<RawImage>().color = standardColor;



        bool isDisabled = disableSpinner;

        // For each haptic device.
        for (int ii = 0; ii < hapticDevices.Length; ii++)
        {
            GameObject hapticDevice = hapticDevices[ii];
            if (hapticDevice == null)
                continue;
            if (hapticDevice.GetComponent<HapticPlugin>() == null)
            {
                Assert.IsNotNull(hapticDevice.GetComponent<HapticPlugin>());
                continue;
            }

            // Don't rotate if we're picking something up.  
            // (This seems like it might be useful, but in practice, it's just confusing.)
            HapticGrabber grabber = hapticDevice.GetComponent<HapticPlugin>().hapticManipulator.GetComponent<HapticGrabber>();
            if (grabber != null && grabber.isGrabbing())
            {
                isDisabled = true;
            }

            HapticPlugin pointerHaptic = hapticDevice.GetComponent<HapticPlugin>();
            
            float newT = (pThresholdX / 2.0f) - pThresholdX * threshold;

            float newX = pointerHaptic.proxyPositionRaw.x;

            // If the X value is greater than the threshold, rotate the "turnTable" object.
            if (Mathf.Abs(newX) > newT && !isDisabled)
            {
                float mag = Mathf.Min(Mathf.Abs(newX), 10 * pThresholdX * 2 * threshold);
                float delta = (mag - newT) * (Mathf.Abs(newX) / newX);
                delta /= pThresholdX * 2;

                turnTable.transform.Rotate(0, -speed * delta * Time.deltaTime, 0);

                // Light up the icon
                if (leftIcon != null && newX < 0)
                    leftIcon.GetComponent<RawImage>().color = usedColor;

                if (rightIcon != null && newX > 0)
                    rightIcon.GetComponent<RawImage>().color = usedColor;
            }
        }


        // If rotation is disabled, for any reason, hide the icons.
        if (leftIcon != null)
            leftIcon.SetActive(!isDisabled);

        if (rightIcon != null)
            rightIcon.SetActive(!isDisabled);

    }
}
