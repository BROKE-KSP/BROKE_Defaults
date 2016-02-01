using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP;
using UnityEngine;
using BROKE;

namespace BROKE_Payroll
{
    public class Payroll : IMultiFundingModifier
    {
        private int level0 = 10;
        private int level1 = 20;
        private int level2 = 40;
        private int level3 = 80;
        private int level4 = 140;
        private int level5 = 200;
        private double standbyPct = 50;
        private Dictionary<string, int> unpaidCrew = new Dictionary<string,int>();

        public Payroll()
        {
            // Initialize the unpaid crew info for all possible kerbals at construction time.
            // This way we avoid the KeyNotFoundExceptions that were cropping up when I didn't do this here
            foreach (ProtoCrewMember crewMember in HighLogic.CurrentGame.CrewRoster.Crew)
            {
                unpaidCrew[crewMember.name] = 0;
            }
            foreach (ProtoCrewMember crewMember in HighLogic.CurrentGame.CrewRoster.Unowned)
            {
                unpaidCrew[crewMember.name] = 0;
            }
            foreach (ProtoCrewMember crewMember in HighLogic.CurrentGame.CrewRoster.Applicants)
            {
                unpaidCrew[crewMember.name] = 0;
            }
            foreach (ProtoCrewMember crewMember in HighLogic.CurrentGame.CrewRoster.Tourist)
            {
                unpaidCrew[crewMember.name] = 0;
            }
        }

        public string GetName()
        {
            return "Payroll";
        }

        public string GetConfigName()
        {
            return "Payroll";
        }

        public void OnEnabled()
        {

        }

        public void OnDisabled()
        {

        }

        public bool hasMainGUI()
        {
            return true;
        }

        public void DrawMainGUI()
        {

        }

        public bool hasSettingsGUI()
        {
            return true;
        }

        public void DrawSettingsGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Level 0 Daily Wages: ");
            GUILayout.FlexibleSpace();
            level0 = Convert.ToInt32(GUILayout.TextField(level0.ToString(), 4, GUILayout.Width(50)));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Level 1 Daily Wages: ");
            GUILayout.FlexibleSpace();
            level1 = Convert.ToInt32(GUILayout.TextField(level1.ToString(), 4, GUILayout.Width(50)));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Level 2 Daily Wages: ");
            GUILayout.FlexibleSpace();
            level2 = Convert.ToInt32(GUILayout.TextField(level2.ToString(), 4, GUILayout.Width(50)));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Level 3 Daily Wages: ");
            GUILayout.FlexibleSpace();
            level3 = Convert.ToInt32(GUILayout.TextField(level3.ToString(), 4, GUILayout.Width(50)));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Level 4 Daily Wages: ");
            GUILayout.FlexibleSpace();
            level4 = Convert.ToInt32(GUILayout.TextField(level4.ToString(), 4, GUILayout.Width(50)));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Level 5 Daily Wages: ");
            GUILayout.FlexibleSpace();
            level5 = Convert.ToInt32(GUILayout.TextField(level5.ToString(), 4, GUILayout.Width(50)));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Percentage of Daily Wage for Standby: ");
            GUILayout.FlexibleSpace();
            standbyPct = Convert.ToInt32(GUILayout.TextField(standbyPct.ToString(), 4, GUILayout.Width(50)));
            GUILayout.EndHorizontal();
        }

        public void DailyUpdate()
        {

        }

        public double GetWages(int level, ProtoCrewMember.RosterStatus status)
        {
            double w;
            switch (level)
            {
                case 0:
                    w = level0;
                    break;
                case 1:
                    w = level1;
                    break;
                case 2:
                    w = level2;
                    break;
                case 3:
                    w = level3;
                    break;
                case 4:
                    w = level4;
                    break;
                case 5:
                    w = level5;
                    break;
                default:
                    w = 10;
                    break;
            }
            if (status == ProtoCrewMember.RosterStatus.Available)
            {
                double pBuf = w / 100;
                w = pBuf * standbyPct;
            }
            return w;
        }

