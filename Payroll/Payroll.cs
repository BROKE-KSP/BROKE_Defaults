using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BROKE;

namespace BROKE_Payroll
{
    public class Payroll : IMultiFundingModifier
    {
        private const int threshold = 50;
        private const int level0 = 10;
        private const int level1 = 20;
        private const int level2 = 40;
        private const int level3 = 80;
        private const int level4 = 140;
        private const int level5 = 200;
        private const double standbyPct = 50;

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
            return false;
        }

        public void DrawSettingsGUI()
        {
            
        }

        public void DailyUpdate()
        {

        }

        public static double GetWages(int level, ProtoCrewMember.RosterStatus status)
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
            return null;
        }

        public void LoadData(ConfigNode node)
        {

        }

        public void OnInvoicePaid(object sender, InvoiceItem.InvoicePaidEventArgs args)
        {

        }

        public void OnInvoiceUnpaid(object sender, EventArgs args)
        {
            var unpaidCrewMember = HighLogic.CurrentGame.CrewRoster.Crew.First(crew => crew.name == ((InvoiceItem)sender).ItemName);
            //TODO: What to do when crew member is unpaid?
        }
    }
}
