# Copier App

## What is it?
A cross-platform (works on Windows, Linux, and Mac) and open-source file watcher and copier application. It allows you to specify folders/files to track and copies them to a target location. It continuously watches changes on the specified files in the background and copies them to the destination directory.

The application has been developed with .NET Core technology and depends on the existence of .NET Core framework on the target machine where it is being used. You can simply download the framework from http://dot.net.

Note: The future release will be built without .net core dependency.

## How to use?

Currently, it is a command-line application; therefore, you need to be passing a few options to be able to tell what to copy, the source directory, and the destination directory.

The application can be used with two verbs: `watch` or `watchWithConfigFile`. If you want to pass all the options manually on the command line, use `watch`. If you would like to store all the configuration in a file and specify it as the source of the options, use `watchWithConfigFile` and specify the config file path. The default configuration file name is `config.txt`. You can specify your own name with `dotnet Copier.dll watchWithConfigFile -c "C:\test\myConfig.txt"`. 

You can use the `help` command to find out more information about a given command such as `dotnet Copier.dll help watch` or `dotnet Copier.dll help wathcWithConfigFile`.

### An example:
A simple way to start off with this tool would be as follows:

```
dotnet Copier.dll watch -s "C:\source-folder\" -f "*.txt" -d "C:\destination-folder"
```
- `watch` is the command that accepts options on the command line instead of from a config file.
- `-s` is the source folder.
- `-f` is the file name pattern in glob format. For instance, `*.txt` means all the files ending with `.txt` extension. 
- `-d` is the destination folder where the files will be copied over.

By default, if a file already exists in the destination directory, the file won't be copied unless told otherwise with `-o` flag. Check out `--help` for more information about all the available options.

### Config File Format
A simple config file looks like the following:

```
-s "C:\Users\Tarik Guney\Desktop\test-files"
-f "*.txt"
-d "C:\Users\Tarik Guney\Desktop\test-files\new-files"
-v
-o
```

You specify each option with their arguments (if applied) in a new line. Do not combine multiple options in the same line.

Type `dotnet Copier.dll help watch` for all the available options you can use in the config file.

### Delayed Copy
One of the major features of this tool is to be able to queue all the files that are changed and copy them to the destination folder with a given delay. For instance, if you specify `-t 5000` option along with the other required options when running the application, it will wait for 5 seconds before copying all the changed files that it has been queuing. An example would look like as following:

```
dotnet Copier.dll watch -s "C:\source-folder\" -f "*.txt" -d "C:\destination-folder" -t 5000
```

### Plugins

Copier app supports plugin integration. You can easily develop your own plugins and have them executed before and after the copy operations. All you have to do is to reference `CopierPluginBase` in your dotnet core class library, and implement `IPostCopyEventListener` and|or `IPreCopyEventListener` interfaces. Once you build your class library, put the `.dll` file under `plugins` folder in the Copier app installation location. If you don't see this folder, simply create it where the Copier.dll file exists.

### Having issues?

Copier app has been developed with an in-depth logging mechanism. You can see all the steps it takes when running it with the debug `-e` flag. If you would like to see more than regular messages but less than debug messages, use `-v` verbose flag. If you still have issues, create a new issue at https://github.com/tarikguney/copier/issues or please send a pull request with the fix. I'd love to see other people's contributions.


## How to download?

You can go to release page (https://github.com/tarikguney/copier/releases) and find the most recent version to download. It is a zip file that you need to unzip on your machine. 

## Watch how this application was developed!

If you speak in Turkish, you can watch the entire process of this application being developed here:

https://www.youtube.com/watch?v=aW8W2gze8JE&list=PL_Z0TaFYSF3LLSRobjiV0y-I18kjRm7cx


## How will you feel when using this tool?

![Laughing](https://media.giphy.com/media/l1IYbqyLSloejiLok/giphy.gif)

Developed by @tarikguney with <3...