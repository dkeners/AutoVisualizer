# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased] - Basically my ToDo :)

### Added

- Image viewer from bitmap component
- Image viewer from path component
- CaptureView compnent
- AutoVisualizer Version component
- Change IP address component
- ControlNet Version component
- ControlNet model_list component
- ControlNet module_list component
- ControlNet control_types component

### Fixed

- Fixed bugs


## [0.2.0] - 2024-01-02

### Added

- CaptureViewport component
- CaptureOptions component
- ControlNet request component
- ContolNet to Stable Diffusion options component

### Changed

- Async and Sync SD_Generate to allow for multi-image output

### Removed

- Toplevel AutoVizualizerComponent that was an unused component

### Fixed

- Input alwayson_scripts in SD_GenerateOptions_Full to correctly convert string JSON data


## [0.1.0] - 2023-12-20

### Added

- SDGenerateASync component for asynchronous image generation.
- SDGenerateSync component for synchronous image generation.
- SDGenerateSettingsEssential component for access to a few settings.
- SDGenerateSettingsBasic component for access to more settings.
- SDGenerateSettingsFull component for access to all settings.
- Icons for each component!
