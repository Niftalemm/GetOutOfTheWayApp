# Map Accessibility Project Unity Game/App

This project is a pilot test for Map Accessibility for legally blind individuals. The goal of this project is to show that navigation and exploration of a virtual environment is possible through the use of directional audio. This project has been built using a combination of Unity (for the building of the virtual environments) and WWise (for the processing of the directional audio)

## Branching Strategy

* `release`: Main branch for "finished" project
* `dev`: Main branch from which development branches will branch
* `feature/<name>`: Branches for implementing/experimenting with features

## Developer Installation Instructions (Overview)
This project uses Unity 2021.3.17f1 and WWise 2022.1.1.8100.

1. Install [Unity 2021.3.17f1](https://unity.com/releases/editor/whats-new/2021.3.17) (this can be done using the link provided or using Unity Hub if it is already installed)
2. Install the [AudioKinetic (AK) Launcher](https://www.audiokinetic.com/en/download)
3. Install WWise 2022.1.1.8100 from the AK Launcher
4. Clone the [Github Repository](https://github.com/fmillion-mnsu/MapAccessibilityProjectApp)
5. Open the project using Unity Hub

## Developer Installation Instructions (Detailed)
#### Installing Unity Hub
If you already have the Unity Hub application installed, skip to ["Installing Unity"](#installing-unity)
1. Go to [this link](https://unity.com/download) and click "Download"
2. On that page, locate the "Download for Windows" button. Once the file is done downloading, open it.
3. Follow the installation instructions on the window which opens.
4. Once the installation completes, open Unity Hub. Sign in with your Unity account.
5. Unity Hub will prompt you to install the Unity Editor. Select "Skip install".
    
#### Installing Unity
Make sure that some version of Visual Studio or VS Code or some other code editor of choice for C#.
1. Once the Unity Hub is installed, go to [this link](https://unity.com/releases/editor/whats-new/2021.3.17) and click the "Install this version with Unity Hub" hyperlink.
2. On the "Add Modules" screen, no boxes are required to be checked, so you may click "continue."
3. Agree to the terms and conditions, and then wait for the unity version to install.
    (While the Unity Editor installs, feel free to begin [Installing the AudioKinetic Launcher](#installing-the-audiokinetic-ak-launcher))
#### Installing the AudioKinetic (AK) Launcher
If you already have the AK Launcher installed, skip to ["Installing WWise"](#installing-wwise)
1. Go to [this link](https://www.audiokinetic.com/en/download)
2. Sign into AudioKinetic on their website if you haven't already.
3. The site should download the WWise Launcher Installer, once it is done installing, open it.
4. Follow the installer instructions when prompted.
5. Once the WWise Launcher is open, sign in.
6. Begin installing WWise

#### Installing WWise
1. Go to the ""WWise" tab in the AudioKinetic Launcher.
2. Click the "Install..." button under "INSTALL NEW VERSION"
3. Under "Packages," make sure check "Authoring," "SDK C++," and "Offline Documentation"
4. Under "Development Platforms" make sure to check "Microsoft"
5. On the "Plug-ins" screen, make sure only the "Mastering Suite" is checked.
6. Wait for the installation to complete

#### Clone the GitHub repository
Make sure Git Bash as well as Unity 2021.3.17f1 and WWise 2022.1.1.8100 are installed before this section. If Git Bash is not installed, install it [here](https://gitforwindows.org/)
1. Find a place on your laptop to clone the repository. Copy the file path to that place.
2. Open the Command Prompt and type in `cd [REPOSITORY_FILE_PATH]` where `[REPOSITORY_FILE_PATH]` is the file path copied in the previous step
3. Type `git clone https://github.com/fmillion-mnsu/MapAccessibilityProjectApp` to clone the repository into your files.
4. In Unity Hub, open the project by clicking on the arrow on the "Open" button in the "Projects" tab and clicking "Add project from disk."
5. Navigate to the folder where the repository was cloned and click "Add Project"
    Make sure the folder you are in is the "MapAccessibilityProjectApp" folder
6. Open the project in the list on the Unity Hub Projects page.