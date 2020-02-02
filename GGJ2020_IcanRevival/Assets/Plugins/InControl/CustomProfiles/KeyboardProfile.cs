using System;
using System.Collections;
using UnityEngine;
using InControl;

namespace Profiles
{
    public class KeyboardOnly : UnityInputDeviceProfile
    {
        public KeyboardOnly()
        {
            Name = "Keyboard Default";
            Meta = "Default profile for keyboard";

            // This profile only works on desktops.
            SupportedPlatforms = new[]
            {
                "Windows",
                "Mac",
                "Linux"
            };

            Sensitivity = 1.0f;
            LowerDeadZone = 0.0f;
            UpperDeadZone = 1.0f;

            ButtonMappings = new[]
            {
                new InputControlMapping
                {
                    Handle = "Action1 - Mouse", // Grab
                    Target = InputControlType.Action1,
                    Source = MouseButton0
                },
                new InputControlMapping
                {
                    Handle = "Action1 - Keyboard",
                    Target = InputControlType.Action1,
					// KeyCodeButton fires when any of the provided KeyCode params are down.
					Source = KeyCodeButton( KeyCode.Space, KeyCode.Return )
                },
                new InputControlMapping
                {
                    Handle = "Action2", // Drop
                    Target = InputControlType.Action2,
                    Source = KeyCodeButton(KeyCode.Escape, KeyCode.Backspace, KeyCode.F)
                },
                new InputControlMapping
                {
                    Handle = "Action3 - Mouse", // Use
                    Target = InputControlType.Action3,
                    Source = MouseButton1
                },
                new InputControlMapping
                {
                    Handle = "Action3 - Keyboard",
                    Target = InputControlType.Action3,
                    Source = KeyCodeButton (KeyCode.E)
                },
                new InputControlMapping
                {
                    Handle = "Action4 - Mouse", // Throw
                    Target = InputControlType.Action4,
                    Source = MouseButton2
                },
                new InputControlMapping
                {
                    Handle = "Action4 - Keyboard", // Throw
                    Target = InputControlType.Action4,
                    Source = KeyCodeButton( KeyCode.R)
                }
            };

            AnalogMappings = new[]
            {
                new InputControlMapping
                {
                    Handle = "Move X",
                    Target = InputControlType.LeftStickX,
					// KeyCodeAxis splits the two KeyCodes over an axis. The first is negative, the second positive.
					Source = KeyCodeAxis( KeyCode.Q, KeyCode.D )
                },
                new InputControlMapping
                {
                    Handle = "Move X - alt",
                    Target = InputControlType.LeftStickX,
					// KeyCodeAxis splits the two KeyCodes over an axis. The first is negative, the second positive.
					Source = KeyCodeAxis( KeyCode.A, KeyCode.D )
                },
                new InputControlMapping
                {
                    Handle = "Move Y",
                    Target = InputControlType.LeftStickY,
					// Notes that up is positive in Unity, therefore the order of KeyCodes is down, up.
					Source = KeyCodeAxis( KeyCode.S, KeyCode.Z )
                },
                new InputControlMapping
                {
                    Handle = "Move Y - alt",
                    Target = InputControlType.LeftStickY,
					// Notes that up is positive in Unity, therefore the order of KeyCodes is down, up.
					Source = KeyCodeAxis( KeyCode.S, KeyCode.W )
                },
                new InputControlMapping
                {
                    Handle = "Look Z",
                    Target = InputControlType.ScrollWheel,
                    Source = MouseScrollWheel,
                    Raw    = true,
                    Scale  = 0.1f
                },
                new InputControlMapping {
                    Handle = "Look X",
                    Target = InputControlType.RightStickX,
                    Source = KeyCodeAxis( KeyCode.LeftArrow, KeyCode.RightArrow )
                },
                new InputControlMapping {
                    Handle = "Look Y",
                    Target = InputControlType.RightStickY,
                    Source = KeyCodeAxis( KeyCode.DownArrow, KeyCode.UpArrow )
                }
            };
        }
    }
}
