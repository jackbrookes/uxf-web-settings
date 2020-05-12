# UXF Web Settings

This is a Unity package that provides an alternative way to start a UXF Session. It can download a settings file from a public URL - allowing researchers to remotely modify experiment settings to builds of experiments deployed outside of a lab.

To use this in your project download the latest [Release](https://github.com/jackbrookes/uxf-web-settings/releases/latest)

## Setup

1. Add the latest UXF package to your project
2. Add the `[UXF_Web_Settings]` prefab to your scene and add the reference to an instance of the `[UXF_Core]` prefab in your scene. See the example scene.

You must host your settings file at a publicly acessible URL (e.g. Amazon S3 bucket) and enter the link in the web address asset.

## UI

This package includes a UI similar to the default UXF UI but designed for end-users rather than researchers. Feel free to delete the UI and replace with one you feel appropriate for your project.

![Screenshot](/screenshot.png)
