# AutoVisualizer
## About
AutoVisualizer is a plugin designed for Grasshopper, providing seamless integration with the Automatic1111 Stable Diffusion API. This powerful combination enables users to generate captivating images directly within the Grasshopper environment, harnessing the capabilities of the Automatic1111 API for stable diffusion processes.

![Static Badge](https://img.shields.io/badge/Build-v0.1.0-green)
![GitHub Release Date - Published_At](https://img.shields.io/github/release-date/dkeners/AutoVisualizer?label=Last%20release%20date%3A%20)
 ![GitHub last commit (branch)](https://img.shields.io/github/last-commit/dkeners/AutoVisualizer/develop?label=Lastest%20Development:)

 ![Static Badge](https://img.shields.io/badge/-4.8-blue?logo=csharp) ![Static Badge](https://img.shields.io/badge/--%23512BD4?logo=dotnet)
 ![Static Badge](https://img.shields.io/badge/-Rhino%207-black?logo=rhinoceros)


## Main Components
### 1. SD_GenerateASync
Can generate an image asynchronously given a prompt and negative prompt, with extra options if needed. Generate button to start another iteration.

### 2. SD_GenerateSettings*
A group of three components offering varying levels of control over final generation settings.

## Installation
### Dependencies
1. Install [Automatic1111](https://github.com/AUTOMATIC1111/stable-diffusion-webui#) locally.
2. Run the new install of Automatic1111 in API mode with one of the methods below:
    - Run Automatic1111 from command line with the argument `--api`
    - Create a new script `webui-api.bat`
      ```webui-api.bat
      @echo off

      set PYTHON=
      set GIT=
      set VENV_DIR=
      set COMMANDLINE_ARGS= --medvram --api

      git pull

      call webui.bat```
### AutoVisualizer
AutoVisualizer can be installed by going to [Releases](https://github.com/dkeners/AutoVisualizer/releases) and downloading the version you would like.
1. In Grasshopper go to `File->Special Folders->Components Folder`
2. Unzip the `AutoVisualizer-X-X-X.zip` assembly to the Grasshopper Plugin folder

## Usage
Usage will be updated when more developement has passed. For now check out the `Component\ComponentTests.gh` file for some examples of each component.

## Configuration
The one important configuration is the IP that the API is located at. Please take note of this and modify  the code, or set your IP in Automatic1111 to be `127.0.0.1:7860`.

## Troubleshooting
- If no images are generated check the following:
  - Make sure that Automatic1111 is running with the `--api` argument.
  - Check the IP that Automatic1111 is running on, default is `http://127.0.0.1:7860`, if it is different update it using the Address component.
- Currently both prompt and negative prompt need inputs to run.

## License
See [LICENSE](LICENSE.txt) for details.

## Acknowledgments
Big shoutout to [@ParametricCamp](https://github.com/ParametricCamp) for their Youtube tutorial series on creating grasshopper components and asynchronous requests.
Ladybug devs for their [imageviewer component](https://docs.ladybug.tools/ladybug-primer/components/4_extra/imageviewer), which provided inspiration for the one included in this plugin, as well as the [BitmapPlus](https://github.com/interopxyz/BitmapPlus) plugin by [@interopxyz](https://github.com/interopxyz).

## Feedback & Contributing
Feel free to provide feedback, suggestions, or report issues. Please reach out about contributing. 

## Change Log
See [CHANGELOG.md](CHANGELOG.md) for a detailed history of changes.
