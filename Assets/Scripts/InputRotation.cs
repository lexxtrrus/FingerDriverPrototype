using UnityEngine;

public class InputRotation : MonoBehaviour
{
    [SerializeField]
    private Transform _steerWheelTransform;

    [SerializeField]
    private float _maxSteerAngle = 90;

    [SerializeField]
    private float _steerAcceleration = 0.25f;

    private float _steerAxis;
    private Vector2 _startSteerWheelPoint;
    private Camera _mainCamera;
    public float SteerAxis => _steerAxis;

    private void Start()
    {
        _mainCamera = Camera.main;
        _startSteerWheelPoint = _mainCamera.WorldToScreenPoint(_steerWheelTransform.position);
    }

    private void Update()
    {
        _steerAxis = CalculateSteerAxis();
        _steerWheelTransform.localEulerAngles = new Vector3(0f, 0f, _steerAxis * _maxSteerAngle);
    }

    private float CalculateSteerAxis()
    {
        if (!Input.GetMouseButton(0)) return 0;

        var angle = Vector2.Angle(Vector2.up, (Vector2) Input.mousePosition - _startSteerWheelPoint);
        angle /= _maxSteerAngle;
        angle = Mathf.Clamp01(angle);

        if (Input.mousePosition.x > _startSteerWheelPoint.x)
        {
            angle *= -1f;
        }

        return Mathf.Lerp(_steerAxis, angle, _steerAcceleration);
    }
}