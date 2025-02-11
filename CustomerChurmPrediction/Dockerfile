# Используем официальный образ .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Устанавливаем рабочую директорию внутри контейнера
WORKDIR /app

# Копируем файлы проекта и восстанавливаем зависимости
COPY *.csproj ./
RUN dotnet restore

# Копируем весь исходный код и собираем проект
COPY . ./
RUN dotnet publish -c Release -o out

# Используем официальный образ .NET Runtime для запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Устанавливаем рабочую директорию
WORKDIR /app

# Копируем собранное приложение из предыдущего контейнера
COPY --from=build /app/out .

# Открываем порт (если приложение работает на 5000/5001)
EXPOSE 5000
EXPOSE 5001

# Запускаем приложение
ENTRYPOINT ["dotnet", "CustomerChurmPrediction.dll"]