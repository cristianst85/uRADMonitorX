# Changelog

## 1.4.0 (2023-09-02)

- Added a new dialog 'uRADMonitor Network' that can be used to list all devices from the uRADMonitor network.
- Application settings from the `config.xml` file are stored (and migrated automatically) into a JSON file named `uRADMonitorX.json`.
- Fixed an issue that was causing the application to be restored outside the visible area of the screen.
- Fixed an issue that was causing the taskbar application icon to be blurred.
- Measurements retrieved from uRADMonitor devices are using internally the `decimal` type representation instead of `double`.
- Code refactoring and optimization.
- Other minor improvements.

## 1.3.3 (2020-08-01)

- Improved Check for Updates functionality.
- Fixed blurred UI issue on high-DPI displays.
- Fixed a bug that was causing a high rate data polling when data fetch was unsuccessful immediately after the application was started.
- Added Git commit hash and branch name to the assembly info / About dialog.
- Code refactoring and optimization.

## 1.3.2 (2020-04-30)

- Updated View device online data sub-menu entries.
- Improved Check for Updates functionality.

## 1.3.1 (2019-03-04)

- Added more conversion units (hPa, mbar) for pressure.
- Fixed compatibility issue with firmware version 138 for model A.

## 1.2.0 (2018-10-21)

- Fixed an issue with 'Check for updates' due to GitHub API no longer supporting requests made with TLSv1 / TLSv1.1.
- Show timestamp of the last successful data fetch in the tooltip text of the system tray icon when an error occurs.

## 1.1.2 (2016-09-22)

- Added a fix for A3 model with firmware version 119.
- Fixed minor bug under Linux.
- Minor UI changes.

## 1.1.1 (2016-09-21)

- Added partial support for A3 model.
- Fixed minor bug.

## 1.1.0 (2016-05-13)

- Added data logging.
- Minor improvements.

## 1.0.2 (2016-02-27)

- Minor improvements.

## 1.0.0 (2015-12-26)

- Added 'Check for updates' feature.

## 0.40.0 (2015-12-06)

- Improved compatibility with Mono for running under Linux.

## 0.39.4 (2015-10-31)

- Fixed a bug that caused application to still be visible in ALT+TAB dialog when 'Start uRADMonitorX minimized' was enabled.

## 0.39.3 (2015-10-14)

- Added date and time to notification message.

## 0.39.2 (2015-09-20)

- Expanded 'View device online data' menu item to work with the new www.uradmonitor.com API.

## 0.39.1 (2015-09-10)

- Fixed a bug in CreateFile() method in XMLSettings.cs.

## 0.39.0 (2015-08-10)

- Changed license to GNU GPLv2 (added README.md, LICENSE, COPYING, CHANGELOG.md files to solution directory).
- Added system tray notifications when temperature/radiation readings are reaching the configured threshold.
- Added tooltips (balloons) for textbox inputs in 'Device configuration' dialog when invalid values are entered.
- System tray context menu is shown on the right side of the mouse cursor.
- Fixed a bug when validating device IP Address in 'Device configuration' dialog.
- Other minor improvements.

## 0.37.0 (2015-07-30)
 
- Added keyboard shortcut (F9) to minimize application.
- Added ToolTip text for system tray (notification area) icon.
- Added a new system tray icon that is shown when an error occurs.
- Added support for specifying port number along with IP address (e.g.: 192.168.0.1:8080).
- Log events when ServerResponseCode is not 200 (OK).
- Improved compatibility with other uRADMOnitor device models.
- Added 'View device online data' item in Tools menu.
- Added command line parameter for advanced usage (not documented).
- Code refactoring and optimization.

## 0.35.0 (2015-07-30)

- Fixed a bug when 'Start uRADMonitorX with Windows' option was unchecked, startup registry key was not deleted from Windows registry.
- Fixed radiation unit cpm (counts per minute) to be showed in lower case instead of upper case.
- Fixed 'Pressure' label is disabled (grayed out) when device has no pressure sensor.

## 0.33.0 (2015-07-30)

- Fixed a bug in logging function.
- Improved UI/UX.

## 0.28.0 (2015-07-30)

- First preview version. No release history before this.