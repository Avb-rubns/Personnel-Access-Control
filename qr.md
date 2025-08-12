# Historias de Usuario - QR Dinámico

## US-QR-001: Generar QR con enlace dinámico
**Como** administrador  
**Quiero** generar un código QR que apunte a un enlace dinámico  
**Para** poder cambiar la URL de destino en cualquier momento sin tener que regenerar el QR.  

**Criterios de aceptación:**
- El endpoint `/api/qr` debe permitir enviar una URL inicial.
- El sistema debe generar un QR único y un *slug* asociado.
- La respuesta debe incluir:
  - URL pública para escanear (`url/api/v1/qr/{slug}`)
  - Imagen del QR (en base64 o enlace de descarga)
  - Slug único
- Si se envía una URL inválida, debe devolver un error `400 Bad Request`.


---

## US-002: Evitar duplicidad de slugs
**Como** sistema  
**Quiero** evitar que dos QR distintos tengan el mismo *slug*  
**Para** garantizar que cada QR redirige a la URL correcta.

**Criterios de aceptación:**
- El *slug* se generará automáticamente si no se envía en la petición.
- Si el usuario envía un *slug* personalizado:
  - Validar que no exista en la base de datos.
  - Si existe, devolver `409 Conflict`.
- El *slug* debe:
  - Ser alfanumérico y opcionalmente incluir guiones.
  - Tener entre 4 y 50 caracteres.
---
---

## US-003: Redirigir a la URL dinámica
**Como** visitante que escanea el QR  
**Quiero** que el sistema me redirija automáticamente a la URL configurada  
**Para** acceder al contenido sin pasos adicionales.

**Criterios de aceptación:**
- Al acceder a `/{slug}`, el sistema debe:
  - Buscar el slug en la base de datos.
  - Redirigir (HTTP 302) a la URL actual asociada.
  - Registrar el evento en las métricas.
- Si el slug no existe, devolver `404 Not Found`.

---

## US-004: Actualizar URL de un QR existente
**Como** administrador  
**Quiero** cambiar la URL de destino de un QR ya generado  
**Para** poder corregir o actualizar el enlace sin modificar el QR.

**Criterios de aceptación:**
- Endpoint `/api/qr/{slug}` con método `PUT`.
- Debe validar que el slug existe.
- Si la nueva URL es inválida, devolver `400 Bad Request`.
- Confirmar actualización con un `200 OK` y devolver los datos actualizados.

---

## US-005: Consultar métricas de escaneos
**Como** administrador  
**Quiero** ver cuántas veces se ha escaneado un QR  
**Para** analizar su uso.

**Criterios de aceptación:**
- Endpoint `/api/qr/{slug}/metrics`.
- Debe devolver:
  - Número total de escaneos.
  - Lista con fecha y hora de cada escaneo.
  - Filtrado opcional por rango de fechas (`from`, `to`).
- Si el slug no existe, devolver `404 Not Found`.

---

## US-006: Listar todos los QR creados
**Como** administrador  
**Quiero** ver todos los QR creados con sus datos  
**Para** gestionarlos fácilmente.

**Criterios de aceptación:**
- Endpoint `/api/qr` con método `GET`.
- Debe devolver:
  - Slug
  - URL actual
  - Fecha de creación
  - Número de escaneos
- Soportar paginación (`page`, `limit`).

---

## US-007: Eliminar un QR
**Como** administrador  
**Quiero** poder eliminar un QR y su slug  
**Para** que no sea accesible ni genere redirecciones.

**Criterios de aceptación:**
- Endpoint `/api/qr/{slug}` con método `DELETE`.
- Debe validar que el slug existe.
- Al eliminar:
  - Quitar la URL asociada.
  - Borrar métricas.
- Devolver `204 No Content`.