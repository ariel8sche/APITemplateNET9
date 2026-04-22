# Documentación de Endpoints OAuth 2.0

Este documento describe los endpoints estándar recomendados para implementar un servidor OAuth 2.0 y OpenID Connect en la API.

## Endpoints Estándar

### 1. /authorize
- **Método:** GET
- **Descripción:** Inicia el flujo de autorización (por ejemplo, flujo de código de autorización). Permite al usuario autenticarse y autorizar a un cliente.
- **Parámetros comunes:**
  - response_type
  - client_id
  - redirect_uri
  - scope
  - state

### 2. /token
- **Método:** POST
- **Descripción:** Intercambia un código de autorización, credenciales de usuario o refresh token por un token de acceso y, opcionalmente, un refresh token.
- **Parámetros comunes:**
  - grant_type
  - code / username / password / refresh_token
  - redirect_uri
  - client_id
  - client_secret

### 3. /userinfo (OpenID Connect)
- **Método:** GET
- **Descripción:** Devuelve información del usuario autenticado asociada al token de acceso.
- **Requiere:** Token de acceso válido (Bearer).

### 4. /revoke
- **Método:** POST
- **Descripción:** Permite revocar un token de acceso o refresh token.
- **Parámetros comunes:**
  - token
  - token_type_hint (opcional)

### 5. /introspect
- **Método:** POST
- **Descripción:** Permite a clientes o recursos validar y obtener metadatos de un token.
- **Parámetros comunes:**
  - token
  - token_type_hint (opcional)

### 6. /.well-known/openid-configuration (OpenID Connect)
- **Método:** GET
- **Descripción:** Devuelve un documento JSON con la configuración y metadatos del servidor de autorización (descubrimiento automático de endpoints y capacidades).

## Notas
- Todos los endpoints deben implementar HTTPS.
- Se recomienda seguir los RFC 6749 (OAuth 2.0), RFC 6750 (Bearer Token), RFC 7009 (Revocation), RFC 7662 (Introspection) y OpenID Connect Core 1.0.
- Los endpoints pueden requerir autenticación básica o mediante parámetros según el flujo y el endpoint.
- Personalizar scopes, claims y flujos según las necesidades del sistema.

## Tareas Pendientes

### 1. Crear tabla User
**Descripción de la tarea (para Ariel):**
Implementar la tabla User en la base de datos para almacenar la información de los usuarios finales que se autenticarán mediante OAuth. Esta tabla debe permitir gestionar usuarios activos/inactivos y almacenar de forma segura el hash de la contraseña. Asegúrate de que los campos Username y Email sean únicos.
**Estructura sugerida:**
- UserId (int, PK, identity)
- Username (nvarchar(100), único, not null)
- PasswordHash (nvarchar(256), not null)
- Email (nvarchar(200), único, not null)
- IsActive (bit, not null, default 1)
- CreatedAt (datetime, not null, default GETDATE())

```sql
CREATE TABLE [User] (
    UserId INT IDENTITY PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    Email NVARCHAR(200) NOT NULL UNIQUE,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
```

### 2. Crear tabla AccessToken
**Descripción de la tarea (para Ariel):**
Crear la tabla AccessToken para registrar los tokens de acceso emitidos a los usuarios y clientes. Debe permitir asociar cada token a un usuario y cliente, controlar su vigencia y revocación. Es fundamental para la validación y gestión de sesiones OAuth.
**Estructura sugerida:**
- AccessTokenId (int, PK, identity)
- UserId (int, FK a User)
- ClientPk (int, FK a Client)
- Token (nvarchar(512), not null)
- ExpiresAt (datetime, not null)
- CreatedAt (datetime, not null, default GETDATE())
- IsRevoked (bit, not null, default 0)

```sql
CREATE TABLE AccessToken (
    AccessTokenId INT IDENTITY PRIMARY KEY,
    UserId INT NOT NULL,
    ClientPk INT NOT NULL,
    Token NVARCHAR(512) NOT NULL,
    ExpiresAt DATETIME NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    IsRevoked BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_AccessToken_User FOREIGN KEY (UserId) REFERENCES [User](UserId),
    CONSTRAINT FK_AccessToken_Client FOREIGN KEY (ClientPk) REFERENCES Client(ClientPk)
);
```

### 3. Crear tabla RefreshToken
**Descripción de la tarea (para Ariel):**
Agregar la tabla RefreshToken para almacenar los refresh tokens asociados a los access tokens. Esta tabla permitirá implementar el flujo de renovación de tokens de acceso y gestionar su revocación de forma segura.
**Estructura sugerida:**
- RefreshTokenId (int, PK, identity)
- AccessTokenId (int, FK a AccessToken)
- Token (nvarchar(512), not null)
- ExpiresAt (datetime, not null)
- CreatedAt (datetime, not null, default GETDATE())
- IsRevoked (bit, not null, default 0)

```sql
CREATE TABLE RefreshToken (
    RefreshTokenId INT IDENTITY PRIMARY KEY,
    AccessTokenId INT NOT NULL,
    Token NVARCHAR(512) NOT NULL,
    ExpiresAt DATETIME NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    IsRevoked BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_RefreshToken_AccessToken FOREIGN KEY (AccessTokenId) REFERENCES AccessToken(AccessTokenId)
);
```

### 4. (Opcional) Crear tabla Consent
**Descripción de la tarea (para Ariel):**
Implementar la tabla Consent para registrar el consentimiento otorgado por los usuarios a los clientes sobre scopes específicos. Esta tabla es útil para cumplir con los flujos de consentimiento explícito y para auditar permisos concedidos.
**Estructura sugerida:**
- ConsentId (int, PK, identity)
- UserId (int, FK a User)
- ClientPk (int, FK a Client)
- Scope (nvarchar(200), not null)
- GrantedAt (datetime, not null, default GETDATE())

```sql
CREATE TABLE Consent (
    ConsentId INT IDENTITY PRIMARY KEY,
    UserId INT NOT NULL,
    ClientPk INT NOT NULL,
    Scope NVARCHAR(200) NOT NULL,
    GrantedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Consent_User FOREIGN KEY (UserId) REFERENCES [User](UserId),
    CONSTRAINT FK_Consent_Client FOREIGN KEY (ClientPk) REFERENCES Client(ClientPk)
);
```
