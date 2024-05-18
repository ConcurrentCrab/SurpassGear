using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTracker : MonoBehaviour {

    public delegate void Handler(Collider coll);

    List<Handler> handlersEnter;
    List<Handler> handlersStay;
    List<Handler> handlersExit;

    void Start() {
        handlersEnter = new List<Handler>();
        handlersStay = new List<Handler>();
        handlersExit = new List<Handler>();
    }

    void Update() {        
    }

    void OnTriggerEnter(Collider coll) {
        foreach (Handler handler in handlersEnter) {
            handler(coll);
        }
    }

    void OnTriggerStay(Collider coll) {
        foreach (Handler handler in handlersStay) {
            handler(coll);
        }
    }

    void OnTriggerExit(Collider coll) {
        foreach (Handler handler in handlersExit) {
            handler(coll);
        }
    }

    public void RegisterHandlerEnter(Handler handler) {
        handlersEnter.Add(handler);
    }

    public void DeregisterHandlerEnter(Handler handler) {
        handlersEnter.Remove(handler);
    }

    public void RegisterHandlerStay(Handler handler) {
        handlersStay.Add(handler);
    }

    public void DeregisterHandlerStay(Handler handler) {
        handlersStay.Remove(handler);
    }
    public void RegisterHandlerExit(Handler handler) {
        handlersExit.Add(handler);
    }

    public void DeregisterHandlerExit(Handler handler) {
        handlersExit.Remove(handler);
    }
}
