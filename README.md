# Falling Apples
---
The purpose of this program is to become familiar with how to read the
coordinates of the cursor into x and y variables and how to generate a random
integer.

## Specifications
---
* Initially, the apple appears as a red dot.
* When the user clicks on "Start" button, the apple will drop vertically.
* When the user clicks on "Pause" button, the animation freezes and the text
string will turn to be "Resume". If the user clicks on "Resume" button, the
animation will resume from the point where it freezes.
* When the user clicks on "Clear" button, the program will return to the initial
state.
* The apple tree is in an orchard. The ground is brown dirt. The sky is blue.
In the distant horizon, the sky meets the ground. Apples fall from the tree
(not seen) at random times. If the apple has reached the yellow panel, then the
apple is damaged. The only time that the user can catch the apple is when it is
low enough, where the apple must reach or pass the horizon line, but does not pass
the line where the yellow panel starts.
* The user "catch" the apple by clicking the cursor on the red ball. When the apple
has been caught, it disappears from the view, and the apple caught counter
increases by one. Else, the apple missed counter increments by one.
* After the apple is caught or missed, a new apple will appear at random places.
For instance, if the user interface is 1280 pixels, then the program picks a
random place between 8 and 1276 for the starting place of the apple.
* Success ratio will show the percentage that the user has successfully caught
the apples.  

## Prerequisites
---
* A virtual machine
* Install mcs

## Instruction on how to run the program
---
1. chmod +x build.sh then ./build.sh
2. sh build.sh

Copyright [2019] [Kien Truong]
