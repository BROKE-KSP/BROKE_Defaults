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
        float BASE_REP_FUNDING_YEARLY = 990.0f;
        float BASE_FUNDING_YEARLY = 10000.0f;

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
            BASE_FUNDING_YEARLY = Single.Parse(GUILayout.TextField(BASE_FUNDING_YEARLY.ToString(), 10));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Base Reputation Funding (per point/year): ");
            BASE_REP_FUNDING_YEARLY = Single.Parse(GUILayout.TextField(BASE_REP_FUNDING_YEARLY.ToString(), 10));
            GUILayout.EndHorizontal();
        }

        public void DailyUpdate()
        {

        }

        public InvoiceItem ProcessQuarterly()
        {
            float f = Math.Max((int)Math.Ceiling(((BASE_FUNDING_YEARLY / 4) + ((BASE_REP_FUNDING_YEARLY / 4) * Reputation.CurrentRep))), 0);
            var invoice = new InvoiceItem(this, f, 0);
            return invoice;
        }

        public InvoiceItem ProcessYearly()
        {
            float f = Math.Max((int)Math.Ceiling((BASE_FUNDING_YEARLY + (BASE_REP_FUNDING_YEARLY * Reputation.CurrentRep))), 0);
            var invoice = new InvoiceItem(this, f, 0);
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
