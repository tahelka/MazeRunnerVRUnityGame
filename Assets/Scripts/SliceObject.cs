using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SliceObject : MonoBehaviour
{
    [SerializeField] private Transform m_StartSlicePoint;
    [SerializeField] private Transform m_EndSlicePoint;
    [SerializeField] private LayerMask m_SliceableLayer;
    [SerializeField] private VelocityEstimator m_VelocityEstimator;
    [SerializeField] private Material m_CrossSectionMaterial;
    [SerializeField] private float m_CutForce = 2000;

    // Update is called once per frame
    void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(m_StartSlicePoint.position, m_EndSlicePoint.position, out RaycastHit hit, m_SliceableLayer);

        if (hasHit)
        {
            GameObject target = hit.transform.gameObject;
            Slice(target);
        }
    }

    public void Slice(GameObject i_Target)
    {
        Vector3 velocity = m_VelocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(m_EndSlicePoint.position - m_StartSlicePoint.position, velocity);
        planeNormal.Normalize();
        SlicedHull hull = i_Target.Slice(m_EndSlicePoint.position, planeNormal);

        if (hull != null) 
        {
            GameObject upperHull = hull.CreateUpperHull(i_Target, m_CrossSectionMaterial);
            setupSlicedComponent(upperHull);
            GameObject lowerHull = hull.CreateLowerHull(i_Target, m_CrossSectionMaterial);
            setupSlicedComponent(lowerHull);
            Destroy(i_Target);
        }
    }

    private void setupSlicedComponent(GameObject i_SlicedObject)
    {
        Rigidbody rb = i_SlicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = i_SlicedObject.AddComponent<MeshCollider>();
        i_SlicedObject.AddComponent<XRGrabInteractable>();
        collider.convex = true;
        i_SlicedObject.layer = LayerMask.NameToLayer("Sliceable");
        rb.AddExplosionForce(m_CutForce, i_SlicedObject.transform.position, 1);
    }
}
