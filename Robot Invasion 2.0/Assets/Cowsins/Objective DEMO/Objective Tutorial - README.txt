This is mainly a template just to make you see how it can work
so, in base of your need, you may need to make some changes
for example:

GameManager prefab in the scene: it has the GameFlowManager scripts that for now is separate from the player
because it has it's own canvas to work for the win and lose transition.
Also you must have in the scene (in this case always as component of the gamemanager object) the Enemy Manager and
the Objective Manager.
Those are needed to take tracks of enemies count on screen and also of all the objective you are goin to use or spawn.

I Changed the Objective script so you don't need to worry to change them a lot, but for the visual you have to change them by yourself

Also most important thing is that as you can see in the folder i created another prefab for the player controller.
In this on i added in the PlayerUI all the visual and components needed to visualize the Objective and messages  on the screen.
I suggest you to open the prefab and see all the changes that mainly can be found in the "PlayerUI" object by scrolling down in the 
inspecto i added 3 scripts

and also by navigating inside, PlayerUI > Container > ObjectsToSway > JumpMotion
you can see that at the bottom i added also 3 Object needed for the visualization of the objectives, and in base of your need
you may need to change the visual properties and positions.

About the EnemyManager is quite simple to make it work with your Enemies, but this depend alway what you use as Enemy AI
for now i register all the enemies in base og their EnemyHEalth component.
In the scene that component is inherit in the TrainingTarget script. And you watch that script on each traini target i added on the Spawn event and on the Death event
the action for Register the anemy and Unregister.
They are NEEDED for the correct behaviour of the EnemyKillObjective

In case you have some other question, becuase i may missed something or wrote something that is not easy to understand.
Contact me on Cowsins Discord.

v 1.1 Added Reward system
in this version you are able to add to every quest all the rewards you want, for now they can be only a coin and exp
When you put a quest or create a quest in the scene you can see that as a variable you have an array that is called "REWARDS"
this mean that you can add and assign the rewards

Q - How can i create a reward?
A- Is pretty simple, in your project, everywhere you want you can create a reward by just left click Create -> COWSINS -> Reward

Q - How can i modify it
A - You have 3 variables the first two are the min and max amount you want to set for the coin and the experience to be given as reward
    and the third variable is just an audioClip you can assign when you complete the quest