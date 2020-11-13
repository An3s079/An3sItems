using Dungeonator;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace An3sMod
{
    class GoopIdentifier
    {
        
        public static void DefineDefaultGoops()
        {
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            GoopIdentifier.goopDefs = new List<GoopDefinition>();
            foreach (string text in GoopIdentifier.goops)
            {
                GoopDefinition goopDefinition;
                try
                {
                    GameObject gameObject = assetBundle.LoadAsset(text) as GameObject;
                    goopDefinition = gameObject.GetComponent<GoopDefinition>();
                }
                catch
                {
                    goopDefinition = (assetBundle.LoadAsset(text) as GoopDefinition);
                }
                goopDefinition.name = text.Replace("assets/data/goops/", "").Replace(".asset", "");
                GoopIdentifier.goopDefs.Add(goopDefinition);
            }
            List<GoopDefinition> list = GoopIdentifier.goopDefs;
            FireDef = GoopIdentifier.goopDefs[0];
            OilDef = GoopIdentifier.goopDefs[1];
            PoisonDef = GoopIdentifier.goopDefs[2];
            BlobulonGoopDef = GoopIdentifier.goopDefs[3];
            WebGoop = GoopIdentifier.goopDefs[4];
            WaterGoop = GoopIdentifier.goopDefs[5];
        }
        private static string[] goops = new string[]
        {
            "assets/data/goops/napalmgoopthatworks.asset",
            "assets/data/goops/oil goop.asset",
            "assets/data/goops/poison goop.asset",
            "assets/data/goops/blobulongoop.asset",
            "assets/data/goops/phasewebgoop.asset",
            "assets/data/goops/water goop.asset",
        };
        private static List<GoopDefinition> goopDefs;

        public static GoopDefinition FireDef;
        public static GoopDefinition OilDef;
        public static GoopDefinition PoisonDef;
        public static GoopDefinition BlobulonGoopDef;
        public static GoopDefinition WebGoop;
        public static GoopDefinition WaterGoop;
        public static GoopDefinition CharmGoopDef = PickupObjectDatabase.GetById(310)?.GetComponent<WingsItem>()?.RollGoop;
        public static GoopDefinition GreenFireDef = (PickupObjectDatabase.GetById(698) as Gun).DefaultModule.projectiles[0].GetComponent<GoopModifier>().goopDefinition;
        public static GoopDefinition CheeseDef = (PickupObjectDatabase.GetById(808) as Gun).DefaultModule.projectiles[0].GetComponent<GoopModifier>().goopDefinition;
    }
}
 

