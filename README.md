# ODBuildPipeline

Basic build pipeline for Unity with Addressables.

## Installation and Usage

- Download repository and put it inside your Asset folder.

- Inside Bash folder open config.sh and set UnityPath, ProjectPath and your target platforms to build.
  ie for building only for Android:

```bash
Android=y
Linux=n
Windows=n
IOS=n
OSX=n
```

- **[Optional]** Set CleanBuildDirectory to erase all previous builds before performing a new build.

## Build

- Make sure UnityEditor is closed.
- Run build.sh bash script to execute a new build:

Linux and Mac:

```bash
./build.sh
```

Windows:

```bash
 bash build.sh
```

## Content update

- Make sure UnityEditor is closed.
- Run build.sh bash script to execute a new build:

Linux and Mac:

```bash
./build.sh
```

Windows:

```bash
 bash build.sh
```
