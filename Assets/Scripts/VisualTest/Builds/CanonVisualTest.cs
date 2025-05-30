using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonVisualTest : MonoBehaviour
{
    [SerializeField] GameObject particle;
    [SerializeField] float currentTime = 12;

    [SerializeField] LineRenderer line;
    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;
    [SerializeField] Transform height;
    [SerializeField] float vertexCount = 12;
    [SerializeField] float point2YPos = 2;

    float time = 0;

    void Update()
    {
        LineSmooth();

        time += Time.deltaTime;

        if (time >= currentTime)
        {
            Instantiate(particle, endPos.position, Quaternion.identity);
            time = 0;
        }
    }

    void LineSmooth()
    {
        //height.position = new Vector3((startPos.position.x + endPos.position.x), point2YPos, (startPos.position.z + endPos.position.z) / 2);
        var pointList = new List<Vector3>();

        for (float ratio = 0; ratio <= 1; ratio += 1 / vertexCount)
        {
            var tangent1 = Vector3.Lerp(startPos.position, height.position, ratio);
            var tangent2 = Vector3.Lerp(height.position, endPos.position, ratio);
            var curve = Vector3.Lerp(tangent1, tangent2, ratio);

            pointList.Add(curve - transform.position);
        }

        line.positionCount = pointList.Count;
        line.SetPositions(pointList.ToArray());
    }

    private void OnDrawGizmos()
    {
        LineSmooth();
    }
}
