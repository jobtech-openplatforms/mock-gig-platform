## Setup
### NOTE: The settings below are pending update to the documentation. For now, stick to the Docker instructions

### Docker

If you're running Docker on your local development machine, you can use 
it for the RavenDB database. To run the database on Windows with persistence 
in data, you can use a command like this to start it - substitute
`D:\RavenDB\Data` for the path on the localhost to save the RavenDB data.

**NOTE**: The local path must exist for the below to work.

```
docker run --rm -d -p 8088:8080 -p 38888:38888 -v D:\RavenDB\Data:/opt/RavenDB/Server/RavenData --name RavenDb-WithData -e RAVEN_Setup_Mode=None -e RAVEN_License_Eula_Accepted=true -e RAVEN_Security_UnsecuredAccessAllowed=PrivateNetwork  ravendb/ravendb
```

**NOTE**: The port above exposes `8088` - because the Vue client in the main project uses the `8080` port.


### Populating the database

In your terminal, navigate to the `\GigDataApi\src\GigPlatformApi` 
(**NOTE**: This is a [different repository](https://github.com/Roombler/af_gigdata_api) ) 
directory and run `dotnet watch run` to start the server, and 
to `MockGigPlatform\src\MockGigPlatform` afterwards (run `dotnet watch run`), 
to start this project's server.

The reason for starting this project last, is that on startup, this project creates the databases in the
Docker image, as long as it's started and running on http://localhost:8088

When this project is running (you should see the address in the output in the terminal), you can 
populate the databases with dummy data for testing. Visit https://localhost:51821/dummy to
add dummy users (you port may be different)

## API

### Adding users to the main API

**NOTE**: ***This is no longer enabled - pending update*** Using Postman, or your favorite API tool, make a call to http://localhost:5003/api/user/add with JSON
in the body, like

```
{
  "email":"test@test.test",
  "photo":"http://inget.foto/nej",
  "name":"My test"
}
```

### Get users

For development, there is an API endpoint to get all users: 

GET `http://localhost:5003/api/user/` 