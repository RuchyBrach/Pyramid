# Pyramid
Created By: Ruchy Brach

## Overview
 This repo is for the software implementation of the game of Pyramid.
This Software will allow the player to build words on a specific letter of the alphabet. A description of the game including the requirements of this implementation are provided below. 

## The Game
 The pyramid consists of 5 layers. The first layer has one textbox, the second layer has two textboxes, the third layer has 3 textboxes, and so on...
The player will be presented with one letter of the alphabet on the upper most textbox of the pyramid. The player will then write a new word in the next layer while including all of the letter(s) from the previous layer.
When all five layers are completed the player's score will go up and the player will be presented with a new pyramid. If the score is the highest the player ever reached, the score will be recorded in the "Best Played" Textbox. 

## Software implementation
### UI elements
The software will present the Pyramid game with the UI elements listed below.
* 5 layered pyramid - The top layer has one textbox, the second layer has two textboxes, and so on...
* A sidebar that includes:
  - Label to display the score
  - Label to display best played (the highest score yet).
  - Five labels that indicate how many tries the player still has. (If the player has 4 tries left, one label will be faded out etc.)
## Game Process and Rules
* The player will be presented  with one letter of the alphabet on the upper most textbox of the pyramid. The player will then write a new word in the next layer while including all of the letter(s) from the previous layer. 
* If the player does not include all of the letter(s) from the previous layer then that layer's text boxes will be cleared and one of the "tries" labels will fade out.
* If the player enters a word that is not in the dictionary then a message will pop up displaying "Word Not Found in Dictionary ", and that layer's text boxes will be cleared out. 
* When all five layers are completed, the score will go up and the player will be presented with a new pyramid.
* When the player has used all five tries, a message will pop up "Game Over". The player will be presented with a new pyramid and the score will be set to 0. If the score is the highest yet, then a message will pop up "Congrats, You Beat the Score", and the "Best Played" label will be updated.
