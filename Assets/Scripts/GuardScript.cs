using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GuardScript : MonoBehaviour {

    enum GuardState {
        Patrolling,
        Searching,
        Shooting,
        Sleeping,
    };

    [SerializeField] CollisionTracker tracker;

    [SerializeField] float turnVel;
    [SerializeField] float shootDelay;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletHolder;
    [SerializeField] float searchPeriod;

    GuardState state;
    float shootWait;
    float searchTimer;
    bool targetFound;
    Vector3 targetPos;
    PatrolScript patrol;

    void Start() {
        state = GuardState.Patrolling;
        shootWait = 0.0f;
        patrol = GetComponent<PatrolScript>();
        tracker.RegisterHandlerEnter(coll => {
            if (coll.gameObject.tag == "player") {
                targetFound = true;
                targetPos = coll.gameObject.transform.position;
            }
        });
        tracker.RegisterHandlerStay(coll => {
            if (coll.gameObject.tag == "player") {
                targetFound = true;
                targetPos = coll.gameObject.transform.position;
            }
        });
        tracker.RegisterHandlerExit(coll => {
            if (coll.gameObject.tag == "player") {
                targetFound = false;
            }
        });
    }

    void Update() {
        // DEBUG
        changeCol();
    }

    void FixedUpdate() {
        // decrement cooldown
        shootWait = Mathf.Clamp(shootWait - Time.deltaTime, 0.0f, shootDelay);

        switch (state) {
            case GuardState.Patrolling:
                if (ScanTarget()) {
                    state = GuardState.Shooting;
                    patrol.agentEnabled = false;
                } else if (patrol.hasReached) {
                    patrol.GoToNext();
                }
                break;
            case GuardState.Searching:
                searchTimer = Mathf.Clamp(searchTimer - Time.deltaTime, 0.0f, searchPeriod);
                if (Mathf.Approximately(searchTimer, 0.0f)) {
                    state = GuardState.Patrolling;
                } else if (targetFound && ScanTarget()) {
                    state = GuardState.Shooting;
                } else {
                    transform.forward = Vector3.RotateTowards(transform.forward, (targetPos - transform.position).normalized, turnVel * Time.deltaTime * Mathf.Deg2Rad, 0.0f);
                }
                break;
            case GuardState.Shooting:
                if (targetFound) {
                    if (Mathf.Approximately(shootWait, 0.0f)) {
                        shootWait = shootDelay;
                        shootBullet(targetPos);
                    }
                } else {
                    state = GuardState.Searching;
                    searchTimer = searchPeriod;
                    patrol.agentEnabled = true;
                    patrol.destination = targetPos;
                }
                break;
            case GuardState.Sleeping:
                break;
        }
    }

    void changeCol() {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        switch (state) {
            case GuardState.Patrolling:
                renderer.materials[0].color = Color.blue;
                break;
            case GuardState.Searching:
                renderer.materials[0].color = Color.yellow;
                break;
            case GuardState.Shooting:
                renderer.materials[0].color = Color.red;
                break;
            case GuardState.Sleeping:
                renderer.materials[0].color = Color.grey;
                break;
        }
    }

    bool ScanTarget() {
        if (targetFound) {
            if (Physics.Raycast(transform.position, targetPos - transform.position, out RaycastHit hitInfo)) {
                if (hitInfo.collider.gameObject.tag == "player") {
                    return true;
                }
            }
        }
        return false;
    }

    void shootBullet(Vector3 target) {
        GameObject bulletInst = Instantiate(bullet, bulletHolder);
        Vector3 bulletOrig = transform.position + (transform.forward * 1.0f);
        bulletInst.transform.position = bulletOrig;
        bulletInst.GetComponent<Rigidbody>().velocity = (target - bulletOrig) * 5.0f;
    }

}
