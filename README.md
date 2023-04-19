# HoloLens-Skills-Training

## Virtual Machine setup
 * Make sure the machine installed is Windows 10
 * Make sure the machine is at least 4vCPUs, 16GiB Memory
 * Download and install Visual Studio 2022 with "Game Development with Unity" module
 * NOTE: You may be able to build the Unity project for Hololens on the VM, but you may not be able to run on it

## Dev environment setup

### Setup Unity Hub
 * Configure your Visual Studio environment by installing the following modules while installing Visual Studio
   * "Game Development with Unity"
   * "UWP Development" - make sure all C++ modules and Windows SDK modules (11 and 10) are selected
 * Once installed, launch Unity Hub from a nenly created desktop icon
 * Make sure you have the latest version (v3.4.2)
 * On relaunching, you will get prompted for installing Unity Editor - go ahead and install it (this might take up to 3 hours)

### Create Unity Hub project
 * On launching after the install
 * If Universal Windows Platform is nto installed, you will need to install it
   * Launch Unity Hub, open File -> Build Settings -> UWP. This will prompt you to install UWP. Note, you may have to restart Unity to see corretion options
 * Reference - https://learn.microsoft.com/en-us/training/modules/learn-mrtk-tutorials/1-3-exercise-configure-unity-for-windows-mixed-reality
 
## Install Windows SDK
 * Download and install from https://developer.microsoft.com/en-us/windows/downloads/windows-sdk/
 * Ensure that UWP modules are selected while installing

### Import the MRTK Unity foundation package
 * Reference - https://learn.microsoft.com/en-us/training/modules/learn-mrtk-tutorials/1-5-exercise-configure-resources?tabs=openxr
 * In case the toolkit doesn't load, you can import individual packages as per instructions here - https://dev.azure.com/aipmr/MixedReality-Unity-Packages/_packaging?_a=feed&feed=Unity-packages
 
## References
 * https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/unity-development-overview