using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TitleGroup = UnityEngine.HeaderAttribute;
using ReadOnly = Sparkfire.Utility.BlankAttribute;
using LabelText = Sparkfire.Utility.BlankAttribute;
using FoldoutGroup = Sparkfire.Utility.BlankAttribute;
using HideIf = Sparkfire.Utility.BlankAttribute;
using ShowIf = Sparkfire.Utility.BlankAttribute;
#endif

namespace AppStateSystem
{
    public enum TimeScaleControlOptions
    {
        None,
        Set,
        Revert
    }

    // ---------------------------------------------------------------------------------------------------------

    [CreateAssetMenu(menuName = "Application State", fileName = "Application State")]
    public class ApplicationState : ScriptableObject
    {
        [field: SerializeField]
        [field: ReadOnly]
        [field: Tooltip("Controlled and used by the ApplicationStateManager, and is managed only at runtime. Should not be used anywhere else.")]
        public int StateID { get; set; } = -1;

        // ---

        [field: TitleGroup("State Interactions")]
        [field: SerializeField]
        [field: Tooltip("Requires all of the following states in order to be active")]
        public List<ApplicationState> SubstateOf { get; private set; }
        [field: SerializeField]
        [field: Tooltip("Cannot be set if any of these states are active")]
        public List<ApplicationState> BlockedBy { get; private set; }

        #region Override

        [field: SerializeField]
        [field: Tooltip("If true, does not override any state")]
        [field: FoldoutGroup("Override Controls")]
        public bool DoNotOverrideAny { get; private set; }
        [field: SerializeField]
        [field: Tooltip("Whether or not the state can be overriden by default. Can still be overwritten by AlwaysOverride")]
        [field: LabelText("Can\'t Override By Default")]
        [field: FoldoutGroup("Override Controls")]
        public bool CantOverrideByDefault { get; private set; }

        [field: SerializeField]
        [field: Tooltip("Does not override the selected states")]
        [field: HideIf("@DoNotOverrideAny")]
        [field: FoldoutGroup("Override Controls")]
        public List<ApplicationState> DoNotOverride { get; private set; }
        [field: SerializeField]
        [field: Tooltip("States to always override, even if CantOverrideByDefault is true")]
        [field: FoldoutGroup("Override Controls")]
        public List<ApplicationState> AlwaysOverride { get; private set; }

        #endregion

        [field: SerializeField]
        [field: Tooltip("Will toggle states (and scenes) if set multiple times")]
        public bool Toggle { get; private set; } = false;

        // ---

        [field: TitleGroup("Time Scale")]
        [field: SerializeField]
        public bool ControlTimeScale { get; private set; } = false;

        [field: SerializeField]
        [field: ShowIf("@ControlTimeScale")]
        [field: FoldoutGroup("Time Scale Options")]
        public TimeScaleControlOptions UpdateTimeScaleOnEnable { get; private set; } = TimeScaleControlOptions.Set;
        [field: SerializeField]
        [field: ShowIf("@ControlTimeScale && UpdateTimeScaleOnEnable == TimeScaleControlOptions.Set")]
        [field: FoldoutGroup("Time Scale Options")]
        public float TimeScaleOnEnable { get; private set; } = 0f;

        [field: SerializeField]
        [field: ShowIf("@ControlTimeScale")]
        [field: FoldoutGroup("Time Scale Options")]
        public TimeScaleControlOptions UpdateTimeScaleOnDisable { get; private set; } = TimeScaleControlOptions.None;
        [field: SerializeField]
        [field: ShowIf("@ControlTimeScale && UpdateTimeScaleOnDisable == TimeScaleControlOptions.Set")]
        [field: FoldoutGroup("Time Scale Options")]
        public float TimeScaleOnDisable { get; private set; } = 1f;

        // ---

        [field: TitleGroup("Misc")]
        [field: SerializeField]
        public bool LockCursor { get; private set; } = false;

        // ----------------------------------------------------------------------------------------------------------

        private void OnValidate()
        {
            if(!DoNotOverrideAny)
                foreach(ApplicationState state in SubstateOf)
                    if(!DoNotOverride.Contains(state))
                        DoNotOverride.Add(state);
        }

        // ----------------------------------------------------------------------------------------------------------

        public void SetStateActive()
        {
            ApplicationStateManager.Instance.SetState(this);
        }

        public bool IsStateActive => ApplicationStateManager.Instance.IsStateActive(this);
    }
}