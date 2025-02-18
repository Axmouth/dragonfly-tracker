FROM microsoft/dotnet:3.1-sdk as build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

COPY DragonflyTacker.csproj /build/

RUN dotnet restore ./build/DragonflyTacker.csproj

COPY . ./build/
WORKDIR /build/
RUN dotnet publish ./DragonflyTacker.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

FROM microsoft/dotnet:3.1-aspnetcore-runtime
WORKDIR /app

COPY --from=build /build/out .

ENTRYPOINT ["dotnet", "DragonflyTacker.dll"] 
