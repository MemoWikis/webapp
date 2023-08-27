@echo off

set IMAGE_NAME=nuxt-memucho

docker images | findstr /C:"%IMAGE_NAME%" >nul
if %errorlevel% equ 0 (
  echo Image '%IMAGE_NAME%' already exists.
) else (
  echo Image '%IMAGE_NAME%' not found. Building the image...
  docker build -t %IMAGE_NAME% ..\.\TrueOrFalse.Frontend.Web.Nuxt\
)

cd ..\.\TrueOrFalse.Frontend.Web.Nuxt\
docker-compose up -d
timeout 1000
