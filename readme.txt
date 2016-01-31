Das Klub Radio Updater
----------------------
Das Klub Radio Updater is a program that fetches podcasts, downloads them (converting them to MP3 if needed) and adds them to playlists.

This program enables the automation of a radio station using ShoutCast (sc_trans and sc_serv). It adds to playlists and takes the newest podcasts and adds them to a special playlist.

After playlists are updated, sc_trans needs to restart to pick up the changes. 

By scheduling the restart of sc_trans and running Das Klub Radio Updater on a recurring basis, an automated podcast based radio station is a reality.

How to use: 
-----------
You can run this console application as a scheduled task on Windows Server. 

1. compile the application
2. zip the contents of the main program's debug directory
3. move the file to the server to deploy to
4. ensure all paths are there for the music and playlists
	a. "C:\MP3s"
	b. "C:\MP3s\downloads"
	c. "C:\MP3s\logs"
	d. "C:\MP3s\music"
	e. "C:\MP3s\playlists"
6. enable write and modify permissions for "NETWORK SERVICE" and "LOCAL SERVICE" on the "C:\MP3s" folder
6. move the "faad.exe" and "lame.exe" files to the "C:\thirdparty" directory
7. create a scheuled task to run "DKRUpdater.Main.exe"

Current deployment: 
-------------------
Das Klub Radio Updater is currently being used to support Das Klub Radio. You can access Das Klub Radio at: https://dasklub.com/radio

You can listen to Das Klub Radio on mobile devices using the ShoutCast or XiiaLive apps (search for: "Das Klub Radio"). 

You can also listen to Das Klub Radio using Winamp or iTunes by opening a stream to the station. 