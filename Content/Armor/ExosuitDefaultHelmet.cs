
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace ExoSuitTest.Content.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class ExosuitDefaultHelmet : ModItem

    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.value = Item.buyPrice(gold: 1,silver: 50);
            Item.rare = ItemRarityID.Purple;
            Item.defense = 2;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Insert(0, new TooltipLine(Mod, "ItemName", "The First Version Of Exosuits")
            {
                OverrideColor = Microsoft.Xna.Framework.Color.Black,
            }
                );
            tooltips.Add(new TooltipLine(Mod, "Tooltip", "It's the first"));
        }
    }
}