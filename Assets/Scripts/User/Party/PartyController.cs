using System.Collections.Generic;
using Assets.Scripts.Misc.GUI;
using UnityEngine;

namespace Assets.Scripts.User.Party
{
    /// <summary>
    /// Shows list of party members, their icon, health and magic
    /// </summary>
    public class PartyController : UI
    {
        public List<IPartyMember> PartyMembers { get; set; } = new List<IPartyMember>();

        public override void GuiDraw()
        {
            if (PartyMembers.Count == 0)
                return;

            base.GuiDraw();
        }

        protected override void Design()
        {
            GUI.BeginGroup(new Rect(ScreenSize.x - 190, 20, 170, 55 * PartyMembers.Count));

            for (var i = 0; i < PartyMembers.Count; i++)
            {
                GUI.BeginGroup(new Rect(0, 55 * i, 200, 45));

                // Icon
                GUI.Box(new Rect(125, 0, 45, 45), PartyMembers[i].Design, "party_member_bg");

                // Health bar
                GUI.Box(new Rect(0, 5, 120, 15), "", "party_stats_bar_bg");
                GUI.Box(new Rect(5, 10, 110 * PartyMembers[i].HealthMlt, 5), "", "hp_bar_fg");

                // Magic bar
                GUI.Box(new Rect(0, 25, 120, 15), "", "party_stats_bar_bg");
                GUI.Box(new Rect(5, 30, 110 * PartyMembers[i].MagicMlt, 5), "", "mp_bar_fg");

                GUI.EndGroup();
            }

            GUI.EndGroup();
        }
    }
}