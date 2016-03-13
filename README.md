# ASP 5 Note Manager
An ASP 5 Note Management Website.

I built this website to learn the ASP 5 framework. The website allows for note creation and management. You can create notebooks to organize notes, and keep track of changes to notes through revisions.

Feel free to pull the project and check it out. You can create an account and walk through the basic note creation process after a brief initial setup:

# Project Setup
Clone the project from this Github repository

To run this project, you must install ASP 5 on your computer. You can install via visual studio or command line from Microsoft https://docs.asp.net/en/latest/getting-started/installing-on-windows.html

Once installed, you must enable the ASP 5 command-line tools using the following command:

dnvm upgrade

Now that you have access to the command line tools, navigate to the Note Manager project directory. We must setup an initial migration for our database using the following commands (run in this order):

dnu restore

dnvm use 1.0.0-rc1-final -p

dnx ef migrations add Initial

dnx ef database update

Open the project in Visual Studio. In the Task Runner Explorer run the Gulp build task and MS build the project. You should now be able to run the project using IIS Express.
