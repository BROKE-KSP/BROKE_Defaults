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

        private Dictionary<string, double> runningCrewSalaries = new Dictionary<string, double>();

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
            standbyPct = Convert.ToDouble(GUILayout.TextField(standbyPct.ToString(), 4, GUILayout.Width(50)));
            GUILayout.EndHorizontal();
        }

        public void DailyUpdate()
        {
            //Every crewmember gets their daily wages factored in
            foreach (ProtoCrewMember crewMember in HighLogic.CurrentGame.CrewRoster.Crew)
            {
                if (!crewMember.rosterStatus.Equals(ProtoCrewMember.RosterStatus.Dead) && !crewMember.rosterStatus.Equals(ProtoCrewMember.RosterStatus.Missing))
                {
                    double paycheck = GetWages(crewMember.experienceLevel, crewMember.rosterStatus);
                    if (!runningCrewSalaries.ContainsKey(crewMember.name))
                    {
                        runningCrewSalaries.Add(crewMember.name, paycheck);
                    }
                    else
                    {
                        runningCrewSalaries[crewMember.name] += paycheck;
                    }
                }
            }
            foreach (ProtoCrewMember crewMember in HighLogic.CurrentGame.CrewRoster.Unowned.Where(crew => unpaidCrew.ContainsKey(crew.name) && unpaidCrew[crew.name] > 0))
            {
                if (!crewMember.rosterStatus.Equals(ProtoCrewMember.RosterStatus.Dead) && !crewMember.rosterStatus.Equals(ProtoCrewMember.RosterStatus.Missing))
                {
                    double paycheck = GetWages(crewMember.experienceLevel, crewMember.rosterStatus);
                    if (!runningCrewSalaries.ContainsKey(crewMember.name))
                    {
                        runningCrewSalaries.Add(crewMember.name, paycheck);
                    }
                    else
                    {
                        runningCrewSalaries[crewMember.name] += paycheck;
                    }
                }
            }
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
            foreach (KeyValuePair<string, double> wageSheet in runningCrewSalaries)
            {
                yield return new InvoiceItem(this, 0, wageSheet.Value, wageSheet.Key);
            }
            runningCrewSalaries.Clear();
        }

        public IEnumerable<InvoiceItem> ProcessYearly()
        {
            return null;
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

            ConfigNode currentSalaries = new ConfigNode();
            foreach (KeyValuePair<string, double> salaries in runningCrewSalaries)
            {
                currentSalaries.AddValue(salaries.Key, salaries.Value);
            }
            settings.AddNode("RunningCrewSalaries", currentSalaries);

            var node = new ConfigNode();
            foreach (var unpaid in unpaidCrew)
            {
                node.AddValue(unpaid.Key, unpaid.Value);
            }
            settings.AddNode("Unpaid", node);
            return settings;
        }

        public void LoadData(ConfigNode node)
        {
            
            if (node.HasValue("level0")) level0 = Int32.Parse(node.GetValue("level0"));
            if (node.HasValue("level1")) level1 = Int32.Parse(node.GetValue("level1"));
            if (node.HasValue("level2")) level2 = Int32.Parse(node.GetValue("level2"));
            if (node.HasValue("level3")) level3 = Int32.Parse(node.GetValue("level3"));
            if (node.HasValue("level4")) level4 = Int32.Parse(node.GetValue("level4"));
            if (node.HasValue("level5")) level5 = Int32.Parse(node.GetValue("level5"));
            if (node.HasValue("standByPct")) standbyPct = (double)double.Parse(node.GetValue("standByPct"));

            runningCrewSalaries.Clear();
            ConfigNode salariesNode = node.GetNode("RunningCrewSalaries");
            foreach (ConfigNode.Value value in salariesNode.values)
            {
                runningCrewSalaries.Add(value.name, double.Parse(value.value));
            }

            unpaidCrew.Clear();
            ConfigNode unpaidNode = node.GetNode("Unpaid");
            foreach (ConfigNode.Value value in unpaidNode.values)
            {
                unpaidCrew.Add(value.name, int.Parse(value.value));
            }
        }

        public void OnInvoicePaid(object sender, InvoiceItem.InvoicePaidEventArgs args)
        {
            if (args.PaidInFull)
            {
                string crewName = ((InvoiceItem)sender).ItemName;
                var unpaidCrewMember = HighLogic.CurrentGame.CrewRoster.Unowned.FirstOrDefault(crew => crew.name == crewName);
                if (unpaidCrew.ContainsKey(crewName))
                {
                    unpaidCrew[crewName]--;
                    if (unpaidCrew[crewName] <= 0)
                    {
                        unpaidCrew[crewName] = 0;
                        if (unpaidCrewMember != null)
                        {
                            unpaidCrewMember.type = ProtoCrewMember.KerbalType.Crew;
                        }
                    }
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
            if (unpaidCrew.ContainsKey(crewName))
            {
                unpaidCrew[crewName]++;
            }
            else
            {
                unpaidCrew.Add(crewName, 1);
            }
        }
    }
}
