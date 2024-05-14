using UnityEngine;
using UnityEngine.AI;

public class PatrolScript : MonoBehaviour {

    [SerializeField] Transform nodes;
    [SerializeField] int[] route;

    NavMeshAgent agent;
    int nodeCurr;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        nodeCurr = 0;
    }

    void Update() {
    }

    public bool HasReached() {
        return agent.remainingDistance == 0.0f;
    }

    public void GoToNext() {
        if (route.Length > 0) {
            agent.destination = nodes.GetChild(route[nodeCurr]).position;
            nodeCurr = (nodeCurr + 1) % route.Length;
        } else {
            agent.destination = nodes.GetChild(nodeCurr).position;
            nodeCurr = (nodeCurr + 1) % nodes.childCount;
        }
    }

}
