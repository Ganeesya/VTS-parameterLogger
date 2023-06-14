日本語のreadmeは[こちら](ReadMeJp.md)

# Introduction
VTS-parameterLogger is a tool that queries parameters from OBS's Lua script to VTubeStudio, recording model movements in a binary format motion file. The binary format motion file is designed for use with the [Aviutl's Live2D plugin](https://ganeesya.github.io/SupportPage/dl/Live2DDrawer/intro.html).

# Strengths
Traditional OBS recording compiles the VTubeStudio image and background image together. In the compiled video, the model's movements cannot be altered from those recorded. However, by separating the background and VTubeStudio information and allowing for post-production of the VTubeStudio image, movements can be added and expressions can be changed during editing. The background video can be stored as an input image using SourceRecord. This software records the movements of VTubeStudio as parameters, making them reusable later.

# Installation

## Installation Steps

1. Select the downloaded Zip file and right-click.
1. Select "Extract All" or "Unzip" from the menu.
1. Find the "obs-studio" folder among the expanded files.
1. Select the "obs-studio" folder and press Ctrl+C to copy.
1. Navigate to the installed "obs-studio" folder. Usually, it is located under "C:\Program Files".
1. Press Ctrl+V to paste the "obs-studio" folder within the installation folder.
1. If a prompt appears to overlay or overwrite the existing "obs-studio" folder, approve it.

## OBS Settings

1. Open the OBS program.
1. Click on "Tools" from the menu bar.
1. Select "Scripts" from the drop-down menu.
1. The script window will open. Click on the "+" button here.
1. The file selection dialog will open. Here, navigate to "C:\Program Files\obs-studio\data\obs-plugins\frontend-tools\scripts\saveParam.lua", select the file, and click "Open". This action loads the script into OBS.
1. An item named "File Path" is displayed at the bottom of the loaded script.
1. Enter the output path in the "File Path" textbox. This path will be used as the output destination for the motion file.

# Usage

1. Start recording or streaming in OBS:
    * Open the OBS application and click on the "Start Recording" or "Start Streaming" button from the main screen.
1. Permission for the plugin in VTubeStudio:
    * The first time, a dialog requesting the use of the "parameterLogger" VTubeStudio plugin will be displayed.
    * Please approve this request.

# Usage of MTB Files

* Using in [Aviutl's Live2D plugin](https://ganeesya.github.io/SupportPage/dl/Live2DDrawer/intro.html)
    * You can use the motion in the video by using the MTB motion of the Aviutl plugin.
* Conversion to motion3.json
    * We plan to release software that converts MTB files to motion3.json, which can be used in the official Live2D.

# MTB File Specifications

[Please refer to this table.](mtb_specs.md)

# Acknowledgements

Access to VTubeStudio uses Tom Farro's [VTS-Sharp](https://github.com/FomTarro/VTS-Sharp). Thank you for providing an excellent library.