# Cuestionario

1. Describe los principales problemas si se guarda la informacion en un archivo de texto cuando deseamos:
    - Buscar un registro especifico en el archivo.
    - Agregar nuevos productos.

Uno de los mayores problemas al guardar informacion en un archivo de texto es que para cualquier operacion **siempre** será imperioso el cargar todo el archivo de texto en memoria (al menos para leerlo), por ende si fuese un archivo gigante podríamos terminar sin memoria al intentar buscar cualquier registro.

De igual manera para agregar nuevos productos lo más sencillo siempre será reescribir todo el archivo, dado que no tenemos que preocuparnos por sobreescribir algún registro más, o sobrepasar el buffer, etc.

2. ¿Qué problemas tuviste para visualizar los datos?

Como hice uso de una biblioteca para crear las tablas de visualización en terminal, el mayor problema sería de parte del cliente al intentar leer una base de datos con muchos registros dado que ocupa mucho espacio en pantalla y por ende se tiene que mover mucho para buscar algún registro.

3. ¿Es bueno usar hojas de cálculo para guardar información?

Depende mucho del contexto, si se tiene claro desde un principio que la información será poca por todo el ciclo de vida de ésta puede ser una opción que no requiere mucha experiencia técnica, pero cuando se tratan de sistemas escalables la respuesta es siempre: no.

4. ¿Qué diferencias existen entre una base de datos y una hoja de cálculo?

En la base de datos los elementos se guardan con ciertos esquemas y estrategias para que este guardado sea eficiente, mientras que una hoja de cálculo no requiere de esto.

5. ¿Qué complejidad tienen las consultas en una hoja de cálculo y en una base de datos?

En general depende de la implementación de la base de datos, las bases de datos modernas implementan para el guardado de la información B-trees, por ende la búsqueda por índices será O(log n), también ésto dependerá de la complejidad de la query que hagamos.
Mientras que en una hoja de cálculo en el mejor de los casos podría ser O(n) pero en general sera cuando menos O(n^2.

6. ¿Cuál fue el primer SMBD que se creó?

Se tienen registros que el primer SMBD creado fue el Integrated Data Store, por Charles Bachman.