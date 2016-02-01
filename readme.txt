----------------------
Das Klub Radio Updater
----------------------
"DKRUpdater.Main.exe" is a program that downloads podcasts (converting them to MP3 if needed from M4A) and then adds them to associated playlists. 

Feeds specifying the "newmusic.pls" playlist get included in a most recent releases playlist of a specified count.

This program enables the automation of a radio station using ShoutCast ("sc_trans.exe" and "sc_serv.exe"). It adds to playlists and takes the newest podcasts and adds them to a special playlist.

After playlists are updated, "sc_trans.exe" needs to restart to pick up the changes. 

By scheduling the restart of "sc_trans.exe" and running Das Klub Radio Updater on a recurring basis, an automated podcast based radio station is a reality.

Each feed is modeled for its own specific requirements.

-----------
How to use: 
-----------
You can run this console application as a scheduled task on Windows Server. 

1. compile the application
2. zip the contents of the main program's debug directory ("dkrupdater\DKRUpdater.Main\bin\Debug")
3. move the file to the server to deploy to
4. ensure all paths are there for the music and playlists
	a. "C:\MP3s"
	b. "C:\MP3s\downloads"
	c. "C:\MP3s\logs"
	d. "C:\MP3s\music"
	e. "C:\MP3s\playlists"
6. enable write and modify permissions for "NETWORK SERVICE" and "LOCAL SERVICE" on the "C:\MP3s" folder
6. move the "faad.exe" and "lame.exe" files to the "C:\thirdparty" directory
7. create a scheduled task to run "DKRUpdater.Main.exe"

NOTES: 

- All files are converted into MP3 due to the MP3 stream licensing.
- Initial podcast fetches can be time consuming, especially when the source is M4A because it has to be converted to MP3 (192 Kpbs).

-------------------
Current deployment: 
-------------------
Das Klub Radio Updater is currently being used to support Das Klub Radio. You can listen to Das Klub Radio at: https://dasklub.com/radio

You can listen to Das Klub Radio on mobile devices using the ShoutCast or XiiaLive apps (search for: "Das Klub Radio"). 

You can also listen to Das Klub Radio using Winamp or iTunes by opening a stream to the station. 

Stream URL: http://radio.dasklub.com:8000/listen.pls?sid=1

----------
Debugging:
----------
Das Klub Radio Updater writes to a log file that can be found in: "C:\MP3s\logs". Please look there for all events occuring related to the program.