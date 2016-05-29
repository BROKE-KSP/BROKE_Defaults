using BROKE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BudgetFunding
{
    public class BudgetFunding : IFundingModifier
    {
        private double base_funding = 200000;
        private double rep_funding = 2000;
        public InvoiceItem ProcessQuarterly()
        {
            //We will only give up to a specified amount (the yearly total)
            double MaxBudget = base_funding + rep_funding * Math.Max(Reputation.CurrentRep, 0);
            //Past that amount we give nothing
            double income = Math.Max(Math.Min(MaxBudget / 4, MaxBudget - Funding.Instance.Funds), 0);
            var invoice = new InvoiceItem(this, income, 0, "Quarterly Budget");
            return invoice;
        }

        public InvoiceItem ProcessYearly()
        {
            return null;
        }

        public void DailyUpdate()
        {
            
        }

        public void DrawMainGUI()
        {
            
        }

        public void DrawSettingsGUI()
        {
            GUILayout.Label("Settings:");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Base funds (yearly): ");
            Double.TryParse(GUILayout.TextField(base_funding.ToString(), 10), out base_funding);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Reputation funds (yearly): ");
            Double.TryParse(GUILayout.TextField(rep_funding.ToString(), 10), out rep_funding);
            GUILayout.EndHorizontal();

            GUILayout.Label("Yearly: " + GetIncome(false, Reputation.CurrentRep, true));
            GUILayout.Label("Quarterly: " + GetIncome(true, Reputation.CurrentRep, true));
            GUILayout.Label("Current Estimate: " + GetIncome(true, Reputation.CurrentRep, false));
        }

        public double GetIncome(bool Quarter=true, float rep=-1, bool IgnoreCurrentFunds=false)
        {
            if (rep == -1)
                rep = Reputation.CurrentRep;

            double yearly = base_funding + rep_funding * Math.Max(rep, 0);
            double income = yearly;

            if (Quarter)
                income = yearly / 4;


            if (!IgnoreCurrentFunds)
                income = Math.Max(Math.Min(income, yearly - Funding.Instance.Funds), 0);

            return income;
        }

        public string GetConfigName()
        {
            return "BudgetFunding";
        }

        public string GetName()
        {
            return "Quarterly Budget";
        }

        public void OnDisabled()
        {
            
        }

        public void OnEnabled()
        {
            
        }

        public void OnInvoicePaid(object sender, InvoiceItem.InvoicePaidEventArgs args)
        {
            
        }

        public void OnInvoiceUnpaid(object sender, EventArgs args)
        {
            
        }

        public ConfigNode SaveData()
        {
            //Save the settings to the save file
            ConfigNode settings = new ConfigNode();
            settings.AddValue("base_funding", base_funding);
            settings.AddValue("rep_funding", rep_funding);
            return settings;
        }

        public void LoadData(ConfigNode node)
        {
            //Load the base_funding and rep_funding info from the save file
            double.TryParse(node.GetValue("base_funding"), out base_funding);
            double.TryParse(node.GetValue("rep_funding"), out rep_funding);
        }

        public bool hasMainGUI()
        {
            return false;
        }

        public bool hasSettingsGUI()
        {
            return true;
        }
    }
}
