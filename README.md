# John Conway's Game of Life in Unity

First iteration of the build
<p align="center">
  <img src="https://i.imgur.com/OF7c04Q.gif">
</p>

Current iteration of the build
<p align="center">
  <img src="https://i.imgur.com/yGDZa41.gif">
</p>

## About the Game of Life
This is a simple project I wanted to try out for fun. The rules to the [Game of Life](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life) are as follows:

- Any live cell with fewer than two live neighbours dies, as if by underpopulation.
- Any live cell with two or three live neighbours lives on to the next generation.
- Any live cell with more than three live neighbours dies, as if by overpopulation.
- Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.

I started off by instantiating sprites to represent each cell and their states. This worked until I decided to increase the grid size and everything started to slow down. My solution to this was rather simple, instead of instantiating thousands of sprites it would be best to use a single [Texture2D](https://docs.unity3d.com/ScriptReference/Texture2D.html) and alter its pixels instead. This worked tremendously well with the current build being able to handle a 700x400 grid with ease. I've also added a simple system paint over the grid while the game is paused.

Unfortunately I've hit another roadblock as anything higher than that significantly reduces performances. I've tried optimizing where I can but it looks I'll have to look into implementing either the [ListLife](http://dotat.at/prog/life/life.html) or [HashLife](https://en.wikipedia.org/wiki/Hashlife) algorithm. Either way its fine for now and I may return to this project in the future.