        public IEnumerable<InvoiceItem> ProcessQuarterly()
        {
            foreach (ProtoCrewMember crewMember in HighLogic.CurrentGame.CrewRoster.Crew)
            {
                if (!crewMember.rosterStatus.Equals(ProtoCrewMember.RosterStatus.Dead) && !crewMember.rosterStatus.Equals(ProtoCrewMember.RosterStatus.Missing))
                {
                    double paycheck = Math.Round(GetWages(crewMember.experienceLevel, crewMember.rosterStatus) * 106.5, MidpointRounding.AwayFromZero);
                    yield return new InvoiceItem(this, 0, paycheck, crewMember.name);
                }
            }
        }

        public IEnumerable<InvoiceItem> ProcessYearly()
        {
            foreach (ProtoCrewMember crewMember in HighLogic.CurrentGame.CrewRoster.Crew)
            {
                if (!crewMember.rosterStatus.Equals(ProtoCrewMember.RosterStatus.Dead) && !crewMember.rosterStatus.Equals(ProtoCrewMember.RosterStatus.Missing))
                {
                    double paycheck = Math.Round(GetWages(crewMember.experienceLevel, crewMember.rosterStatus) * 426, MidpointRounding.AwayFromZero);
                    yield return new InvoiceItem(this, 0, paycheck, crewMember.name);
                }
            }
        }

        public ConfigNode SaveData()
        {
            ConfigNode settings = new ConfigNode();
            settings.AddValue("level0", level0);
            settings.AddValue("level1", level1);
            settings.AddValue("level2", level2);
            settings.AddValue("level3", level3);
            settings.AddValue("level4", level4);
            settings.AddValue("level5", level5);
            settings.AddValue("standByPct", standbyPct);
            foreach (var unpaid in unpaidCrew)
            {
                var node = new ConfigNode("Unpaid");
                node.AddValue("Name", unpaid.Key);
                node.AddValue("NumberUnpaid", unpaid.Value.ToString());
                settings.AddData(node);
            }
            return settings;
        }

        public void LoadData(ConfigNode node)
        {
            if (node.HasValue("level0")) level0 = (Int32)Int32.Parse(node.GetValue("level0"));
            if (node.HasValue("level1")) level1 = (Int32)Int32.Parse(node.GetValue("level1"));
            if (node.HasValue("level2")) level2 = (Int32)Int32.Parse(node.GetValue("level2"));
            if (node.HasValue("level3")) level3 = (Int32)Int32.Parse(node.GetValue("level3"));
            if (node.HasValue("level4")) level4 = (Int32)Int32.Parse(node.GetValue("level4"));
            if (node.HasValue("level5")) level5 = (Int32)Int32.Parse(node.GetValue("level5"));
            if (node.HasValue("standByPct")) standbyPct = (Int32)Int32.Parse(node.GetValue("standByPct"));
            foreach (var unpaidNode in node.GetNodes("Unpaid"))
            {
                unpaidCrew.Add(HighLogic.CurrentGame.CrewRoster.Unowned.First(crew => crew.name == unpaidNode.GetValue("name")).name,
                    int.Parse(node.GetValue("NumberUnpaid")));
            }
        }

        public void OnInvoicePaid(object sender, InvoiceItem.InvoicePaidEventArgs args)
        {
            if (args.PaidInFull)
            {
                var unpaidCrewMember = HighLogic.CurrentGame.CrewRoster.Unowned.FirstOrDefault(crew => crew.name == ((InvoiceItem)sender).ItemName);
                if (unpaidCrewMember == null) return;
                unpaidCrew[unpaidCrewMember.name]--;
                if(unpaidCrew[unpaidCrewMember.name] <= 0)
                {
                    unpaidCrew[unpaidCrewMember.name] = 0;
                    unpaidCrewMember.type = ProtoCrewMember.KerbalType.Crew;
                }
            }

        }

        public void OnInvoiceUnpaid(object sender, EventArgs args)
        {
            var crewName = ((InvoiceItem)sender).ItemName;
            var unpaidCrewMember = HighLogic.CurrentGame.CrewRoster.Crew.FirstOrDefault(crew => crew.name == crewName);
            if (unpaidCrewMember != null)
            {
                unpaidCrewMember.type = ProtoCrewMember.KerbalType.Unowned; 
            }
            unpaidCrew[unpaidCrewMember.name] = unpaidCrew[unpaidCrewMember.name] + 1;
        }
    }
}
