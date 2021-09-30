
# extDebug.UGUI - Unity UI for extDebug

Created by [iam1337](https://github.com/iam1337)

![](https://img.shields.io/badge/unity-2021.1%20or%20later-green.svg)
[![⚙ Build and Release](https://github.com/Iam1337/extDebug.UGUI/actions/workflows/ci.yml/badge.svg)](https://github.com/Iam1337/extDebug.UGUI/actions/workflows/ci.yml)
[![openupm](https://img.shields.io/npm/v/com.iam1337.extdebug.ugui?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.iam1337.extdebug.ugui/)
[![](https://img.shields.io/github/license/iam1337/extDebug.UGUI.svg)](https://github.com/Iam1337/extDebug.UGUI/blob/master/LICENSE)

## Introduction

This is an extension to support Unity UI and TextMeshPro in [extDebug](https://github.com/Iam1337/extDebug).

## Installation:

Be sure to install [extDebug](https://github.com/Iam1337/extDebug) before installing.

**Old school**

Just copy the [Assets/extDebug.UGUI](Assets/extDebug.UGUI) folder into your Assets directory within your Unity project, or [download latest extDebug.UGUI.unitypackage](https://github.com/iam1337/extDebug.UGUI/releases).

**OpenUPM**

Via [openupm-cli](https://github.com/openupm/openupm-cli):<br>
```
openupm add com.iam1337.extdebug.ugui
```

Or if you don't have it, add the scoped registry to manifest.json with the desired dependency semantic version:
```
"scopedRegistries": [
	{
		"name": "package.openupm.com",
		"url": "https://package.openupm.com",
		"scopes": [
			"com.iam1337.extdebug",
			"com.iam1337.extdebug.ugui",
		]
	}
],
"dependencies": {
	"com.iam1337.extdebug": "1.5.0"
	"com.iam1337.extdebug.ugui": "1.0.0"
}
```

**Package Manager**

Project supports Unity Package Manager. To install the project as a Git package do the following:

1. In Unity, open **Window > Package Manager**.
2. Press the **+** button, choose **"Add package from git URL..."**
3. Enter "https://github.com/iam1337/extDebug.UGUI.git#upm" and press Add.

## extDebug.Menu.UGUI - Debug Menu UGUI Render
To change the default IMGUI render on Unity UI, you need to set instance of `DMUGUIRender` in `DM.Render`.

### Examples:

Full examples you can find in [Examples/UGUI.Menu](Assets/extDebug.UGUI/Examples/UGUI.Menu) folder.

**Unity UI**<br>
```C#
GameObject MenuObject; // extDebug Unity UI Root instance. The object to hide when the menu is hidden.
Text MenuText;         // Unity UI Text instance. The object in which the menu text will be displayed.

// Initialize Unity UI render
DM.Render = new DMUGUIRender(MenuObject, MenuText);
```

**Unity UI and TextMeshPro**<br>
```C#
GameObject MenuObject; // extDebug Unity UI Root instance. The object to hide when the menu is hidden.
TextMeshProUGUI MenuText; // TextMeshPro UI Text instance. The object in which the menu text will be displayed.

// Initialize Unity UI render
DM.Render = new DMUGUIRender(MenuObject, MenuText);
```

## extDebug.Notifications.UGUI - Debug Notifications UGUI Render
To change the default IMGUI render on Unity UI, you need to set instance of `DMUGUIRender` in `DN.Render`.

### Examples:

Full examples you can find in [Examples/UGUI.Notifications](Assets/extDebug.UGUI/Examples/UGUI.Notifications) folder.

**Unity UI or TextMeshPro**<br>
```C#
RectTransform NotifyAnchor; // Ref to the transform of the component on which the notification will be instantiated.
DNUGUIItem NotifyPrefab; // Notification prefab. Inside this prefab, you can choose between Unity UI and TextMeshPro

// Initialize Unity UI render
DN.Render = new DNUGUIRender(NotifyAnchor, NotifyPrefab);
```

## Author Contacts:
\> [telegram.me/iam1337](http://telegram.me/iam1337) <br>
\> [ext@iron-wall.org](mailto:ext@iron-wall.org)

## License
This project is under the MIT License.
