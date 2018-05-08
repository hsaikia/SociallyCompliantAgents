﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct SW
{
    public Vector3 pos;
    public Vector3 vel;
    public Vector3 acc;
    public Vector3 forward;
    public Vector3 target;
    public float radius;
    public float targetRadius;

    float maxSpeed;
    float minSpeed;

    public void init(Vector3 x_, Vector3 target_)
    {
        pos = x_;
        vel = new Vector3(0f, 0f, 0f);
        acc = new Vector3(0f, 0f, 0f);
        forward = new Vector3(0f, 0f, 1f);
        target = target_;
        radius = 1.0f;
        targetRadius = 1.0f;
        maxSpeed = 0.2f;
        minSpeed = 0.001f;
    }

    private void restrictSpeed(){
        if(vel.magnitude > maxSpeed){
            vel = vel.normalized * maxSpeed;
        }
        if(vel.magnitude < minSpeed){
            vel = vel.normalized * minSpeed;
        }
    }

    // accelerate in the direction of forward velocity
    public void accelerateForward(float inc){ 
        //Debug.Log("Accelerating Forward");
        acc = acc + forward * inc;
        vel = vel + acc; // unit time

        restrictSpeed();

        pos = pos + vel; // unit time
        if(vel.magnitude > 0.0001f){
            forward = vel.normalized;
        }
    }

    public void accelerateRight(float inc){
        // Since our plane is the XZ plane, we take the cross product of the forward direction with the up direction (0, 1, 0)
        // to get the right direction 
        Vector3 right = Vector3.Cross(new Vector3(0f, 1f, 0f), forward).normalized;
        acc = acc + right * inc;
        vel = vel + acc;

        restrictSpeed();

        pos = pos + vel;
        if(vel.magnitude > 0.0001f){
            forward = vel.normalized;
        }
    }

    public void maintainSpeed(){
        acc = new Vector3(0f, 0f, 0f);
        pos = pos + vel;
        if(vel.magnitude > 0.0001f){
            forward = vel.normalized;
        }
    }

    public bool targetReached(){
        Vector3 d = target - pos;
        if(d.magnitude < targetRadius){
            return true;
        }
        return false;
    }

    public bool withinBounds(Vector3 minB, Vector3 maxB){
        if(pos.x < minB.x || pos.x > maxB.x || pos.z < minB.z || pos.z > maxB.z){
            return false;
        }
        return true;
    }

    public bool isCollidingWith(SW sw)
    {
        Vector3 d = pos - sw.pos;
        //Debug.Log("distance between agents " + d.magnitude);
        if(sw.radius > d.magnitude)
        {
            return true;
        }
        return false;
    }

    // 1 if forward is exactly facing the target
    // -1 if forward is facing exactly opposite the target
    public float cosineOrientation(){
        var A = target - pos;
        var B = forward;
        return Vector3.Dot(A, B) / (A.magnitude * B.magnitude);
    }
}