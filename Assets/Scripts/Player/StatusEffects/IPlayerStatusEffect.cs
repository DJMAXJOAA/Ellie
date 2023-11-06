﻿using Assets.Scripts.Player;

namespace Assets.Scripts.StatusEffects
{
    public interface IPlayerStatusEffect
    {
        public void ApplyStatusEffect(PlayerStatusEffectController controller, StatusEffectInfo info);
        
    }
}
