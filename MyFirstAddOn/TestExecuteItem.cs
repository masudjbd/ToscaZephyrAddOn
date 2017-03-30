using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tricentis.TCAddOns;

namespace ZephyrAddOn
{
    class TestExecuteItem : TCAddOnMenuItem
    {

        public override void Execute(TCAddOnTaskContext context)
        {
             context.ShowMessageBox("Test Execute", "You clicked on a menu item.");
        }
        public override string ID => "TestExecute";
        public override string MenuText => "Test Execute";

    }
}
