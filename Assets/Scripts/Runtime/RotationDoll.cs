using UnityEngine;

namespace Runtime
{
    public class RotationDoll : MonoBehaviour
    {
        private new Camera camera;
        private Vector3 mousePositionA;
        private Vector3 mousePositionB;
        private Vector3 rotation;
        private float angle;
        private Vector3 vector3;

        private void Awake()
        {
            camera = Camera.main;
            vector3 = transform.position;
        }

        private void OnMouseDown()
        {
            rotation = transform.rotation.eulerAngles;
            mousePositionA = Input.mousePosition - camera.WorldToScreenPoint(vector3);
        }

        private void OnMouseDrag()
        {
            mousePositionB = Input.mousePosition - camera.WorldToScreenPoint(vector3);

            angle = Vector2.SignedAngle(mousePositionA - vector3, mousePositionB - vector3);
            transform.rotation = Quaternion.Euler(0, angle + rotation.y, 0);
        }
    }
}