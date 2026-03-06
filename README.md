Horizon FutureVest

Aplicación web para análisis comparativo de países mediante indicadores macroeconómicos.
El sistema permite registrar datos económicos, procesarlos mediante un modelo matemático y generar un ranking de países junto con una estimación de retorno de inversión.

Desarrollado con .NET, ASP.NET Core, Entity Framework Core y Microsoft SQL Server.

Tecnologías










Tabla de contenido

Descripción del proyecto

Arquitectura del sistema

Modelo de datos

Modelo matemático

Funcionalidades principales

Requisitos del sistema

Instalación

Estructura del proyecto

Posibles mejoras

Descripción del proyecto

Horizon FutureVest es una herramienta de análisis económico orientada a organizar indicadores macroeconómicos de distintos países y evaluarlos mediante un modelo cuantitativo.

A partir de los datos registrados, el sistema realiza:

normalización de indicadores económicos

cálculo de scoring ponderado

estimación de retorno potencial de inversión

generación de ranking comparativo de países

simulación de escenarios modificando el peso de los indicadores

Esto permite identificar qué países presentan mejores condiciones económicas según los criterios definidos en el modelo.

<img width="1586" height="771" alt="image" src="https://github.com/user-attachments/assets/6c8d4613-b80f-460a-9850-01ddaaf85982" />
Arquitectura del sistema

La aplicación utiliza una arquitectura en capas, lo que facilita la organización del código y la mantenibilidad del sistema.

Capa de presentación

Gestiona la interacción con el usuario mediante el patrón MVC.

Componentes principales:

Controllers

Views (Razor)

ViewModels

Responsabilidades:

mostrar interfaces web

recibir datos ingresados por el usuario

presentar resultados del ranking

Capa de lógica de negocio

Contiene las reglas principales del sistema.

Responsabilidades:

validación de datos

ejecución del modelo matemático

cálculo del ranking

estimación de retorno de inversión

control de integridad de datos

Esta capa implementa el motor de análisis económico del sistema.

Capa de acceso a datos

Gestiona la comunicación con la base de datos mediante Entity Framework Core.

Responsabilidades:

persistencia de datos

consultas de indicadores

almacenamiento de configuraciones

Modelo de datos

El sistema se basa en las siguientes entidades principales.

País

Representa cada país evaluado en el sistema.

Campos principales:

nombre del país

código ISO

Macroindicador

Define los indicadores económicos utilizados en el modelo.

Campos principales:

nombre del indicador

peso dentro del modelo

propiedad más alto es mejor

<img width="1506" height="637" alt="image" src="https://github.com/user-attachments/assets/8ec4d615-4d8e-47ee-b8fa-bef69a8f76e6" />
Indicador por país

Registra los valores económicos asociados a cada país.

Campos principales:

país

macroindicador

valor del indicador

año

<img width="1500" height="725" alt="image" src="https://github.com/user-attachments/assets/a30fc5f7-10b6-47d5-87e8-8b7a4645b019" />
Configuración de retorno

Almacena los parámetros utilizados para calcular la estimación de retorno de inversión.

Campos:

tasa mínima

tasa máxima

<img width="1544" height="733" alt="image" src="https://github.com/user-attachments/assets/a5c7df2e-da14-4b90-994a-26d88722ddd7" />
Modelo matemático

El sistema utiliza un modelo de scoring ponderado basado en normalización Min-Max, lo que permite comparar indicadores económicos que tienen escalas diferentes.

El proceso incluye:

Validación de datos

Normalización de indicadores

Cálculo de sub-puntajes

Cálculo del scoring total

Normalización de indicadores
<img width="227" height="86" alt="image" src="https://github.com/user-attachments/assets/f8e84f77-b0d7-44ed-b7ef-9b354cab80f4" /> <img width="265" height="67" alt="image" src="https://github.com/user-attachments/assets/92cef09a-08ca-4a04-9855-d0d6c2d84245" />

Cuando valores más altos representan mejor desempeño:

Norm = (Valor − Min) / (Max − Min)

Cuando valores más bajos representan mejor desempeño:

Norm = (Max − Valor) / (Max − Min)

Caso especial:

Si Max = Min → Norm = 0.5
Cálculo de sub-puntajes

Cada indicador normalizado se multiplica por su peso.

<img width="527" height="62" alt="image" src="https://github.com/user-attachments/assets/dc651422-2001-4e81-80e7-7eaa76d14ad4" />
SubScore = Norm × Peso
<img width="499" height="73" alt="image" src="https://github.com/user-attachments/assets/d2477cc1-0391-4e73-9677-e793c268e413" />
Cálculo del scoring total

El puntaje final se obtiene sumando todos los sub-puntajes.

Score = Σ SubScore

Rango del resultado:

0 ≤ Score ≤ 1

Un valor más alto indica mejor desempeño relativo del país.

<img width="576" height="64" alt="image" src="https://github.com/user-attachments/assets/cdd4c81e-3356-4b1f-be97-d3e13aa03bda" />
Estimación de retorno de inversión

El sistema estima una tasa potencial de retorno mediante una función lineal.

Return = rmin + (rmax − rmin) × Score

Valores por defecto:

rmin = 2%
rmax = 15%
Funcionalidades principales
Generación de ranking

Permite calcular el ranking de países para un año específico.

Validaciones previas:

suma de pesos = 1

países con indicadores completos

mínimo dos países para comparación

Resultados mostrados:

país

código ISO

scoring

retorno estimado

<img width="1465" height="619" alt="image" src="https://github.com/user-attachments/assets/c1e82a5b-2218-4484-b037-0051f8a5d000" />
Gestión de países

Permite administrar los países registrados:

crear

editar

eliminar

visualizar listado

Gestión de macroindicadores

Permite definir los indicadores utilizados en el modelo.

Cada indicador incluye:

nombre

peso

propiedad más alto es mejor

<img width="1446" height="471" alt="image" src="https://github.com/user-attachments/assets/8ec53178-25dc-4bc5-971b-0b13bca3eec5" />

Regla del sistema:

la suma de pesos no puede superar 1

Registro de indicadores por país

Permite registrar valores económicos por país.

Datos almacenados:

país

indicador

valor

año

Restricciones:

no duplicidad de indicadores por año

un solo valor por indicador y país en el mismo período

Configuración de tasas de retorno

Permite configurar los parámetros del cálculo de retorno.

Regla:

tasa mínima < tasa máxima
Simulador de ranking

Permite analizar escenarios alternativos modificando los pesos de los indicadores.

Características:

no altera la configuración original

permite comparar distintos enfoques de análisis económico

<img width="1450" height="737" alt="image" src="https://github.com/user-attachments/assets/0ea54fcb-a17f-420c-a84b-e537d53129e0" />
Requisitos del sistema

Para ejecutar el proyecto se requiere:

.NET SDK

Microsoft SQL Server

navegador web moderno

entorno de desarrollo como Microsoft Visual Studio

Instalación
Clonar el repositorio
git clone https://github.com/usuario/horizon-futurevest.git
Acceder al proyecto
cd horizon-futurevest
Configurar la cadena de conexión

Editar el archivo:

appsettings.json
Ejecutar migraciones
dotnet ef database update
Ejecutar la aplicación
dotnet run

Abrir en el navegador:

https://localhost:xxxx
Estructura del proyecto
HorizonFutureVest
│
├── Controllers
├── Models
├── ViewModels
├── Views
├── Data
├── Services
├── Migrations
└── wwwroot
