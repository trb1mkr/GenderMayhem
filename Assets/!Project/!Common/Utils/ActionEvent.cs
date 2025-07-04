using System;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

namespace GenderMayhem.Actions
{
    public enum PlayerInputAction
    {
        Use,
        UseCanceled,
        AltUse
    }

    [Serializable]
    public class ActionEvent
    {
        [ShowInInspector, ValueDropdown("GetEnumTypesInNamespace")]
        public Type ActionType { get; set; }

        [EnableIf("@ActionType != null && ActionType.IsEnum")]
        [ShowInInspector, ValueDropdown("GetEnumValues")]
        public Enum Action { get; set; }

        public UnityEvent Event = new();

        public ActionEvent(Type actionType, Enum action, List<UnityAction> delegates)
        {
            ActionType = actionType;
            Action = action;
            foreach (var method in delegates)
                Event.AddListener(method);
        }

        private IEnumerable<Type> GetEnumTypesInNamespace()
        {
            string targetNamespace = "GenderMayhem.Actions";

            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsEnum && type.Namespace == targetNamespace);
        }

        private IEnumerable<Enum> GetEnumValues()
        {
            if (ActionType != null && ActionType.IsEnum)
                return Enum.GetValues(ActionType).Cast<Enum>();

            return Enumerable.Empty<Enum>();
        }
    }

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