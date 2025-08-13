# Personnel-Acess-Control 

## Licencia

Este proyecto está bajo la licencia [Creative Commons Attribution-NonCommercial 4.0 International (CC BY-NC 4.0)](https://creativecommons.org/licenses/by-nc/4.0/).


## 📌 Descripción

**Personnel-Acess-Control** 


## Características

- **JWT Authentication:** Genera tokens JWT para usuarios autenticados.
- **Refresh Tokens:** Permite la renovación del JWT mediante un refresh token rodante.
- **Middleware personalizado:** Valida el JWT en cada solicitud, con posibilidad de excluir endpoints públicos (login, registro, refresh).
- **Integración con Repositories:** Uso combinado de EFC y Dapper para el acceso a datos, separando la lógica de usuarios y sesiones.
- **Estructura modular:** Controladores separados para autenticación (login, logout, refresh) y registro, con soporte para futuros métodos como OAuth y API Keys.


---

## 🚀 Tecnologías Utilizadas

- **.NET 9**  
- **Entity Framework Core (EFC)**  
- **Dapper**  
- **Inyección de Dependencias**  
- **Scala** (para documentación)  
- **SQL Server** (como base de datos)  

---

## 📂 Arquitectura

El proyecto sigue los principios de **Clean Architecture**, dividiendo la solución en las siguientes capas:

1. **Application** → Contiene los casos de uso y lógica de negocio.  
2. **Domain** → Define las entidades y reglas de negocio.  
3. **Infrastructure** → Implementa la persistencia de datos con EFC y Dapper.  
4. **Presentation (WebAPI)** → Expone los endpoints y gestiona las solicitudes HTTP.  

---

## 📌 Instalación y Configuración

### 🔹 Requisitos previos
- .NET 9 instalado  
- SQL Server configurado  
- Configurar la cadena de conexión en `appsettings.json`  

### 🔹 Clonar el repositorio
```sh
git clone https://github.com/Avb-rubns/Personnel-Access-Control.git
cd Personnel-Access-Control
