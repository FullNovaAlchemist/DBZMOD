﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using DBZMOD.Destruction;
using DBZMOD.Util;

namespace DBZMOD.Projectiles
{
    public class SuperSpiritBombBall : KiProjectile
    {
        int rocksFloating = 0;
        const int MAX_ROCKS = 25;
        const float BASE_SCALE = 1f;
        const float SCALE_INCREASE = 0.05f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Super Spirit Bomb");
        }

        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 28;
            projectile.light = 1f;
            projectile.aiStyle = 1;
            aiType = 14;
            projectile.friendly = true;
            projectile.extraUpdates = 2;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 1600;
            projectile.tileCollide = false;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            KiDrainRate = 10;
        }

        private float HeldTime
        {
            get
            {
                return projectile.ai[0];
            }
            set
            {
                projectile.ai[0] = value;
            }
        }

        private bool isInitialized = false;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            if (!isInitialized)
            {
                HeldTime = 1;
                isInitialized = true;
            }

            if (player.channel && HeldTime > 0)
            {
                projectile.scale = BASE_SCALE + SCALE_INCREASE * HeldTime;
                projectile.position = player.Center + new Vector2(0, -40 - (projectile.scale * 17));
                HeldTime++;

                // reduced from 25.
                for (int d = 0; d < 15; d++)
                {
                    // loop hitch for variance.
                    if (Main.rand.NextFloat() < 0.3f)
                        continue;

                    float angle = Main.rand.NextFloat(360);
                    float angleRad = MathHelper.ToRadians(angle);
                    Vector2 position = new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));

                    Dust tDust = Dust.NewDustDirect(projectile.position + (position * (20 + 12.5f * projectile.scale)), projectile.width, projectile.height, 230, 0f, 0f, 213, default(Color), 2.0f);
                    tDust.velocity = Vector2.Normalize((projectile.position + (projectile.Size / 2)) - tDust.position) * 2;
                    tDust.noGravity = true;
                }

                //Rock effect
                if (DBZMOD.IsTickRateElapsed(10) && rocksFloating < MAX_ROCKS)
                {
                    // only some of the time, keeps it a little more varied.
                    if (Main.rand.NextFloat() < 0.6f)
                    {
                        rocksFloating++;
                        BaseFloatingDestructionProj.SpawnNewFloatingRock(player, projectile);
                    }
                }

                projectile.netUpdate = true;

                if (projectile.timeLeft < 399)
                {
                    projectile.timeLeft = 400;
                }

                MyPlayer.ModPlayer(player).AddKi(-5, true, false);
                ProjectileUtil.ApplyChannelingSlowdown(player);

                projectile.netUpdate2 = true;

                // depleted check, release the ball
                if (MyPlayer.ModPlayer(player).IsKiDepleted())
                {
                    player.channel = false;
                }
                int soundtimer = 0;
                soundtimer++;
                if(soundtimer > 120)
                {
                    SoundUtil.PlayCustomSound("Sounds/SpiritBombCharge", player, 0.5f);
                    soundtimer = 0;
                }
            }
            else if (HeldTime > 0)
            {
                HeldTime = 0;
                projectile.timeLeft = (int)Math.Ceiling(projectile.scale * 15) + 600;
                projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.Center) * 2.8f;
                projectile.tileCollide = false;
                projectile.damage *= (int)projectile.scale / 2;
                SoundUtil.PlayCustomSound("Sounds/SpiritBombFire", player);
            }
        }

        public override void OnHitNPC(NPC npc, int damage, float knockback, bool crit)
        {
            projectile.scale -= 0.25f;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float radius = projectile.width * projectile.scale / 2f;
            float rSquared = radius * radius;

            return rSquared > Vector2.DistanceSquared(Vector2.Clamp(projectile.Center, targetHitbox.TopLeft(), targetHitbox.BottomRight()), projectile.Center);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            int radius = (int)Math.Ceiling(projectile.width / 2f * projectile.scale);
            DBZMOD.Circle.ApplyShader(radius);
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}