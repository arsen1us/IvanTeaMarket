# ���������� ����������� ����� .NET SDK ��� ������
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# ������������� ������� ���������� ������ ����������
WORKDIR /app

# �������� ����� ������� � ��������������� �����������
COPY *.csproj ./
RUN dotnet restore

# �������� ���� �������� ��� � �������� ������
COPY . ./
RUN dotnet publish -c Release -o out

# ���������� ����������� ����� .NET Runtime ��� �������
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# ������������� ������� ����������
WORKDIR /app

# �������� ��������� ���������� �� ����������� ����������
COPY --from=build /app/out .

# ��������� ���� (���� ���������� �������� �� 5000/5001)
EXPOSE 5000
EXPOSE 5001

# ��������� ����������
ENTRYPOINT ["dotnet", "CustomerChurmPrediction.dll"]