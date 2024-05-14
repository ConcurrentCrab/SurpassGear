using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTracker : MonoBehaviour {

    [NonSerialized] public List<Collision> collisions;
    [NonSerialized] public List<float> collisionsDur;

    void Start() {
        collisions = new List<Collision>();
        collisionsDur = new List<float>();
    }

    void Update() {        
    }

    void OnCollisionEnter(Collision collision) {
        collisions.Add(collision);
        collisionsDur.Add(0.0f);
    }

    void OnCollisionExit(Collision collision) {
        int idx = collisions.IndexOf(collision);
        collisions.RemoveAt(idx);
        collisionsDur.RemoveAt(idx);
    }

    void OnCollisionStay(Collision collision) {
        int idx = collisions.IndexOf(collision);
        collisionsDur[idx] += Time.deltaTime;
    }

}
