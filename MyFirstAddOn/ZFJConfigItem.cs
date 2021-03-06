﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tricentis.TCAddOns;
using Tricentis.TCAPIObjects;
using Tricentis.TCAPIObjects.Objects;

namespace ZephyrAddOn
{
    class ZFJConfigItem : TCAddOnMenuItem
    {

        public override void Execute(TCAddOnTaskContext context)
        {
           context.ShowMessageBox("Configuration", "You clicked on Configuration menu item.");
        }

        public override string ID => "ZFJConfig";
        public override string MenuText => "ZEE Config";

        public override string MenuItemImageName => "zephyr.ico";

    }
}
