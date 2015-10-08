using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BROKE;

namespace BROKE_Payroll
{
    public class Payroll : IFundingModifier
    {
        private int threshold = 50;
        private int level0 = 10;
        private int level1 = 20;
        private int level2 = 40;
        private int level3 = 80;
        private int level4 = 140;
        private int level5 = 200;
        private float standbyPct = 50;

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
            return false;
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

        public float GetWages(int level, string status)
        {
            float w = level0;
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
            if (status == "Available")
            {
                float pBuf = w / 100;
                w = pBuf * standbyPct;
            }
            return w;
        }

        public InvoiceItem ProcessQuarterly()
        {
            float bill = 0f;
            foreach (ProtoCrewMember crewMember in HighLogic.CurrentGame.CrewRoster.Crew)
            {
                if (!crewMember.rosterStatus.Equals(ProtoCrewMember.RosterStatus.Dead) && !crewMember.rosterStatus.Equals(ProtoCrewMember.RosterStatus.Missing))
                {
                    float paycheck = (int)Math.Round(GetWages(crewMember.experienceLevel, crewMember.rosterStatus.ToString()) * 106.5);
                    bill += paycheck;
                }
            }
            var invoice = new InvoiceItem(this, 0, bill);
            return invoice;
        }

        public InvoiceItem ProcessYearly()
        {
            float bill = 0f;
            foreach (ProtoCrewMember crewMember in HighLogic.CurrentGame.CrewRoster.Crew)
            {
                if (!crewMember.rosterStatus.Equals(ProtoCrewMember.RosterStatus.Dead) && !crewMember.rosterStatus.Equals(ProtoCrewMember.RosterStatus.Missing))
                {
                    float paycheck = (int)Math.Round(GetWages(crewMember.experienceLevel, crewMember.rosterStatus.ToString()) * 426);
                    bill += paycheck;
                }
            }
            var invoice = new InvoiceItem(this, 0, bill);
            return invoice;
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

        }
    }
}
