using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Transform player;

    // Update is called once per frame
    private void Update()
    {
        _mainCamera.transform.position = new Vector3(player.position.x, player.position.y, _mainCamera.transform.position.z);
    }
}
