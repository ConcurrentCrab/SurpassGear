using UnityEngine;
using UnityEngine.AI;

public class PatrolScript : MonoBehaviour {

    [SerializeField] Transform nodes;
    [SerializeField] int[] route;

    NavMeshAgent agent;
    int nodeCurr;

    public bool agentEnabled {
        get {
            return agent.enabled;
        }
        set {
            agent.enabled = value;
        }
    }

    public Vector3 destination {
        get {
            return agent.destination;
        }
        set {
            agent.destination = value;
        }
    }

    public bool hasReached {
        get {
            return Mathf.Approximately(agent.remainingDistance, 0.0f);
        }
    }

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        nodeCurr = 0;
        if (route.Length == 0) {
            route = new int[nodes.childCount];
            for (int i = 0; i < route.Length; i++) {
                route[i] = i;
            }
        }
    }

    void Update() {
    }

    private void OnDrawGizmosSelected() {
        if (nodes == null) {
            return;
        }
        Gizmos.color = Color.green;
        Vector3[] points = new Vector3[route.Length];
        for (int i = 0; i < route.Length; i++) {
            points[i] = nodes.GetChild(route[i]).transform.position;
        }
        Gizmos.DrawLineStrip(points, true);
    }

    public void GoToNext() {
        agent.destination = nodes.GetChild(route[nodeCurr]).position;
        nodeCurr = (nodeCurr + 1) % route.Length;
    }

}
