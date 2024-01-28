using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

public class CharacterBody : MonoBehaviour
{
    [SerializeField]
    private List<Collider> controllerColliders = new List<Collider>();
    [SerializeField]
    private List<Rigidbody> controllerRigidBodies = new List<Rigidbody>();
    
    [SerializeField]
    private Animator animator;

    private Avatar cachedAvatar;
    
    private List<Collider> colliders = new List<Collider>();
    private List<Rigidbody> rigidBodies = new List<Rigidbody>();

    void Awake()
    {
        colliders = GetComponentsInChildren<Collider>().ToList();
        rigidBodies = GetComponentsInChildren<Rigidbody>().ToList();
        foreach (var collider in colliders)
        {
            collider.gameObject.AddComponent<DamageSensor>();
        }
        GoAnimated();
    }

    public void GoRagdoll()
    {
        foreach (var collider in colliders)
        {
            collider.isTrigger = false;
        }
        
        foreach (var collider in controllerColliders)
        {
            collider.isTrigger = true;
        }
        
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = false;
            rigidBody.useGravity = true;
        }
        
        foreach (var rigidBody in controllerRigidBodies)
        {
            rigidBody.isKinematic = true;
        }

        if (animator.avatar != null)
        {
            cachedAvatar = animator.avatar;
        }
        animator.avatar = null;
        animator.enabled = false;
    }
    
    public void GoAnimated()
    {
        foreach (var collider in colliders)
        {
            collider.isTrigger = true;
        }
        
        foreach (var collider in controllerColliders)
        {
            collider.isTrigger = false;
        }
        
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = true;
        }
        
        foreach (var rigidBody in controllerRigidBodies)
        {
            rigidBody.isKinematic = false;
            rigidBody.useGravity = true;
        }
        
        if (cachedAvatar != null)
        {
            animator.avatar = cachedAvatar;
        }
        animator.enabled = true;
    }
}
