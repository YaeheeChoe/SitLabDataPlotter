using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataPlotter : MonoBehaviour
{
    // Name of the input file, no extension
    public string inputfile;
    
    // List for holding data from CSV reader
    private List<Dictionary<string, object>> pointList;

    // Indices for columns to be assigned
    public int columnX = 0;
    public int columnY = 1;
    public int columnZ = 2;

    // Full column names
    public string xName;
    public string yName;
    public string zName;

    public float plotScale = 10;
    private float lineThickness = 0.1f;
    public Material lineMaterial;
    public GameObject txtAsset;


    // The prefab for the data points that will be instantiated
    public GameObject PointPrefab;

    // Object which will contain instantiated prefabs in hiearchy
    public GameObject PointHolder;

    private Vector3 origin = new Vector3(0,0,0);

    // Use this for initialization
    void Start()
    {
        // Set pointlist to results of function Reader with argument inputfile
        pointList = CSVReader.Read(inputfile);

        DrawGrid();
        //Log to console
        Debug.Log(pointList);

        // Declare list of strings, fill with keys (column names)
        List<string> columnList = new List<string>(pointList[1].Keys);

        // Print number of keys (using .count)
        Debug.Log("There are " + columnList.Count + " columns in the CSV");

        // Assign column name from columnList to Name variables
        xName = columnList[columnX];
        yName = columnList[columnY];
        zName = columnList[columnZ];
        Debug.Log("Column name is " +xName+", "+ yName+", "+zName);

        // Get maxes of each axis
        float xMax = FindMaxValue(xName);
        float yMax = FindMaxValue(yName);
        float zMax = FindMaxValue(zName);

        // Get minimums of each axis
        //float xMin = FindMinValue(xName);
        //float yMin = FindMinValue(yName);
        //float zMin = FindMinValue(zName);
        Debug.Log("Column Max is " + xMax + ", " + yMax + ", " + zMax);


        //Loop through Pointlist
        for (var i = 0; i < pointList.Count; i++)
        {
            // Get value in poinList at ith "row", in "column" Name, normalize
            float x =
                (System.Convert.ToSingle(pointList[i][xName]) )
                / (xMax );

            float y =
                (System.Convert.ToSingle(pointList[i][yName]) )
                / (yMax );

            float z =
                (System.Convert.ToSingle(pointList[i][zName]) )
                / (zMax);

            // Instantiate as gameobject variable so that it can be manipulated within loop
            GameObject dataPoint = Instantiate(
                    PointPrefab,
                    new Vector3(x, y, z) * plotScale,
                    Quaternion.identity);

            // Make child of PointHolder object, to keep points within container in hiearchy
            dataPoint.transform.parent = PointHolder.transform;

            // Assigns original values to dataPointName
            string dataPointName =
                pointList[i][xName] + " "
                + pointList[i][yName] + " "
                + pointList[i][zName];

            // Assigns name to the prefab
            dataPoint.transform.name = dataPointName;

            // Gets material color and sets it to a new RGB color we define
            dataPoint.GetComponent<Renderer>().material.color =
              new Color(0.5f, 0.5f, 1f, 1.0f);
        }

    }

    private float FindMaxValue(string columnName)
    {
        //set initial value to first value
        float maxValue = Convert.ToSingle(pointList[0][columnName]);

        //Loop through Dictionary, overwrite existing maxValue if new value is larger
        for (var i = 0; i < pointList.Count; i++)
        {
            if (maxValue < Convert.ToSingle(pointList[i][columnName]))
                maxValue = Convert.ToSingle(pointList[i][columnName]);
        }

        //Spit out the max value
        return maxValue;
    }

    private float FindMinValue(string columnName)
    {
        float minValue = Convert.ToSingle(pointList[0][columnName]);

        //Loop through Dictionary, overwrite existing minValue if new value is smaller
        for (var i = 0; i < pointList.Count; i++)
        {
            if (Convert.ToSingle(pointList[i][columnName]) < minValue)
                minValue = Convert.ToSingle(pointList[i][columnName]);
        }

        return minValue;
    }
    private void DrawGrid()
    {
        var inter = plotScale / 10;
        for (var i = 0; i < 10; i++)
        {
            DrawLine(origin + Vector3.right *i* inter, Vector3.forward * plotScale + origin+ Vector3.right *i*inter);
            DrawLine(origin + Vector3.up * i * inter, Vector3.forward * plotScale + origin + Vector3.up * i * inter);
            DrawLine(origin + Vector3.forward * i * inter, Vector3.right * plotScale + origin + Vector3.forward * i * inter);
            DrawLine(origin + Vector3.up * i * inter, Vector3.right * plotScale + origin + Vector3.up * i * inter);
            DrawLine(origin + Vector3.forward * i * inter, Vector3.up * plotScale + origin + Vector3.forward * i * inter);
            DrawLine(origin + Vector3.right * i * inter, Vector3.up * plotScale + origin + Vector3.right * i * inter);

            GameObject txt = Instantiate(txtAsset);
            txt.transform.SetParent(transform);
            txt.transform.position = origin + Vector3.right * plotScale + Vector3.forward * i * inter;

            GameObject txt2 = Instantiate(txtAsset);
            txt2.transform.SetParent(transform);
            txt2.transform.position = origin + Vector3.forward * plotScale + Vector3.right * i * inter;

            GameObject txt3 = Instantiate(txtAsset);
            txt3.transform.SetParent(transform);
            txt3.transform.position = origin + Vector3.forward * plotScale + Vector3.up * i * inter;
        }
    }
    private void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject line = new GameObject();
        line.transform.SetParent(this.transform);
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = lineThickness;
        lineRenderer.endWidth = lineThickness;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.SetColors(Color.gray, Color.gray);
    }

}