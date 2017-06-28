@echo off

setlocal

cd /d "%~dp0"

set solutionDir=%cd%
set outDir=%solutionDir%\bin

if exist %outDir% rmdir /s/q %outDir%


set config=Release
set platform=Any CPU


echo Building solution 'Graph_2013.sln' (%config%^|%platform%)
%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe /nologo "/p:SolutionDir=%solutionDir%"\ "/p:Configuration=%config%" "/p:Platform=%platform%" "/p:OutDir=%outDir%"\ /verbosity:minimal "Graph_2013.sln"

if ERRORLEVEL 1 goto :EOF


.nuget\nuget pack

rmdir /s/q bin
