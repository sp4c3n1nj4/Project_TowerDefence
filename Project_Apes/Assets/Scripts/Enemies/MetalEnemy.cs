using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalEnemy : Enemy
{
    public static new EnemyType enemyType = EnemyType.clay;

    public List<Resource> drops;

    private void Start()
    {
        drops = new List<Resource>();
    }

    public override void DestroyEnemy()
    {
        for (int i = 0; i < typesTaken.Count; i++)
        {
            switch (typesTaken[i])
            {
                case DamageType.fire:
                    drops.Add(new Resource(ResourceType.ingot, 3));
                    break;
                case DamageType.ice:
                    break;
                case DamageType.electric:
                    break;
                case DamageType.steam:
                    break;
                case DamageType.water:
                    break;
                default:
                    drops.Add(new Resource(ResourceType.gold, 1));
                    break;
            }
        }

        base.DestroyEnemy();
    }
}
