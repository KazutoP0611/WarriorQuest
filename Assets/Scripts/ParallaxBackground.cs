using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera mainCamera;
    private float lastMainCameraPositionX;
    private float cameraHalfWidth;

    [SerializeField] private ParallaxLayer[] backgroundLayers;

    private void Awake()
    {
        mainCamera = Camera.main;
        cameraHalfWidth = mainCamera.orthographicSize/* This will give you the Camera's half height*/ * mainCamera.aspect;
        CalculateImageLength();
    }

    private void FixedUpdate()
    {
        if (mainCamera.transform.position.x != lastMainCameraPositionX)
        {
            float currentCamerPositionX = mainCamera.transform.position.x;
            float distanceToMove = currentCamerPositionX - lastMainCameraPositionX;
            lastMainCameraPositionX = currentCamerPositionX;

            float cameraRightEdge = currentCamerPositionX + cameraHalfWidth;
            float cameraLeftEdge = currentCamerPositionX - cameraHalfWidth;

            foreach (ParallaxLayer layer in backgroundLayers)
            {
                layer.Move(distanceToMove);
                layer.LoopBackground(cameraLeftEdge, cameraRightEdge);
            }
        }
    }

    public void CalculateImageLength()
    {
        foreach (ParallaxLayer layer in backgroundLayers)
            layer.CalculateImageWidth();
    }
}
