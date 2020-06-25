#!/bin/bash
DIR="${BASH_SOURCE%/*}"
if [[ ! -d "$DIR" ]]; then DIR="$PWD"; fi
. "$DIR/config.sh"

start=$(date +%s)

if [ $CleanBuildDirectory == y ]; then
    echo "Cleaning Build directory"
    rm -rf "$ProjectPath/Builds"
fi

function execute_build() {
    echo "Start content update for $1..."
    $UnityPath -quit -batchmode -nographics -projectPath $ProjectPath -executeMethod BuildScript.PerformAddressablesContentUpdate -logFile $ProjectPath/ServerData/ContentUpdate.log
    echo "$1 Completed."
}

if [ $Linux == y ]; then
    execute_build LinuxBuild
fi
if [ $Android == y ]; then
    execute_build AndroidBuild
fi
if [ $IOS == y ]; then
    execute_build IosBuild
fi
if [ $OSX == y ]; then
    execute_build OSXBuild
fi
if [ $Windows == y ]; then
    execute_build WindowsBuild
fi

end=$(date +%s)
echo Build complete in $(expr $end - $start) seconds
