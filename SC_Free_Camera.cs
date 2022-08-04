using UnityEngine;

public class SC_Free_Camera : MonoBehaviour {
    Quaternion _xQuat;
    Quaternion _yQuat;

    static float _xSensibility;
    static float _ySensibility;

    float _xZradius = 200f;
    float _yradius = 30f;

    float _x;
    float _y;
    float _z;

    Vector2 _rotation = Vector2.zero;
    const string XAxis = "Mouse X"; //Strings in direct code generate garbage, storing and re-using them creates no garbage
    const string YAxis = "Mouse Y";
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;

    float _cameraSpeed = 8.0f;

    float _distanceToTerrain;

    void Start() {
        SetMouseSensibility();
    }

    static void SetMouseSensibility() {
        _xSensibility = PlayerPrefs.GetFloat("horizontalMouseSensibility", 3f);
        _ySensibility = PlayerPrefs.GetFloat("VerticalMouseSensibility", 3f);
    }
    
    void Update() {
        _rotation.x += Input.GetAxis(XAxis) * _xSensibility;
        _rotation.y += Input.GetAxis(YAxis) * _ySensibility;
        _rotation.y = Mathf.Clamp(_rotation.y, -yRotationLimit, yRotationLimit);

        if (_rotation.x > 360) _rotation.x = 0;
        if (_rotation.x < 0) _rotation.x = 360;

        Rotate(_rotation.x, _rotation.y);
        Move();
    }

        // Rotation (paternity by KarlRamstedt)
    void Rotate(float rotationX, float rotationY) {

        _xQuat = Quaternion.AngleAxis(rotationX, Vector3.up);
        _yQuat = Quaternion.AngleAxis(rotationY, Vector3.left);

        transform.localRotation = _xQuat * _yQuat;
        // Quaternions seem to rotate more consistently than EulerAngles.
        // Sensitivity seemed to change slightly at certain degrees using Euler.
    }

    // Movement
    void Move() {
        // Deplacement
        _x = Input.GetAxis("Horizontal");
        _z = Input.GetAxis("Vertical");

        // Acceleration
        if (Input.GetKeyDown(KeyCode.LeftShift)) _cameraSpeed += 10f;
        if (Input.GetKeyUp(KeyCode.LeftShift)) _cameraSpeed = 8f;

        // Up
        if (Input.GetKeyDown(KeyCode.Space)) _y += 1f;
        if (Input.GetKeyUp(KeyCode.Space)) _y = 0f;

        Vector3 movement = new Vector3(_x, _y, _z);
        transform.Translate(movement * _cameraSpeed * Time.deltaTime);
    }
}