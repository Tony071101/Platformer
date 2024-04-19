using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform followTarget;

    //Start pos for the parallax game object.
    private Vector2 startPosition;
    //Start Z value of the parallax game object.
    private float startZValue;
    //Distance of the camera has moved from start pos of parallax object.
    private Vector2 cameraMoveSinceStart => (Vector2)_camera.transform.position - startPosition;
    private float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;
    private float clippingPlane => (_camera.transform.position.z + (zDistanceFromTarget > 0 ? 
                                    _camera.farClipPlane : _camera.nearClipPlane));
    //The further the object from the player, the faster the parallax effect object. Drag it's z value closer to the target
    //to make it move slower.
    private float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;

    private void Start() 
    {
        startPosition = transform.position;
        startZValue = transform.position.z;
    }

    private void Update() 
    {
        ParallaxEffect();
    }

    private void ParallaxEffect()
    {
        if(followTarget == null) {
            return;
        }
        else {
            Vector2 newPosition = startPosition + cameraMoveSinceStart * parallaxFactor;

            transform.position = new Vector3(newPosition.x, newPosition.y, startZValue);
        }
    }
}
