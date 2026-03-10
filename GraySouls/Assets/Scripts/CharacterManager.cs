using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Lock On Transform")]
    public Transform lockOnTransform;

    [Header("Combat Colliders")]
    public CriticalDamageCollider backStabCollider;
    public CriticalDamageCollider riposteCollider;

    [Header("Combat Flag")]
    public bool canBeRiposted;
    public bool isParrying;
    public bool canBeParried;
    public bool isBlocking;
    public bool isInvulnerable;

    [Header("Movement Flags")]
    public bool isRotatingWithRootMotion;
    public bool canRotate;

    public int pendingCriticalDamage;
}
