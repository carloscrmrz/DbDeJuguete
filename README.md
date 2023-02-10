# DbDeJuguete

Programa para la practica 01 de Fundamentos de Bases de Datos en la Facultad de Ciencias.

## Instrucciones de uso

La aplicación ya viene precargada con una base de datos de Productos, asi como una base de datos de Ventas, dentro del mismo programa se pueden crear mas bases de datos.
La aplicación está construida en .NET 7, y por ende es multiplataforma, para correr la aplicación solo es necesario descargar el directorio dist dentro del repositorio y ejecutar el archivo `DbDeJuguete.exe` o si se gusta, usar el .NET SDK para construir la aplicación.

Ya teniendo el SDK (>= .NET 7) descargado solo tenemos que correr un par de comandos:

Nos situamos en el directorio de la solución, en particular la carpeta raiz cuyo nombre es "DbDeJuguete" o si se le dio otro nombre al directorio será el que tenga el archivo DbDeJuguete.sln

```
$ dotnet build
```

Una vez hecho esto, se nos indicará la ruta donde se crearon los archivos de distribución, la cual por defecto será DbDeJuguete/DbDeJuguete/dist, podemos ir directamente a esta carpeta y ejecutar el archivo `.exe` o desde la consola de comandos:

```
$ dotnet run --project DbDeJuguete/DbDeJuguete.csproj
```

### Roadmap

La aplicación está aún en un estado larvario, por ende existen un par de bugs que el corporativo tiene en la mira, nuestro roadmap tiene como objetivo corregir estos bugs:

- Al crear una nueva base de datos, la lista que contiene los nombres para seleccionar una base de datos no se actualiza automáticamente, y por ende el usuario tiene que retroceder al menu principal y volver a seleccionar la lista para que se muestre actualizada.

Y vaya... esos parecen ser todos los bugs que encontramos!
