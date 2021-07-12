using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace SummonHeart.Sounds
{
    public abstract class BaseSound : ModSound
    {
        public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
        {
            soundInstance = sound.CreateInstance();
            soundInstance.Volume = volume * 0.5f;
            return soundInstance;
        }
    }
}