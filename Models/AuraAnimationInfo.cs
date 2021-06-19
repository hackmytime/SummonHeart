using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ModLoader;

namespace SummonHeart.Models
{
    public class AuraAnimationInfo
    {
        public string auraAnimationSpriteName;
        public int frames;
        public int frameTimerLimit;
        public int priority;
        public BlendState blendState;
        public string startupSoundName;
        public string loopSoundName;
        public int loopSoundDuration;
        public bool isKaiokenAura;
        public bool isFormAura;
        public bool isStarting;
        public int startingFrames;
        public int startingFrameCounter;
        public DustDelegate doStartingDust;
        public DustDelegate doDust;
        public delegate void DustDelegate(SummonHeartPlayer modPlayer, AuraAnimationInfo info);

        public AuraAnimationInfo()
        {
        }

        public AuraAnimationInfo(string spriteName, int frames, int frameTimer, BlendState blendState, string startupSound, string loopSoundName, int loopSoundDuration, bool isForm, bool isKaioken, DustDelegate dustDelegate, int startingFrames, DustDelegate startingDustDelegate, int priority)
        {
            auraAnimationSpriteName = spriteName;
            this.frames = frames;
            frameTimerLimit = frameTimer;
            this.blendState = blendState;
            startupSoundName = startupSound;
            this.loopSoundName = loopSoundName;
            this.loopSoundDuration = loopSoundDuration;
            isKaiokenAura = isKaioken;
            isFormAura = isForm;
            doDust = dustDelegate;
            this.startingFrames = startingFrames;
            startingFrameCounter = 0;
            doStartingDust = startingDustDelegate;
            this.priority = priority;
        }

        public Texture2D GetTexture()
        {
            Mod mod = ModLoader.GetMod("SummonHeart");
            return mod.GetTexture(auraAnimationSpriteName);
        }

        public int GetHeight()
        {
            return GetTexture().Height / frames;
        }

        public int GetWidth()
        {
            return GetTexture().Width;
        }

        public Tuple<float, Vector2> GetAuraRotationAndPosition(SummonHeartPlayer modPlayer)
        {
            // update handler to reorient the charge up aura after the aura offsets are defined.
            bool isPlayerMostlyStationary = Math.Abs(modPlayer.player.velocity.X) <= 6F && Math.Abs(modPlayer.player.velocity.Y) <= 6F;
            float rotation = 0f;
            Vector2 position = Vector2.Zero;
            float scale = GetAuraScale(modPlayer);
            int auraOffsetY = GetAuraOffsetY(modPlayer);

            position = modPlayer.player.Center + new Vector2(0f, auraOffsetY);
            rotation = 0f;
            return new Tuple<float, Vector2>(rotation, position);
        }

        public void ProcessDust(SummonHeartPlayer modPlayer)
        {
            if (doDust != null)
                doDust(modPlayer, this);
        }

        public void ProcessStartingDust(SummonHeartPlayer modPlayer)
        {
            if (doStartingDust != null)
                doStartingDust(modPlayer, this);
        }

        public Vector2 GetCenter(SummonHeartPlayer modPlayer)
        {
            return GetAuraRotationAndPosition(modPlayer).Item2 + new Vector2(GetWidth(), GetHeight()) * 0.5f;
        }

        public int GetAuraOffsetY(SummonHeartPlayer modPlayer)
        {
            var frameHeight = GetHeight();
            var scale = GetAuraScale(modPlayer);
            // easy automatic aura offset.
            return (int)-(frameHeight / 2 * scale - modPlayer.player.height * 0.6f);
        }

        public float GetAuraScale(SummonHeartPlayer modPlayer)
        {
            // universal scale handling
            // scale is based on kaioken level, which gets set to 0
            var baseScale = 1.0f;
            int totalBloodGas = modPlayer.eyeBloodGas + modPlayer.handBloodGas + modPlayer.bodyBloodGas + modPlayer.footBloodGas;
            
            return baseScale * 0.5f * (2.5f * totalBloodGas / 400000 + 1);
        }
    }
}
