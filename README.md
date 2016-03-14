# ASP 5 Note Manager
ASP 5 Note Management Website

I built this website to learn the ASP 5 framework. The website allows for note creation and management. You can create notebooks to organize notes, and keep track of changes to notes through revisions.

Feel free to pull the project and check it out. You can create an account and walk through the basic note creation process after a brief initial setup:

### Project Setup

To run this project, you must install ASP 5 on your computer. You can install via visual studio or command line from Microsoft https://docs.asp.net/en/latest/getting-started/installing-on-windows.html

Once installed, enable the ASP 5 command-line tools

```
$ dnvm upgrade
```

Clone the project from this Github repository and navigate to the project directory

```
$ git clone https://github.com/davidjpfeiffer/NoteManager
$ cd NodeManager
```

Now that you have access to the command line tools, we must setup an initial migration for our database

```
$ dnu restore
$ dnvm use 1.0.0-rc1-final -p
$ dnx ef migrations add Initial
$ dnx ef database update
```

Next we install our Node packages and run a Gulp build

```
$ npm install
$ gulp build
```

Open the project in Visual Studio. You should now be able to run the project using IIS Express.
