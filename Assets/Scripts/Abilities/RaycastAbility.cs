//Created by Robert Bryant
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Raycast Ability", menuName ="Ability/Raycast", order = 3)]
public class RaycastAbility : Ability
{

    public float abilityDamage;
    public float abilityRange;
    public float abilityDuration;
    public Color abilityColor;
    public Material abilityMaterial;

    private RaycastShoot rcShoot;

    public override void Initialize(GameObject obj)
    {
        rcShoot = obj.GetComponent<RaycastShoot>();
        rcShoot.Initialize();
    }

    public override void TriggerAbility()
    {
        rcShoot.abilityDamage = abilityDamage;
        rcShoot.abilityRange = abilityRange;
        rcShoot.abilityDuration = abilityDuration;
        rcShoot.lineRenderer.material = abilityMaterial;
        rcShoot.lineRenderer.material.color = abilityColor;

        rcShoot.Shoot();
    }
}
