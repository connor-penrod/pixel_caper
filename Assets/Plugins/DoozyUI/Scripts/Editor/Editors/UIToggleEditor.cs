// Copyright (c) 2015 - 2017 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace DoozyUI
{
    [CustomEditor(typeof(UIToggle), true)]
    [DisallowMultipleComponent]
    [CanEditMultipleObjects]
    public class UIToggleEditor : QEditor
    {
        UIToggle uiToggle { get { return (UIToggle)target; } }

        SerializedProperty
            allowMultipleClicks, disableButtonInterval,
            deselectButtonOnClick,

            useOnPointerEnter, onPointerEnterDisableInterval,
            onPointerEnterSoundToggleOn, onPointerEnterSoundToggleOff,
            customOnPointerEnterSoundToggleOn, customOnPointerEnterSoundToggleOff,
            OnPointerEnterToggleOn, OnPointerEnterToggleOff,
            onPointerEnterPunchPresetCategory, onPointerEnterPunchPresetName, loadOnPointerEnterPunchPresetAtRuntime,
            onPointerEnterPunch,
            onPointerEnterPunchMove, onPointerEnterPunchMoveEnabled, onPointerEnterPunchMovePunch, onPointerEnterPunchMoveStartDelay, onPointerEnterPunchMoveDuration, onPointerEnterPunchMoveVibrato, onPointerEnterPunchMoveElasticity,
            onPointerEnterPunchRotate, onPointerEnterPunchRotateEnabled, onPointerEnterPunchRotatePunch, onPointerEnterPunchRotateStartDelay, onPointerEnterPunchRotateDuration, onPointerEnterPunchRotateVibrato, onPointerEnterPunchRotateElasticity,
            onPointerEnterPunchScale, onPointerEnterPunchScaleEnabled, onPointerEnterPunchScalePunch, onPointerEnterPunchScaleStartDelay, onPointerEnterPunchScaleDuration, onPointerEnterPunchScaleVibrato, onPointerEnterPunchScaleElasticity,
            onPointerEnterGameEventsToggleOn, onPointerEnterGameEventsToggleOff,

            useOnPointerExit, onPointerExitDisableInterval,
            onPointerExitSoundToggleOn, onPointerExitSoundToggleOff,
            customOnPointerExitSoundToggleOn, customOnPointerExitSoundToggleOff,
            OnPointerExitToggleOn, OnPointerExitToggleOff,
            onPointerExitPunchPresetCategory, onPointerExitPunchPresetName, loadOnPointerExitPunchPresetAtRuntime,
            onPointerExitPunch,
            onPointerExitPunchMove, onPointerExitPunchMoveEnabled, onPointerExitPunchMovePunch, onPointerExitPunchMoveStartDelay, onPointerExitPunchMoveDuration, onPointerExitPunchMoveVibrato, onPointerExitPunchMoveElasticity,
            onPointerExitPunchRotate, onPointerExitPunchRotateEnabled, onPointerExitPunchRotatePunch, onPointerExitPunchRotateStartDelay, onPointerExitPunchRotateDuration, onPointerExitPunchRotateVibrato, onPointerExitPunchRotateElasticity,
            onPointerExitPunchScale, onPointerExitPunchScaleEnabled, onPointerExitPunchScalePunch, onPointerExitPunchScaleStartDelay, onPointerExitPunchScaleDuration, onPointerExitPunchScaleVibrato, onPointerExitPunchScaleElasticity,
            onPointerExitGameEventsToggleOn, onPointerExitGameEventsToggleOff,

            useOnClick, waitForOnClick,
            onClickSoundToggleOn, onClickSoundToggleOff,
            customOnClickSoundToggleOn, customOnClickSoundToggleOff,
            OnClickToggleOn, OnClickToggleOff,
            onClickPunchPresetCategory, onClickPunchPresetName, loadOnClickPunchPresetAtRuntime,
            onClickPunch,
            onClickPunchMove, onClickPunchMoveEnabled, onClickPunchMovePunch, onClickPunchMoveStartDelay, onClickPunchMoveDuration, onClickPunchMoveVibrato, onClickPunchMoveElasticity,
            onClickPunchRotate, onClickPunchRotateEnabled, onClickPunchRotatePunch, onClickPunchRotateStartDelay, onClickPunchRotateDuration, onClickPunchRotateVibrato, onClickPunchRotateElasticity,
            onClickPunchScale, onClickPunchScaleEnabled, onClickPunchScalePunch, onClickPunchScaleStartDelay, onClickPunchScaleDuration, onClickPunchScaleVibrato, onClickPunchScaleElasticity,
            onClickGameEventsToggleOn, onClickGameEventsToggleOff;

        AnimBool
           showOnPointerEnter, showOnPointerEnterPreset, showOnPointerEnterPunchMove, showOnPointerEnterPunchRotate, showOnPointerEnterPunchScale, showOnPointerEnterEvents, showOnPointerEnterGameEvents, showOnPointerEnterNavigation,
           showOnPointerExit, showOnPointerExitPreset, showOnPointerExitPunchMove, showOnPointerExitPunchRotate, showOnPointerExitPunchScale, showOnPointerExitEvents, showOnPointerExitGameEvents, showOnPointerExitNavigation,
           showOnClick, showOnClickPreset, showOnClickPunchMove, showOnClickPunchRotate, showOnClickPunchScale, showOnClickEvents, showOnClickGameEvents, showOnClickNavigation;

        int
            onPointerEnterSoundIndexToggleOn,
            onPointerEnterSoundIndexToggleOff,
            onPointerExitSoundIndexToggleOn,
            onPointerExitSoundIndexToggleOff,
            onClickSoundIndexToggleOn,
            onClickSoundIndexToggleOff;

        string newPresetCategoryName = "";
        string newPresetName = "";

        int onPointerEnterPunchPresetCategoryNameIndex;
        int onPointerEnterPunchPresetNameIndex;
        bool onPointerEnterPunchNewPreset = false;
        bool onPointerEnterPunchNewCategoryName = false;

        int onPointerExitPunchPresetCategoryNameIndex;
        int onPointerExitPunchPresetNameIndex;
        bool onPointerExitPunchNewPreset = false;
        bool onPointerExitPunchNewCategoryName = false;

        int onClickPunchPresetCategoryNameIndex;
        int onClickPunchPresetNameIndex;
        bool onClickPunchNewPreset = false;
        bool onClickPunchNewCategoryName = false;

        EditorNavigationPointerData onPointerEnterEditorNavigationDataToggleOn = new EditorNavigationPointerData();
        EditorNavigationPointerData onPointerEnterEditorNavigationDataToggleOff = new EditorNavigationPointerData();
        EditorNavigationPointerData onPointerExitEditorNavigationDataToggleOn = new EditorNavigationPointerData();
        EditorNavigationPointerData onPointerExitEditorNavigationDataToggleOff = new EditorNavigationPointerData();
        EditorNavigationPointerData onClickEditorNavigationDataToggleOn = new EditorNavigationPointerData();
        EditorNavigationPointerData onClickEditorNavigationDataToggleOff = new EditorNavigationPointerData();

        bool ControlPanelSelected = false;
        bool refreshData = true;

        void SerializedObjectFindProperties()
        {
            #region Settings
            allowMultipleClicks = serializedObject.FindProperty("allowMultipleClicks");
            disableButtonInterval = serializedObject.FindProperty("disableButtonInterval");
            deselectButtonOnClick = serializedObject.FindProperty("deselectButtonOnClick");
            #endregion
            #region PointerEnter
            useOnPointerEnter = serializedObject.FindProperty("useOnPointerEnter");
            onPointerEnterDisableInterval = serializedObject.FindProperty("onPointerEnterDisableInterval");
            onPointerEnterSoundToggleOn = serializedObject.FindProperty("onPointerEnterSoundToggleOn");
            onPointerEnterSoundToggleOff = serializedObject.FindProperty("onPointerEnterSoundToggleOff");
            customOnPointerEnterSoundToggleOn = serializedObject.FindProperty("customOnPointerEnterSoundToggleOn");
            customOnPointerEnterSoundToggleOff = serializedObject.FindProperty("customOnPointerEnterSoundToggleOff");
            OnPointerEnterToggleOn = serializedObject.FindProperty("OnPointerEnterToggleOn");
            OnPointerEnterToggleOff = serializedObject.FindProperty("OnPointerEnterToggleOff");
            onPointerEnterPunchPresetCategory = serializedObject.FindProperty("onPointerEnterPunchPresetCategory");
            onPointerEnterPunchPresetName = serializedObject.FindProperty("onPointerEnterPunchPresetName");
            loadOnPointerEnterPunchPresetAtRuntime = serializedObject.FindProperty("loadOnPointerEnterPunchPresetAtRuntime");
            onPointerEnterPunch = serializedObject.FindProperty("onPointerEnterPunch");
            onPointerEnterPunchMove = onPointerEnterPunch.FindPropertyRelative("move");
            onPointerEnterPunchMoveEnabled = onPointerEnterPunchMove.FindPropertyRelative("enabled");
            onPointerEnterPunchMovePunch = onPointerEnterPunchMove.FindPropertyRelative("punch");
            onPointerEnterPunchMoveStartDelay = onPointerEnterPunchMove.FindPropertyRelative("startDelay");
            onPointerEnterPunchMoveDuration = onPointerEnterPunchMove.FindPropertyRelative("duration");
            onPointerEnterPunchMoveVibrato = onPointerEnterPunchMove.FindPropertyRelative("vibrato");
            onPointerEnterPunchMoveElasticity = onPointerEnterPunchMove.FindPropertyRelative("elasticity");
            onPointerEnterPunchRotate = onPointerEnterPunch.FindPropertyRelative("rotate");
            onPointerEnterPunchRotateEnabled = onPointerEnterPunchRotate.FindPropertyRelative("enabled");
            onPointerEnterPunchRotatePunch = onPointerEnterPunchRotate.FindPropertyRelative("punch");
            onPointerEnterPunchRotateStartDelay = onPointerEnterPunchRotate.FindPropertyRelative("startDelay");
            onPointerEnterPunchRotateDuration = onPointerEnterPunchRotate.FindPropertyRelative("duration");
            onPointerEnterPunchRotateVibrato = onPointerEnterPunchRotate.FindPropertyRelative("vibrato");
            onPointerEnterPunchRotateElasticity = onPointerEnterPunchRotate.FindPropertyRelative("elasticity");
            onPointerEnterPunchScale = onPointerEnterPunch.FindPropertyRelative("scale");
            onPointerEnterPunchScaleEnabled = onPointerEnterPunchScale.FindPropertyRelative("enabled");
            onPointerEnterPunchScalePunch = onPointerEnterPunchScale.FindPropertyRelative("punch");
            onPointerEnterPunchScaleStartDelay = onPointerEnterPunchScale.FindPropertyRelative("startDelay");
            onPointerEnterPunchScaleDuration = onPointerEnterPunchScale.FindPropertyRelative("duration");
            onPointerEnterPunchScaleVibrato = onPointerEnterPunchScale.FindPropertyRelative("vibrato");
            onPointerEnterPunchScaleElasticity = onPointerEnterPunchScale.FindPropertyRelative("elasticity");
            onPointerEnterGameEventsToggleOn = serializedObject.FindProperty("onPointerEnterGameEventsToggleOn");
            onPointerEnterGameEventsToggleOff = serializedObject.FindProperty("onPointerEnterGameEventsToggleOff");
            #endregion
            #region PointerExit
            useOnPointerExit = serializedObject.FindProperty("useOnPointerExit");
            onPointerExitDisableInterval = serializedObject.FindProperty("onPointerExitDisableInterval");
            onPointerExitSoundToggleOn = serializedObject.FindProperty("onPointerExitSoundToggleOn");
            onPointerExitSoundToggleOff = serializedObject.FindProperty("onPointerExitSoundToggleOff");
            customOnPointerExitSoundToggleOn = serializedObject.FindProperty("customOnPointerExitSoundToggleOn");
            customOnPointerExitSoundToggleOff = serializedObject.FindProperty("customOnPointerExitSoundToggleOff");
            OnPointerExitToggleOn = serializedObject.FindProperty("OnPointerExitToggleOn");
            OnPointerExitToggleOff = serializedObject.FindProperty("OnPointerExitToggleOff");
            onPointerExitPunchPresetCategory = serializedObject.FindProperty("onPointerExitPunchPresetCategory");
            onPointerExitPunchPresetName = serializedObject.FindProperty("onPointerExitPunchPresetName");
            loadOnPointerExitPunchPresetAtRuntime = serializedObject.FindProperty("loadOnPointerExitPunchPresetAtRuntime");
            onPointerExitPunch = serializedObject.FindProperty("onPointerExitPunch");
            onPointerExitPunchMove = onPointerExitPunch.FindPropertyRelative("move");
            onPointerExitPunchMoveEnabled = onPointerExitPunchMove.FindPropertyRelative("enabled");
            onPointerExitPunchMovePunch = onPointerExitPunchMove.FindPropertyRelative("punch");
            onPointerExitPunchMoveStartDelay = onPointerExitPunchMove.FindPropertyRelative("startDelay");
            onPointerExitPunchMoveDuration = onPointerExitPunchMove.FindPropertyRelative("duration");
            onPointerExitPunchMoveVibrato = onPointerExitPunchMove.FindPropertyRelative("vibrato");
            onPointerExitPunchMoveElasticity = onPointerExitPunchMove.FindPropertyRelative("elasticity");
            onPointerExitPunchRotate = onPointerExitPunch.FindPropertyRelative("rotate");
            onPointerExitPunchRotateEnabled = onPointerExitPunchRotate.FindPropertyRelative("enabled");
            onPointerExitPunchRotatePunch = onPointerExitPunchRotate.FindPropertyRelative("punch");
            onPointerExitPunchRotateStartDelay = onPointerExitPunchRotate.FindPropertyRelative("startDelay");
            onPointerExitPunchRotateDuration = onPointerExitPunchRotate.FindPropertyRelative("duration");
            onPointerExitPunchRotateVibrato = onPointerExitPunchRotate.FindPropertyRelative("vibrato");
            onPointerExitPunchRotateElasticity = onPointerExitPunchRotate.FindPropertyRelative("elasticity");
            onPointerExitPunchScale = onPointerExitPunch.FindPropertyRelative("scale");
            onPointerExitPunchScaleEnabled = onPointerExitPunchScale.FindPropertyRelative("enabled");
            onPointerExitPunchScalePunch = onPointerExitPunchScale.FindPropertyRelative("punch");
            onPointerExitPunchScaleStartDelay = onPointerExitPunchScale.FindPropertyRelative("startDelay");
            onPointerExitPunchScaleDuration = onPointerExitPunchScale.FindPropertyRelative("duration");
            onPointerExitPunchScaleVibrato = onPointerExitPunchScale.FindPropertyRelative("vibrato");
            onPointerExitPunchScaleElasticity = onPointerExitPunchScale.FindPropertyRelative("elasticity");
            onPointerExitGameEventsToggleOn = serializedObject.FindProperty("onPointerExitGameEventsToggleOn");
            onPointerExitGameEventsToggleOff = serializedObject.FindProperty("onPointerExitGameEventsToggleOff");
            #endregion
            #region OnClick
            useOnClick = serializedObject.FindProperty("useOnClick");
            waitForOnClick = serializedObject.FindProperty("waitForOnClick");
            onClickSoundToggleOn = serializedObject.FindProperty("onClickSoundToggleOn");
            onClickSoundToggleOff = serializedObject.FindProperty("onClickSoundToggleOff");
            customOnClickSoundToggleOn = serializedObject.FindProperty("customOnClickSoundToggleOn");
            customOnClickSoundToggleOff = serializedObject.FindProperty("customOnClickSoundToggleOff");
            OnClickToggleOn = serializedObject.FindProperty("OnClickToggleOn");
            OnClickToggleOff = serializedObject.FindProperty("OnClickToggleOff");
            onClickPunchPresetCategory = serializedObject.FindProperty("onClickPunchPresetCategory");
            onClickPunchPresetName = serializedObject.FindProperty("onClickPunchPresetName");
            loadOnClickPunchPresetAtRuntime = serializedObject.FindProperty("loadOnClickPunchPresetAtRuntime");
            onClickPunch = serializedObject.FindProperty("onClickPunch");
            onClickPunchMove = onClickPunch.FindPropertyRelative("move");
            onClickPunchMoveEnabled = onClickPunchMove.FindPropertyRelative("enabled");
            onClickPunchMovePunch = onClickPunchMove.FindPropertyRelative("punch");
            onClickPunchMoveStartDelay = onClickPunchMove.FindPropertyRelative("startDelay");
            onClickPunchMoveDuration = onClickPunchMove.FindPropertyRelative("duration");
            onClickPunchMoveVibrato = onClickPunchMove.FindPropertyRelative("vibrato");
            onClickPunchMoveElasticity = onClickPunchMove.FindPropertyRelative("elasticity");
            onClickPunchRotate = onClickPunch.FindPropertyRelative("rotate");
            onClickPunchRotateEnabled = onClickPunchRotate.FindPropertyRelative("enabled");
            onClickPunchRotatePunch = onClickPunchRotate.FindPropertyRelative("punch");
            onClickPunchRotateStartDelay = onClickPunchRotate.FindPropertyRelative("startDelay");
            onClickPunchRotateDuration = onClickPunchRotate.FindPropertyRelative("duration");
            onClickPunchRotateVibrato = onClickPunchRotate.FindPropertyRelative("vibrato");
            onClickPunchRotateElasticity = onClickPunchRotate.FindPropertyRelative("elasticity");
            onClickPunchScale = onClickPunch.FindPropertyRelative("scale");
            onClickPunchScaleEnabled = onClickPunchScale.FindPropertyRelative("enabled");
            onClickPunchScalePunch = onClickPunchScale.FindPropertyRelative("punch");
            onClickPunchScaleStartDelay = onClickPunchScale.FindPropertyRelative("startDelay");
            onClickPunchScaleDuration = onClickPunchScale.FindPropertyRelative("duration");
            onClickPunchScaleVibrato = onClickPunchScale.FindPropertyRelative("vibrato");
            onClickPunchScaleElasticity = onClickPunchScale.FindPropertyRelative("elasticity");
            onClickGameEventsToggleOn = serializedObject.FindProperty("onClickGameEventsToggleOn");
            onClickGameEventsToggleOff = serializedObject.FindProperty("onClickGameEventsToggleOff");
            #endregion
        }

        void GenerateInfoMessages()
        {
            infoMessage = new Dictionary<string, InfoMessage>
            {
                { "OnPointerEnterLoadPresetAtRuntime", new InfoMessage() { title = "Runtime Preset", message = onPointerEnterPunchPresetCategory.stringValue + " / " + onPointerEnterPunchPresetName.stringValue, type = InfoMessageType.Info, show = new AnimBool(loadOnPointerEnterPunchPresetAtRuntime.boolValue, Repaint) } },
                { "OnPointerExitLoadPresetAtRuntime", new InfoMessage() { title = "Runtime Preset", message = onPointerExitPunchPresetCategory.stringValue + " / " + onPointerExitPunchPresetName.stringValue, type = InfoMessageType.Info, show = new AnimBool(loadOnPointerExitPunchPresetAtRuntime.boolValue, Repaint) } },
                { "OnClickLoadPresetAtRuntime", new InfoMessage() { title = "Runtime Preset", message = onClickPunchPresetCategory.stringValue + " / " + onClickPunchPresetName.stringValue, type = InfoMessageType.Info, show = new AnimBool(loadOnClickPunchPresetAtRuntime.boolValue, Repaint) } }
            };
        }

        void InitAnimBools()
        {
            showOnPointerEnter = new AnimBool(false, Repaint);
            showOnPointerEnterPreset = new AnimBool(false, Repaint);
            showOnPointerEnterPunchMove = new AnimBool(false, Repaint);
            showOnPointerEnterPunchRotate = new AnimBool(false, Repaint);
            showOnPointerEnterPunchScale = new AnimBool(false, Repaint);
            showOnPointerEnterEvents = new AnimBool(false, Repaint);
            showOnPointerEnterGameEvents = new AnimBool(false, Repaint);
            showOnPointerEnterNavigation = new AnimBool(false, Repaint);

            showOnPointerExit = new AnimBool(false, Repaint);
            showOnPointerExitPreset = new AnimBool(false, Repaint);
            showOnPointerExitPunchMove = new AnimBool(false, Repaint);
            showOnPointerExitPunchRotate = new AnimBool(false, Repaint);
            showOnPointerExitPunchScale = new AnimBool(false, Repaint);
            showOnPointerExitEvents = new AnimBool(false, Repaint);
            showOnPointerExitGameEvents = new AnimBool(false, Repaint);
            showOnPointerExitNavigation = new AnimBool(false, Repaint);

            showOnClick = new AnimBool(false, Repaint);
            showOnClickPreset = new AnimBool(false, Repaint);
            showOnClickPunchMove = new AnimBool(false, Repaint);
            showOnClickPunchRotate = new AnimBool(false, Repaint);
            showOnClickPunchScale = new AnimBool(false, Repaint);
            showOnClickEvents = new AnimBool(false, Repaint);
            showOnClickGameEvents = new AnimBool(false, Repaint);
            showOnClickNavigation = new AnimBool(false, Repaint);
        }

        protected override void OnEnable()
        {
            requiresContantRepaint = true;
            SerializedObjectFindProperties();
            GenerateInfoMessages();
            InitAnimBools();
        }

        void RefreshData(bool forcedRefresh = false)
        {
            serializedObject.Update();
            RefreshUISounds(forcedRefresh);
            RefreshPunchAnimations(forcedRefresh);
            RefreshNavigationData(forcedRefresh);
            serializedObject.ApplyModifiedProperties();
        }
        void RefreshUISounds(bool forcedRefresh)
        {
            RefreshUISoundsDatabase(forcedRefresh);
            ValidateUISounds();
        }
        void RefreshPunchAnimations(bool forcedRefresh)
        {
            RefreshPunchAnimationsPresets(forcedRefresh);
            ValidatePunchAnimationsPresets();
        }

        public override void OnInspectorGUI()
        {
            DrawHeader(DUIResources.headerUIToggle.texture, WIDTH_420, HEIGHT_42);
            if (refreshData) //refresh needs to be executed this way because OnEnable is called 3 times when entering PlayMode, thus adding a lot of wait time for the developer (that is unacceptable); until we figure out why that happends, this solution will have to do.
            {
                RefreshData();
                refreshData = false;
            }
            if (!ControlPanelWindow.Selected && ControlPanelSelected)
            {
                RefreshData();
                ControlPanelSelected = false;
            }
            else if (ControlPanelWindow.Selected && !ControlPanelSelected)
            {
                ControlPanelSelected = true;
            }
            serializedObject.Update();
            DrawTopButtons();
            DrawSettings();
            DrawOnPointerEnter();
            DrawOnPointerExit();
            DrawOnClick();
            serializedObject.ApplyModifiedProperties();
            QUI.Space(SPACE_4);
        }

        void DrawTopButtons()
        {
            QUI.BeginHorizontal(WIDTH_420);
            {
                if (QUI.Button("UISounds Database"))
                {
                    ControlPanelWindow.Open(ControlPanelWindow.Section.UISounds);
                }
                if (QUI.Button("Refresh Data"))
                {
                    RefreshData(true);
                }
            }
            QUI.EndHorizontal();
            QUI.Space(SPACE_2);
        }
        void DrawSettings()
        {
            QUI.BeginHorizontal(WIDTH_420);
            {
                QUI.PropertyField(allowMultipleClicks, 12);
                QUI.Label("allow multiple clicks", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 120);
                if (!allowMultipleClicks.boolValue)
                {
                    QUI.Space(55);
                    QUI.Label("disable button interval", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 126);
                    QUI.PropertyField(disableButtonInterval, 38);
                    QUI.Label("seconds", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 50);
                }
            }
            QUI.EndHorizontal();
            QUI.BeginHorizontal(WIDTH_420);
            {
                QUI.PropertyField(deselectButtonOnClick, 12);
                QUI.Label("deselect button on click", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 140);
            }
            QUI.EndHorizontal();
            QUI.Space(SPACE_4);
        }

        void DrawOnPointerEnter()
        {
            if (DUI.DUISettings == null || DUI.DUISettings.UIToggle_Inspector_HideOnPointerEnter) { useOnPointerEnter.boolValue = false; return; }
            QUI.BeginHorizontal(WIDTH_420);
            {
                if (!useOnPointerEnter.boolValue)
                {
                    QUI.DrawTexture(DUIResources.barOnPointerEnterDisabled.texture, 336, 21);
                    if (showOnPointerEnter.target) { showOnPointerEnter.target = false; }
                }
                else
                {
                    if (QUI.Button(DUIStyles.GetStyle(showOnPointerEnter.target ? DUIStyles.ButtonStyle.OnPointerEnter : DUIStyles.ButtonStyle.OnPointerEnterCollapsed), 336, 21)) { showOnPointerEnter.target = !showOnPointerEnter.target; }
                }
                if (QUI.Button(DUIStyles.GetStyle(useOnPointerEnter.boolValue ? DUIStyles.ButtonStyle.BarButtonEnabled : DUIStyles.ButtonStyle.BarButtonDisabled), 84, 21)) { useOnPointerEnter.boolValue = !useOnPointerEnter.boolValue; if (useOnPointerEnter.boolValue) { showOnPointerEnter.target = true; } }
            }
            QUI.EndHorizontal();
            infoMessage["OnPointerEnterLoadPresetAtRuntime"].show.target = loadOnPointerEnterPunchPresetAtRuntime.boolValue;
            infoMessage["OnPointerEnterLoadPresetAtRuntime"].message = onPointerEnterPunchPresetCategory.stringValue + " / " + onPointerEnterPunchPresetName.stringValue;
            DrawInfoMessage("OnPointerEnterLoadPresetAtRuntime", WIDTH_420);
            QUI.Space(SPACE_4);
            if (!useOnPointerEnter.boolValue) { return; }
            if (QUI.BeginFadeGroup(showOnPointerEnter.faded))
            {
                QUI.BeginVertical(WIDTH_420);
                {
                    DrawOnPointerEnterSettings();
                    QUI.Space(SPACE_2);
                    DrawOnPointerEnterSound();
                    QUI.Space(SPACE_2);
                    DrawOnPointerEnterPreset();
                    QUI.Space(SPACE_2);
                    DrawOnPointerEnterPunchMove();
                    QUI.Space(SPACE_2);
                    DrawOnPointerEnterPunchRotate();
                    QUI.Space(SPACE_2);
                    DrawOnPointerEnterPunchScale();
                    QUI.Space(SPACE_4);
                    QUI.DrawTexture(DUIResources.barToggleOn.texture, 420, 18);
                    QUI.Space(SPACE_2);
                    DrawOnPointerEnterEventsToggleOn();
                    QUI.Space(SPACE_2);
                    DrawOnPointerEnterGameEventsToggleOn();
                    QUI.Space(SPACE_2);
                    DrawOnPointerEnterNavigationToggleOn();
                    QUI.Space(SPACE_4);
                    QUI.DrawTexture(DUIResources.barToggleOff.texture, 420, 18);
                    QUI.Space(SPACE_2);
                    DrawOnPointerEnterEventsToggleOff();
                    QUI.Space(SPACE_2);
                    DrawOnPointerEnterGameEventsToggleOff();
                    QUI.Space(SPACE_2);
                    DrawOnPointerEnterNavigationToggleOff();
                    QUI.Space(SPACE_16);
                }
                QUI.EndVertical();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnPointerEnterSettings()
        {
            QUI.BeginHorizontal(WIDTH_420);
            {
                QUI.Label("disable interval", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), WIDTH_105);
                QUI.PropertyField(onPointerEnterDisableInterval, 38);
                QUI.Label("seconds", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 50);
            }
            QUI.EndHorizontal();
        }
        void DrawOnPointerEnterSound()
        {
            QUI.BeginHorizontal(WIDTH_420);
            {
                QUI.DrawTexture(DUIResources.minibarToggleOn.texture, 100, 18);
                QUI.Space(80);
                QUI.Label("play sound", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 65);
                if (customOnPointerEnterSoundToggleOn.boolValue)
                {
                    QUI.PropertyField(onPointerEnterSoundToggleOn, 130);
                }
                else
                {
                    QUI.BeginChangeCheck();
                    {
                        onPointerEnterSoundIndexToggleOn = EditorGUILayout.Popup(onPointerEnterSoundIndexToggleOn, DUI.UISoundNamesUIButtons.ToArray(), GUILayout.Width(130));
                    }
                    if (QUI.EndChangeCheck())
                    {
                        Undo.RecordObject(uiToggle, "Updated Play Sound");
                        onPointerEnterSoundToggleOn.stringValue = DUI.UISoundNamesUIButtons[onPointerEnterSoundIndexToggleOn];
                    }
                }
                QUI.Space(SPACE_4);
                QUI.BeginChangeCheck();
                {
                    QUI.PropertyField(customOnPointerEnterSoundToggleOn, 12);
                }
                if (QUI.EndChangeCheck())
                {
                    if (!customOnPointerEnterSoundToggleOn.boolValue)
                    {
                        if (!DUI.UISoundNamesUIButtons.Contains(onPointerEnterSoundToggleOn.stringValue))
                        {
                            if (EditorUtility.DisplayDialog("Action Required", "The '" + onPointerEnterSoundToggleOn.stringValue + "' ui sound does not exist in the UISounds database.\nDo you want to add it now?", "Yes", "No"))
                            {
                                DUI.CreateUISound(onPointerEnterSoundToggleOn.stringValue, SoundType.UIButtons, null);
                            }
                            else
                            {
                                onPointerEnterSoundToggleOn.stringValue = DUI.DEFAULT_SOUND_NAME;
                            }
                        }
                        Undo.RecordObject(uiToggle, "Updated Play Sound");
                        onPointerEnterSoundIndexToggleOn = DUI.UISoundNamesUIButtons.IndexOf(onPointerEnterSoundToggleOn.stringValue);
                        RefreshUISounds(false);
                    }
                }
                QUI.Label("custom", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 48);
                QUI.Space(SPACE_2);
                if (QUI.Button(DUIStyles.GetStyle(DUIStyles.ButtonStyle.ButtonPlay), 18, 18)) { DUIUtils.PreviewSound(onPointerEnterSoundToggleOn.stringValue); }
                QUI.Space(SPACE_2);
                if (QUI.Button(DUIStyles.GetStyle(DUIStyles.ButtonStyle.ButtonStop), 18, 18)) { DUIUtils.StopSoundPreview(onPointerEnterSoundToggleOn.stringValue); }
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2);

            QUI.BeginHorizontal(WIDTH_420);
            {
                QUI.DrawTexture(DUIResources.minibarToggleOff.texture, 100, 18);
                QUI.Space(80);
                QUI.Label("play sound", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 65);
                if (customOnPointerEnterSoundToggleOff.boolValue)
                {
                    QUI.PropertyField(onPointerEnterSoundToggleOff, 130);
                }
                else
                {
                    QUI.BeginChangeCheck();
                    {
                        onPointerEnterSoundIndexToggleOff = EditorGUILayout.Popup(onPointerEnterSoundIndexToggleOff, DUI.UISoundNamesUIButtons.ToArray(), GUILayout.Width(130));
                    }
                    if (QUI.EndChangeCheck())
                    {
                        Undo.RecordObject(uiToggle, "Updated Play Sound");
                        onPointerEnterSoundToggleOff.stringValue = DUI.UISoundNamesUIButtons[onPointerEnterSoundIndexToggleOff];
                    }
                }
                QUI.Space(SPACE_4);
                QUI.BeginChangeCheck();
                {
                    QUI.PropertyField(customOnPointerEnterSoundToggleOff, 12);
                }
                if (QUI.EndChangeCheck())
                {
                    if (!customOnPointerEnterSoundToggleOff.boolValue)
                    {
                        if (!DUI.UISoundNamesUIButtons.Contains(onPointerEnterSoundToggleOff.stringValue))
                        {
                            if (EditorUtility.DisplayDialog("Action Required", "The '" + onPointerEnterSoundToggleOff.stringValue + "' ui sound does not exist in the UISounds database.\nDo you want to add it now?", "Yes", "No"))
                            {
                                DUI.CreateUISound(onPointerEnterSoundToggleOff.stringValue, SoundType.UIButtons, null);
                            }
                            else
                            {
                                onPointerEnterSoundToggleOff.stringValue = DUI.DEFAULT_SOUND_NAME;
                            }
                        }
                        Undo.RecordObject(uiToggle, "Updated Play Sound");
                        onPointerEnterSoundIndexToggleOff = DUI.UISoundNamesUIButtons.IndexOf(onPointerEnterSoundToggleOff.stringValue);
                        RefreshUISounds(false);
                    }
                }
                QUI.Label("custom", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 48);
                QUI.Space(SPACE_2);
                if (QUI.Button(DUIStyles.GetStyle(DUIStyles.ButtonStyle.ButtonPlay), 18, 18)) { DUIUtils.PreviewSound(onPointerEnterSoundToggleOff.stringValue); }
                QUI.Space(SPACE_2);
                if (QUI.Button(DUIStyles.GetStyle(DUIStyles.ButtonStyle.ButtonStop), 18, 18)) { DUIUtils.StopSoundPreview(onPointerEnterSoundToggleOff.stringValue); }
            }
            QUI.EndHorizontal();
        }
        void DrawOnPointerEnterPreset()
        {
            if (QUI.Button(DUIStyles.GetStyle(showOnPointerEnterPreset.target ? DUIStyles.ButtonStyle.barPunchPreset : DUIStyles.ButtonStyle.barPunchPresetCollapsed), WIDTH_420, 18)) { showOnPointerEnterPreset.target = !showOnPointerEnterPreset.target; }
            if (QUI.BeginFadeGroup(showOnPointerEnterPreset.faded))
            {
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    if (!onPointerEnterPunchNewPreset)
                    {
                        QUI.BeginHorizontal(WIDTH_420 - 8);
                        {
                            SaveColors();
                            QUI.SetGUIBackgroundColor(DUIColors.BlueLight.Color);
                            if (QUI.Button("Load Preset"))
                            {
                                if (serializedObject.isEditingMultipleObjects)
                                {
                                    Punch iPunch = UIAnimatorUtil.GetPunch(onPointerEnterPunchPresetCategory.stringValue, onPointerEnterPunchPresetName.stringValue);
                                    Undo.RecordObjects(targets, "Load Preset to Multiple Targets");
                                    for (int i = 0; i < targets.Length; i++)
                                    {
                                        UIButton iTarget = (UIButton)targets[i];
                                        iTarget.onPointerEnterPunch = iPunch.Copy();
                                    }
                                }
                                else
                                {
                                    Undo.RecordObject(uiToggle, "Load Preset");
                                    uiToggle.onPointerEnterPunch = UIAnimatorUtil.GetPunch(onPointerEnterPunchPresetCategory.stringValue, onPointerEnterPunchPresetName.stringValue);
                                }
                                QUI.ExitGUI();
                            }
                            QUI.SetGUIBackgroundColor(DUIColors.GreenLight.Color);
                            if (QUI.Button("New Preset"))
                            {
                                onPointerEnterPunchNewPreset = true;
                                onPointerEnterPunchNewCategoryName = false;
                                newPresetCategoryName = onPointerEnterPunchPresetCategory.stringValue;
                                newPresetName = "";
                            }
                            QUI.SetGUIBackgroundColor(DUIColors.RedLight.Color);
                            if (QUI.Button("Delete Preset"))
                            {
                                if (EditorUtility.DisplayDialog("Delete Preset", "Are you sure you want to delete the '" + onPointerEnterPunchPresetName.stringValue + "' preset from the '" + onPointerEnterPunchPresetCategory.stringValue + "' preset category?", "Yes", "No"))
                                {
                                    UIAnimatorUtil.DeletePunchPreset(onPointerEnterPunchPresetCategory.stringValue, onPointerEnterPunchPresetName.stringValue);
                                    if (serializedObject.isEditingMultipleObjects)
                                    {
                                        for (int i = 0; i < targets.Length; i++)
                                        {
                                            UIButton iTarget = (UIButton)targets[i];
                                            if (iTarget.onPointerEnterPunchPresetCategory.Equals(onPointerEnterPunchPresetCategory.stringValue) ||
                                                iTarget.onPointerEnterPunchPresetName.Equals(onPointerEnterPunchPresetName.stringValue))
                                            {
                                                iTarget.onPointerEnterPunchPresetCategory = UIAnimatorUtil.DEFAULT_PRESET_CATEGORY;
                                                iTarget.onPointerEnterPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                            }
                                        }
                                    }
                                    onPointerEnterPunchPresetCategory.stringValue = UIAnimatorUtil.DEFAULT_PRESET_CATEGORY;
                                    onPointerEnterPunchPresetName.stringValue = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                    serializedObject.ApplyModifiedProperties();
                                    RefreshPunchAnimations(true);
                                }
                            }
                            RestoreColors();
                        }
                        QUI.EndHorizontal();
                        QUI.BeginHorizontal(WIDTH_420 - 8);
                        {
                            QUI.Label("Preset Category", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), WIDTH_105);
                            QUI.BeginChangeCheck();
                            {
                                onPointerEnterPunchPresetCategoryNameIndex = EditorGUILayout.Popup(onPointerEnterPunchPresetCategoryNameIndex, UIAnimatorUtil.PunchPresetCategories.ToArray());

                            }
                            if (QUI.EndChangeCheck())
                            {
                                onPointerEnterPunchPresetCategory.stringValue = UIAnimatorUtil.PunchPresetCategories[onPointerEnterPunchPresetCategoryNameIndex];
                                onPointerEnterPunchPresetNameIndex = 0;
                                onPointerEnterPunchPresetName.stringValue = UIAnimatorUtil.GetPunchPresetNames(onPointerEnterPunchPresetCategory.stringValue)[onPointerEnterPunchPresetNameIndex];
                            }

                        }
                        QUI.EndHorizontal();
                        QUI.BeginHorizontal(WIDTH_420 - 8);
                        {
                            QUI.Label("Preset Name", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), WIDTH_105);
                            QUI.BeginChangeCheck();
                            {
                                onPointerEnterPunchPresetNameIndex = EditorGUILayout.Popup(onPointerEnterPunchPresetNameIndex, UIAnimatorUtil.GetPunchPresetNames(onPointerEnterPunchPresetCategory.stringValue).ToArray());
                            }
                            if (QUI.EndChangeCheck())
                            {
                                onPointerEnterPunchPresetName.stringValue = UIAnimatorUtil.GetPunchPresetNames(onPointerEnterPunchPresetCategory.stringValue)[onPointerEnterPunchPresetNameIndex];
                            }
                        }
                        QUI.EndHorizontal();

                    }
                    else
                    {
                        QUI.BeginHorizontal(WIDTH_420 - 8);
                        {
                            SaveColors();
                            QUI.SetGUIBackgroundColor(DUIColors.GreenLight.Color);
                            if (QUI.Button("Save Preset"))
                            {
                                if (onPointerEnterPunchNewCategoryName && string.IsNullOrEmpty(newPresetCategoryName.Trim()))
                                {
                                    EditorUtility.DisplayDialog("Information", "The new preset category name cannot be an empty string.", "Ok");
                                }
                                else if (string.IsNullOrEmpty(newPresetName.Trim()))
                                {
                                    EditorUtility.DisplayDialog("Information", "The new preset name cannot be an empty string.", "Ok");
                                }
                                else if (UIAnimatorUtil.PunchPresetExists(newPresetCategoryName, newPresetName))
                                {
                                    EditorUtility.DisplayDialog("Information", "There is another preset with the '" + newPresetName + "' preset name in the '" + newPresetCategoryName + "' preset category. Try a different preset name maybe?", "Ok");
                                }
                                else
                                {
                                    UIAnimatorUtil.CreatePunchPreset(newPresetCategoryName, newPresetName, uiToggle.onPointerEnterPunch.Copy());
                                    if (serializedObject.isEditingMultipleObjects)
                                    {
                                        for (int i = 0; i < targets.Length; i++)
                                        {
                                            UIButton iTarget = (UIButton)targets[i];
                                            iTarget.onPointerEnterPunchPresetCategory = newPresetCategoryName;
                                            iTarget.onPointerEnterPunchPresetName = newPresetName;
                                        }
                                    }
                                    onPointerEnterPunchPresetCategory.stringValue = newPresetCategoryName;
                                    onPointerEnterPunchPresetName.stringValue = newPresetName;
                                    serializedObject.ApplyModifiedProperties();
                                    RefreshPunchAnimations(true);
                                    onPointerEnterPunchNewPreset = false;
                                    onPointerEnterPunchNewCategoryName = false;
                                    newPresetCategoryName = "";
                                    newPresetName = "";
                                }
                            }
                            QUI.SetGUIBackgroundColor(DUIColors.RedLight.Color);
                            if (QUI.Button("Cancel"))
                            {
                                onPointerEnterPunchNewPreset = false;
                                onPointerEnterPunchNewCategoryName = false;
                                newPresetCategoryName = "";
                                newPresetName = "";
                            }
                            RestoreColors();
                        }
                        QUI.EndHorizontal();
                        if (!onPointerEnterPunchNewCategoryName)
                        {
                            QUI.BeginHorizontal(WIDTH_420 - 8);
                            {
                                QUI.Label("Select Category", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), WIDTH_105);
                                QUI.BeginChangeCheck();
                                {
                                    onPointerEnterPunchPresetCategoryNameIndex = EditorGUILayout.Popup(onPointerEnterPunchPresetCategoryNameIndex, UIAnimatorUtil.PunchPresetCategories.ToArray(), GUILayout.Width(200));
                                }
                                if (QUI.EndChangeCheck())
                                {
                                    onPointerEnterPunchPresetCategory.stringValue = UIAnimatorUtil.PunchPresetCategories[onPointerEnterPunchPresetCategoryNameIndex];
                                    onPointerEnterPunchPresetNameIndex = 0;
                                    newPresetCategoryName = onPointerEnterPunchPresetCategory.stringValue;
                                }
                                QUI.BeginChangeCheck();
                                {
                                    onPointerEnterPunchNewCategoryName = QUI.Toggle(onPointerEnterPunchNewCategoryName);
                                }
                                if (QUI.EndChangeCheck())
                                {
                                    if (onPointerEnterPunchNewCategoryName) { newPresetCategoryName = ""; }
                                }
                                QUI.Label("new category", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 80);
                            }
                            QUI.EndHorizontal();
                        }
                        else
                        {
                            QUI.BeginHorizontal(WIDTH_420 - 8);
                            {
                                QUI.Label("New Category", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), WIDTH_105);
                                newPresetCategoryName = EditorGUILayout.TextArea(newPresetCategoryName, GUILayout.Width(200));
                                onPointerEnterPunchNewCategoryName = QUI.Toggle(onPointerEnterPunchNewCategoryName);
                                QUI.Label("new category", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 80);
                            }
                            QUI.EndHorizontal();
                        }
                        QUI.BeginHorizontal(WIDTH_420 - 8);
                        {
                            QUI.Label("New Preset Name", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), WIDTH_105);
                            newPresetName = EditorGUILayout.TextArea(newPresetName);
                        }
                        QUI.EndHorizontal();
                    }
                    QUI.BeginHorizontal(WIDTH_420 - 8);
                    {
                        QUI.Toggle(loadOnPointerEnterPunchPresetAtRuntime);
                        QUI.Label("Load preset at runtime (overrides current settings)", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), WIDTH_420 - 12);
                    }
                    QUI.EndHorizontal();
                    QUI.Space(SPACE_2);
                }
                QUI.EndVertical();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnPointerEnterPunchMove()
        {
            if (QUI.Button(DUIStyles.GetStyle(onPointerEnterPunchMoveEnabled.boolValue ? DUIStyles.ButtonStyle.PunchMove : DUIStyles.ButtonStyle.PunchMoveDisabled), WIDTH_420, 18)) { onPointerEnterPunchMoveEnabled.boolValue = !onPointerEnterPunchMoveEnabled.boolValue; }
            showOnPointerEnterPunchMove.target = onPointerEnterPunchMoveEnabled.boolValue;
            DrawPunch(DUIColors.GreenLight.Color, showOnPointerEnterPunchMove, onPointerEnterPunchMovePunch, onPointerEnterPunchMoveStartDelay, onPointerEnterPunchMoveDuration, onPointerEnterPunchMoveVibrato, onPointerEnterPunchMoveElasticity);
        }
        void DrawOnPointerEnterPunchRotate()
        {
            if (QUI.Button(DUIStyles.GetStyle(onPointerEnterPunchRotateEnabled.boolValue ? DUIStyles.ButtonStyle.PunchRotate : DUIStyles.ButtonStyle.PunchRotateDisabled), WIDTH_420, 18)) { onPointerEnterPunchRotateEnabled.boolValue = !onPointerEnterPunchRotateEnabled.boolValue; }
            showOnPointerEnterPunchRotate.target = onPointerEnterPunchRotateEnabled.boolValue;
            DrawPunch(DUIColors.OrangeLight.Color, showOnPointerEnterPunchRotate, onPointerEnterPunchRotatePunch, onPointerEnterPunchRotateStartDelay, onPointerEnterPunchRotateDuration, onPointerEnterPunchRotateVibrato, onPointerEnterPunchRotateElasticity);
        }
        void DrawOnPointerEnterPunchScale()
        {
            if (QUI.Button(DUIStyles.GetStyle(onPointerEnterPunchScaleEnabled.boolValue ? DUIStyles.ButtonStyle.PunchScale : DUIStyles.ButtonStyle.PunchScaleDisabled), WIDTH_420, 18)) { onPointerEnterPunchScaleEnabled.boolValue = !onPointerEnterPunchScaleEnabled.boolValue; }
            showOnPointerEnterPunchScale.target = onPointerEnterPunchScaleEnabled.boolValue;
            DrawPunch(DUIColors.RedLight.Color, showOnPointerEnterPunchScale, onPointerEnterPunchScalePunch, onPointerEnterPunchScaleStartDelay, onPointerEnterPunchScaleDuration, onPointerEnterPunchScaleVibrato, onPointerEnterPunchScaleElasticity);
        }
        void DrawOnPointerEnterEventsToggleOn()
        {
            if (QUI.Button(DUIStyles.GetStyle(showOnPointerEnterEvents.target ? DUIStyles.ButtonStyle.Events : DUIStyles.ButtonStyle.EventsCollapsed), WIDTH_420, 18)) { showOnPointerEnterEvents.target = !showOnPointerEnterEvents.target; }
            if (QUI.BeginFadeGroup(showOnPointerEnterEvents.faded))
            {
                SaveColors();
                QUI.SetGUIBackgroundColor(DUIColors.BlueLight.Color);
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    QUI.PropertyField(OnPointerEnterToggleOn, new GUIContent() { text = "OnPointerEnterToggleOn" }, WIDTH_420 - 6);
                    QUI.Space(SPACE_8);
                }
                QUI.EndVertical();
                RestoreColors();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnPointerEnterGameEventsToggleOn()
        {
            if (QUI.Button(DUIStyles.GetStyle(showOnPointerEnterGameEvents.target ? DUIStyles.ButtonStyle.GameEvents : DUIStyles.ButtonStyle.GameEventsCollapsed), WIDTH_420, 18)) { showOnPointerEnterGameEvents.target = !showOnPointerEnterGameEvents.target; }
            if (QUI.BeginFadeGroup(showOnPointerEnterGameEvents.faded))
            {
                SaveColors();
                QUI.SetGUIBackgroundColor(DUIColors.BlueLight.Color);
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    QUI.DrawList(onPointerEnterGameEventsToggleOn, WIDTH_420, "No Game Events are sent OnPointerEnterToggleOn...");
                    QUI.Space(SPACE_8);
                }
                QUI.EndVertical();
                RestoreColors();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnPointerEnterNavigationToggleOn()
        {
            if (!UIManager.IsNavigationEnabled) { QUI.Space(-SPACE_2); return; }
            if (QUI.Button(DUIStyles.GetStyle(showOnPointerEnterNavigation.target ? DUIStyles.ButtonStyle.Navigation : DUIStyles.ButtonStyle.NavigationCollapsed), WIDTH_420, 18)) { showOnPointerEnterNavigation.target = !showOnPointerEnterNavigation.target; }
            if (QUI.BeginFadeGroup(showOnPointerEnterNavigation.faded))
            {
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    DrawNavigationData(uiToggle.onPointerEnterNavigationToggleOn, onPointerEnterEditorNavigationDataToggleOn);
                    QUI.Space(SPACE_8);
                }
                QUI.EndVertical();
                QUI.ResetColors();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnPointerEnterEventsToggleOff()
        {
            if (QUI.Button(DUIStyles.GetStyle(showOnPointerEnterEvents.target ? DUIStyles.ButtonStyle.Events : DUIStyles.ButtonStyle.EventsCollapsed), WIDTH_420, 18)) { showOnPointerEnterEvents.target = !showOnPointerEnterEvents.target; }
            if (QUI.BeginFadeGroup(showOnPointerEnterEvents.faded))
            {
                SaveColors();
                QUI.SetGUIBackgroundColor(DUIColors.BlueLight.Color);
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    QUI.PropertyField(OnPointerEnterToggleOff, new GUIContent() { text = "OnPointerEnterToggleOff" }, WIDTH_420 - 6);
                    QUI.Space(SPACE_8);
                }
                QUI.EndVertical();
                RestoreColors();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnPointerEnterGameEventsToggleOff()
        {
            if (QUI.Button(DUIStyles.GetStyle(showOnPointerEnterGameEvents.target ? DUIStyles.ButtonStyle.GameEvents : DUIStyles.ButtonStyle.GameEventsCollapsed), WIDTH_420, 18)) { showOnPointerEnterGameEvents.target = !showOnPointerEnterGameEvents.target; }
            if (QUI.BeginFadeGroup(showOnPointerEnterGameEvents.faded))
            {
                SaveColors();
                QUI.SetGUIBackgroundColor(DUIColors.BlueLight.Color);
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    QUI.DrawList(onPointerEnterGameEventsToggleOff, WIDTH_420, "No Game Events are sent OnPointerEnterToggleOff...");
                    QUI.Space(SPACE_8);
                }
                QUI.EndVertical();
                RestoreColors();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnPointerEnterNavigationToggleOff()
        {
            if (!UIManager.IsNavigationEnabled) { QUI.Space(-SPACE_2); return; }
            if (QUI.Button(DUIStyles.GetStyle(showOnPointerEnterNavigation.target ? DUIStyles.ButtonStyle.Navigation : DUIStyles.ButtonStyle.NavigationCollapsed), WIDTH_420, 18)) { showOnPointerEnterNavigation.target = !showOnPointerEnterNavigation.target; }
            if (QUI.BeginFadeGroup(showOnPointerEnterNavigation.faded))
            {
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    DrawNavigationData(uiToggle.onPointerEnterNavigationToggleOff, onPointerEnterEditorNavigationDataToggleOff);
                    QUI.Space(SPACE_8);
                }
                QUI.EndVertical();
                QUI.ResetColors();
            }
            QUI.EndFadeGroup();
        }

        void DrawOnPointerExit()
        {
            if (DUI.DUISettings == null || DUI.DUISettings.UIToggle_Inspector_HideOnPointerExit) { useOnPointerExit.boolValue = false; return; }
            QUI.BeginHorizontal(WIDTH_420);
            {
                if (!useOnPointerExit.boolValue)
                {
                    QUI.DrawTexture(DUIResources.barOnPointerExitDisabled.texture, 336, 21);
                    if (showOnPointerExit.target) { showOnPointerExit.target = false; }
                }
                else
                {
                    if (QUI.Button(DUIStyles.GetStyle(showOnPointerExit.target ? DUIStyles.ButtonStyle.OnPointerExit : DUIStyles.ButtonStyle.OnPointerExitCollapsed), 336, 21)) { showOnPointerExit.target = !showOnPointerExit.target; }
                }
                if (QUI.Button(DUIStyles.GetStyle(useOnPointerExit.boolValue ? DUIStyles.ButtonStyle.BarButtonEnabled : DUIStyles.ButtonStyle.BarButtonDisabled), 84, 21)) { useOnPointerExit.boolValue = !useOnPointerExit.boolValue; if (useOnPointerExit.boolValue) { showOnPointerExit.target = true; } }
            }
            QUI.EndHorizontal();
            infoMessage["OnPointerExitLoadPresetAtRuntime"].show.target = loadOnPointerExitPunchPresetAtRuntime.boolValue;
            infoMessage["OnPointerExitLoadPresetAtRuntime"].message = onPointerExitPunchPresetCategory.stringValue + " / " + onPointerExitPunchPresetName.stringValue;
            DrawInfoMessage("OnPointerExitLoadPresetAtRuntime", WIDTH_420);
            QUI.Space(SPACE_4);
            if (!useOnPointerExit.boolValue) { return; }
            if (QUI.BeginFadeGroup(showOnPointerExit.faded))
            {
                QUI.BeginVertical(WIDTH_420);
                {
                    DrawOnPointerExitSettings();
                    QUI.Space(SPACE_2);
                    DrawOnPointerExitSound();
                    QUI.Space(SPACE_2);
                    DrawOnPointerExitPreset();
                    QUI.Space(SPACE_2);
                    DrawOnPointerExitPunchMove();
                    QUI.Space(SPACE_2);
                    DrawOnPointerExitPunchRotate();
                    QUI.Space(SPACE_2);
                    DrawOnPointerExitPunchScale();
                    QUI.Space(SPACE_4);
                    QUI.DrawTexture(DUIResources.barToggleOn.texture, 420, 18);
                    QUI.Space(SPACE_2);
                    DrawOnPointerExitEventsToggleOn();
                    QUI.Space(SPACE_2);
                    DrawOnPointerExitGameEventsToggleOn();
                    QUI.Space(SPACE_2);
                    DrawOnPointerExitNavigationToggleOn();
                    QUI.Space(SPACE_4);
                    QUI.DrawTexture(DUIResources.barToggleOff.texture, 420, 18);
                    QUI.Space(SPACE_2);
                    DrawOnPointerExitEventsToggleOff();
                    QUI.Space(SPACE_2);
                    DrawOnPointerExitGameEventsToggleOff();
                    QUI.Space(SPACE_2);
                    DrawOnPointerExitNavigationToggleOff();
                    QUI.Space(SPACE_16);
                }
                QUI.EndVertical();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnPointerExitSettings()
        {
            QUI.BeginHorizontal(WIDTH_420);
            {
                QUI.Label("disable interval", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), WIDTH_105);
                QUI.PropertyField(onPointerExitDisableInterval, 38);
                QUI.Label("seconds", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 50);
            }
            QUI.EndHorizontal();
        }
        void DrawOnPointerExitSound()
        {
            QUI.BeginHorizontal(WIDTH_420);
            {
                QUI.DrawTexture(DUIResources.minibarToggleOn.texture, 100, 18);
                QUI.Space(80);
                QUI.Label("play sound", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 65);
                if (customOnPointerExitSoundToggleOn.boolValue)
                {
                    QUI.PropertyField(onPointerExitSoundToggleOn, 130);
                }
                else
                {
                    QUI.BeginChangeCheck();
                    {
                        onPointerExitSoundIndexToggleOn = EditorGUILayout.Popup(onPointerExitSoundIndexToggleOn, DUI.UISoundNamesUIButtons.ToArray(), GUILayout.Width(130));
                    }
                    if (QUI.EndChangeCheck())
                    {
                        Undo.RecordObject(uiToggle, "Updated Play Sound");
                        onPointerExitSoundToggleOn.stringValue = DUI.UISoundNamesUIButtons[onPointerExitSoundIndexToggleOn];
                    }
                }
                QUI.Space(SPACE_4);
                QUI.BeginChangeCheck();
                {
                    QUI.PropertyField(customOnPointerExitSoundToggleOn, 12);
                }
                if (QUI.EndChangeCheck())
                {
                    if (!customOnPointerExitSoundToggleOn.boolValue)
                    {
                        if (!DUI.UISoundNamesUIButtons.Contains(onPointerExitSoundToggleOn.stringValue))
                        {
                            if (EditorUtility.DisplayDialog("Action Required", "The '" + onPointerExitSoundToggleOn.stringValue + "' ui sound does not exist in the UISounds database.\nDo you want to add it now?", "Yes", "No"))
                            {
                                DUI.CreateUISound(onPointerExitSoundToggleOn.stringValue, SoundType.UIButtons, null);
                            }
                            else
                            {
                                onPointerExitSoundToggleOn.stringValue = DUI.DEFAULT_SOUND_NAME;
                            }
                        }
                        Undo.RecordObject(uiToggle, "Updated Play Sound");
                        onPointerExitSoundIndexToggleOn = DUI.UISoundNamesUIButtons.IndexOf(onPointerExitSoundToggleOn.stringValue);
                        RefreshUISounds(false);
                    }
                }
                QUI.Label("custom", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 48);
                QUI.Space(SPACE_2);
                if (QUI.Button(DUIStyles.GetStyle(DUIStyles.ButtonStyle.ButtonPlay), 18, 18)) { DUIUtils.PreviewSound(onPointerExitSoundToggleOn.stringValue); }
                QUI.Space(SPACE_2);
                if (QUI.Button(DUIStyles.GetStyle(DUIStyles.ButtonStyle.ButtonStop), 18, 18)) { DUIUtils.StopSoundPreview(onPointerExitSoundToggleOn.stringValue); }
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2);

            QUI.BeginHorizontal(WIDTH_420);
            {
                QUI.DrawTexture(DUIResources.minibarToggleOff.texture, 100, 18);
                QUI.Space(80);
                QUI.Label("play sound", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 65);
                if (customOnPointerExitSoundToggleOff.boolValue)
                {
                    QUI.PropertyField(onPointerExitSoundToggleOff, 130);
                }
                else
                {
                    QUI.BeginChangeCheck();
                    {
                        onPointerExitSoundIndexToggleOff = EditorGUILayout.Popup(onPointerExitSoundIndexToggleOff, DUI.UISoundNamesUIButtons.ToArray(), GUILayout.Width(130));
                    }
                    if (QUI.EndChangeCheck())
                    {
                        Undo.RecordObject(uiToggle, "Updated Play Sound");
                        onPointerExitSoundToggleOff.stringValue = DUI.UISoundNamesUIButtons[onPointerExitSoundIndexToggleOff];
                    }
                }
                QUI.Space(SPACE_4);
                QUI.BeginChangeCheck();
                {
                    QUI.PropertyField(customOnPointerExitSoundToggleOff, 12);
                }
                if (QUI.EndChangeCheck())
                {
                    if (!customOnPointerExitSoundToggleOff.boolValue)
                    {
                        if (!DUI.UISoundNamesUIButtons.Contains(onPointerExitSoundToggleOff.stringValue))
                        {
                            if (EditorUtility.DisplayDialog("Action Required", "The '" + onPointerExitSoundToggleOff.stringValue + "' ui sound does not exist in the UISounds database.\nDo you want to add it now?", "Yes", "No"))
                            {
                                DUI.CreateUISound(onPointerExitSoundToggleOff.stringValue, SoundType.UIButtons, null);
                            }
                            else
                            {
                                onPointerExitSoundToggleOff.stringValue = DUI.DEFAULT_SOUND_NAME;
                            }
                        }
                        Undo.RecordObject(uiToggle, "Updated Play Sound");
                        onPointerExitSoundIndexToggleOff = DUI.UISoundNamesUIButtons.IndexOf(onPointerExitSoundToggleOff.stringValue);
                        RefreshUISounds(false);
                    }
                }
                QUI.Label("custom", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 48);
                QUI.Space(SPACE_2);
                if (QUI.Button(DUIStyles.GetStyle(DUIStyles.ButtonStyle.ButtonPlay), 18, 18)) { DUIUtils.PreviewSound(onPointerExitSoundToggleOff.stringValue); }
                QUI.Space(SPACE_2);
                if (QUI.Button(DUIStyles.GetStyle(DUIStyles.ButtonStyle.ButtonStop), 18, 18)) { DUIUtils.StopSoundPreview(onPointerExitSoundToggleOff.stringValue); }
            }
            QUI.EndHorizontal();
        }
        void DrawOnPointerExitPreset()
        {
            if (QUI.Button(DUIStyles.GetStyle(showOnPointerExitPreset.target ? DUIStyles.ButtonStyle.barPunchPreset : DUIStyles.ButtonStyle.barPunchPresetCollapsed), WIDTH_420, 18)) { showOnPointerExitPreset.target = !showOnPointerExitPreset.target; }
            if (QUI.BeginFadeGroup(showOnPointerExitPreset.faded))
            {
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    if (!onPointerExitPunchNewPreset)
                    {
                        QUI.BeginHorizontal(WIDTH_420 - 8);
                        {
                            SaveColors();
                            QUI.SetGUIBackgroundColor(DUIColors.BlueLight.Color);
                            if (QUI.Button("Load Preset"))
                            {
                                if (serializedObject.isEditingMultipleObjects)
                                {
                                    Punch iPunch = UIAnimatorUtil.GetPunch(onPointerExitPunchPresetCategory.stringValue, onPointerExitPunchPresetName.stringValue);
                                    Undo.RecordObjects(targets, "Load Preset to Multiple Targets");
                                    for (int i = 0; i < targets.Length; i++)
                                    {
                                        UIButton iTarget = (UIButton)targets[i];
                                        iTarget.onPointerExitPunch = iPunch.Copy();
                                    }
                                }
                                else
                                {
                                    Undo.RecordObject(uiToggle, "Load Preset");
                                    uiToggle.onPointerExitPunch = UIAnimatorUtil.GetPunch(onPointerExitPunchPresetCategory.stringValue, onPointerExitPunchPresetName.stringValue);
                                }
                                QUI.ExitGUI();
                            }
                            QUI.SetGUIBackgroundColor(DUIColors.GreenLight.Color);
                            if (QUI.Button("New Preset"))
                            {
                                onPointerExitPunchNewPreset = true;
                                onPointerExitPunchNewCategoryName = false;
                                newPresetCategoryName = onPointerExitPunchPresetCategory.stringValue;
                                newPresetName = "";
                            }
                            QUI.SetGUIBackgroundColor(DUIColors.RedLight.Color);
                            if (QUI.Button("Delete Preset"))
                            {
                                if (EditorUtility.DisplayDialog("Delete Preset", "Are you sure you want to delete the '" + onPointerExitPunchPresetName.stringValue + "' preset from the '" + onPointerExitPunchPresetCategory.stringValue + "' preset category?", "Yes", "No"))
                                {
                                    UIAnimatorUtil.DeletePunchPreset(onPointerExitPunchPresetCategory.stringValue, onPointerExitPunchPresetName.stringValue);
                                    if (serializedObject.isEditingMultipleObjects)
                                    {
                                        for (int i = 0; i < targets.Length; i++)
                                        {
                                            UIButton iTarget = (UIButton)targets[i];
                                            if (iTarget.onPointerExitPunchPresetCategory.Equals(onPointerExitPunchPresetCategory.stringValue) ||
                                                iTarget.onPointerExitPunchPresetName.Equals(onPointerExitPunchPresetName.stringValue))
                                            {
                                                iTarget.onPointerExitPunchPresetCategory = UIAnimatorUtil.DEFAULT_PRESET_CATEGORY;
                                                iTarget.onPointerExitPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                            }
                                        }
                                    }
                                    onPointerExitPunchPresetCategory.stringValue = UIAnimatorUtil.DEFAULT_PRESET_CATEGORY;
                                    onPointerExitPunchPresetName.stringValue = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                    serializedObject.ApplyModifiedProperties();
                                    RefreshPunchAnimations(true);
                                }
                            }
                            RestoreColors();
                        }
                        QUI.EndHorizontal();
                        QUI.BeginHorizontal(WIDTH_420 - 8);
                        {
                            QUI.Label("Preset Category", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), WIDTH_105);
                            QUI.BeginChangeCheck();
                            {
                                onPointerExitPunchPresetCategoryNameIndex = EditorGUILayout.Popup(onPointerExitPunchPresetCategoryNameIndex, UIAnimatorUtil.PunchPresetCategories.ToArray());

                            }
                            if (QUI.EndChangeCheck())
                            {
                                onPointerExitPunchPresetCategory.stringValue = UIAnimatorUtil.PunchPresetCategories[onPointerExitPunchPresetCategoryNameIndex];
                                onPointerExitPunchPresetNameIndex = 0;
                                onPointerExitPunchPresetName.stringValue = UIAnimatorUtil.GetPunchPresetNames(onPointerExitPunchPresetCategory.stringValue)[onPointerExitPunchPresetNameIndex];
                            }

                        }
                        QUI.EndHorizontal();
                        QUI.BeginHorizontal(WIDTH_420 - 8);
                        {
                            QUI.Label("Preset Name", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), WIDTH_105);
                            QUI.BeginChangeCheck();
                            {
                                onPointerExitPunchPresetNameIndex = EditorGUILayout.Popup(onPointerExitPunchPresetNameIndex, UIAnimatorUtil.GetPunchPresetNames(onPointerExitPunchPresetCategory.stringValue).ToArray());
                            }
                            if (QUI.EndChangeCheck())
                            {
                                onPointerExitPunchPresetName.stringValue = UIAnimatorUtil.GetPunchPresetNames(onPointerExitPunchPresetCategory.stringValue)[onPointerExitPunchPresetNameIndex];
                            }
                        }
                        QUI.EndHorizontal();

                    }
                    else
                    {
                        QUI.BeginHorizontal(WIDTH_420 - 8);
                        {
                            SaveColors();
                            QUI.SetGUIBackgroundColor(DUIColors.GreenLight.Color);
                            if (QUI.Button("Save Preset"))
                            {
                                if (onPointerExitPunchNewCategoryName && string.IsNullOrEmpty(newPresetCategoryName.Trim()))
                                {
                                    EditorUtility.DisplayDialog("Information", "The new preset category name cannot be an empty string.", "Ok");
                                }
                                else if (string.IsNullOrEmpty(newPresetName.Trim()))
                                {
                                    EditorUtility.DisplayDialog("Information", "The new preset name cannot be an empty string.", "Ok");
                                }
                                else if (UIAnimatorUtil.PunchPresetExists(newPresetCategoryName, newPresetName))
                                {
                                    EditorUtility.DisplayDialog("Information", "There is another preset with the '" + newPresetName + "' preset name in the '" + newPresetCategoryName + "' preset category. Try a different preset name maybe?", "Ok");
                                }
                                else
                                {
                                    UIAnimatorUtil.CreatePunchPreset(newPresetCategoryName, newPresetName, uiToggle.onPointerExitPunch.Copy());
                                    if (serializedObject.isEditingMultipleObjects)
                                    {
                                        for (int i = 0; i < targets.Length; i++)
                                        {
                                            UIButton iTarget = (UIButton)targets[i];
                                            iTarget.onPointerExitPunchPresetCategory = newPresetCategoryName;
                                            iTarget.onPointerExitPunchPresetName = newPresetName;
                                        }
                                    }
                                    onPointerExitPunchPresetCategory.stringValue = newPresetCategoryName;
                                    onPointerExitPunchPresetName.stringValue = newPresetName;
                                    serializedObject.ApplyModifiedProperties();
                                    RefreshPunchAnimations(true);
                                    onPointerExitPunchNewPreset = false;
                                    onPointerExitPunchNewCategoryName = false;
                                    newPresetCategoryName = "";
                                    newPresetName = "";
                                }
                            }
                            QUI.SetGUIBackgroundColor(DUIColors.RedLight.Color);
                            if (QUI.Button("Cancel"))
                            {
                                onPointerExitPunchNewPreset = false;
                                onPointerExitPunchNewCategoryName = false;
                                newPresetCategoryName = "";
                                newPresetName = "";
                            }
                            RestoreColors();
                        }
                        QUI.EndHorizontal();
                        if (!onPointerExitPunchNewCategoryName)
                        {
                            QUI.BeginHorizontal(WIDTH_420 - 8);
                            {
                                QUI.Label("Select Category", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), WIDTH_105);
                                QUI.BeginChangeCheck();
                                {
                                    onPointerExitPunchPresetCategoryNameIndex = EditorGUILayout.Popup(onPointerExitPunchPresetCategoryNameIndex, UIAnimatorUtil.PunchPresetCategories.ToArray(), GUILayout.Width(200));
                                }
                                if (QUI.EndChangeCheck())
                                {
                                    onPointerExitPunchPresetCategory.stringValue = UIAnimatorUtil.PunchPresetCategories[onPointerExitPunchPresetCategoryNameIndex];
                                    onPointerExitPunchPresetNameIndex = 0;
                                    newPresetCategoryName = onPointerExitPunchPresetCategory.stringValue;
                                }
                                QUI.BeginChangeCheck();
                                {
                                    onPointerExitPunchNewCategoryName = QUI.Toggle(onPointerExitPunchNewCategoryName);
                                }
                                if (QUI.EndChangeCheck())
                                {
                                    if (onPointerExitPunchNewCategoryName) { newPresetCategoryName = ""; }
                                }
                                QUI.Label("new category", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 80);
                            }
                            QUI.EndHorizontal();
                        }
                        else
                        {
                            QUI.BeginHorizontal(WIDTH_420 - 8);
                            {
                                QUI.Label("New Category", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), WIDTH_105);
                                newPresetCategoryName = EditorGUILayout.TextArea(newPresetCategoryName, GUILayout.Width(200));
                                onPointerExitPunchNewCategoryName = QUI.Toggle(onPointerExitPunchNewCategoryName);
                                QUI.Label("new category", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 80);
                            }
                            QUI.EndHorizontal();
                        }
                        QUI.BeginHorizontal(WIDTH_420 - 8);
                        {
                            QUI.Label("New Preset Name", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), WIDTH_105);
                            newPresetName = EditorGUILayout.TextArea(newPresetName);
                        }
                        QUI.EndHorizontal();
                    }
                    QUI.BeginHorizontal(WIDTH_420 - 8);
                    {
                        QUI.Toggle(loadOnPointerExitPunchPresetAtRuntime);
                        QUI.Label("Load preset at runtime (overrides current settings)", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), WIDTH_420 - 12);
                    }
                    QUI.EndHorizontal();
                    QUI.Space(SPACE_2);
                }
                QUI.EndVertical();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnPointerExitPunchMove()
        {
            if (QUI.Button(DUIStyles.GetStyle(onPointerExitPunchMoveEnabled.boolValue ? DUIStyles.ButtonStyle.PunchMove : DUIStyles.ButtonStyle.PunchMoveDisabled), WIDTH_420, 18)) { onPointerExitPunchMoveEnabled.boolValue = !onPointerExitPunchMoveEnabled.boolValue; }
            showOnPointerExitPunchMove.target = onPointerExitPunchMoveEnabled.boolValue;
            DrawPunch(DUIColors.GreenLight.Color, showOnPointerExitPunchMove, onPointerExitPunchMovePunch, onPointerExitPunchMoveStartDelay, onPointerExitPunchMoveDuration, onPointerExitPunchMoveVibrato, onPointerExitPunchMoveElasticity);
        }
        void DrawOnPointerExitPunchRotate()
        {
            if (QUI.Button(DUIStyles.GetStyle(onPointerExitPunchRotateEnabled.boolValue ? DUIStyles.ButtonStyle.PunchRotate : DUIStyles.ButtonStyle.PunchRotateDisabled), WIDTH_420, 18)) { onPointerExitPunchRotateEnabled.boolValue = !onPointerExitPunchRotateEnabled.boolValue; }
            showOnPointerExitPunchRotate.target = onPointerExitPunchRotateEnabled.boolValue;
            DrawPunch(DUIColors.OrangeLight.Color, showOnPointerExitPunchRotate, onPointerExitPunchRotatePunch, onPointerExitPunchRotateStartDelay, onPointerExitPunchRotateDuration, onPointerExitPunchRotateVibrato, onPointerExitPunchRotateElasticity);
        }
        void DrawOnPointerExitPunchScale()
        {
            if (QUI.Button(DUIStyles.GetStyle(onPointerExitPunchScaleEnabled.boolValue ? DUIStyles.ButtonStyle.PunchScale : DUIStyles.ButtonStyle.PunchScaleDisabled), WIDTH_420, 18)) { onPointerExitPunchScaleEnabled.boolValue = !onPointerExitPunchScaleEnabled.boolValue; }
            showOnPointerExitPunchScale.target = onPointerExitPunchScaleEnabled.boolValue;
            DrawPunch(DUIColors.RedLight.Color, showOnPointerExitPunchScale, onPointerExitPunchScalePunch, onPointerExitPunchScaleStartDelay, onPointerExitPunchScaleDuration, onPointerExitPunchScaleVibrato, onPointerExitPunchScaleElasticity);
        }
        void DrawOnPointerExitEventsToggleOn()
        {
            if (QUI.Button(DUIStyles.GetStyle(showOnPointerExitEvents.target ? DUIStyles.ButtonStyle.Events : DUIStyles.ButtonStyle.EventsCollapsed), WIDTH_420, 18)) { showOnPointerExitEvents.target = !showOnPointerExitEvents.target; }
            if (QUI.BeginFadeGroup(showOnPointerExitEvents.faded))
            {
                SaveColors();
                QUI.SetGUIBackgroundColor(DUIColors.BlueLight.Color);
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    QUI.PropertyField(OnPointerExitToggleOn, new GUIContent() { text = "OnPointerExitToggleOn" }, WIDTH_420 - 6);
                    QUI.Space(SPACE_8);
                }
                QUI.EndVertical();
                RestoreColors();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnPointerExitGameEventsToggleOn()
        {
            if (QUI.Button(DUIStyles.GetStyle(showOnPointerExitGameEvents.target ? DUIStyles.ButtonStyle.GameEvents : DUIStyles.ButtonStyle.GameEventsCollapsed), WIDTH_420, 18)) { showOnPointerExitGameEvents.target = !showOnPointerExitGameEvents.target; }
            if (QUI.BeginFadeGroup(showOnPointerExitGameEvents.faded))
            {
                SaveColors();
                QUI.SetGUIBackgroundColor(DUIColors.BlueLight.Color);
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    QUI.DrawList(onPointerExitGameEventsToggleOn, WIDTH_420, "No Game Events are sent OnPointerExitToggleOn...");
                    QUI.Space(SPACE_8);
                }
                QUI.EndVertical();
                RestoreColors();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnPointerExitNavigationToggleOn()
        {
            if (!UIManager.IsNavigationEnabled) { QUI.Space(-SPACE_2); return; }
            if (QUI.Button(DUIStyles.GetStyle(showOnPointerExitNavigation.target ? DUIStyles.ButtonStyle.Navigation : DUIStyles.ButtonStyle.NavigationCollapsed), WIDTH_420, 18)) { showOnPointerExitNavigation.target = !showOnPointerExitNavigation.target; }
            if (QUI.BeginFadeGroup(showOnPointerExitNavigation.faded))
            {
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    DrawNavigationData(uiToggle.onPointerExitNavigationToggleOn, onPointerExitEditorNavigationDataToggleOn);
                    QUI.Space(SPACE_8);
                }
                QUI.EndVertical();
                QUI.ResetColors();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnPointerExitEventsToggleOff()
        {
            if (QUI.Button(DUIStyles.GetStyle(showOnPointerExitEvents.target ? DUIStyles.ButtonStyle.Events : DUIStyles.ButtonStyle.EventsCollapsed), WIDTH_420, 18)) { showOnPointerExitEvents.target = !showOnPointerExitEvents.target; }
            if (QUI.BeginFadeGroup(showOnPointerExitEvents.faded))
            {
                SaveColors();
                QUI.SetGUIBackgroundColor(DUIColors.BlueLight.Color);
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    QUI.PropertyField(OnPointerExitToggleOff, new GUIContent() { text = "OnPointerExitToggleOff" }, WIDTH_420 - 6);
                    QUI.Space(SPACE_8);
                }
                QUI.EndVertical();
                RestoreColors();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnPointerExitGameEventsToggleOff()
        {
            if (QUI.Button(DUIStyles.GetStyle(showOnPointerExitGameEvents.target ? DUIStyles.ButtonStyle.GameEvents : DUIStyles.ButtonStyle.GameEventsCollapsed), WIDTH_420, 18)) { showOnPointerExitGameEvents.target = !showOnPointerExitGameEvents.target; }
            if (QUI.BeginFadeGroup(showOnPointerExitGameEvents.faded))
            {
                SaveColors();
                QUI.SetGUIBackgroundColor(DUIColors.BlueLight.Color);
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    QUI.DrawList(onPointerExitGameEventsToggleOff, WIDTH_420, "No Game Events are sent OnPointerExitToggleOff...");
                    QUI.Space(SPACE_8);
                }
                QUI.EndVertical();
                RestoreColors();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnPointerExitNavigationToggleOff()
        {
            if (!UIManager.IsNavigationEnabled) { QUI.Space(-SPACE_2); return; }
            if (QUI.Button(DUIStyles.GetStyle(showOnPointerExitNavigation.target ? DUIStyles.ButtonStyle.Navigation : DUIStyles.ButtonStyle.NavigationCollapsed), WIDTH_420, 18)) { showOnPointerExitNavigation.target = !showOnPointerExitNavigation.target; }
            if (QUI.BeginFadeGroup(showOnPointerExitNavigation.faded))
            {
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    DrawNavigationData(uiToggle.onPointerExitNavigationToggleOff, onPointerExitEditorNavigationDataToggleOff);
                    QUI.Space(SPACE_8);
                }
                QUI.EndVertical();
                QUI.ResetColors();
            }
            QUI.EndFadeGroup();
        }

        void DrawOnClick()
        {
            if (DUI.DUISettings == null || DUI.DUISettings.UIToggle_Inspector_HideOnClick) { useOnClick.boolValue = false; return; }
            QUI.BeginHorizontal(WIDTH_420);
            {
                if (!useOnClick.boolValue)
                {
                    QUI.DrawTexture(DUIResources.barOnClickDisabled.texture, 336, 21);
                    if (showOnClick.target) { showOnClick.target = false; }
                }
                else
                {
                    if (QUI.Button(DUIStyles.GetStyle(showOnClick.target ? DUIStyles.ButtonStyle.OnClick : DUIStyles.ButtonStyle.OnClickCollapsed), 336, 21)) { showOnClick.target = !showOnClick.target; }
                }
                if (QUI.Button(DUIStyles.GetStyle(useOnClick.boolValue ? DUIStyles.ButtonStyle.BarButtonEnabled : DUIStyles.ButtonStyle.BarButtonDisabled), 84, 21)) { useOnClick.boolValue = !useOnClick.boolValue; if (useOnClick.boolValue) { showOnClick.target = true; } }
            }
            QUI.EndHorizontal();
            infoMessage["OnClickLoadPresetAtRuntime"].show.target = loadOnClickPunchPresetAtRuntime.boolValue;
            infoMessage["OnClickLoadPresetAtRuntime"].message = onClickPunchPresetCategory.stringValue + " / " + onClickPunchPresetName.stringValue;
            DrawInfoMessage("OnClickLoadPresetAtRuntime", WIDTH_420);
            QUI.Space(SPACE_4);
            if (!useOnClick.boolValue) { return; }
            if (QUI.BeginFadeGroup(showOnClick.faded))
            {
                QUI.BeginVertical(WIDTH_420);
                {
                    DrawOnClickSettings();
                    QUI.Space(SPACE_2);
                    DrawOnClickSound();
                    QUI.Space(SPACE_2);
                    DrawOnClickPreset();
                    QUI.Space(SPACE_2);
                    DrawOnClickPunchMove();
                    QUI.Space(SPACE_2);
                    DrawOnClickPunchRotate();
                    QUI.Space(SPACE_2);
                    DrawOnClickPunchScale();
                    QUI.Space(SPACE_4);
                    QUI.DrawTexture(DUIResources.barToggleOn.texture, 420, 18);
                    QUI.Space(SPACE_2);
                    DrawOnClickEventsToggleOn();
                    QUI.Space(SPACE_2);
                    DrawOnClickGameEventsToggleOn();
                    QUI.Space(SPACE_2);
                    DrawOnClickNavigationToggleOn();
                    QUI.Space(SPACE_4);
                    QUI.DrawTexture(DUIResources.barToggleOff.texture, 420, 18);
                    QUI.Space(SPACE_2);
                    DrawOnClickEventsToggleOff();
                    QUI.Space(SPACE_2);
                    DrawOnClickGameEventsToggleOff();
                    QUI.Space(SPACE_2);
                    DrawOnClickNavigationToggleOff();
                    QUI.Space(SPACE_16);
                }
                QUI.EndVertical();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnClickSettings()
        {
            QUI.BeginHorizontal(WIDTH_420);
            {
                QUI.PropertyField(waitForOnClick, 12);
                QUI.Label("wait for animation", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 120);
            }
            QUI.EndHorizontal();
        }
        void DrawOnClickSound()
        {
            QUI.BeginHorizontal(WIDTH_420);
            {
                QUI.DrawTexture(DUIResources.minibarToggleOn.texture, 100, 18);
                QUI.Space(80);
                QUI.Label("play sound", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 65);
                if (customOnClickSoundToggleOn.boolValue)
                {
                    QUI.PropertyField(onClickSoundToggleOn, 130);
                }
                else
                {
                    QUI.BeginChangeCheck();
                    {
                        onClickSoundIndexToggleOn = EditorGUILayout.Popup(onClickSoundIndexToggleOn, DUI.UISoundNamesUIButtons.ToArray(), GUILayout.Width(130));
                    }
                    if (QUI.EndChangeCheck())
                    {
                        Undo.RecordObject(uiToggle, "Updated Play Sound");
                        onClickSoundToggleOn.stringValue = DUI.UISoundNamesUIButtons[onClickSoundIndexToggleOn];
                    }
                }
                QUI.Space(SPACE_4);
                QUI.BeginChangeCheck();
                {
                    QUI.PropertyField(customOnClickSoundToggleOn, 12);
                }
                if (QUI.EndChangeCheck())
                {
                    if (!customOnClickSoundToggleOn.boolValue)
                    {
                        if (!DUI.UISoundNamesUIButtons.Contains(onClickSoundToggleOn.stringValue))
                        {
                            if (EditorUtility.DisplayDialog("Action Required", "The '" + onClickSoundToggleOn.stringValue + "' ui sound does not exist in the UISounds database.\nDo you want to add it now?", "Yes", "No"))
                            {
                                DUI.CreateUISound(onClickSoundToggleOn.stringValue, SoundType.UIButtons, null);
                            }
                            else
                            {
                                onClickSoundToggleOn.stringValue = DUI.DEFAULT_SOUND_NAME;
                            }
                        }
                        Undo.RecordObject(uiToggle, "Updated Play Sound");
                        onClickSoundIndexToggleOn = DUI.UISoundNamesUIButtons.IndexOf(onClickSoundToggleOn.stringValue);
                        RefreshUISounds(false);
                    }
                }
                QUI.Label("custom", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 48);
                QUI.Space(SPACE_2);
                if (QUI.Button(DUIStyles.GetStyle(DUIStyles.ButtonStyle.ButtonPlay), 18, 18)) { DUIUtils.PreviewSound(onClickSoundToggleOn.stringValue); }
                QUI.Space(SPACE_2);
                if (QUI.Button(DUIStyles.GetStyle(DUIStyles.ButtonStyle.ButtonStop), 18, 18)) { DUIUtils.StopSoundPreview(onClickSoundToggleOn.stringValue); }
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2);

            QUI.BeginHorizontal(WIDTH_420);
            {
                QUI.DrawTexture(DUIResources.minibarToggleOff.texture, 100, 18);
                QUI.Space(80);
                QUI.Label("play sound", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 65);
                if (customOnClickSoundToggleOff.boolValue)
                {
                    QUI.PropertyField(onClickSoundToggleOff, 130);
                }
                else
                {
                    QUI.BeginChangeCheck();
                    {
                        onClickSoundIndexToggleOff = EditorGUILayout.Popup(onClickSoundIndexToggleOff, DUI.UISoundNamesUIButtons.ToArray(), GUILayout.Width(130));
                    }
                    if (QUI.EndChangeCheck())
                    {
                        Undo.RecordObject(uiToggle, "Updated Play Sound");
                        onClickSoundToggleOff.stringValue = DUI.UISoundNamesUIButtons[onClickSoundIndexToggleOff];
                    }
                }
                QUI.Space(SPACE_4);
                QUI.BeginChangeCheck();
                {
                    QUI.PropertyField(customOnClickSoundToggleOff, 12);
                }
                if (QUI.EndChangeCheck())
                {
                    if (!customOnClickSoundToggleOff.boolValue)
                    {
                        if (!DUI.UISoundNamesUIButtons.Contains(onClickSoundToggleOff.stringValue))
                        {
                            if (EditorUtility.DisplayDialog("Action Required", "The '" + onClickSoundToggleOff.stringValue + "' ui sound does not exist in the UISounds database.\nDo you want to add it now?", "Yes", "No"))
                            {
                                DUI.CreateUISound(onClickSoundToggleOff.stringValue, SoundType.UIButtons, null);
                            }
                            else
                            {
                                onClickSoundToggleOff.stringValue = DUI.DEFAULT_SOUND_NAME;
                            }
                        }
                        Undo.RecordObject(uiToggle, "Updated Play Sound");
                        onClickSoundIndexToggleOff = DUI.UISoundNamesUIButtons.IndexOf(onClickSoundToggleOff.stringValue);
                        RefreshUISounds(false);
                    }
                }
                QUI.Label("custom", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 48);
                QUI.Space(SPACE_2);
                if (QUI.Button(DUIStyles.GetStyle(DUIStyles.ButtonStyle.ButtonPlay), 18, 18)) { DUIUtils.PreviewSound(onClickSoundToggleOff.stringValue); }
                QUI.Space(SPACE_2);
                if (QUI.Button(DUIStyles.GetStyle(DUIStyles.ButtonStyle.ButtonStop), 18, 18)) { DUIUtils.StopSoundPreview(onClickSoundToggleOff.stringValue); }
            }
            QUI.EndHorizontal();
        }
        void DrawOnClickPreset()
        {
            if (QUI.Button(DUIStyles.GetStyle(showOnClickPreset.target ? DUIStyles.ButtonStyle.barPunchPreset : DUIStyles.ButtonStyle.barPunchPresetCollapsed), WIDTH_420, 18)) { showOnClickPreset.target = !showOnClickPreset.target; }
            if (QUI.BeginFadeGroup(showOnClickPreset.faded))
            {
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    if (!onClickPunchNewPreset)
                    {
                        QUI.BeginHorizontal(WIDTH_420 - 8);
                        {
                            SaveColors();
                            QUI.SetGUIBackgroundColor(DUIColors.BlueLight.Color);
                            if (QUI.Button("Load Preset"))
                            {
                                if (serializedObject.isEditingMultipleObjects)
                                {
                                    Punch iPunch = UIAnimatorUtil.GetPunch(onClickPunchPresetCategory.stringValue, onClickPunchPresetName.stringValue);
                                    Undo.RecordObjects(targets, "Load Preset to Multiple Targets");
                                    for (int i = 0; i < targets.Length; i++)
                                    {
                                        UIButton iTarget = (UIButton)targets[i];
                                        iTarget.onClickPunch = iPunch.Copy();
                                    }
                                }
                                else
                                {
                                    Undo.RecordObject(uiToggle, "Load Preset");
                                    uiToggle.onClickPunch = UIAnimatorUtil.GetPunch(onClickPunchPresetCategory.stringValue, onClickPunchPresetName.stringValue);
                                }
                                QUI.ExitGUI();
                            }
                            QUI.SetGUIBackgroundColor(DUIColors.GreenLight.Color);
                            if (QUI.Button("New Preset"))
                            {
                                onClickPunchNewPreset = true;
                                onClickPunchNewCategoryName = false;
                                newPresetCategoryName = onClickPunchPresetCategory.stringValue;
                                newPresetName = "";
                            }
                            QUI.SetGUIBackgroundColor(DUIColors.RedLight.Color);
                            if (QUI.Button("Delete Preset"))
                            {
                                if (EditorUtility.DisplayDialog("Delete Preset", "Are you sure you want to delete the '" + onClickPunchPresetName.stringValue + "' preset from the '" + onClickPunchPresetCategory.stringValue + "' preset category?", "Yes", "No"))
                                {
                                    UIAnimatorUtil.DeletePunchPreset(onClickPunchPresetCategory.stringValue, onClickPunchPresetName.stringValue);
                                    if (serializedObject.isEditingMultipleObjects)
                                    {
                                        for (int i = 0; i < targets.Length; i++)
                                        {
                                            UIButton iTarget = (UIButton)targets[i];
                                            if (iTarget.onClickPunchPresetCategory.Equals(onClickPunchPresetCategory.stringValue) ||
                                                iTarget.onClickPunchPresetName.Equals(onClickPunchPresetName.stringValue))
                                            {
                                                iTarget.onClickPunchPresetCategory = UIAnimatorUtil.DEFAULT_PRESET_CATEGORY;
                                                iTarget.onClickPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                            }
                                        }
                                    }
                                    onClickPunchPresetCategory.stringValue = UIAnimatorUtil.DEFAULT_PRESET_CATEGORY;
                                    onClickPunchPresetName.stringValue = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                    serializedObject.ApplyModifiedProperties();
                                    RefreshPunchAnimations(true);
                                }
                            }
                            RestoreColors();
                        }
                        QUI.EndHorizontal();
                        QUI.BeginHorizontal(WIDTH_420 - 8);
                        {
                            QUI.Label("Preset Category", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), WIDTH_105);
                            QUI.BeginChangeCheck();
                            {
                                onClickPunchPresetCategoryNameIndex = EditorGUILayout.Popup(onClickPunchPresetCategoryNameIndex, UIAnimatorUtil.PunchPresetCategories.ToArray());

                            }
                            if (QUI.EndChangeCheck())
                            {
                                onClickPunchPresetCategory.stringValue = UIAnimatorUtil.PunchPresetCategories[onClickPunchPresetCategoryNameIndex];
                                onClickPunchPresetNameIndex = 0;
                                onClickPunchPresetName.stringValue = UIAnimatorUtil.GetPunchPresetNames(onClickPunchPresetCategory.stringValue)[onClickPunchPresetNameIndex];
                            }

                        }
                        QUI.EndHorizontal();
                        QUI.BeginHorizontal(WIDTH_420 - 8);
                        {
                            QUI.Label("Preset Name", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), WIDTH_105);
                            QUI.BeginChangeCheck();
                            {
                                onClickPunchPresetNameIndex = EditorGUILayout.Popup(onClickPunchPresetNameIndex, UIAnimatorUtil.GetPunchPresetNames(onClickPunchPresetCategory.stringValue).ToArray());
                            }
                            if (QUI.EndChangeCheck())
                            {
                                onClickPunchPresetName.stringValue = UIAnimatorUtil.GetPunchPresetNames(onClickPunchPresetCategory.stringValue)[onClickPunchPresetNameIndex];
                            }
                        }
                        QUI.EndHorizontal();

                    }
                    else
                    {
                        QUI.BeginHorizontal(WIDTH_420 - 8);
                        {
                            SaveColors();
                            QUI.SetGUIBackgroundColor(DUIColors.GreenLight.Color);
                            if (QUI.Button("Save Preset"))
                            {
                                if (onClickPunchNewCategoryName && string.IsNullOrEmpty(newPresetCategoryName.Trim()))
                                {
                                    EditorUtility.DisplayDialog("Information", "The new preset category name cannot be an empty string.", "Ok");
                                }
                                else if (string.IsNullOrEmpty(newPresetName.Trim()))
                                {
                                    EditorUtility.DisplayDialog("Information", "The new preset name cannot be an empty string.", "Ok");
                                }
                                else if (UIAnimatorUtil.PunchPresetExists(newPresetCategoryName, newPresetName))
                                {
                                    EditorUtility.DisplayDialog("Information", "There is another preset with the '" + newPresetName + "' preset name in the '" + newPresetCategoryName + "' preset category. Try a different preset name maybe?", "Ok");
                                }
                                else
                                {
                                    UIAnimatorUtil.CreatePunchPreset(newPresetCategoryName, newPresetName, uiToggle.onClickPunch.Copy());
                                    if (serializedObject.isEditingMultipleObjects)
                                    {
                                        for (int i = 0; i < targets.Length; i++)
                                        {
                                            UIButton iTarget = (UIButton)targets[i];
                                            iTarget.onClickPunchPresetCategory = newPresetCategoryName;
                                            iTarget.onClickPunchPresetName = newPresetName;
                                        }
                                    }
                                    onClickPunchPresetCategory.stringValue = newPresetCategoryName;
                                    onClickPunchPresetName.stringValue = newPresetName;
                                    serializedObject.ApplyModifiedProperties();
                                    RefreshPunchAnimations(true);
                                    onClickPunchNewPreset = false;
                                    onClickPunchNewCategoryName = false;
                                    newPresetCategoryName = "";
                                    newPresetName = "";
                                }
                            }
                            QUI.SetGUIBackgroundColor(DUIColors.RedLight.Color);
                            if (QUI.Button("Cancel"))
                            {
                                onClickPunchNewPreset = false;
                                onClickPunchNewCategoryName = false;
                                newPresetCategoryName = "";
                                newPresetName = "";
                            }
                            RestoreColors();
                        }
                        QUI.EndHorizontal();
                        if (!onClickPunchNewCategoryName)
                        {
                            QUI.BeginHorizontal(WIDTH_420 - 8);
                            {
                                QUI.Label("Select Category", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), WIDTH_105);
                                QUI.BeginChangeCheck();
                                {
                                    onClickPunchPresetCategoryNameIndex = EditorGUILayout.Popup(onClickPunchPresetCategoryNameIndex, UIAnimatorUtil.PunchPresetCategories.ToArray(), GUILayout.Width(200));
                                }
                                if (QUI.EndChangeCheck())
                                {
                                    onClickPunchPresetCategory.stringValue = UIAnimatorUtil.PunchPresetCategories[onClickPunchPresetCategoryNameIndex];
                                    onClickPunchPresetNameIndex = 0;
                                    newPresetCategoryName = onClickPunchPresetCategory.stringValue;
                                }
                                QUI.BeginChangeCheck();
                                {
                                    onClickPunchNewCategoryName = QUI.Toggle(onClickPunchNewCategoryName);
                                }
                                if (QUI.EndChangeCheck())
                                {
                                    if (onClickPunchNewCategoryName) { newPresetCategoryName = ""; }
                                }
                                QUI.Label("new category", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 80);
                            }
                            QUI.EndHorizontal();
                        }
                        else
                        {
                            QUI.BeginHorizontal(WIDTH_420 - 8);
                            {
                                QUI.Label("New Category", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), WIDTH_105);
                                newPresetCategoryName = EditorGUILayout.TextArea(newPresetCategoryName, GUILayout.Width(200));
                                onClickPunchNewCategoryName = QUI.Toggle(onClickPunchNewCategoryName);
                                QUI.Label("new category", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 80);
                            }
                            QUI.EndHorizontal();
                        }
                        QUI.BeginHorizontal(WIDTH_420 - 8);
                        {
                            QUI.Label("New Preset Name", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), WIDTH_105);
                            newPresetName = EditorGUILayout.TextArea(newPresetName);
                        }
                        QUI.EndHorizontal();
                    }
                    QUI.BeginHorizontal(WIDTH_420 - 8);
                    {
                        QUI.Toggle(loadOnClickPunchPresetAtRuntime);
                        QUI.Label("Load preset at runtime (overrides current settings)", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), WIDTH_420 - 12);
                    }
                    QUI.EndHorizontal();
                    QUI.Space(SPACE_2);
                }
                QUI.EndVertical();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnClickPunchMove()
        {
            if (QUI.Button(DUIStyles.GetStyle(onClickPunchMoveEnabled.boolValue ? DUIStyles.ButtonStyle.PunchMove : DUIStyles.ButtonStyle.PunchMoveDisabled), WIDTH_420, 18)) { onClickPunchMoveEnabled.boolValue = !onClickPunchMoveEnabled.boolValue; }
            showOnClickPunchMove.target = onClickPunchMoveEnabled.boolValue;
            DrawPunch(DUIColors.GreenLight.Color, showOnClickPunchMove, onClickPunchMovePunch, onClickPunchMoveStartDelay, onClickPunchMoveDuration, onClickPunchMoveVibrato, onClickPunchMoveElasticity);
        }
        void DrawOnClickPunchRotate()
        {
            if (QUI.Button(DUIStyles.GetStyle(onClickPunchRotateEnabled.boolValue ? DUIStyles.ButtonStyle.PunchRotate : DUIStyles.ButtonStyle.PunchRotateDisabled), WIDTH_420, 18)) { onClickPunchRotateEnabled.boolValue = !onClickPunchRotateEnabled.boolValue; }
            showOnClickPunchRotate.target = onClickPunchRotateEnabled.boolValue;
            DrawPunch(DUIColors.OrangeLight.Color, showOnClickPunchRotate, onClickPunchRotatePunch, onClickPunchRotateStartDelay, onClickPunchRotateDuration, onClickPunchRotateVibrato, onClickPunchRotateElasticity);
        }
        void DrawOnClickPunchScale()
        {
            if (QUI.Button(DUIStyles.GetStyle(onClickPunchScaleEnabled.boolValue ? DUIStyles.ButtonStyle.PunchScale : DUIStyles.ButtonStyle.PunchScaleDisabled), WIDTH_420, 18)) { onClickPunchScaleEnabled.boolValue = !onClickPunchScaleEnabled.boolValue; }
            showOnClickPunchScale.target = onClickPunchScaleEnabled.boolValue;
            DrawPunch(DUIColors.RedLight.Color, showOnClickPunchScale, onClickPunchScalePunch, onClickPunchScaleStartDelay, onClickPunchScaleDuration, onClickPunchScaleVibrato, onClickPunchScaleElasticity);
        }
        void DrawOnClickEventsToggleOn()
        {
            if (QUI.Button(DUIStyles.GetStyle(showOnClickEvents.target ? DUIStyles.ButtonStyle.Events : DUIStyles.ButtonStyle.EventsCollapsed), WIDTH_420, 18)) { showOnClickEvents.target = !showOnClickEvents.target; }
            if (QUI.BeginFadeGroup(showOnClickEvents.faded))
            {
                SaveColors();
                QUI.SetGUIBackgroundColor(DUIColors.BlueLight.Color);
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    QUI.PropertyField(OnClickToggleOn, new GUIContent() { text = "OnClickToggleOn" }, WIDTH_420 - 6);
                    QUI.Space(SPACE_8);
                }
                QUI.EndVertical();
                RestoreColors();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnClickGameEventsToggleOn()
        {
            if (QUI.Button(DUIStyles.GetStyle(showOnClickGameEvents.target ? DUIStyles.ButtonStyle.GameEvents : DUIStyles.ButtonStyle.GameEventsCollapsed), WIDTH_420, 18)) { showOnClickGameEvents.target = !showOnClickGameEvents.target; }
            if (QUI.BeginFadeGroup(showOnClickGameEvents.faded))
            {
                SaveColors();
                QUI.SetGUIBackgroundColor(DUIColors.BlueLight.Color);
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    QUI.DrawList(onClickGameEventsToggleOn, WIDTH_420, "No Game Events are sent OnClickToggleOn...");
                    QUI.Space(SPACE_8);
                }
                QUI.EndVertical();
                RestoreColors();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnClickNavigationToggleOn()
        {
            if (!UIManager.IsNavigationEnabled) { QUI.Space(-SPACE_2); return; }
            if (QUI.Button(DUIStyles.GetStyle(showOnClickNavigation.target ? DUIStyles.ButtonStyle.Navigation : DUIStyles.ButtonStyle.NavigationCollapsed), WIDTH_420, 18)) { showOnClickNavigation.target = !showOnClickNavigation.target; }
            if (QUI.BeginFadeGroup(showOnClickNavigation.faded))
            {
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    DrawNavigationData(uiToggle.onClickNavigationToggleOn, onClickEditorNavigationDataToggleOn);
                    QUI.Space(SPACE_8);
                }
                QUI.EndVertical();
                QUI.ResetColors();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnClickEventsToggleOff()
        {
            if (QUI.Button(DUIStyles.GetStyle(showOnClickEvents.target ? DUIStyles.ButtonStyle.Events : DUIStyles.ButtonStyle.EventsCollapsed), WIDTH_420, 18)) { showOnClickEvents.target = !showOnClickEvents.target; }
            if (QUI.BeginFadeGroup(showOnClickEvents.faded))
            {
                SaveColors();
                QUI.SetGUIBackgroundColor(DUIColors.BlueLight.Color);
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    QUI.PropertyField(OnClickToggleOff, new GUIContent() { text = "OnClickToggleOff" }, WIDTH_420 - 6);
                    QUI.Space(SPACE_8);
                }
                QUI.EndVertical();
                RestoreColors();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnClickGameEventsToggleOff()
        {
            if (QUI.Button(DUIStyles.GetStyle(showOnClickGameEvents.target ? DUIStyles.ButtonStyle.GameEvents : DUIStyles.ButtonStyle.GameEventsCollapsed), WIDTH_420, 18)) { showOnClickGameEvents.target = !showOnClickGameEvents.target; }
            if (QUI.BeginFadeGroup(showOnClickGameEvents.faded))
            {
                SaveColors();
                QUI.SetGUIBackgroundColor(DUIColors.BlueLight.Color);
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    QUI.DrawList(onClickGameEventsToggleOff, WIDTH_420, "No Game Events are sent OnClickToggleOff...");
                    QUI.Space(SPACE_8);
                }
                QUI.EndVertical();
                RestoreColors();
            }
            QUI.EndFadeGroup();
        }
        void DrawOnClickNavigationToggleOff()
        {
            if (!UIManager.IsNavigationEnabled) { QUI.Space(-SPACE_2); return; }
            if (QUI.Button(DUIStyles.GetStyle(showOnClickNavigation.target ? DUIStyles.ButtonStyle.Navigation : DUIStyles.ButtonStyle.NavigationCollapsed), WIDTH_420, 18)) { showOnClickNavigation.target = !showOnClickNavigation.target; }
            if (QUI.BeginFadeGroup(showOnClickNavigation.faded))
            {
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.Space(SPACE_2);
                    DrawNavigationData(uiToggle.onClickNavigationToggleOff, onClickEditorNavigationDataToggleOff);
                    QUI.Space(SPACE_8);
                }
                QUI.EndVertical();
                QUI.ResetColors();
            }
            QUI.EndFadeGroup();
        }

        void DrawPunch(Color color, AnimBool show, SerializedProperty punch, SerializedProperty startDelay, SerializedProperty duration, SerializedProperty vibrato, SerializedProperty elasicity)
        {
            if (QUI.BeginFadeGroup(show.faded))
            {
                SaveColors();
                QUI.SetGUIBackgroundColor(color);
                QUI.BeginVertical(WIDTH_420);
                {
                    QUI.BeginHorizontal(WIDTH_420);
                    {
                        QUI.Label("punch", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 40);
                        QUI.PropertyField(punch, 240);
                        QUI.Space(SPACE_4);
                        QUI.Label("elasticity", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 54);
                        QUI.PropertyField(elasicity, 66);
                    }
                    QUI.EndHorizontal();
                    QUI.BeginHorizontal(WIDTH_420);
                    {
                        QUI.Label("duration", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 50);
                        QUI.PropertyField(duration, 78);
                        QUI.Space(SPACE_4);
                        QUI.Label("start delay", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 62);
                        QUI.PropertyField(startDelay, 78);
                        QUI.Space(SPACE_4);
                        QUI.Label("vibrato", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 42);
                        QUI.PropertyField(vibrato, 78);
                    }
                    QUI.EndHorizontal();
                    QUI.Space(SPACE_8);
                }
                QUI.EndVertical();
                RestoreColors();
            }
            QUI.EndFadeGroup();
        }

        void DrawNavigationData(NavigationPointerData navData, EditorNavigationPointerData editorNavData)
        {
            if (DUI.UIElementsDatabase == null) { DUI.RefreshUIElementsDatabase(); }

            SaveColors();
            QUI.SetGUIBackgroundColor(DUIColors.BlueLight.Color);
            QUI.BeginHorizontal(WIDTH_420);
            {
                QUI.Space(30);
                bool tempBool = navData.addToNavigationHistory;
                QUI.BeginChangeCheck();
                tempBool = QUI.Toggle(tempBool);
                if (QUI.EndChangeCheck())
                {
                    Undo.RecordObject(uiToggle, "Updated NavigationPointer AddToNavigationHistory");
                    navData.addToNavigationHistory = tempBool;
                }
                QUI.Label("Add To Navigation History", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal), 370);
            }
            QUI.EndHorizontal();
            RestoreColors();
            QUI.Space(SPACE_2);
            DrawNavigationDataList(DUIResources.barShow.texture, navData.show, editorNavData.showIndex, DUIColors.GreenLight.Color);
            QUI.Space(SPACE_4);
            DrawNavigationDataList(DUIResources.barHide.texture, navData.hide, editorNavData.hideIndex, DUIColors.RedLight.Color);
        }
        void DrawNavigationDataList(Texture header, List<NavigationPointer> list, List<EditorNavigationPointer> listIndex, Color color)
        {
            if (listIndex.Count != list.Count) { RefreshNavigationData(); }
            QUI.DrawTexture(header, 420, 18);
            SaveColors();
            QUI.SetGUIBackgroundColor(color);

            QUI.BeginHorizontal(WIDTH_420);
            {
                QUI.Space(30);
                QUI.BeginVertical(WIDTH_420 - 30);
                {
                    QUI.Space(-SPACE_2);
                    if (list.Count > 0)
                    {
                        QUI.BeginHorizontal(WIDTH_420 - 32);
                        {
                            QUI.Label("Element Category", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelSmall), 179);
                            QUI.Label("Element Name", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelSmall), 179);
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();
                        QUI.Space(-SPACE_4);
                    }
                    string customName = "";
                    for (int i = 0; i < list.Count; i++)
                    {
                        QUI.BeginHorizontal(WIDTH_420 - 32);
                        {
                            QUI.BeginChangeCheck();
                            listIndex[i].categoryIndex = EditorGUILayout.Popup(listIndex[i].categoryIndex, DUI.UIElementCategories.ToArray(), GUILayout.Width(179));
                            if (QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(uiToggle, "Updated NavigationPointer Category");
                                list[i].category = DUI.UIElementCategories[listIndex[i].categoryIndex];

                                if (list[i].category.Equals(DUI.CUSTOM_NAME))
                                {
                                    listIndex[i].nameIndex = 0;
                                    list[i].name = "";
                                }
                                else if (DUI.UIElementNameExists(list[i].category, list[i].name))
                                {
                                    listIndex[i].nameIndex = DUI.UIElementsDatabase[list[i].category].data.IndexOf(list[i].name);
                                }
                                else
                                {
                                    if (!list[i].name.Equals(DUI.DEFAULT_ELEMENT_NAME) && !string.IsNullOrEmpty(list[i].name.Trim()) && EditorUtility.DisplayDialog("Action Required", "The '" + list[i].name + "' element name does not exist in the '" + DUI.UIElementCategories[listIndex[i].categoryIndex] + "' category database.\nDo you want to add it now?", "Yes", "No"))
                                    {
                                        DUI.AddUIElementName(DUI.UIElementCategories[listIndex[i].categoryIndex], list[i].name);
                                        RefreshNavigationDataList(list, listIndex);
                                        listIndex[i].nameIndex = DUI.GetUIElementNames(list[i].category).IndexOf(list[i].name);
                                    }
                                    else
                                    {
                                        listIndex[i].nameIndex = 0;
                                        list[i].name = DUI.UIElementsDatabase[list[i].category].data[0];
                                    }
                                }
                            }

                            if (!list[i].category.Equals(DUI.CUSTOM_NAME))
                            {
                                QUI.BeginChangeCheck();
                                listIndex[i].nameIndex = EditorGUILayout.Popup(listIndex[i].nameIndex, DUI.UIElementsDatabase[list[i].category].ToArray(), GUILayout.Width(179));
                                if (QUI.EndChangeCheck())
                                {
                                    Undo.RecordObject(uiToggle, "Updated NavigationPointer Name");
                                    list[i].name = DUI.UIElementsDatabase[list[i].category].data[listIndex[i].nameIndex];
                                }
                            }
                            else
                            {
                                customName = list[i].name;
                                QUI.BeginChangeCheck();
                                customName = EditorGUILayout.TextField(customName, GUILayout.Width(179));
                                if (QUI.EndChangeCheck())
                                {
                                    Undo.RecordObject(uiToggle, "Updated NavigationPointer Name");
                                    list[i].name = customName;
                                }
                            }
                            QUI.BeginVertical(18);
                            {
                                QUI.Space(-1);
                                if (QUI.ButtonMinus())
                                {
                                    Undo.RecordObject(uiToggle, "Removed NavigationPointer");
                                    list.RemoveAt(i);
                                    listIndex.RemoveAt(i);
                                    RefreshNavigationDataList(list, listIndex);
                                    QUI.ExitGUI();
                                }
                            }
                            QUI.EndVertical();
                        }
                        QUI.EndHorizontal();
                    }
                    if (list.Count > 0)
                    {
                        QUI.Space(-SPACE_4);
                    }

                    QUI.BeginHorizontal(WIDTH_420 - 32);
                    {
                        if (list.Count == 0)
                        {
                            QUI.Label("List is empty... Click [+] to start...", DUIStyles.GetStyle(DUIStyles.TextStyle.LabelNormal));
                        }
                        QUI.FlexibleSpace();
                        if (QUI.ButtonPlus())
                        {
                            Undo.RecordObject(uiToggle, "Added NavigationPointer");
                            list.Add(new NavigationPointer(DUI.DEFAULT_CATEGORY_NAME, DUI.DEFAULT_ELEMENT_NAME));
                            listIndex.Add(new EditorNavigationPointer(DUI.UIElementCategories.IndexOf(DUI.DEFAULT_CATEGORY_NAME), DUI.UIElementsDatabase[DUI.DEFAULT_CATEGORY_NAME].data.IndexOf(DUI.DEFAULT_ELEMENT_NAME)));
                            RefreshNavigationDataList(list, listIndex);
                        }
                    }
                    QUI.EndHorizontal();
                }
                QUI.EndVertical();
            }
            QUI.EndHorizontal();
            if (list.Count > 0) { QUI.Space(SPACE_8); }
            QUI.ResetColors();
        }

        void RefreshUISoundsDatabase(bool forcedRefresh = false)
        {
            if (DUI.UISoundsDatabase == null || forcedRefresh)
            {
                DUI.RefreshUISoundsDatabase();
            }
        }

        void ValidateUISounds()
        {
            if (!customOnPointerEnterSoundToggleOn.boolValue)
            {
                if (string.IsNullOrEmpty(onPointerEnterSoundToggleOn.stringValue) ||
                   onPointerEnterSoundToggleOn.stringValue.Equals(DUI.DEFAULT_SOUND_NAME) ||
                   !DUI.UISoundNameExists(onPointerEnterSoundToggleOn.stringValue, SoundType.UIButtons))
                {
                    onPointerEnterSoundToggleOn.stringValue = DUI.DEFAULT_SOUND_NAME;
                }
                onPointerEnterSoundIndexToggleOn = DUI.UISoundNamesUIButtons.IndexOf(onPointerEnterSoundToggleOn.stringValue);
            }
            if (!customOnPointerEnterSoundToggleOff.boolValue)
            {
                if (string.IsNullOrEmpty(onPointerEnterSoundToggleOff.stringValue) ||
                   onPointerEnterSoundToggleOff.stringValue.Equals(DUI.DEFAULT_SOUND_NAME) ||
                   !DUI.UISoundNameExists(onPointerEnterSoundToggleOff.stringValue, SoundType.UIButtons))
                {
                    onPointerEnterSoundToggleOff.stringValue = DUI.DEFAULT_SOUND_NAME;
                }
                onPointerEnterSoundIndexToggleOff = DUI.UISoundNamesUIButtons.IndexOf(onPointerEnterSoundToggleOff.stringValue);
            }


            if (!customOnPointerExitSoundToggleOn.boolValue)
            {
                if (string.IsNullOrEmpty(onPointerExitSoundToggleOn.stringValue) ||
                   onPointerExitSoundToggleOn.stringValue.Equals(DUI.DEFAULT_SOUND_NAME) ||
                   !DUI.UISoundNameExists(onPointerExitSoundToggleOn.stringValue, SoundType.UIButtons))
                {
                    onPointerExitSoundToggleOn.stringValue = DUI.DEFAULT_SOUND_NAME;
                }
                onPointerExitSoundIndexToggleOn = DUI.UISoundNamesUIButtons.IndexOf(onPointerExitSoundToggleOn.stringValue);
            }
            if (!customOnPointerExitSoundToggleOff.boolValue)
            {
                if (string.IsNullOrEmpty(onPointerExitSoundToggleOff.stringValue) ||
                   onPointerExitSoundToggleOff.stringValue.Equals(DUI.DEFAULT_SOUND_NAME) ||
                   !DUI.UISoundNameExists(onPointerExitSoundToggleOff.stringValue, SoundType.UIButtons))
                {
                    onPointerExitSoundToggleOff.stringValue = DUI.DEFAULT_SOUND_NAME;
                }
                onPointerExitSoundIndexToggleOff = DUI.UISoundNamesUIButtons.IndexOf(onPointerExitSoundToggleOff.stringValue);
            }

            if (!customOnClickSoundToggleOn.boolValue)
            {
                if (string.IsNullOrEmpty(onClickSoundToggleOn.stringValue) ||
                   onClickSoundToggleOn.stringValue.Equals(DUI.DEFAULT_SOUND_NAME) ||
                   !DUI.UISoundNameExists(onClickSoundToggleOn.stringValue, SoundType.UIButtons))
                {
                    onClickSoundToggleOn.stringValue = DUI.DEFAULT_SOUND_NAME;
                }
                onClickSoundIndexToggleOn = DUI.UISoundNamesUIButtons.IndexOf(onClickSoundToggleOn.stringValue);
            }
            if (!customOnClickSoundToggleOff.boolValue)
            {
                if (string.IsNullOrEmpty(onClickSoundToggleOff.stringValue) ||
                   onClickSoundToggleOff.stringValue.Equals(DUI.DEFAULT_SOUND_NAME) ||
                   !DUI.UISoundNameExists(onClickSoundToggleOff.stringValue, SoundType.UIButtons))
                {
                    onClickSoundToggleOff.stringValue = DUI.DEFAULT_SOUND_NAME;
                }
                onClickSoundIndexToggleOff = DUI.UISoundNamesUIButtons.IndexOf(onClickSoundToggleOff.stringValue);
            }
        }

        void RefreshPunchAnimationsPresets(bool forcedRefresh = false)
        {
            if (UIAnimatorUtil.PunchDataPresetsDatabase == null || forcedRefresh)
            {
                UIAnimatorUtil.RefreshPunchDataPresetsDatabase();
            }
        }

        void ValidatePunchAnimationsPresets()
        {
            //preset category is empty or preset category does not exist -> reset to default
            if (string.IsNullOrEmpty(onPointerEnterPunchPresetCategory.stringValue) ||
                !UIAnimatorUtil.PunchPresetCategoryExists(onPointerEnterPunchPresetCategory.stringValue))
            {
                onPointerEnterPunchPresetCategory.stringValue = UIAnimatorUtil.DEFAULT_PRESET_CATEGORY;
            }
            if (string.IsNullOrEmpty(onPointerExitPunchPresetCategory.stringValue) ||
                !UIAnimatorUtil.PunchPresetCategoryExists(onPointerExitPunchPresetCategory.stringValue))
            {
                onPointerExitPunchPresetCategory.stringValue = UIAnimatorUtil.DEFAULT_PRESET_CATEGORY;
            }
            if (string.IsNullOrEmpty(onClickPunchPresetCategory.stringValue) ||
                !UIAnimatorUtil.PunchPresetCategoryExists(onClickPunchPresetCategory.stringValue))
            {
                onClickPunchPresetCategory.stringValue = UIAnimatorUtil.DEFAULT_PRESET_CATEGORY;
            }

            //preset name is empty or preset name does not exist in set category -> reset to default preset category and preset name
            if (string.IsNullOrEmpty(onPointerEnterPunchPresetName.stringValue) ||
                !UIAnimatorUtil.PunchPresetExists(onPointerEnterPunchPresetCategory.stringValue, onPointerEnterPunchPresetName.stringValue))
            {
                onPointerEnterPunchPresetCategory.stringValue = UIAnimatorUtil.DEFAULT_PRESET_CATEGORY;
                onPointerEnterPunchPresetName.stringValue = UIAnimatorUtil.DEFAULT_PRESET_NAME;
            }
            if (string.IsNullOrEmpty(onPointerExitPunchPresetName.stringValue) ||
              !UIAnimatorUtil.PunchPresetExists(onPointerExitPunchPresetCategory.stringValue, onPointerExitPunchPresetName.stringValue))
            {
                onPointerExitPunchPresetCategory.stringValue = UIAnimatorUtil.DEFAULT_PRESET_CATEGORY;
                onPointerExitPunchPresetName.stringValue = UIAnimatorUtil.DEFAULT_PRESET_NAME;
            }
            if (string.IsNullOrEmpty(onClickPunchPresetName.stringValue) ||
              !UIAnimatorUtil.PunchPresetExists(onClickPunchPresetCategory.stringValue, onClickPunchPresetName.stringValue))
            {
                onClickPunchPresetCategory.stringValue = UIAnimatorUtil.DEFAULT_PRESET_CATEGORY;
                onClickPunchPresetName.stringValue = UIAnimatorUtil.DEFAULT_PRESET_NAME;
            }

            onPointerEnterPunchPresetCategoryNameIndex = UIAnimatorUtil.PunchPresetCategories.IndexOf(onPointerEnterPunchPresetCategory.stringValue);
            onPointerEnterPunchPresetNameIndex = UIAnimatorUtil.GetPunchPresetNames(onPointerEnterPunchPresetCategory.stringValue).IndexOf(onPointerEnterPunchPresetName.stringValue);
            onPointerExitPunchPresetCategoryNameIndex = UIAnimatorUtil.PunchPresetCategories.IndexOf(onPointerExitPunchPresetCategory.stringValue);
            onPointerExitPunchPresetNameIndex = UIAnimatorUtil.GetPunchPresetNames(onPointerExitPunchPresetCategory.stringValue).IndexOf(onPointerExitPunchPresetName.stringValue);
            onClickPunchPresetCategoryNameIndex = UIAnimatorUtil.PunchPresetCategories.IndexOf(onClickPunchPresetCategory.stringValue);
            onClickPunchPresetNameIndex = UIAnimatorUtil.GetPunchPresetNames(onClickPunchPresetCategory.stringValue).IndexOf(onClickPunchPresetName.stringValue);
        }

        void RefreshNavigationData(bool forcedRefresh = false)
        {
            if (!UIManager.IsNavigationEnabled) { return; }
            if (DUI.UIElementsDatabase == null || forcedRefresh) { DUI.RefreshUIElementsDatabase(); }
            if (useOnPointerEnter.boolValue || forcedRefresh)
            {
                RefreshNavigationDataList(uiToggle.onPointerEnterNavigationToggleOn.show, onPointerEnterEditorNavigationDataToggleOn.showIndex);
                RefreshNavigationDataList(uiToggle.onPointerEnterNavigationToggleOn.hide, onPointerEnterEditorNavigationDataToggleOn.hideIndex);

                RefreshNavigationDataList(uiToggle.onPointerEnterNavigationToggleOff.show, onPointerEnterEditorNavigationDataToggleOff.showIndex);
                RefreshNavigationDataList(uiToggle.onPointerEnterNavigationToggleOff.hide, onPointerEnterEditorNavigationDataToggleOff.hideIndex);
            }
            if (useOnPointerExit.boolValue || forcedRefresh)
            {
                RefreshNavigationDataList(uiToggle.onPointerExitNavigationToggleOn.show, onPointerExitEditorNavigationDataToggleOn.showIndex);
                RefreshNavigationDataList(uiToggle.onPointerExitNavigationToggleOn.hide, onPointerExitEditorNavigationDataToggleOn.hideIndex);

                RefreshNavigationDataList(uiToggle.onPointerExitNavigationToggleOff.show, onPointerExitEditorNavigationDataToggleOff.showIndex);
                RefreshNavigationDataList(uiToggle.onPointerExitNavigationToggleOff.hide, onPointerExitEditorNavigationDataToggleOff.hideIndex);
            }
            if (useOnClick.boolValue || forcedRefresh)
            {
                RefreshNavigationDataList(uiToggle.onClickNavigationToggleOn.show, onClickEditorNavigationDataToggleOn.showIndex);
                RefreshNavigationDataList(uiToggle.onClickNavigationToggleOn.hide, onClickEditorNavigationDataToggleOn.hideIndex);

                RefreshNavigationDataList(uiToggle.onClickNavigationToggleOff.show, onClickEditorNavigationDataToggleOff.showIndex);
                RefreshNavigationDataList(uiToggle.onClickNavigationToggleOff.hide, onClickEditorNavigationDataToggleOff.hideIndex);
            }
        }
        void RefreshNavigationDataList(List<NavigationPointer> list, List<EditorNavigationPointer> listIndex)
        {
            listIndex.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                listIndex.Add(new EditorNavigationPointer(0, 0));
                if (list[i].category.Equals(DUI.CUSTOM_NAME))
                {
                    listIndex[i].categoryIndex = DUI.UIElementCategories.IndexOf(list[i].category);
                    listIndex[i].nameIndex = 0;
                    continue;
                }
                else if (DUI.UIElementCategoryExists(list[i].category))
                {
                    listIndex[i].categoryIndex = DUI.UIElementCategories.IndexOf(list[i].category);
                    if (DUI.UIElementNameExists(list[i].category, list[i].name) && !list[i].category.Equals(DUI.CUSTOM_NAME))
                    {
                        listIndex[i].nameIndex = DUI.UIElementsDatabase[list[i].category].IndexOf(list[i].name);
                        continue;
                    }
                    else if (list[i].category.Equals(DUI.CUSTOM_NAME))
                    {
                        listIndex[i].nameIndex = 0;
                        continue;
                    }
                }

                if (!DUI.UIElementsDatabase.ContainsKey(DUI.DEFAULT_CATEGORY_NAME) ||
                   !DUI.UIElementNameExists(DUI.DEFAULT_CATEGORY_NAME, DUI.DEFAULT_ELEMENT_NAME))
                {
                    DUI.RefreshUIElementsDatabase();
                }
                list[i].category = DUI.DEFAULT_CATEGORY_NAME;
                listIndex[i].categoryIndex = DUI.UIElementCategories.IndexOf(DUI.DEFAULT_CATEGORY_NAME);
                list[i].name = DUI.DEFAULT_ELEMENT_NAME;
                listIndex[i].nameIndex = DUI.UIElementsDatabase[list[i].category].IndexOf(DUI.DEFAULT_ELEMENT_NAME);
            }
        }
    }
}
