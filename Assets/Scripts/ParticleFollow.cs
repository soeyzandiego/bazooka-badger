using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFollow : MonoBehaviour
{
    [SerializeField] Vector3 offset = new Vector3(-0.813f, 0.044f);

    Transform target;

    void Update()
    {
        if (target != null)
        {
            transform.position = target.transform.position + offset;
        }
    }

    public void SetFollowTarget(Transform _target)
    {
        target = _target;
    }
}
