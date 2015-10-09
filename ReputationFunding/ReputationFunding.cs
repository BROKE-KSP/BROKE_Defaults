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
            GUILayout.Label("Hello, World!");
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
            base_funding = Single.Parse(GUILayout.TextField(base_funding.ToString(), 10));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Base Reputation Funding (per point/year): ");
            rep_funding = Single.Parse(GUILayout.TextField(rep_funding.ToString(), 10));
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
            float f = Math.Max((int)Math.Ceiling((base_funding + (rep_funding * Reputation.CurrentRep))), 0);
            var invoice = new InvoiceItem(this, f, 0);
            return invoice;
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
