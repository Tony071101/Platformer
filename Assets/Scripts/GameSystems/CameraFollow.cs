using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera _mainCamera;
    private Transform playerPos;
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        playerPos = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_mainCamera != null)
        {
            _mainCamera.transform.position = new Vector3(playerPos.position.x, playerPos.position.y, _mainCamera.transform.position.z);
        }
    }
}
