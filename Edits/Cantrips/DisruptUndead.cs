using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using UnboundedArcana.Extensions;
using UnboundedArcana.Utilities.Builders;
using static UnboundedArcana.Utilities.OwlcatUtilites;


namespace UnboundedArcana.Edits
{
    partial class Cantrips
    {
        public static void EditDisruptUndead()
        {
            const string disruptUndeadGuid = "652739779aa05504a9ad5db1db6d02ae";

            try
            {
                var disruptUndead = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>(disruptUndeadGuid);
                disruptUndead.m_Description = CreateLocalizedString("You direct a ray of positive energy. You must make a ranged touch attack to hit, and if the ray hits an undead creature it deals 1d6 + half your caster level (max 10) points of positive energy damage.");
                var contextRankConfig = new ContextRankConfigBuilder
                {
                    BaseValueType = ContextRankBaseValueType.CasterLevel,
                    Type = AbilityRankType.DamageBonus,
                    Progression = ContextRankProgression.Div2,
                    Max = 10
                }.Build();
                disruptUndead.AddComponent(contextRankConfig);
                var runAction = disruptUndead.GetComponent<AbilityEffectRunAction>();
                var dealDamageAction = runAction.Actions.Actions.FirstOfType<ContextActionDealDamage>();
                dealDamageAction.Value.BonusValue = new ContextValue
                {
                    ValueType = ContextValueType.Rank,
                    ValueRank = AbilityRankType.DamageBonus
                };

                Main.Logger.Log($"Successfully installed Disrupt Undead edit!");
            }
            catch (Exception e)
            {
                Main.Logger.Error($"Error when trying to edit Disrupt Undead spell! {e.Message}, {e.Source}");
            }
        }
    }
}
