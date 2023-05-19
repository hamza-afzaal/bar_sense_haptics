using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrabberCollisionUpdate : MonoBehaviour
{

    public GameObject starterTextPanel = null;
    public GameObject barValuePanel = null;

    public GameObject tempComp = null;
    public GameObject dateComp = null;
    public GameObject precipComp = null;
    public GameObject humidityComp = null;
    public GameObject minTempComp = null;
    public GameObject maxTempComp = null;
    public enum FILTER {TEMP, PRECIPT, HUMID};
    public FILTER filterValue = FILTER.TEMP;

    // Start is called before the first frame update
    void Start()
    {
        if (barValuePanel == null || starterTextPanel == null)
        {
            Debug.Log("Panels not found!");
            return;
        }

        barValuePanel.SetActive(false);
        starterTextPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            filterValue = FILTER.TEMP;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            filterValue = FILTER.PRECIPT;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            filterValue = FILTER.HUMID;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        BarValue collidedBarValues = collision.gameObject.GetComponent<BarValue>();
        if (collidedBarValues == null || starterTextPanel == null || barValuePanel == null ||
            tempComp == null || dateComp == null || precipComp == null ||
            humidityComp == null || minTempComp == null || maxTempComp == null)
            return;


        tempComp.GetComponent<TextMeshProUGUI>().text = collidedBarValues.MEAN_TEMPERATURE + " C";
        dateComp.GetComponent<TextMeshProUGUI>().text = collidedBarValues.LOCAL_DATE_FORMATTED;
        precipComp.GetComponent<TextMeshProUGUI>().text = collidedBarValues.TOTAL_PRECIPITATION + "%";
        humidityComp.GetComponent<TextMeshProUGUI>().text = collidedBarValues.MEAN_HUMIDITY + "%";
        minTempComp.GetComponent<TextMeshProUGUI>().text = collidedBarValues.MIN_TEMP + " C";
        maxTempComp.GetComponent<TextMeshProUGUI>().text = collidedBarValues.MAX_TEMP + " C";


        barValuePanel.SetActive(true);
        starterTextPanel.SetActive(false);

    }

    private void OnCollisionStay(Collision collision)
    {

        barValuePanel.SetActive(true);
        starterTextPanel.SetActive(false);


        HapticGrabber grabber = GetComponent<HapticGrabber>();
        if (grabber == null)
        {
            return;
        }

        GameObject thatObject = collision.gameObject;

        for (int i = 0; i < thatObject.transform.parent.childCount; i++)
        {
            GameObject siblingObject = thatObject.transform.parent.GetChild(i).gameObject;
            if (siblingObject.name == "Plane")
                continue;

            switch (filterValue)
            {
                case FILTER.TEMP:
                    Vector3 siblingMeshCenter = siblingObject.GetComponent<MeshFilter>().mesh.bounds.center;
                    Vector3 thatMeshCenter = thatObject.GetComponent<MeshFilter>().mesh.bounds.center;
                    if (grabber.isButtonOne())
                    {
                        if (thatMeshCenter.y < siblingMeshCenter.y)
                            siblingObject.SetActive(false);
                    }
                    else if (grabber.isButtonTwo())
                    {
                        if (thatMeshCenter.y > siblingMeshCenter.y)
                            siblingObject.SetActive(false);
                    }
                    else
                        siblingObject.SetActive(true);
                    break;
                case FILTER.HUMID:
                    float siblingHumidity = siblingObject.GetComponent<BarValue>().NORM_MEAN_HUMIDITY;
                    float thatMeshHumidity = thatObject.GetComponent<BarValue>().NORM_MEAN_HUMIDITY;
                    if (grabber.isButtonOne())
                    {
                        if (thatMeshHumidity < siblingHumidity)
                            siblingObject.SetActive(false);
                    }
                    else if (grabber.isButtonTwo())
                    {
                        if (thatMeshHumidity > siblingHumidity)
                            siblingObject.SetActive(false);
                    }
                    else
                        siblingObject.SetActive(true);
                    break;
                case FILTER.PRECIPT:
                    float siblingPrecipt = siblingObject.GetComponent<BarValue>().NORM_TOTAL_PRECIPITATION;
                    float thatMeshPrecipt = thatObject.GetComponent<BarValue>().NORM_TOTAL_PRECIPITATION;
                    if (grabber.isButtonOne())
                    {
                        if (thatMeshPrecipt < siblingPrecipt)
                            siblingObject.SetActive(false);
                    }
                    else if (grabber.isButtonTwo())
                    {
                        if (thatMeshPrecipt > siblingPrecipt)
                            siblingObject.SetActive(false);
                    }
                    else
                        siblingObject.SetActive(true);
                    break;
            }

        }


    }

    private void OnCollisionExit(Collision collision)
    {
        barValuePanel.SetActive(false);
        starterTextPanel.SetActive(true);

        GameObject thatObject = collision.gameObject;
        for (int i = 0; i < thatObject.transform.parent.childCount; i++)
        {
            GameObject siblingObject = thatObject.transform.parent.GetChild(i).gameObject;
            if (siblingObject.name == "Plane")
                continue;

            siblingObject.SetActive(true);

        }
    }
}
