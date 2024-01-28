using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterBody : MonoBehaviour
{
    [SerializeField]
    private List<Collider> controllerColliders = new List<Collider>();
    [SerializeField]
    private List<Rigidbody> controllerRigidBodies = new List<Rigidbody>();
    
    [SerializeField]
    private Animator animator;
    
    public GameObject Hips;

    private Avatar cachedAvatar;
    private Avatar startingAvatar;
    
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
        startingAvatar = animator.avatar;
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
        
        foreach (var rigidBody in controllerRigidBodies)
        {
            rigidBody.isKinematic = true;
        }
        
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = false;
            rigidBody.useGravity = true;
        }

        //if (animator.avatar != null)
        //{
        //    cachedAvatar = animator.avatar;
        //}
        //animator.avatar = null;
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
        
        //if (cachedAvatar != null)
        //{
        //    animator.avatar = startingAvatar;
        //}
        animator.enabled = true;
    }

    public void ResetPosition()
    {
        Hips.transform.localPosition = Vector3.zero;
    }
}
