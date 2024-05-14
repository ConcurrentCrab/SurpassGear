using UnityEngine;

public class CameraScript : MonoBehaviour {

    const int cameraModeLen = 3;

    static readonly Vector3[] cameraRelPos = {
        new Vector3(0.0f, 4.0f, -2.0f),
        new Vector3(0.0f, 0.5f, 0.5f),
        new Vector3(0.0f, 2.0f, 2.5f),
    };

    static readonly Quaternion[] cameraRelRot = {
        Quaternion.Euler(60.0f, 0.0f, 0.0f),
        Quaternion.Euler(0.0f, 0.0f, 0.0f),
        Quaternion.Euler(30.0f, 180.0f, 0.0f),
    };

    static readonly bool[] cameraRelMul = {
        false,
        true,
        true,
    };

    [SerializeField] float cameraSmoothTime;
    [SerializeField] Transform cameraObj;

    Vector3 cameraPosVel;
    Vector3 cameraRotVel;

    void Start() {
    }

    void Update() {
    }

    void FixedUpdate() {
        int cameraMode = (int) GetComponent<PlayerMovement>().controlMode;
        if (cameraMode >= cameraModeLen) {
            Debug.LogFormat("CameraScript: invalid value for cameraMode: {0}", cameraMode);
            return;
        }
        Quaternion cameraRot = Quaternion.Euler(0.0f, cameraRelMul[cameraMode] ? transform.rotation.eulerAngles.y : 0.0f, 0.0f);
        Vector3 cameraPosTar = transform.position + (cameraRot * cameraRelPos[cameraMode]);
        Quaternion cameraRotTar = cameraRot * cameraRelRot[cameraMode];
        cameraObj.position = Vector3.SmoothDamp(cameraObj.position, cameraPosTar, ref cameraPosVel, cameraSmoothTime);
        cameraObj.rotation = QuaternionUtils.SmoothDamp(cameraObj.rotation, cameraRotTar, ref cameraRotVel, cameraSmoothTime);
    }

    void LateUpdate() {
    }

}
