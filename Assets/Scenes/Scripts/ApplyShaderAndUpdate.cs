using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyShaderAndUpdate : MonoBehaviour
{

    public float METALLIC_FACTOR = 1.0f;
    public string FILE_NAME = "processed_data";

    // Start is called before the first frame update
    void Start()
    {
        List<Dictionary<string, object>> data = CSVReader.Read(FILE_NAME);
        data.ForEach(x =>
        {
            string convertedString = Convert.ToDateTime(x["LOCAL_DATE"]).ToString("yyyy-M-d");
            x["LOCAL_DATE"] = convertedString;
        });

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject siblingObject = transform.GetChild(i).gameObject;
            if (siblingObject.name == "Plane")
                continue;  

            string name_split = siblingObject.name.Split('_')[1];
            Dictionary<string, object> selected = data.Find(entry =>
            {
                return name_split == (string)entry["LOCAL_DATE"];
            });
            BarValue appiedBarValues = ApplyBarValues(siblingObject, selected);
            
            Renderer matRenderer = siblingObject.GetComponent<Renderer>();
            Material specMaterial = new Material(Shader.Find("Autodesk Interactive"));
            if (((string)selected["FROST_TEMP"]).ToUpper() == "TRUE")
            {
                //specMaterial.SetColor("_Color", new Color(0, 0.36f, 1));
                //specMaterial.SetColor("_Color", new Color(0.53f, 0.68f, 0.88f));
                specMaterial.SetColor("_Color", new Color(0.541f, 0.882f, 0.976f));
                if (appiedBarValues.NORM_TOTAL_PRECIPITATION > 0.5f)
                    specMaterial.SetFloat("_Metallic", appiedBarValues.NORM_TOTAL_PRECIPITATION);
                specMaterial.SetFloat("_Glossiness", 1.0f);

            }
            else
            {
                //specMaterial.SetColor("_Color", Color.red);
                //specMaterial.SetColor("_Color", new Color(0.47f, 0.18f, 0.60f));
                specMaterial.SetColor("_Color", new Color(0.603f, 0.062f, 0.450f));
                if (appiedBarValues.NORM_TOTAL_PRECIPITATION > 0.5f)
                    specMaterial.SetFloat("_Metallic", appiedBarValues.NORM_TOTAL_PRECIPITATION);
                specMaterial.SetFloat("_Glossiness", 1.0f);
            }   

            matRenderer.material = specMaterial;

        }
    }

    private static BarValue ApplyBarValues(GameObject siblingObject, Dictionary<string, object> selected)
    {
        BarValue barValue = siblingObject.GetComponent<BarValue>();
        barValue.LOCAL_DATE = (string)selected["LOCAL_DATE"];
        barValue.LOCAL_DATE_FORMATTED = Convert.ToDateTime(selected["LOCAL_DATE"]).ToString("MMM dd, yyyy");
        barValue.LOCAL_MONTH = (int)selected["LOCAL_MONTH"];
        barValue.LOCAL_DAY = (int)selected["LOCAL_DAY"];
        barValue.MEAN_TEMPERATURE = Convert.ToSingle(selected["MEAN_TEMPERATURE"]);
        barValue.NORM_MEAN_TEMP = Convert.ToSingle(selected["NORM_MEAN_TEMP"]);
        barValue.NORM_TOTAL_PRECIPITATION = Convert.ToSingle(selected["NORM_TOTAL_PRECIPITATION"]);
        barValue.TOTAL_PRECIPITATION = Convert.ToSingle(selected["TOTAL_PRECIPITATION"]);
        barValue.MIN_TEMP = Convert.ToSingle(selected["MIN_TEMP"]);
        barValue.MAX_TEMP = Convert.ToSingle(selected["MAX_TEMP"]);
        barValue.MEAN_HUMIDITY = Convert.ToSingle(selected["MEAN_HUMIDITY"]);
        barValue.NORM_MEAN_HUMIDITY = Convert.ToSingle(selected["NORM_MEAN_HUMIDITY"]);
        barValue.FROST_TEMP = Convert.ToBoolean(selected["FROST_TEMP"]);
        return barValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
