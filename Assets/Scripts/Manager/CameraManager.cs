using UnityEngine;
using System;
using _UTILITY;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : SingletonMono<CameraManager>
{

    [Header("debug")]
    [SerializeField] private Camera _playerCam;
    [SerializeField] private Camera _cinematicCam;
    [SerializeField] private Transform _target;
    public bool _camMoving { get; private set; }

    void Start()
    {
        _playerCam = GameManager.Instance.player.Cam;
        _cinematicCam = GetComponentInChildren<Camera>();
        _cinematicCam.gameObject.SetActive(false);
    }

    public void MoveCamToPlace(Transform position)
    {

        _cinematicCam.transform.position = _playerCam.transform.position;
        _playerCam.gameObject.SetActive(false);
        _cinematicCam.gameObject.SetActive(true);

        StartCoroutine(MoveCamToTarget(position, _cinematicCam));
    }

    public void ReturntToPlayer()
    {
        StartCoroutine(MoveCamToTarget(GameManager.Instance.player.transform, _cinematicCam,true));
    }


    public IEnumerator MoveCamToTarget(Transform target, Camera maincam, bool returning = false)
    {
        float distance = Vector2.Distance(maincam.transform.position, target.transform.position);
        _camMoving = true;
        while (distance > 0.5f)
        {
            distance = Vector2.Distance(maincam.transform.position, target.transform.position);

            maincam.transform.position = Vector2.Lerp(maincam.transform.position, target.transform.position, Time.deltaTime);
            maincam.transform.position = new Vector3(maincam.transform.position.x, maincam.transform.position.y,-10);
            yield return new WaitForEndOfFrame();

        }

        if (returning)
        {
            _playerCam.gameObject.SetActive(true);
            _cinematicCam.gameObject.SetActive(false);
        }
        _camMoving = false;
    }




}
