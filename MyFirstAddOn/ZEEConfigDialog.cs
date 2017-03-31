using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tricentis.TCAddOns;
using Tricentis.TCAPIObjects.Objects;

namespace ZephyrAddOn
{
    class ZEEConfigDialog : TCAddOnOptionsDialogEntry
    {

        protected override string DisplayedName
        {
            get
            {
                return "ZEE Settings";
            }
        }

        protected override string SettingName
        {
            get
            {
               return "ZEE Settings here";
            }
        }
    }
}
