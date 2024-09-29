﻿using System;
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
        public static void EditJolt()
        {
            const string joltGuid = "16e23c7a8ae53cc42a93066d19766404";

            try
            {
                var jolt = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>(joltGuid);
                jolt.m_Description = CreateLocalizedString("You cause a spark of electricity to strike the target with a successful ranged touch attack. The spell deals 1d3 + half of your caster level (max 10) points of electricity damage.");
                var contextRankConfig = new ContextRankConfigBuilder
                {
                    BaseValueType = ContextRankBaseValueType.CasterLevel,
                    Type = AbilityRankType.DamageBonus,
                    Progression = ContextRankProgression.Div2,
                    Max = 10
                }.Build();
                jolt.AddComponent(contextRankConfig);
                var runAction = jolt.GetComponent<AbilityEffectRunAction>();
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

                Main.Logger.Log($"Successfully installed Jolt edit!");
            }
            catch (Exception e)
            {
                Main.Logger.Error($"Error when trying to edit Jolt spell! {e.Message}, {e.StackTrace}");
            }
        }
    }
}
