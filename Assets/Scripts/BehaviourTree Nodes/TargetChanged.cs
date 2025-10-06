using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/TargetChanged")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "TargetChanged", message: "Agent spotter [Target]", category: "Events", id: "00afdf4fad27a1d58e9bc3e86c7bc322")]
public sealed partial class TargetChanged : EventChannel<GameObject> { }

