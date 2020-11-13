using System;
using System.Collections.Generic;
using System.Linq;
using ItemAPI;
using UnityEngine;
using System.Text;
using System.Collections;


namespace An3sMod
{
    class FearBullets : BulletStatusEffectItem


    {
        
        private static FleePlayerData fleeData;
        public static void Init()
        {
            GameObject obj = new GameObject();
            var item = obj.AddComponent<FearBullets>();
            string itemName = "Fear Bullets";
            string resourceName = "An3sMod/Resources/ScaredBullets";
            GameObject gameObject = new GameObject(itemName);
            ItemBuilder.AddSpriteToObject(itemName, resourceName, gameObject);
            string shortDesc = "Embed Fear in them";
            string longDesc = "GIves a small chance to make enemies fear you upon shooting them\n\n" +
                "The bullets them self are always afraid, upon being lodged in another creature they can transfer that feeling of dread to another.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ans");
            item.quality = PickupObject.ItemQuality.C;


        }
        
      
        public void PostProcessProjectile(Projectile proj, float f)
        { 
            proj.OnHitEnemy += this.OnHitEnemy;
            
        }
        private void OnHitEnemy(Projectile proj, SpeculativeRigidbody enemy, bool fatal)
        {
            if (UnityEngine.Random.value <= 0.3)
            {
                enemy.aiActor.behaviorSpeculator.FleePlayerData = FearBullets.fleeData;
                FleePlayerData fleePlayerData = new FleePlayerData();
                GameManager.Instance.StartCoroutine(FearBullets.RemoveFear(enemy.aiActor));
            }
          
        }
       
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            Tools.Print($"Player picked up {this.DisplayName}");
            player.PostProcessProjectile += this.PostProcessProjectile;
            bool flag = FearBullets.fleeData == null || FearBullets.fleeData.Player != player;
            if (flag)
            {
                FearBullets.fleeData = new FleePlayerData();
                FearBullets.fleeData.Player = player;
                FearBullets.fleeData.StartDistance = 500f;
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            Tools.Print($"Player dropped {this.DisplayName}");
            player.PostProcessProjectile -= this.PostProcessProjectile;
            return base.Drop(player);
        }

        private static IEnumerator RemoveFear(AIActor aiactor)
        {
            yield return new WaitForSeconds(3f);
            aiactor.behaviorSpeculator.FleePlayerData = null;
            yield break;
        }
    }



}

