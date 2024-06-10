
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["HotelBookingAPI/HotelBookingAPI.csproj", "HotelBookingAPI/"]
COPY ["HotelBooking.Core/HotelBooking.Core.csproj", "HotelBooking.Core/"]
COPY ["HotelBooking.Infrastructure/HotelBooking.Infrastructure.csproj", "HotelBooking.Infrastructure/"]
COPY ["HotelBooking.Messaging/HotelBooking.Messaging.csproj", "HotelBooking.Messaging/"]
RUN dotnet restore "HotelBookingAPI/HotelBookingAPI.csproj"


COPY . .
RUN dotnet build "HotelBookingAPI/HotelBookingAPI.csproj" -c Release -o /app/build

RUN dotnet publish "HotelBookingAPI/HotelBookingAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "HotelBookingAPI.dll"]