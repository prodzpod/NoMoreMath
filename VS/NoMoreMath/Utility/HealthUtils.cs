﻿using RoR2;

namespace NoMoreMath.Utility
{
    public static class HealthUtils
    {
        public static bool IsInvincible(CharacterBody characterBody)
        {
            HealthComponent healthComponent = characterBody.healthComponent;
            if (!healthComponent || healthComponent.godMode)
                return true;

            if (characterBody.HasBuff(DLC1Content.Buffs.BearVoidReady))
                return true;

            if (characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility))
                return true;

            if (characterBody.HasBuff(RoR2Content.Buffs.Immune))
                return true;

            if (characterBody.HasBuff(JunkContent.Buffs.BodyArmor))
                return true;

            return false;
        }

        public static float CalculateEffectiveHealth(CharacterBody characterBody, float health)
        {
            if (!characterBody)
                return health;

            if (IsInvincible(characterBody))
                return float.PositiveInfinity;

            float effectiveHealth = health;
            if (characterBody.HasBuff(RoR2Content.Buffs.DeathMark))
            {
                effectiveHealth /= 1.5f;
            }

            HealthComponent healthComponent = characterBody.healthComponent;

            float adaptiveArmor;
            if (healthComponent)
            {
#pragma warning disable Publicizer001 // Accessing a member that was not originally public
                adaptiveArmor = healthComponent.adaptiveArmorValue;
#pragma warning restore Publicizer001 // Accessing a member that was not originally public
            }
            else
            {
                adaptiveArmor = 0f;
            }

            float armor = characterBody.armor + adaptiveArmor;

            float damageMultiplierFromArmor = armor >= 0f ? 1f - (armor / (armor + 100f))
                                                          : 2f - (100f / (100f - armor));

            effectiveHealth /= damageMultiplierFromArmor;

            return effectiveHealth;
        }
    }
}
