using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Weapon")]
public class WeaponInfo : ScriptableObject
{
    public GameObject weaponPrefab;
    public float weaponCooldown;
    public int weaponDamage;
    public float weaponRange;

    [Header("Damage Exemptions")]
    public List<string> noDamageTags; // Tags that take zero damage

    public int GetDamageForTarget(GameObject target)
    {
        if (target != null && noDamageTags.Contains(target.tag))
        {
            return 0; // No damage if the target's tag is in the no-damage list
        }
        return weaponDamage;
    }
}
