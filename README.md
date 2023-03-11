# TicTacToe
 Web api for tic-tac-toe game
 
 First, you need  to start your MySQL server. Then you need to change connection string in ``` application.json ```

Start web api project(migrations will be done).

To play a game you need(I do it with swagger):
	1. Create player one. ``` /player/create-player ```, give id, name, playerBoards set to null("playerBoards" : null)
	2. Create second player. The same as for first player, but give another id and name
	3. Create board to play. ``` /board/create-board ``` give id's of first and second player and board size(for example 3x3)
	4. Let's play a game ``` /game/play-game ``` give boardId, playerId and position where you want to put your sign.
		This will be one step of one player, continue like this to play full game.
		
	Game will be finished after one player make line of 3 signs. 
	You can understand this when you try to make a step and get 400 error game finished.