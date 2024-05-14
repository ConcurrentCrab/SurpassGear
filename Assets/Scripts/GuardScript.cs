using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GuardScript : MonoBehaviour {

    enum GuardState {
        Patrolling,
        Searching,
        Shooting,
        Sleeping,
    };

    [SerializeField] float visionRadius;
    [SerializeField] float visionAngle;
    [SerializeField] LayerMask visionLayers;

    [SerializeField] float shootDelay;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletHolder;

    GuardState state;
    float shootWait;

    void Start() {
        state = GuardState.Patrolling;
        shootWait = 0.0f;
    }

    void Update() {
        // DEBUG
        changeCol();
    }

    void FixedUpdate() {
        // decrement cooldown
        shootWait = Mathf.Clamp(shootWait - Time.deltaTime, 0.0f, shootDelay);

        ScanTarget();

        switch (state) {
            case GuardState.Patrolling:
                if (GetComponent<PatrolScript>().HasReached()) {
                    GetComponent<PatrolScript>().GoToNext();
                }
                break;
            case GuardState.Searching:
                break;
            case GuardState.Shooting:
                break;
            case GuardState.Sleeping:
                break;
        }
    }

    void changeCol() {
        switch (state) {
            case GuardState.Patrolling:
                GetComponent<MeshRenderer>().materials[0].color = Color.blue;
                break;
            case GuardState.Searching:
                GetComponent<MeshRenderer>().materials[0].color = Color.yellow;
                break;
            case GuardState.Shooting:
                GetComponent<MeshRenderer>().materials[0].color = Color.red;
                break;
            case GuardState.Sleeping:
                GetComponent<MeshRenderer>().materials[0].color = Color.grey;
                break;
        }
    }

    void ScanTarget() {
        Collider[] collisions = Physics.OverlapSphere(transform.position, visionRadius, visionLayers, QueryTriggerInteraction.Ignore);
        foreach (Collider collider in collisions) {
            if (collider.gameObject.tag == "player") {
                Vector3 dir = (collider.transform.position - transform.position).normalized;
                if (Vector3.Dot(dir, transform.forward) >= Mathf.Cos((visionAngle * Mathf.Deg2Rad) / 2)) {
                    if (Physics.Raycast(transform.position, collider.ClosestPoint(transform.position) - transform.position, out RaycastHit hitInfo, visionRadius, visionLayers)) {
                        if (hitInfo.collider.gameObject.tag == "player") {
                            shootBullet(collider.gameObject.transform.position);
                        }
                    }
                }
            }
        }
    }

    void shootBullet(Vector3 target) {
        if (shootWait > 0.0f) {
            return;
        }
        shootWait = shootDelay;
        GameObject bulletInst = Instantiate(bullet, bulletHolder);
        Vector3 bulletOrig = transform.position + (transform.forward * 1.0f);
        bulletInst.transform.position = bulletOrig;
        bulletInst.GetComponent<Rigidbody>().velocity = (target - bulletOrig) * 5.0f;
    }

}
