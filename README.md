# ASP 5 Note Manager

This project was built by David Pfeiffer to learn about the ASP 5 framework. The website allows users to create notes and notebooks and has built in features to manage both.

Feel free to pull the project and make changes. You can create an account and walk through the basic note creation process after a brief initial setup.

### Project Setup

To run this project, you must install ASP 5 on your computer. You can install via visual studio or command line from Microsoft https://docs.asp.net/en/latest/getting-started/installing-on-windows.html

Once installed, enable the ASP 5 command-line tools

```
$ dnvm upgrade
```

Clone the project from this Github repository and navigate to the project directory

```
$ git clone https://github.com/davidjpfeiffer/note-manager
$ cd note-manager
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
