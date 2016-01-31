Das Klub Radio Updater
----------------------

You can run this console application as a scheduled task on Windows Server. 

1. compile the application
2. zip the contents of the main program's debug directory
3. move the file to the server to deploy to
4. ensure all paths are there for the music and playlists
	a. C:\MP3s
	b. C:\MP3s\downloads
	c. C:\MP3s\logs
	d. C:\MP3s\music
	e. C:\MP3s\playlists
6. enable write and modify permissions for NETWORK SERVICE and LOCAL SERVICE on the MP3 folder
5. move the faad.exe and lame.exe files to the C:\thirdparty directory
6. create a scheuled task to run DKRUpdater.Main.exe