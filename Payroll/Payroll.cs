using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BROKE;

namespace BROKE_Payroll
{
    public class Payroll : IFundingModifier
    {

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

        public InvoiceItem ProcessQuarterly()
        {
            return null;
        }

        public InvoiceItem ProcessYearly()
        {
            return null;
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
