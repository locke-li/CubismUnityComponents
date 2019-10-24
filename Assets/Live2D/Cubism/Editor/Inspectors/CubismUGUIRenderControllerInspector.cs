﻿/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using Live2D.Cubism.Rendering;
using System;
using UnityEditor;
using UnityEngine;


namespace Live2D.Cubism.Editor.Inspectors
{
    /// <summary>
    /// Inspector for <see cref="CubismRenderController"/>s.
    /// </summary>
    [CustomEditor(typeof(CubismUGUIRenderController))]
    internal sealed class CubismUGUIRenderControllerInspector : UnityEditor.Editor
    {
        private bool ShowSorting { get; set; }

        private bool ShowAdvanced { get; set; }

        #region Editor

        /// <summary>
        /// Draws the inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var controller = target as CubismUGUIRenderController;


            // Fail silently.
            if (controller == null)
            {
                return;
            }


            // Show settings.
            EditorGUI.BeginChangeCheck();


            controller.Opacity = EditorGUILayout.Slider("Opacity", controller.Opacity, 0f, 1f);


            ShowSorting = EditorGUILayout.Foldout(ShowSorting, "Sorting", EditorStyles.boldFont);

            if (ShowSorting)
            {
                controller.SortingLayer = EditorGUILayout.TextField("Layer", controller.SortingLayer);
                controller.SortingOrder = EditorGUILayout.IntField("Order In Layer", controller.SortingOrder);
                controller.SortingMode = (CubismSortingMode)EditorGUILayout.EnumPopup("Mode", (Enum)controller.SortingMode);
            }


            ShowAdvanced = EditorGUILayout.Foldout(ShowAdvanced, "Advanced", EditorStyles.boldFont);

            if (ShowAdvanced)
            {
                controller.CameraToFace = EditorGUILayout.ObjectField("Camera To Face", controller.CameraToFace, typeof(Camera), true) as Camera;
                controller.OpacityHandler = EditorGUILayout.ObjectField("Opacity Handler", controller.OpacityHandler, typeof(object), true);
                controller.DrawOrderHandler = EditorGUILayout.ObjectField("Draw Order Handler", controller.DrawOrderHandler, typeof(object), true);

                if (controller.SortingMode.SortByDepth())
                {
                    controller.DepthOffset = EditorGUILayout.FloatField("Depth Offset", controller.DepthOffset);
                }
            }


            // Save any changes.
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(controller);


                foreach (var renderer in controller.Renderers)
                {
                    EditorUtility.SetDirty(renderer);
                    // HACK Get mesh renderer directly.
                    EditorUtility.SetDirty(renderer.GetComponent<MeshRenderer>());
                }
            }
        }

        #endregion
    }
}
