using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    public enum ControlMode {
        Normal,
        FirstPerson,
        Peek,
    };

    [SerializeField] float moveAccel;
    [SerializeField] float moveVelMax;
    [SerializeField] float forwardSmoothTime;
    [SerializeField] LayerMask wallLayer;

    [NonSerialized] public ControlMode controlMode;

    Vector2 inputDir;
    bool inputFirstPerson;
    Vector2 moveVel;
    Vector3 forwardVel;
    Vector3 wallDir;

    void Start() {
        controlMode = ControlMode.Normal;
    }

    void Update() {
    }

    void FixedUpdate() {
        if (inputFirstPerson) {
            controlMode = ControlMode.FirstPerson;
        } else if (checkWall()) {
            controlMode = ControlMode.Peek;
        } else {
            controlMode = ControlMode.Normal;
        }
        switch (controlMode) {
            case ControlMode.Normal:
                moveNormal();
                break;
            case ControlMode.FirstPerson:
                break;
            case ControlMode.Peek:
                movePeek();
                break;
        }
    }

    bool checkWall() {
        Vector3 rayDir = new Vector3(inputDir.x, 0.0f, inputDir.y);
        if (Physics.Raycast(transform.position, rayDir, out RaycastHit hit, 2.0f, wallLayer, QueryTriggerInteraction.Ignore)) {
            if (hit.distance < 0.6f) {
                wallDir = rayDir;
                return true;
            }
        }
        return false;
    }

    void movePeek() {
        transform.forward = Vector3.SmoothDamp(transform.forward, Quaternion.Euler(0.0f, 180.0f, 0.0f) * wallDir, ref forwardVel, forwardSmoothTime / 4.0f);
    }

    void moveNormal() {
        transform.forward = Vector3.SmoothDamp(transform.forward, new Vector3(inputDir.x, 0.0f, inputDir.y), ref forwardVel, forwardSmoothTime);
        Vector2 moveVelTarget = inputDir * moveVelMax;
        float moveVelDelta = moveAccel * Time.deltaTime;
        moveVel.x = Mathf.MoveTowards(moveVel.x, moveVelTarget.x, moveVelDelta);
        moveVel.y = Mathf.MoveTowards(moveVel.y, moveVelTarget.y, moveVelDelta);
        transform.position += new Vector3(moveVel.x, 0.0f, moveVel.y) * Time.deltaTime;
    }

    public void OnInputMove(InputAction.CallbackContext value) {
        inputDir = Vector2.ClampMagnitude(value.ReadValue<Vector2>(), 1.0f);
    }

    public void OnInputFirstPerson(InputAction.CallbackContext value) {
        inputFirstPerson = (value.ReadValue<float>() != 0.0f) ? true : false;
    }

}
