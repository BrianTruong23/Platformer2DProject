# Platformer 2D Project

A simple 2D platformer built in Unity. The player moves through the level, collects coins, avoids hazards and enemies, and reaches the goal after collecting enough coins.

## Gameplay

The player controls a character in a side-scrolling platform level. Coins are placed throughout the scene, and the player must collect at least 2 coins before the goal can complete the level.

When the player touches a coin, the coin disappears and the coin counter updates. If the player reaches the goal with enough coins, the game displays a level-completed message and plays the win sound.

## Level Description

The level is a castle-themed platform challenge with floating stone platforms, fire hazards along the bottom of the map, enemy characters, collectible coins, and a goal flag near the end. The coin counter appears in the top-left corner so the player can track progress while moving through the level.

Players need to jump between platforms, avoid falling into the fire, dodge enemies, collect enough coins, and then reach the flag to clear the level.

## Controls

- Move left: `A` or `Left Arrow`
- Move right: `D` or `Right Arrow`
- Jump: `Space`

The player can jump twice before needing to touch the ground again.

## Objective

1. Collect enough coins to clear the level.
2. Avoid enemies and fire hazards.
3. Reach the goal after collecting at least 2 coins.

## Hazards

Touching an enemy or fire causes a game over. The game-over sound plays, then the level restarts.

## Audio

The game includes sound effects and background music:

- Coin collected sound when a coin is picked up
- Game-over sound when the player loses
- Game-won sound when the level is completed
- Background song that loops throughout the game

## Unity Setup Notes

Objects should use these tags:

- Coins: `Coin`
- Ground: `Ground`
- Enemies: `Enemy`
- Fire hazards: `Fire`
- Goal: `Goal`

Coins should have a `Collider2D`. They can use either trigger colliders or normal collision colliders.
