using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tricentis.TCAddOns;
using Tricentis.TCAPIObjects.Objects;

namespace ZephyrAddOn
{
    public class DecoratedRunTask : TCAddOnTask
    {

        public override TCObject Execute(TCObject objectToExecuteOn, TCAddOnTaskContext taskContext)
        {

            taskContext.ShowMessageBox("Attention", "This entry will be run via an AddOnTask");
            (objectToExecuteOn as ExecutionList).Run();

            return null;

        }



        public override string Name => "Execute";

        public override Type ApplicableType => typeof(ExecutionList);

        public override bool IsTaskPossible(TCObject obj) => obj.DisplayedName.StartsWith("Create");

        public override bool RequiresChangeRights => true;

    }
}
