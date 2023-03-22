cd C:/DockerContainer/nginx
set image_name=nginxreverse

echo Stoppen aller Container, die auf %image_name% basieren...
for /f "tokens=*" %%i in ('docker ps -aqf "ancestor=%image_name%"') do (
    echo Stoppen des Containers %%i...
    docker stop %%i
    echo L�schen des Containers %%i...
    docker rm %%i
)

echo L�schen des Images %image_name%...
docker rmi %image_name%

echo Abgeschlossen.
docker build -t nginxreverse:latest .
@REM docker run -p 3001:3001 -it nginxreverse
docker compose up

timeout 1000