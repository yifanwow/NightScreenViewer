# NightScreenViewer

![website image](./img/nsv_logo.png) 
---
**NightScreenViewer** is a utility designed to enhance the multi-monitor experience by automatically dimming secondary screens when a specific application is running in full-screen mode on the primary monitor. This tool aims to minimize distractions and reduce eye strain during tasks such as gaming, movie watching, or focused work sessions.
     
Users can customize the opacity of the darkening overlay on the secondary screens to suit their preferences. Built for the Windows platform using JavaScript and C#, NightScreenViewer leverages Windows APIs to detect full-screen applications and manage screen overlays efficiently. This project is ideal for users who utilize multiple monitors and seek a seamless and distraction-free viewing experience.    
#### **[中文说明](README_zh.md)** 
## Features

- **Automatic Detection**: Automatically detects when a specified application goes into full-screen mode on the primary monitor.
- **Adjustable Opacity**: Users can adjust the opacity of the dark overlay applied to secondary screens to ensure comfort and visibility according to their preferences.
- **Best user experience**: Always trying to do better.

![website image](./img/nsv.png) 

## Development Plan

#### *Version - 0.1.7.25.24*
- FrontEnd UIUX √  
- Frame √

#### *Version - 0.1.7.26.24*
- Implemented basic functionality, allowing users to manually enable and disable the overlay mode √
- Animation effects for enabling, disabling, and adjusting the overlay √
- Backend code organized √
- Frontend debounce processing to optimize the frequency of requests sent to the backend when changing opacity √
- When users focus on other windows, the focused window is brought to the front even if the overlay is enabled, improving user experience √
- The backend can detect if the currently focused window is on the primary screen and update whether the overlay window is at the forefront √

#### *Version - 0.1.7.27.24*
- Implemented automatic fullscreen detection √
- Automatically enable blackout mode when in fullscreen √
- Exit blackout mode when exiting fullscreen or when the focused software is not in fullscreen √
- Manually turning off or on blackout mode will disable automatic mode to prevent logical conflicts √
- UI optimization: button update, different color distinction for slider √

#### *Version - 0.1.7.31.24*
- Added frontend components for mirror mode √
- UI Optimization: Redesigned buttons, frontend update √

### Over All Development

1. **Phase 1: Setup and Initial Testing**
   - Set up the project repository and basic project structure. √ 7.25
   - Implement core functionality to detect full-screen applications.  √ 7.27
   - Create a basic overlay window that can cover secondary screens. √ 7.26
   - Implement system tray integration for easy access and adjustments. √ 7.25

2. **Phase 2: Feature Enhancement**
   - Develop the feature to adjust the opacity of the overlay. √ 7.26
   - ~~Implement the functionality to manage a list of applications that will trigger the overlay.~~(Development is on hold, it's not a very high priority, it feels less practical in real life experience, and it can be completely replaced by auto mode.) 7.29
   - ~~Hot-key function.~~(Development is on hold, it's not a very high priority, it feels less practical in real life experience, and it can be completely replaced by auto mode.) 7.29
   - Mirror Mode.

3. **Phase 3: GUI Development**
   - Design and implement a graphical user interface that allows users to easily configure the application settings.
   
4. **Phase 4: Testing and Documentation**
   - Prepare user documentation and setup guides.


## Installation

Instructions on how to install and configure NightScreenViewer will be provided with the first stable release.

## Planned Features

- Configurable list of applications that trigger the dimming.
- Option to set different opacities for other screens.
- User-friendly GUI for easy configuration of settings.
- Hot-Key to turn on and turn off the feature.
- Animation during the status change.

## Contact

For support, feature requests, or any queries, please open an issue in the GitHub repository issue tracker.

