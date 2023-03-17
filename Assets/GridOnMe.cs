using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOnMe : MonoBehaviour
{
    public GameObject cube;
    public int gridSizeX = 10;
    public int gridSizeY = 10;
    public float lineThickness = 0.1f;
    public Material lineMaterial;

    private void Start()
    {
        Vector3 cubeSize = cube.GetComponent<Renderer>().bounds.size;
        float xStep = cubeSize.x / gridSizeX;
        float yStep = cubeSize.y / gridSizeY;

        for (int i = 0; i <= gridSizeX; i++)
        {
            DrawLine(new Vector3(cubeSize.x * (-0.5f) + i * xStep, cubeSize.y * (-0.5f), cubeSize.z * 0.5f),
                     new Vector3(cubeSize.x * (-0.5f) + i * xStep, cubeSize.y * (-0.5f), cubeSize.z * (-0.5f)));
        }

        for (int i = 0; i <= gridSizeY; i++)
        {
            DrawLine(new Vector3(cubeSize.x * (-0.5f), cubeSize.y * (-0.5f) + i * yStep, cubeSize.z * 0.5f),
                     new Vector3(cubeSize.x * 0.5f, cubeSize.y * (-0.5f) + i * yStep, cubeSize.z * 0.5f));
        }
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject line = new GameObject();
        line.transform.SetParent(cube.transform);
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = lineThickness;
        lineRenderer.endWidth = lineThickness;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}