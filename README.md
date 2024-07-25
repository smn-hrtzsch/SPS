# SPS
Hello and welcome to SPS! 
## What is SPS?
Glad you asked. SPS, also called Sports Prediction System is a friendly and fun competition experience, where Software meets Your favorite sport.
## How does it work?
### User Experience
In SPS the user is King. We wanted to enable you and all your friends and family to bid each other off, whilst enjoying watching or listening to the newest upcomming events. The software itself is desinged as a multi user tool for one device. You can add members, display their scores in the current schedule and, of course, enhance your dedication to sport by the included tip algorithm.

### On top of that,...
You can get a daily newsletter to stay on track, which matches are happening just on the same day, so you don't miss any predictions and are abled to show your prediction partners how well you are informed about the current events.

### So?
How about a first try?
What do you say? 

## Installation
- To get the last stable release of SPS, check out the "main" branch

### Clone repository
For new and unexperienced users, we recommend cloning the repository onto your local machine.
To do so, choose a place on your local maschine where you want to save the project in.  
Now open the choosen directory in your file explorer and open with integrated terminal.  
Paste the following command and press 'enter'.  

```sh
git clone https://github.com/smn-hrtzsch/SPS
```
The repository should now be cloned onto you local maschine and you can now go on with the usage instructions.

### Download release source code
You can also download the release [source code](https://github.com/smn-hrtzsch/SPS/archive/refs/tags/v.1.0.0.zip) by clicking this link.  
You can then direct into the download path and follow the instructions below.

## Useage
Firstly, check out the download instructions above and choose the realease you would like to run on your machine.

### Checklist
- here is your checklist for a normal installation
  - DOTNET 8.x is installed and up and running
  - You already cloned your favored branch into your local file system
### Starting the programm
- Starting the programm on Linux:
  - Open a new terminal
  - Navigate to the path you cloned the repository in (e.g.: cd documents/, cd softwaredevelopment/, cd project/, cd SPS/)
  - Enter the 'src' folder: with "cd src/"
  - Enter the 'programm' folder: with "cd Program/"
  - Restore the dependencies: with "dotnet restore" 
  - Build the project: with "dontet build"
  - Run the project: with "dotnet run"
 
- Starting the programm on Windows:
  - Open the file browser and navigate to the project folder
  - Open the 'src' folder
  - Open the 'Program' folder
  - Right-click insode the folder and select 'Open in Terminal'
  - Restore the dependencies: with "dotnet restore" 
  - Build the project: with "dontet build"
  - Run the project: with "dotnet run"
 
- Starting the programm on MacOS
  - Open the terminal application
  - Navigate to the path you cloned the repository in (e.g.: cd path/to/repository)
  - Enter the 'src' folder: with "cd src/"
  - Enter the 'Program' folder: with "cd Program/"
  - Restore the dependencies: with "dotnet restore" 
  - Build the project: with "dontet build"
  - Run the project: with "dotnet run"

## Your first impressions
After running the application for the first time you will have to register. Later, after you or your partners already used the application a few times, you can safely login again using the information from your registration. The algorithm will save all your data in CSV-Files, so you can access the data by clicking on 'MemberData.csv' in the 'csv-files' folder.

### Register
For registration you can press [2] in the login screen. You will be asked to enter a email address, your full name and a password. This data can later be used to verify your user account and keep establish a user session state, that persists, even after the program terminated.

### Login
After you and your friends and family registrated every user wanting to participate in the prediction game, you can login with your email adress and the password. Notice that only one user can use the application at a time. This was implemented so that cheating is prevented and the game is fair. This also guarantees the most fun after logging in again after a while and seeing how your partners scored compared to you. This enhances the competition and also makes the real-time event more enjoyable.

### Sports Prediction System Application
After setting up the users account and login data you are now ready to use the application features. The user interface, running as a console application, will present you four options to choose from:
1. Display Members
2. Display Scores
3. Add Prediction
4. Save and Exit

#### 1. Dispay Members
The 'Dispay Members' feature is abled to give a quick and useful overview to all members, preferably your partners, participating in the prediction schedule. The prediction schedule is the order of the series of events that will be predicted. This will be of use later, when we get to the prediction algorithm we created for you. 

All there is left to say for now is, that the 'Display Member' option displays the 'MemberID', the 'Forename' and the 'Surname' of each member in the prediction schedule. You might press any key to return to the main menu.

#### 2. Display Scores
Furthermore, we added a 'Display Scores' feature. Here, you can see the score of each member in the participating schedule, which is the part where you can see the predictions your partners did.

You will be presented with the 'MemberID', the member '(Forename)', the participating schedule 'EM_2024' and the 'Total Score' for each member. Again, this is a display feature and you might press any key to return to the main menu.

#### 3. Add Prediction
You could describe the 'Add Prediction' feature like this: The 'Add Prediction' feature in SPS is what the CPU in a computer is, the heart of the whole system. 

This feature will present you with all matches in your current schedule that you can predict that day. The algorithm will update the matches you can predict daily according to the schedule. Therefore, you can only predict certain matches if they take place at the same day in the near future. 

After closing the program through the 'Save and Exit' interface otpion, your predictions will be written into the 'PredictionData.csv' file. This functionality is implemented so that the next time you rerun the program, your scores and the score of your partners will be calculated for the 'Display Scores' option.

If there are no more matches to predict that day, reopening the 'Add Prediction' interface option will give you a full overview, which predictions you already did. These predictions are preding for the calculate score function. 

To exit and return to the main menu you might want to press the 'esc' button.

#### 4. Save and Exit
The 'Save and Exit' feature is rellay easy to understand. Press [4] in the main user interface and you will be asked if you want to save your data into a comma-separated values (CSV) file. Foreach CSV-file we implemented a Reader- and Writer-functions for you. If you press any key, apart from the 'esc' key your data will safly be stored and you will exit the application interface. You can rerun the application any time by tiping 'dotnet run' again.

## Summary
SPS is a fun and exciting sports-prediction-simulator. If we were able to make you want to try it, you are now ready to go. Have a fun time predicting the current EM_2024 event.
