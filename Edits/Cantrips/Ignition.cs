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
        public static void EditIgnition()
        {
            const string ignitionGuid = "564c2ac83c7844beb1921e69ab159ac6";

            try
            {
                var ignition = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>(ignitionGuid);
                ignition.m_Description = CreateLocalizedString("You cause a spark of fire to strike the target with a successful ranged touch attack. The spell deals 1d3 + half of your caster level (max 10) points of fire energy damage.");
                var contextRankConfig = new ContextRankConfigBuilder
                {
                    BaseValueType = ContextRankBaseValueType.CasterLevel,
                    Type = AbilityRankType.DamageBonus,
                    Progression = ContextRankProgression.Div2,
                    Max = 10
                }.Build();
                ignition.AddComponent(contextRankConfig);
                var runAction = ignition.GetComponent<AbilityEffectRunAction>();
                var runActionConditional = runAction.Actions.Actions.FirstOfType<Conditional>();

                var dealDamageActionIfFalse = runActionConditional.IfFalse.Actions.FirstOfType<ContextActionDealDamage>();
                dealDamageActionIfFalse.Value.BonusValue = new ContextValue
                {
                    ValueType = ContextValueType.Rank,
                    ValueRank = AbilityRankType.DamageBonus
                };

                var dealDamageActionIfTrue = runActionConditional.IfTrue.Actions.FirstOfType<ContextActionDealDamage>();
                dealDamageActionIfTrue.Value.BonusValue = new ContextValue
                {
                    ValueType = ContextValueType.Rank,
                    ValueRank = AbilityRankType.DamageBonus
                };

                Main.Logger.Log($"Successfully installed Ignition edit!");
            }
            catch (Exception e)
            {
                Main.Logger.Error($"Error when trying to edit Ignition spell! {e.Message}, {e.StackTrace}");
            }
        }
    }
}
