using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroisTestVisual : MonoBehaviour
{
    [SerializeField] Sprite[] _allSprites;
    [SerializeField] Vector3[] scaleAsteroids;

    void Start()
    {
        InitialStat();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            InitialStat();
    }

    void InitialStat()
    {
        GetComponent<SpriteRenderer>().sprite = _allSprites[Random.Range(0, _allSprites.Length)];

        transform.localScale = scaleAsteroids[Random.Range(0, scaleAsteroids.Length)];

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 361)));
    }
}
