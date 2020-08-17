#!/bin/bash
#/*******************************************************************************
# *Author: Kien Truong
# *Program: Falling Apples
#******************************************************************************/

echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile FallingApplesl.cs to create the file: FallingApplesl.dll
mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:FallingApples.dll FallingApples.cs

echo Link the previously created dll file to create an executable file.
mcs -r:System -r:System.Windows.Forms -r:FallingApples.dll -out:Falling.exe main.cs

echo View the list of files in the current folder
ls -l

echo Run the Falling Apples program.
./Falling.exe

echo The script has terminated.
