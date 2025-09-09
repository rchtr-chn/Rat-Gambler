<h2>🐀 Rat Gambler</h2>
  <img width=200px align="left" src=https://img.itch.zone/aW1nLzIyOTc2MDY5LnBuZw==/original/7pJkfp.png>

  You’re a down-on-your-luck rat who made one very bad deal with the Rat Mafia.<br/>
  Now, buried in debt and cornered by crooks, your only way out is to gamble for cookies… because in the underworld, cookies are currency.<br/>
  At the end of the line, you’ll gamble it all against the Rat Mafia Boss.
  One last hand… one last cookie… one last chance at financial freedom.<br/>
  Will you claw your way out of debt, or will the mob bury you under a mountain of crumbs?


<h2>⬇️ Game Pages</h2>
  itch.io: https://rchtr-chn.itch.io/rat-gambler
  
<h2>🎮 Controls</h2>

  | Input | Function |
  | -------------------- | --------------------- |
  | Hold and move cursor | Select and play cards |
  
<h2>📋 Project Information</h2>

  ![Unity Version 6000.2.2f1](https://img.shields.io/badge/Unity_Version-6000.2.2f1-FFFFFF.svg?style=flat-square&logo=unity) <br/>
  Game Build: ![WebGL](https://img.shields.io/badge/WebGL-990000.svg?style=flat-square&logo=WebGL) ![Windows](https://img.shields.io/badge/Windows-004fe1.svg?style=flat-square&logo=windows) <br/>
  Most art assets are made by our game artists, aside from orange point sign and trashbin vector from [![Pixabay](https://img.shields.io/badge/Pixabay-191B26.svg?style=flat-square&logo=Pixabay)](https://pixabay.com) <br/>
  All SFX can be found in [![Pixabay](https://img.shields.io/badge/Pixabay-191B26.svg?style=flat-square&logo=Pixabay)](https://pixabay.com) <br/> <br/>
  
  <b>Team:</b>
  - <a href="https://github.com/rchtr-chn">Richter Cheniago</a> (Game programmer)
  - Sony Aliem (Game designer and artist)
  - <a href="https://www.behance.net/epenaja">Melvern Sjah</a> (Game designer and artist)

<h2>💡 My Contributions</h2>

  as the sole programmer of this project, I am tasked to make all of the mechanics that make the game function as intended, such as the card system, the turn based system, the shop system, etc.

<h2>📜 Scripts</h2>

  | Script | Description |
  | ------ | ----------- |
  | `DeckManagerScript.cs` | Manages starting deck and saves any modification done to deck by player |
  | `HandManagerScript.cs` | Receives cards from `DeckManagerScript.cs` to be drawn on hand and returned to when needed|
  | `GameManagerScript.cs` | Organizes and centralized other minor managers and manages the turn-based system |
  | `ShopManagerScript.cs` | Manages the shop's cards to be displayed and sold to the player |
  | `Card.cs` | Blueprint for SOs that will carry a card's value and the potential card effect |
  | etc. |

<h2>📂 Folder Descriptions</h2>

  ```
  ├── Rat-Gambler                      # Root folder of this project
    ...
    ├── Assets                         # Assets folder of this project
      ...
      ├── Audio                        # Stores all BGM and audio clips used in this project
      ├── Fonts                        # Stores all fonts used in this project
      ├── Resources                    # Parent folder to organize blueprints (Scriptable Objects) and prefabs
        ├── CardData                   # Parent folder of all scriptable object types that are used in this project
          ...
        ├── Prefabs                    # Parent folder that stores prefabs that are instantiated during the project's runtime
          ...
      ├── Scenes                       # Stores all Unity Scenes used in this project
      ├── Scripts                      # Parent folder of all types of scripts that are used in this project
        ├── BackgroundManagers         # Stores scripts related to managers that function the game in the background
        ├── CardBehavior               # Stores scripts related to a card prefab
        ├── CardEffects                # Stores scripts consisting the logic behind every power cards
        ├── Cardshop                   # Stores scripts related to the card shop
        ├── CardSystem                 # Stores scripts related to card deck creation and usability during gameplay
        ├── Cookie                     # Stores scripts related to wagering cookies mechanic and cookie value modification
      ├── Sprites                      # Parent folder of all sprites that are used in this project
      ...
    ...
  ...
  ```
