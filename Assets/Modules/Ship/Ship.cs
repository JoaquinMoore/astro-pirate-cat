using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private float _Distance;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && DistanceCheck())
        {
            OpenShipMenu();
        }
        Debug.Log(DistanceCheck());
    }


    public bool DistanceCheck()
    {
        return (Vector2.Distance(this.transform.position, _player.transform.position) <= _Distance);
    }

    public void OpenShipMenu()
    {
        Debug.Log("ew");
    }

    public void Ondamage()
    {
        Debug.Log("ow");
    }

    public void Ondeath()
    {
        Destroy(gameObject);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, _Distance);
    }
}
