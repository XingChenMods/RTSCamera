using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace RTSCamera.CommandSystem.CampaignGame
{
    public class CommandSystemSkillBehavior : CampaignBehaviorBase
    {
        public static int RequiredTacticsLevelToIssueChargeToFormationOrder = 25;
        public override void RegisterEvents()
        {
            CampaignEvents.HeroGainedSkill.AddNonSerializedListener(this, OnHeroGainedSKill);
            CampaignEvents.OnMissionStartedEvent.AddNonSerializedListener(this, OnMissionStarted);
        }

        private void OnMissionStarted(IMission mission)
        {
            Update();
        }

        private void OnHeroGainedSKill(Hero hero, SkillObject skill, int change, bool shouldShowNotify)
        {
            if (Mission.Current != null && hero == GetHeroForTacticLevel())
            {
                Update();
            }
        }

        public override void SyncData(IDataStore dataStore)
        {
        }

        public static bool CanIssueChargeToFormationOrder = true;

        public static void Update()
        {
            CanIssueChargeToFormationOrder = CheckCanIssueChargeToFormationOrder();
        }
        private static bool CheckCanIssueChargeToFormationOrder()
        {
            return true;
        }
        public static Hero GetHeroForTacticLevel()
        {
            return Campaign.Current?.MainParty?.GetEffectiveRoleHolder(SkillEffect.PerkRole.PartyLeader) ??
                   (Game.Current?.PlayerTroop == null ? null : Hero.MainHero);
        }
    }
}
