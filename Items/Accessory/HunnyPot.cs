﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;

using Microsoft.Xna.Framework.Graphics;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework;
using Conquest.Assets.Common;

namespace Conquest.Items.Accessory
{
    public class HunnyPot : ModItem
    {
        Effect GoldenFX;
        static void SetEffectParameters(Effect effect)
        {
            effect.Parameters["uTime"].SetValue((float)(Main.timeForVisualEffects * 0.032f));
        }
        static bool ShaderTooltip(DrawableTooltipLine line, Effect shader)
        {
            Vector2 textPos = new Vector2(line.X, line.Y);
            for (float i = 0; i < 1; i += 0.25f)
            {
                Vector2 borderOffset = (i * MathF.Tau).ToRotationVector2() * 2;
                ChatManager.DrawColorCodedString(Main.spriteBatch, line.Font, line.Text, textPos + borderOffset, Color.Black, line.Rotation, line.Origin, line.BaseScale);
            }
            SetEffectParameters(shader);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, shader, Main.UIScaleMatrix);
            ChatManager.DrawColorCodedString(Main.spriteBatch, line.Font, line.Text, textPos, Color.Red, line.Rotation, line.Origin, line.BaseScale);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
            return false;
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (GoldenFX == null)
                GoldenFX = ModContent.Request<Effect>("Conquest/Assets/Shaders/Gradient", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
            if (line.Index == 0)
            {
                return ShaderTooltip(line, GoldenFX);
            }
            return true;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.value = Item.buyPrice(0, 40, 0, 0);
            Item.rare = ModContent.RarityType<ArtifactRarity>();

        }
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            return incomingItem.rare != ModContent.RarityType<ArtifactRarity>() || equippedItem.ModItem is not HunnyPot;

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().HunnyPot = true;
        }
    }
}
