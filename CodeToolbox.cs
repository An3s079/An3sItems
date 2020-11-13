using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace An3sMod
{
    public static class CodeToolbox
    {
        public static Gun GetGunById(this PickupObjectDatabase database, int id)
        {
            return (PickupObjectDatabase.GetById(id) as Gun);
        }

        public static Gun GetGunById(int id)
        {
            return CodeToolbox.GetGunById(null, id);
        }
        public static bool PlayerHasActiveSynergy(this PlayerController player, string synergyNameToCheck)
        {
            foreach (int index in player.ActiveExtraSynergies)
            {
                AdvancedSynergyEntry synergy = GameManager.Instance.SynergyManager.synergies[index];
                if (synergy.NameKey == synergyNameToCheck)
                {
                    return true;
                }
            }
            return false;
        }
        public static void AddPassiveStatModifier(this Gun gun, PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod modifyMethod)
        {
            gun.passiveStatModifiers = gun.passiveStatModifiers.Concat(new StatModifier[] { new StatModifier { statToBoost = statType, amount = amount, modifyType = modifyMethod } }).ToArray();
        }

        public static void AddCurrentGunStatModifier(this Gun gun, PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod modifyMethod)
        {
            gun.currentGunStatModifiers = gun.currentGunStatModifiers.Concat(new StatModifier[] { new StatModifier { statToBoost = statType, amount = amount, modifyType = modifyMethod } }).ToArray();
        }
    }
    public static class AlterItemStats
    {
        public static void AddStatToPassive(PassiveItem item, PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            StatModifier modifier = new StatModifier
            {
                amount = amount,
                statToBoost = statType,
                modifyType = method
            };

            if (item.passiveStatModifiers == null)
                item.passiveStatModifiers = new StatModifier[] { modifier };
            else
                item.passiveStatModifiers = item.passiveStatModifiers.Concat(new StatModifier[] { modifier }).ToArray();
        }
        public static void RemoveStatFromPassive(PassiveItem item, PlayerStats.StatType statType)
        {
            var newModifiers = new List<StatModifier>();
            for (int i = 0; i < item.passiveStatModifiers.Length; i++)
            {
                if (item.passiveStatModifiers[i].statToBoost != statType)
                    newModifiers.Add(item.passiveStatModifiers[i]);
            }
            item.passiveStatModifiers = newModifiers.ToArray();
        }


        public static tk2dSpriteDefinition CopyDefinitionFrom(this tk2dSpriteDefinition other)
        {
            tk2dSpriteDefinition result = new tk2dSpriteDefinition
            {
                boundsDataCenter = new Vector3
                {
                    x = other.boundsDataCenter.x,
                    y = other.boundsDataCenter.y,
                    z = other.boundsDataCenter.z
                },
                boundsDataExtents = new Vector3
                {
                    x = other.boundsDataExtents.x,
                    y = other.boundsDataExtents.y,
                    z = other.boundsDataExtents.z
                },
                colliderConvex = other.colliderConvex,
                colliderSmoothSphereCollisions = other.colliderSmoothSphereCollisions,
                colliderType = other.colliderType,
                colliderVertices = other.colliderVertices,
                collisionLayer = other.collisionLayer,
                complexGeometry = other.complexGeometry,
                extractRegion = other.extractRegion,
                flipped = other.flipped,
                indices = other.indices,
                material = new Material(other.material),
                materialId = other.materialId,
                materialInst = new Material(other.materialInst),
                metadata = other.metadata,
                name = other.name,
                normals = other.normals,
                physicsEngine = other.physicsEngine,
                position0 = new Vector3
                {
                    x = other.position0.x,
                    y = other.position0.y,
                    z = other.position0.z
                },
                position1 = new Vector3
                {
                    x = other.position1.x,
                    y = other.position1.y,
                    z = other.position1.z
                },
                position2 = new Vector3
                {
                    x = other.position2.x,
                    y = other.position2.y,
                    z = other.position2.z
                },
                position3 = new Vector3
                {
                    x = other.position3.x,
                    y = other.position3.y,
                    z = other.position3.z
                },
                regionH = other.regionH,
                regionW = other.regionW,
                regionX = other.regionX,
                regionY = other.regionY,
                tangents = other.tangents,
                texelSize = new Vector2
                {
                    x = other.texelSize.x,
                    y = other.texelSize.y
                },
                untrimmedBoundsDataCenter = new Vector3
                {
                    x = other.untrimmedBoundsDataCenter.x,
                    y = other.untrimmedBoundsDataCenter.y,
                    z = other.untrimmedBoundsDataCenter.z
                },
                untrimmedBoundsDataExtents = new Vector3
                {
                    x = other.untrimmedBoundsDataExtents.x,
                    y = other.untrimmedBoundsDataExtents.y,
                    z = other.untrimmedBoundsDataExtents.z
                }
            };
            List<Vector2> uvs = new List<Vector2>();
            foreach (Vector2 vector in other.uvs)
            {
                uvs.Add(new Vector2
                {
                    x = vector.x,
                    y = vector.y
                });
            }
            result.uvs = uvs.ToArray();
            List<Vector3> colliderVertices = new List<Vector3>();
            foreach (Vector3 vector in other.colliderVertices)
            {
                colliderVertices.Add(new Vector3
                {
                    x = vector.x,
                    y = vector.y,
                    z = vector.z
                });
            }
            result.colliderVertices = colliderVertices.ToArray();
            return result;
        }

        public static void SetProjectileSpriteRight(this Projectile proj, string name, int pixelWidth, int pixelHeight, int? overrideColliderPixelWidth = null, int? overrideColliderPixelHeight = null)
        {
            try
            {
                bool flag = overrideColliderPixelWidth == null;
                if (flag)
                {
                    overrideColliderPixelWidth = new int?(pixelWidth);
                }
                bool flag2 = overrideColliderPixelHeight == null;
                if (flag2)
                {
                    overrideColliderPixelHeight = new int?(pixelHeight);
                }
                float num = (float)pixelWidth / 16f;
                float num2 = (float)pixelHeight / 16f;
                float x = (float)overrideColliderPixelWidth.Value / 16f;
                float y = (float)overrideColliderPixelHeight.Value / 16f;
                proj.GetAnySprite().spriteId = ETGMod.Databases.Items.ProjectileCollection.inst.GetSpriteIdByName(name);
                tk2dSpriteDefinition tk2dSpriteDefinition = ETGMod.Databases.Items.ProjectileCollection.inst.spriteDefinitions[(PickupObjectDatabase.GetById(12) as Gun).DefaultModule.projectiles[0].GetAnySprite().spriteId].CopyDefinitionFrom();
                tk2dSpriteDefinition.boundsDataCenter = new Vector3(num / 2f, num2 / 2f, 0f);
                tk2dSpriteDefinition.boundsDataExtents = new Vector3(num, num2, 0f);
                tk2dSpriteDefinition.untrimmedBoundsDataCenter = new Vector3(num / 2f, num2 / 2f, 0f);
                tk2dSpriteDefinition.untrimmedBoundsDataExtents = new Vector3(num, num2, 0f);
                tk2dSpriteDefinition.position0 = new Vector3(0f, 0f, 0f);
                tk2dSpriteDefinition.position1 = new Vector3(0f + num, 0f, 0f);
                tk2dSpriteDefinition.position2 = new Vector3(0f, 0f + num2, 0f);
                tk2dSpriteDefinition.position3 = new Vector3(0f + num, 0f + num2, 0f);
                tk2dSpriteDefinition.colliderVertices[1].x = x;
                tk2dSpriteDefinition.colliderVertices[1].y = y;
                tk2dSpriteDefinition.name = name;
                ETGMod.Databases.Items.ProjectileCollection.inst.spriteDefinitions[proj.GetAnySprite().spriteId] = tk2dSpriteDefinition;
                proj.baseData.force = 0f;
            }
            catch (Exception ex)
            {
                ETGModConsole.Log("Ooops! Seems like something got very, Very, VERY wrong. Here's the exception:", false);
                ETGModConsole.Log(ex.ToString(), false);
            }

        }
    }

    public class CompanionisedEnemyBulletModifiers : BraveBehaviour //----------------------------------------------------------------------------------------------
    {
        public CompanionisedEnemyBulletModifiers()
        {
            this.baseBulletDamage = 10f;
            this.TintBullets = false;
            this.TintColor = Color.grey;
            this.jammedDamageMultiplier = 2f;
        }
        public void Start()
        {
            enemy = base.aiActor;
            AIBulletBank bulletBank2 = enemy.bulletBank;
            foreach (AIBulletBank.Entry bullet in bulletBank2.Bullets)
            {
                bullet.BulletObject.GetComponent<Projectile>().BulletScriptSettings.preventPooling = true;
            }
            if (enemy.aiShooter != null)
            {
                AIShooter aiShooter = enemy.aiShooter;
                aiShooter.PostProcessProjectile = (Action<Projectile>)Delegate.Combine(aiShooter.PostProcessProjectile, new Action<Projectile>(this.PostProcessSpawnedEnemyProjectiles));
            }

            if (enemy.bulletBank != null)
            {
                AIBulletBank bulletBank = enemy.bulletBank;
                bulletBank.OnProjectileCreated = (Action<Projectile>)Delegate.Combine(bulletBank.OnProjectileCreated, new Action<Projectile>(this.PostProcessSpawnedEnemyProjectiles));
            }
        }
        private void PostProcessSpawnedEnemyProjectiles(Projectile proj)
        {
            if (TintBullets) { proj.AdjustPlayerProjectileTint(this.TintColor, 1); }
            if (enemy != null)
            {
                if (enemy.aiActor != null)
                {
                    proj.baseData.damage = baseBulletDamage;
                    if (enemy.aiActor.IsBlackPhantom) { proj.baseData.damage = baseBulletDamage * jammedDamageMultiplier; }
                }
            }
            else { ETGModConsole.Log("Shooter is NULL"); }
        }

        private AIActor enemy;
        public float baseBulletDamage;
        public float jammedDamageMultiplier;
        public bool TintBullets;
        public Color TintColor;

    }

    public class AdvancedGameStatsManager
    {
        public AdvancedGameStatsManager()
        {
            this.m_flags = new HashSet<CustomDungeonFlags>(new CustomDungeonFlagsComparer());
        }
        public bool GetFlag(CustomDungeonFlags flag)
        {
            if (flag == CustomDungeonFlags.NONE)
            {
                Debug.LogError("Something is attempting to get a NONE save flag!");
                return false;
            }
            return this.m_flags.Contains(flag);
        }

        public void SetFlag(CustomDungeonFlags flag, bool value)
        {
            if (flag == CustomDungeonFlags.NONE)
            {
                Debug.LogError("Something is attempting to set a NONE save flag!");
                return;
            }
            if (value)
            {
                this.m_flags.Add(flag);
            }
            else
            {
                this.m_flags.Remove(flag);
            }
        }

        public static AdvancedGameStatsManager Instance
        {
            get
            {
                return m_instance;
            }
        }

        private static AdvancedGameStatsManager m_instance;
        public HashSet<CustomDungeonFlags> m_flags;

      

        public class CustomDungeonFlagsComparer : IEqualityComparer<CustomDungeonFlags>
        {
            public bool Equals(CustomDungeonFlags x, CustomDungeonFlags y)
            {
                return x == y;
            }

            public int GetHashCode(CustomDungeonFlags obj)
            {
                return (int)obj;
            }
        }
    }
}