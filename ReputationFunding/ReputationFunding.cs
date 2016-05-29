using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP;
using UnityEngine;
using BROKE;

namespace BROKE_RepuationFunding
{
    public class ReputationFunding : IFundingModifier
    {
        float rep_funding = 990.0f;
        float base_funding = 10000.0f;

        public string GetName()
        {
            return "Gov. Funding";
        }

        public string GetConfigName()
        {
            return "ReputationFunding";
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
            //GUILayout.Label("Hello, World!");
        }

        public bool hasSettingsGUI()
        {
            return true;
        }

        public void DrawSettingsGUI()
        {
            GUILayout.Label("Funding Settings:");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Base Yearly Funding: ");
            //MM 5/29/16: Switched to TryParse from Parse as TryParse won't cause an error if it fails
            Single.TryParse(GUILayout.TextField(base_funding.ToString(), 10), out base_funding);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Base Reputation Funding (per point/year): ");
            //MM 5/29/16: Switched to TryParse from Parse as TryParse won't cause an error if it fails
            Single.TryParse(GUILayout.TextField(rep_funding.ToString(), 10), out rep_funding);
            GUILayout.EndHorizontal();
        }

        public void DailyUpdate()
        {

        }

        public InvoiceItem ProcessQuarterly()
        {
            float f = Math.Max((int)Math.Ceiling(((base_funding / 4) + ((rep_funding / 4) * Reputation.CurrentRep))), 0);
            var invoice = new InvoiceItem(this, f, 0);
            return invoice;
        }

        public InvoiceItem ProcessYearly()
        {
           // float f = Math.Max((int)Math.Ceiling((base_funding + (rep_funding * Reputation.CurrentRep))), 0);
           // var invoice = new InvoiceItem(this, f, 0);
           // return invoice;
            return null;
            //MM 5/29/16: This would cause the funding to be given twice, since the quarter is processed as well as the year
        }

        public ConfigNode SaveData()
        {
            ConfigNode settings = new ConfigNode();
            settings.AddValue("base_funding", base_funding);
            settings.AddValue("rep_funding", rep_funding);
            return settings;
        }

        public void LoadData(ConfigNode node)
        {
            float.TryParse(node.GetValue("base_funding"), out base_funding);
            float.TryParse(node.GetValue("rep_funding"), out rep_funding);
        }

        public void OnInvoicePaid(object sender, InvoiceItem.InvoicePaidEventArgs args)
        {

        }

        public void OnInvoiceUnpaid(object sender, EventArgs args)
        {

        }
    }
}
