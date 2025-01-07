# Persi Scope
Cross platform software defined osciloscope which uses multy layer software architecture.
Only tested and compiled on windows, but could be compiled for other platforms like Linux, MAC an wathever platform that AvaloniaUI  supports (like Android etc.)

# Architecture

## Hardware Layer
Codes for comunicating data with (different) Hardwares goes here.

## Library Layer
Codes for core features, like FFT or storing data goe here.
SharpFFTW is used for FFT operation in this layer.

## GUI Layer
Code for visualizing the stored data goes here.
The code architecture is based on MVVM with AvaloniaUI.

# How to build

Install visual studio 2022, follow [this](https://docs.avaloniaui.net/docs/get-started/install-the-avalonia-extension) to install avalonia extensions on visual studio, and there should be no problem to compile the code.

# Screenshots

The software is under development, currently have a windows 95 theme!

![FFT](/Docs/screenshots/prealpha/fft.png)


![About](/Docs/screenshots/prealpha/about.png)


![Connect](/Docs/screenshots/prealpha/connect.png)

