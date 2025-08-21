using System;
using System.Collections.Generic;
using System.Linq;

namespace GenderMayhem.Actions
{
    [Serializable]
    public class ActionEventsGroup
    {
        public List<ActionEvent> ActionEvents = new();

        public bool InvokeSuitableActions(Enum value)
        {
            var actions = ActionEvents.Where(x => x.Action.ToString() == value.ToString())?.ToList();
            actions?.ForEach(x => x.Event.Invoke());
            return actions?.Any() ?? false;
        }
    }
}